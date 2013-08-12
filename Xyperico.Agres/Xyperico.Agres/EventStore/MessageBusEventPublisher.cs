using CuttingEdge.Conditions;
using Xyperico.Agres.MessageBus;


namespace Xyperico.Agres.EventStore
{
  public class MessageBusEventPublisher : IEventPublisher
  {
    protected IMessageBus MessageBus { get; set; }


    public MessageBusEventPublisher(IMessageBus messageBus)
    {
      Condition.Requires(messageBus, "messageBus").IsNotNull();
      MessageBus = messageBus;
    }


    public void Publish(IEvent e)
    {
      MessageBus.Publish(e);
    }
  }
}
