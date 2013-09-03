using System;
using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using log4net;
using Xyperico.Agres.DocumentStore;
using Xyperico.Agres.MessageBus.RouteHandling;


namespace Xyperico.Agres.MessageBus.Subscription
{
  public class SubscriptionService : ISubscriptionService
  {
    private static ILog Logger = LogManager.GetLogger(typeof(SubscriptionService));

    #region Dependencies

    public IRouteManager RouteManager { get; set; }

    #endregion


    /// <summary>
    /// Create SubscriptionService with empty message routing and supplied input queue name.
    /// </summary>
    /// <param name="subscriptionStoreFactory"></param>
    /// <param name="inputQueueName"></param>
    public SubscriptionService(IDocumentStoreFactory subscriptionStoreFactory, IRouteManager routeManager, QueueName inputQueueName)
    {
      Initialize(subscriptionStoreFactory, routeManager, inputQueueName);
    }


    /// <summary>
    /// Create SubscriptionService and configure it from configuration file.
    /// </summary>
    /// <param name="subscriptionStoreFactory"></param>
    public SubscriptionService(IDocumentStoreFactory subscriptionStoreFactory, IRouteManager routeManager)
    {
      Initialize(subscriptionStoreFactory, routeManager, MessageBusSettings.Settings.InputQueue);
      foreach (MessageBusSettings.MessageRoute route in MessageBusSettings.Settings.Routes)
        RouteManager.AddRoute(route.Messages, route.Destination);
    }


    protected void Initialize(IDocumentStoreFactory subscriptionStoreFactory, IRouteManager routeManager, QueueName inputQueueName)
    {
      Condition.Requires(subscriptionStoreFactory, "subscriptionStoreFactory").IsNotNull();
      Condition.Requires(inputQueueName, "inputQueueName").IsNotNull();
      Condition.Requires(routeManager, "routeManager").IsNotNull();
      RouteManager = routeManager;
      Subscriptions = subscriptionStoreFactory.Create<Type, SubscriptionRegistration>();
      InputQueueName = inputQueueName;
    }


    #region ISubscriptionService

    public QueueName InputQueueName { get; protected set; }


    public void Subscribe(Type messageType, IMessageSink messageSink)
    {
      Condition.Requires(messageType, "messageType").IsNotNull();
      Condition.Requires(messageSink, "messageSink").IsNotNull();

      QueueName destination = RouteManager.GetDestinationForMessage(messageType);
      if (destination == null)
        throw new InvalidOperationException(string.Format("Could not find any message routing information for message type '{0}'.", messageType));

      Logger.DebugFormat("Subscribing to {0} at {1}.", messageType, destination);
      SubscribeCommand cmd = new SubscribeCommand(messageType, InputQueueName);
      Message msg = new Message(cmd);
      messageSink.Send(destination, msg);
    }


    public void AddSubscriber(Type messageType, QueueName destination)
    {
      Condition.Requires(messageType, "messageType").IsNotNull();
      Condition.Requires(destination, "destination").IsNotNull();

      SubscriptionRegistration registration;
      if (!Subscriptions.TryGet(messageType, out registration))
        registration = new SubscriptionRegistration();

      if (!registration.SubscriberQueueNames.Any(q => q == destination))
        registration.SubscriberQueueNames.Add(destination.Name);

      Subscriptions.Put(messageType, registration);
    }


    public IEnumerable<QueueName> GetSubscribers(Type messageType)
    {
      Condition.Requires(messageType, "messageType").IsNotNull();

      SubscriptionRegistration registration;
      if (Subscriptions.TryGet(messageType, out registration))
        return registration.SubscriberQueueNames.Select(name => new QueueName(name));
      return Enumerable.Empty<QueueName>();
    }

    #endregion


    #region Internals

    IDocumentStore<Type, SubscriptionRegistration> Subscriptions { get; set; }

    #endregion
  }
}
