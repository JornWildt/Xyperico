namespace Xyperico.Agres
{
  public interface ICommand<TId>
    where TId : IIdentity
  {
    TId Id { get; }
  }
}
