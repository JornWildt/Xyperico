using CuttingEdge.Conditions;
using Xyperico.Agres.EventStore;
using Xyperico.Agres.Serialization;
using Msmq = System.Messaging;


namespace Xyperico.Agres.MSMQ
{
  public static class MSMQEventStoreConfigurationExtensions
  {
    public static EventStoreConfiguration WithMSMQPublisher(this EventStoreConfiguration cfg, string queueName)
    {
      Condition.Requires(cfg, "cfg").IsNotNull();
      Condition.Requires(queueName, "queueName").IsNotNullOrEmpty();

      ISerializer messageSerializer = Xyperico.Agres.EventStore.EventStoreConfigurationExtensions.GetMessageSerializer(cfg);
      Msmq.IMessageFormatter messageFormater = new MSMQMessageFormatter(messageSerializer);
      IEventPublisher publisher = new MSMQEventPublisher(queueName, messageFormater);

      EventStoreConfigurationExtensions.SetEventPublisher(cfg, publisher);

      return cfg;
    }
  }
}
