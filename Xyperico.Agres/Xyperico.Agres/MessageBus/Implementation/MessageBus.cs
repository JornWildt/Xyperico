using System;
using CuttingEdge.Conditions;
using Xyperico.Agres.MessageBus.Subscription;
using Xyperico.Agres.MessageBus.RouteHandling;


namespace Xyperico.Agres.MessageBus.Implementation
{
  public class MessageBus : IMessageBus
  {
    ISubscriptionService SubscriptionService { get; set; }

    IRouteManager RouteManager { get; set; }

    IMessageSink MessageSink { get; set; }


    public MessageBus(ISubscriptionService subscriptionService, IRouteManager routeManager, IMessageSink messageSink)
    {
      Condition.Requires(subscriptionService, "subscriptionService").IsNotNull();
      Condition.Requires(routeManager, "routeManager").IsNotNull();
      Condition.Requires(messageSink, "messageSink").IsNotNull();
      SubscriptionService = subscriptionService;
      RouteManager = routeManager;
      MessageSink = messageSink;
    }


    public void Publish(IEvent e)
    {
      Condition.Requires(e, "e").IsNotNull();

      foreach (QueueName subscriber in SubscriptionService.GetSubscribers(e.GetType()))
      {
        Send(subscriber, e);
      }
    }


    public void Subscribe(Type eventType)
    {
      Condition.Requires(eventType, "eventType").IsNotNull();
      SubscriptionService.Subscribe(eventType, MessageSink);
    }


    public void Subscribe<T>() where T : IEvent
    {
      SubscriptionService.Subscribe(typeof(T), MessageSink);
    }


    public void Unsubscribe(Type eventType)
    {
      Condition.Requires(eventType, "eventType").IsNotNull();
      throw new NotImplementedException();
    }

    
    public void Unsubscribe<T>() where T : IEvent
    {
      throw new NotImplementedException();
    }

    
    public void Send(IMessage msg)
    {
      Condition.Requires(msg, "msg").IsNotNull();
      QueueName destination = RouteManager.GetDestinationForMessage(msg.GetType());
      if (destination == null)
        throw new InvalidOperationException(string.Format("No route registered for message '{0}' - cannot send.", msg));
      Send(destination, msg);
    }


    public void Send(QueueName destination, IMessage msg)
    {
      Condition.Requires(destination, "destination").IsNotNull();
      Condition.Requires(msg, "msg").IsNotNull();
      Message m = new Message(msg);
      MessageSink.Send(destination, m);
    }
  }
}
