namespace Xyperico.Agres
{
  public class Identity<TId> : IIdentity
  {
    protected TId Id { get; set; }


    public static readonly Identity<TId> Empty = new Identity<TId>();


    public virtual string Literal
    {
      get { return Id.ToString(); }
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

      return Id.Equals(other.Id);
    }


    public override int GetHashCode()
    {
      return Id.GetHashCode();
    }


    public override string ToString()
    {
      return Id.ToString();
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
