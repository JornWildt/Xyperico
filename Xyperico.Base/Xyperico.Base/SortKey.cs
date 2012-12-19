using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Xyperico.Base
{
  public enum SortKeyDirection { Ascending, Descending }

  public class SortKey<TSource>
  {
    public SortKeyDirection Direction { get; private set; }

    public Expression<Func<TSource, object>> Key { get; private set; }


    public SortKey(SortKeyDirection direction, Expression<Func<TSource, object>> key)
    {
      Direction = direction;
      Key = key;
    }
  }


  public class SortKeyList<TSource> : List<SortKey<TSource>>
  {
    public static SortKeyList<TSource> CreateKeyList(SortKeyDirection direction, Expression<Func<TSource, object>> key)
    {
      SortKeyList<TSource> list = new SortKeyList<TSource>();
      list.Add(new SortKey<TSource>(direction, key));
      return list;
    }


    /// <summary>
    /// Parse traditional SQL order-by statement into a SortKeyList.
    /// </summary>
    /// <remarks><para>The order-by input is a comma separated string of fields names with added direction keywords.
    /// For instance "Title, Age desc". The field names are case-sensitive whereas the direction keywords are case-insensitive.
    /// Possible values for the direction keywords are "ascending", "descending", "asc", "desc".</para>
    /// <para>Developers should actually try to avoid using this method. It is primarily here to connect GUI and code where the GUI
    /// cannot produce the <c>Func&lt;TSource,object&gt;</c> objects directly.</para></remarks>
    /// <param name="orderBy">Order-by string.</param>
    /// <returns>A SortKeyList representing the order-by string.</returns>
    public static SortKeyList<TSource> CreateKeyList(string orderBy)
    {
      SortKeyList<TSource> list = new SortKeyList<TSource>();

      if (orderBy == null)
        return list;
      orderBy = orderBy.Trim();
      if (orderBy.Length == 0)
        return list;

      string[] parts = orderBy.Split(',');
      Type sourceType = typeof(TSource);
      foreach (string part in parts)
      {
        string[] keydir = part.Trim().Split(' ');

        string key = keydir[0].Trim();
        string dirstr = (keydir.Length > 1 ? keydir[1].Trim() : null);
        SortKeyDirection dir = ParseDirection(dirstr);

        list.AddKey(dir, ParseFieldReference(sourceType, key));
      }

      return list;
    }


    protected static SortKeyDirection ParseDirection(string dirstr)
    {
      SortKeyDirection dir = SortKeyDirection.Ascending;
      if (string.IsNullOrEmpty(dirstr))
        return dir;

      if (dirstr.ToLower() == "desc" || dirstr.ToLower() == "descending")
        dir = SortKeyDirection.Descending;
      else if (dirstr.ToLower() != "asc" && dirstr.ToLower() != "ascending")
        throw new ArgumentException(string.Format("Unknown direction keyword '{0}'", dirstr));

      return dir;
    }


    protected static Expression<Func<TSource, object>> ParseFieldReference(Type sourceType, string fieldname)
    {
      ParameterExpression p = Expression.Parameter(sourceType, "e");

      Expression body = p;

      string[] properties = fieldname.Split('.');
      Type propertyType = sourceType;
      foreach (string property in properties)
      {
        PropertyInfo pi = propertyType.GetProperty(property);
        if (pi == null)
          throw new ArgumentException(String.Format("Unknown property {0} on type {1}", property, propertyType));
        body = Expression.Property(body, pi);

        propertyType = pi.PropertyType;
      }

      body = Expression.Convert(body, typeof(object));

      LambdaExpression l = Expression.Lambda(body, p);

      return (Expression<Func<TSource, object>>)l;
    }


    public SortKeyList<TSource> AddKey(SortKeyDirection direction, Expression<Func<TSource, object>> key)
    {
      Add(new SortKey<TSource>(direction, key));
      return this;
    }
  }


  public static class SortKeyListExtensions
  {
    public static IEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> query, string sortKey)
    {
      return OrderBy(query, SortKeyList<TSource>.CreateKeyList(sortKey));
    }

    public static IEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> query, SortKeyList<TSource> sortKeys)
    {
      if (sortKeys != null && sortKeys.Count > 0)
      {
        IOrderedEnumerable<TSource> orderedQuery;

        var sortKey0 = sortKeys[0];
        if (sortKey0.Direction == SortKeyDirection.Ascending)
          orderedQuery = query.OrderBy(GetDelegateFromExpression(sortKey0.Key));
        else
          orderedQuery = query.OrderByDescending(GetDelegateFromExpression(sortKey0.Key));

        for (int i = 1; i < sortKeys.Count; ++i)
        {
          var sortKey = sortKeys[i];
          if (sortKey.Direction == SortKeyDirection.Ascending)
            orderedQuery = orderedQuery.ThenBy(GetDelegateFromExpression(sortKey.Key));
          else
            orderedQuery = orderedQuery.ThenByDescending(GetDelegateFromExpression(sortKey.Key));
        }

        query = orderedQuery;
      }

      return query;
    }


    public static IEnumerable<TSource> GetPaged<TSource>(this IEnumerable<TSource> query, int pageIndex, int pageSize)
    {
      return query.Skip(pageIndex * pageSize).Take(pageSize);
    }


    public static IEnumerable<TSource> OrderByPaged<TSource>(this IEnumerable<TSource> query, int pageIndex, int pageSize, SortKeyList<TSource> sortKeys)
    {
      return query.OrderBy(sortKeys).Skip(pageIndex * pageSize).Take(pageSize);
    }


    static Func<TSource, object> GetDelegateFromExpression<TSource>(Expression<Func<TSource, object>> expr)
    {
      // To do one day: add some caching since compile is rather expensive.

      return (Func<TSource, object>)expr.Compile();
    }


    public static IQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, SortKeyList<TSource> sortKeys)
    {
      if (sortKeys != null && sortKeys.Count > 0)
      {
        IOrderedQueryable<TSource> orderedQuery;

        var sortKey0 = sortKeys[0];
        if (sortKey0.Direction == SortKeyDirection.Ascending)
          orderedQuery = query.OrderBy(sortKey0.Key);
        else
          orderedQuery = query.OrderByDescending(sortKey0.Key);

        for (int i = 1; i < sortKeys.Count; ++i)
        {
          var sortKey = sortKeys[i];
          if (sortKey.Direction == SortKeyDirection.Ascending)
            orderedQuery = orderedQuery.ThenBy(sortKey.Key);
          else
            orderedQuery = orderedQuery.ThenByDescending(sortKey.Key);
        }

        query = orderedQuery;
      }

      return query;
    }


    public static IQueryable<TSource> GetPaged<TSource>(this IQueryable<TSource> query, int pageIndex, int pageSize)
    {
      return query.Skip(pageIndex * pageSize).Take(pageSize);
    }


    public static IQueryable<TSource> OrderByPaged<TSource>(this IQueryable<TSource> query, int pageIndex, int pageSize, SortKeyList<TSource> sortKeys)
    {
      return query.OrderBy(sortKeys).Skip(pageIndex * pageSize).Take(pageSize);
    }
  }
}
