namespace Xyperico.Agres.Contract
{
  public interface ICommand<TId>
    where TId : IIdentity
  {
    TId Id { get; }
  }
}
