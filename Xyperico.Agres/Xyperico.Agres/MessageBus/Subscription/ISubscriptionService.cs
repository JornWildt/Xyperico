using System;
using System.Collections.Generic;


namespace Xyperico.Agres.MessageBus.Subscription
{
  public interface ISubscriptionService
  {
    QueueName InputQueueName { get; }

    /// <summary>
    /// Add outgoing message route (defining the receiving subscription queue for subscriptions per message type)
    /// </summary>
    /// <param name="messageTypeFilter"></param>
    /// <param name="destination"></param>
    void AddRoute(string messageTypeFilter, QueueName destination);


    IEnumerable<RouteRegistration> GetRoutes();

    /// <summary>
    /// Local process making a subscription request to remote service
    /// </summary>
    /// <param name="messageType"></param>
    void Subscribe(Type messageType, IMessageSink messageSink);

    /// <summary>
    /// Remote service subscribing to local process
    /// </summary>
    /// <param name="messageType"></param>
    /// <param name="destination"></param>
    void AddSubscriber(Type messageType, QueueName destination);


    /// <summary>
    /// Get all remote subscribers for a single message
    /// </summary>
    /// <param name="messageType"></param>
    /// <returns></returns>
    IEnumerable<string> GetSubscribers(Type messageType);
  }
}
