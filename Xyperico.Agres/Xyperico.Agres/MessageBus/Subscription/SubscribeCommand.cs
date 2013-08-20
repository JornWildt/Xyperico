using System;
using System.Runtime.Serialization;
using CuttingEdge.Conditions;


namespace Xyperico.Agres.MessageBus.Subscription
{
  [DataContract]
  [Serializable]
  public class SubscribeCommand : ICommand
  {
    [DataMember(Order=1)]
    public string SubscribedMessagesTypeName { get; set; }

    [DataMember(Order=2)]
    public string SubscriberQueueName { get; set; }

    public SubscribeCommand()
    {
    }

    public SubscribeCommand(Type subscribedMessageType, QueueName subscriberQueue)
    {
      Condition.Requires(subscribedMessageType, "subscribedMessageType").IsNotNull();
      Condition.Requires(subscriberQueue, "subscriberQueue").IsNotNull();
      SubscribedMessagesTypeName = subscribedMessageType.AssemblyQualifiedName;
      SubscriberQueueName = subscriberQueue.Name;
    }
  }
}
