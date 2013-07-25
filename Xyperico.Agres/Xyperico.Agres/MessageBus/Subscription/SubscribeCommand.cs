using CuttingEdge.Conditions;
using System;


namespace Xyperico.Agres.MessageBus.Subscription
{
  public class SubscribeCommand : ICommand
  {
    public string SubscribedMessagesTypeName { get; protected set; }
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
