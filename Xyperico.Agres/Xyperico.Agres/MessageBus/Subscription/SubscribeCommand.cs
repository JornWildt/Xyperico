using CuttingEdge.Conditions;
using System;
using System.Runtime.Serialization;


namespace Xyperico.Agres.MessageBus.Subscription
{
  [DataContract]
  public class SubscribeCommand : ICommand
  {
    [DataMember(Order=1)]
    public string SubscribedMessagesTypeName { get; protected set; }

    [DataMember(Order=2)]
    public string SubscriberQueueName { get; protected set; }

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
