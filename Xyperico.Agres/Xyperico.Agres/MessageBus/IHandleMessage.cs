namespace Xyperico.Agres.MessageBus
{
  public interface IHandleMessage<T>
  {
    void Handle(T message);
  }
}
