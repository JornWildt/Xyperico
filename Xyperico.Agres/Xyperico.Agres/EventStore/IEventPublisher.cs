namespace Xyperico.Agres.EventStore
{
  public interface IEventPublisher
  {
    void Publish(IEvent e);
  }
}
