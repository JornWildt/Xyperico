using System;
using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using Xyperico.Agres.DocumentStore;


namespace Xyperico.Agres.MessageBus.Subscription
{
  public class SubscriptionService : ISubscriptionService
  {
    /// <summary>
    /// Create SubscriptionService with empty message routing and supplied input queue name.
    /// </summary>
    /// <param name="subscriptionStoreFactory"></param>
    /// <param name="inputQueueName"></param>
    public SubscriptionService(IDocumentStoreFactory subscriptionStoreFactory, QueueName inputQueueName)
    {
      Initialize(subscriptionStoreFactory, inputQueueName);
    }


    /// <summary>
    /// Create SubscriptionService and configure it from configuration file.
    /// </summary>
    /// <param name="subscriptionStoreFactory"></param>
    public SubscriptionService(IDocumentStoreFactory subscriptionStoreFactory)
    {
      Initialize(subscriptionStoreFactory, MessageBusSettings.Settings.InputQueue);
      foreach (MessageBusSettings.MessageRoute route in MessageBusSettings.Settings.Routes)
        AddRoute(route.Messages, route.Endpoint);
    }


    protected void Initialize(IDocumentStoreFactory subscriptionStoreFactory, QueueName inputQueueName)
    {
      Condition.Requires(subscriptionStoreFactory, "subscriptionStoreFactory").IsNotNull();
      Condition.Requires(inputQueueName, "inputQueueName").IsNotNull();
      Routes = new List<RouteRegistration>();
      Subscriptions = subscriptionStoreFactory.Create<Type, SubscriptionRegistration>();
      InputQueueName = inputQueueName;
    }


    #region ISubscriptionService

    public QueueName InputQueueName { get; protected set; }


    public void AddRoute(string messageTypeFilter, QueueName destination)
    {
      Condition.Requires(messageTypeFilter, "messageTypeFilter").IsNotNull();
      Condition.Requires(destination, "destination").IsNotNull();

      Routes.Add(new RouteRegistration(messageTypeFilter, destination));
    }


    public IEnumerable<RouteRegistration> GetRoutes()
    {
      return Routes;
    }


    public QueueName GetDestinationForMessage(Type messageType)
    {
      RouteRegistration route = FindRoute(messageType);
      if (route == null)
        return null;
      return route.Destination;
    }


    public void Subscribe(Type messageType, IMessageSink messageSink)
    {
      Condition.Requires(messageType, "messageType").IsNotNull();
      Condition.Requires(messageSink, "messageSink").IsNotNull();

      RouteRegistration route = FindRoute(messageType);
      if (route == null)
        throw new InvalidOperationException(string.Format("Could not find any message routing information for message type '{0}'.", messageType));

      SubscribeCommand cmd = new SubscribeCommand(messageType, InputQueueName);
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


    public IEnumerable<QueueName> GetSubscribers(Type messageType)
    {
      Condition.Requires(messageType, "messageType").IsNotNull();

      SubscriptionRegistration registration;
      if (Subscriptions.TryGet(messageType, out registration))
        return registration.SubscriberQueueNames;
      return Enumerable.Empty<QueueName>();
    }

    #endregion


    #region Internals

    List<RouteRegistration> Routes { get; set; }

    IDocumentStore<Type, SubscriptionRegistration> Subscriptions { get; set; }


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
