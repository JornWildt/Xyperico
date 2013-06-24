namespace Xyperico.Agres
{
  public interface ICommand
  {
  }


  public interface ICommand<TId> : ICommand
    where TId : IIdentity
  {
    TId Id { get; }
  }
}
