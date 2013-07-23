using System;
using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using Xyperico.Agres.DocumentStore;


namespace Xyperico.Agres.MessageBus.Subscription
{
  public class SubscriptionService : ISubscriptionService
  {
    public SubscriptionService(IDocumentStoreFactory subscriptionStoreFactory, QueueName localQueueName)
    {
      Condition.Requires(subscriptionStoreFactory, "subscriptionStoreFactory").IsNotNull();
      Condition.Requires(localQueueName, "localQueueName").IsNotNull();
      Routes = new List<RouteRegistration>();
      Subscriptions = subscriptionStoreFactory.Create<Type, SubscriptionRegistration>();
      LocalQueueName = localQueueName;
    }


    #region ISubscriptionService

    public void AddRoute(string messageTypeFilter, QueueName destination)
    {
      Condition.Requires(messageTypeFilter, "messageTypeFilter").IsNotNull();
      Condition.Requires(destination, "destination").IsNotNull();

      Routes.Add(new RouteRegistration(messageTypeFilter, destination));
    }


    public void Subscribe(Type messageType, IMessageSink messageSink)
    {
      Condition.Requires(messageType, "messageType").IsNotNull();
      Condition.Requires(messageSink, "messageSink").IsNotNull();

      RouteRegistration route = FindRoute(messageType);
      if (route == null)
        throw new InvalidOperationException(string.Format("Could not find any message routing information for message type '{0}'.", messageType));

      SubscribeCommand cmd = new SubscribeCommand(messageType, LocalQueueName);
      Message msg = new Message(cmd);
      messageSink.Send(route.Destination, msg);
    }


    public void AddSubscriber(Type messageType, QueueName destination)
    {
      Condition.Requires(messageType, "messageType").IsNotNull();
      Condition.Requires(destination, "destination").IsNotNull();

      SubscriptionRegistration registration;
      if (!Subscriptions.TryGet(messageType, out registration))
        registration = new SubscriptionRegistration();

      registration.SubscriberQueueNames.Add(destination.Name);

      Subscriptions.Put(messageType, registration);
    }


    public IEnumerable<string> GetSubscribers(Type messageType)
    {
      Condition.Requires(messageType, "messageType").IsNotNull();

      SubscriptionRegistration registration;
      if (Subscriptions.TryGet(messageType, out registration))
        return registration.SubscriberQueueNames;
      return Enumerable.Empty<string>();
    }

    #endregion


    #region Internals

    List<RouteRegistration> Routes { get; set; }

    IDocumentStore<Type, SubscriptionRegistration> Subscriptions { get; set; }

    QueueName LocalQueueName { get; set; }


    private RouteRegistration FindRoute(Type messageType)
    {
      string msgTypeName = messageType.ToString();
      foreach (RouteRegistration route in Routes)
      {
        if (route.Matches(messageType))
          return route;
      }
      return null;
    }

    #endregion
  }
}
