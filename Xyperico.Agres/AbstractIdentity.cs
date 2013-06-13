namespace Xyperico.Agres
{
  public class AbstractIdentity<TId> : IIdentity
  {
    protected TId Id { get; set; }


    public static readonly AbstractIdentity<TId> Empty = new AbstractIdentity<TId>();


    public virtual string Literal
    {
      get { return Id.ToString(); }
    }


    public AbstractIdentity()
    {
      Id = default(TId);
    }


    public AbstractIdentity(TId id)
    {
      Id = id;
    }


    public AbstractIdentity(AbstractIdentity<TId> id)
    {
      Id = id.Id;
    }


    public override bool Equals(object obj)
    {
      if (ReferenceEquals(this, obj))
        return true;

      AbstractIdentity<TId> other = obj as AbstractIdentity<TId>;
      if (other == null)
        return false;

      return Equals(other);
    }


    public bool Equals(AbstractIdentity<TId> other)
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


    public static bool operator ==(AbstractIdentity<TId> left, AbstractIdentity<TId> right)
    {
      return Equals(left, right);
    }


    public static bool operator !=(AbstractIdentity<TId> left, AbstractIdentity<TId> right)
    {
      return !Equals(left, right);
    }
  }
}
