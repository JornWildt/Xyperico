﻿using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using CuttingEdge.Conditions;


namespace Xyperico.Agres
{
  [DataContract]
  public abstract class Identity<TId> : IIdentity
    where TId : IEquatable<TId>
  {
    /// <summary>
    /// Internal representation of ID. Not supposed to be unique between different aggregate types.
    /// </summary>
    [DataMember(Order = 1)]
    protected TId Id { get; set; }


    private string _literal;

    /// <summary>
    /// String literal representation of ID - to be used as actual concrete ID when storing and looking up identities.
    /// </summary>
    public virtual string Literal
    {
      get 
      { 
        if (_literal == null)
          _literal = Prefix + (Id != null ? Id.ToString() : "<null>");
        return _literal;
      }
      set
      {
        Condition.Requires(value, "value").IsNotNullOrEmpty().StartsWith(Prefix);
        string id = value.Substring(Prefix.Length);
        Id = ParseId(id);
      }
    }

    
    private TId ParseId(string str)
    {
      TId id = default(TId);
      var converter = TypeDescriptor.GetConverter(typeof(TId));
      if (converter != null)
        id = (TId)converter.ConvertFromString(str);
      if (id == null || id.Equals(default(TId)))
        throw new InvalidOperationException(string.Format("Could not convert string '{0}' to {1}.", str, typeof(TId)));
      return id;
    }


    public Identity()
    {
      Id = default(TId);
    }


    public Identity(TId id)
    {
      Id = id;
    }


    public Identity(Identity<TId> id)
    {
      Id = id.Id;
    }


    /// <summary>
    /// Get string to be prefixed to the base ID - the combination is used in <see cref="Literal"/>.
    /// </summary>
    /// <remarks>If you use Guid as the base ID type then you can leave the prefix empty.</remarks>
    protected abstract string Prefix { get; }


    public override bool Equals(object obj)
    {
      if (ReferenceEquals(this, obj))
        return true;

      Identity<TId> other = obj as Identity<TId>;
      if (other == null)
        return false;

      return Equals(other);
    }


    public bool Equals(Identity<TId> other)
    {
      if (other == null)
        return false;

      return Literal.Equals(other.Literal);
    }


    public override int GetHashCode()
    {
      return Literal.GetHashCode();
    }


    public override string ToString()
    {
      return Literal;
    }


    public static bool operator ==(Identity<TId> left, Identity<TId> right)
    {
      return Equals(left, right);
    }


    public static bool operator !=(Identity<TId> left, Identity<TId> right)
    {
      return !Equals(left, right);
    }
  }
}
