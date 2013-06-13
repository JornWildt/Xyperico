using System;
using System.Collections.Generic;
using System.Reflection;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization;


namespace Xyperico.Base.MongoDB
{
  public class ProtectedMemberFinderConvention
  {
    #region IMemberFinderConvention Members

    public IEnumerable<MemberInfo> FindMembers(Type type)
    {
      //foreach (var fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
      //{
      //    if (fieldInfo.IsInitOnly || fieldInfo.IsLiteral)
      //    {
      //        // we can't write
      //        continue;
      //    }

      //    yield return fieldInfo;
      //}

      foreach (var propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
      {
        if (!propertyInfo.CanRead || (!propertyInfo.CanWrite && type.Namespace != null))
        {
          // we can't write or it is anonymous...
          continue;
        }

        // skip indexers
        if (propertyInfo.GetIndexParameters().Length != 0)
        {
          continue;
        }

        // skip overridden properties (they are already included by the base class)
        var getMethodInfo = propertyInfo.GetGetMethod(true);
        if (getMethodInfo.IsVirtual && getMethodInfo.GetBaseDefinition().DeclaringType != type)
        {
          continue;
        }

        yield return propertyInfo;
      }
    }

    #endregion


    #region IClassMapConvention

    public void Apply(BsonClassMap classMap)
    {
      throw new NotImplementedException();
    }

    public string Name
    {
      get { throw new NotImplementedException(); }
    }

    #endregion IClassMapConvention
  }
}
