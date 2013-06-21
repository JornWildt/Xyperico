namespace Xyperico.Agres
{
  public interface IHaveIdentity<TId>
  {
    TId Id { get; }
  }
}
