using CuttingEdge.Conditions;


namespace Xyperico.Agres.MessageBus.Subscription
{
  public class SubscribeCommand : ICommand
  {
    public string SubscribedMessagesTypeName { get; protected set; }
    public string SubscriberQueueName { get; protected set; }


    public SubscribeCommand(string subscribedMessagesTypeName, string subscriberQueueName)
    {
      Condition.Requires(subscribedMessagesTypeName, "subscribedMessagesTypeName").IsNotNullOrEmpty();
      Condition.Requires(subscriberQueueName, "subscriberQueueName").IsNotNullOrEmpty();
      SubscribedMessagesTypeName = subscribedMessagesTypeName;
      SubscriberQueueName = subscriberQueueName;
    }
  }
}
