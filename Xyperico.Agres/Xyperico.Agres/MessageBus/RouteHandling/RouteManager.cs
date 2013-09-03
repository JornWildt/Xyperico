using System;
using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;


namespace Xyperico.Agres.MessageBus.RouteHandling
{
  public class RouteManager : IRouteManager
  {
    #region IRouteManager

    public void AddRoute(string messageFilter, QueueName destination)
    {
      Condition.Requires(messageFilter, "messageFilter").IsNotNull();
      Condition.Requires(destination, "destination").IsNotNull();

      if (Routes.Any(r => r.MessageFilter == messageFilter && r.Destination == destination))
        throw new InvalidOperationException(string.Format("The subscription for {0} at {1} is already added.", messageFilter, destination));
      Routes.Add(new RouteRegistration(messageFilter, destination));
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

    #endregion


    public RouteManager()
    {
      Routes = new List<RouteRegistration>();
    }


    #region Internals

    List<RouteRegistration> Routes { get; set; }


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
