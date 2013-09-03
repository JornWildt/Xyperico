using System;
using System.Collections.Generic;


namespace Xyperico.Agres.MessageBus.RouteHandling
{
  public interface IRouteManager
  {
    /// <summary>
    /// Add outgoing message route (defining the receiving subscription queue for subscriptions per message type)
    /// </summary>
    /// <param name="messageFilter"></param>
    /// <param name="destination"></param>
    void AddRoute(string messageFilter, QueueName destination);


    IEnumerable<RouteRegistration> GetRoutes();

    QueueName GetDestinationForMessage(Type messageType);
  }
}
