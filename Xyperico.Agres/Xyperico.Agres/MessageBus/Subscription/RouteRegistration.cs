using CuttingEdge.Conditions;
using System;


namespace Xyperico.Agres.MessageBus.Subscription
{
  public class RouteRegistration
  {
    public string MessageFilter { get; private set; }
    
    public QueueName Destination { get; private set; }

    
    public RouteRegistration(string messageFilter, QueueName destination)
    {
      Condition.Requires(messageFilter, "messageFilter").IsNotNullOrEmpty();
      Condition.Requires(destination, "destination").IsNotNull();
      MessageFilter = messageFilter;
      Destination = destination;
    }


    public bool Matches(Type messageType)
    {
      Condition.Requires(messageType, "messageType").IsNotNull();
      return messageType.ToString().StartsWith(MessageFilter);
    }
  }
}
