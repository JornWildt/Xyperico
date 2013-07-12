namespace Xyperico.Agres
{
  public interface ICommand : IMessage
  {
  }


  public interface ICommand<TId> : ICommand
    where TId : IIdentity
  {
    TId Id { get; }
  }
}
