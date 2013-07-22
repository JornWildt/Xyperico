using CuttingEdge.Conditions;
using System;


namespace Xyperico.Agres.MessageBus.Subscription
{
  public class RouteRegistration
  {
    public string MessageTypeFilter { get; private set; }
    
    public QueueName Destination { get; private set; }

    
    public RouteRegistration(string messageTypeFilter, QueueName destination)
    {
      Condition.Requires(messageTypeFilter, "messageTypeFilter").IsNotNullOrEmpty();
      Condition.Requires(destination, "destination").IsNotNull();
      MessageTypeFilter = messageTypeFilter;
      Destination = destination;
    }


    public bool Matches(Type messageType)
    {
      Condition.Requires(messageType, "messageType").IsNotNull();
      return messageType.ToString().StartsWith(MessageTypeFilter);
    }
  }
}
