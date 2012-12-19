using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;


namespace Xyperico.Web.Mvc
{
  public class ActionUrlConfigurationElement : ConfigurationElement
  {
    [ConfigurationProperty("Action")]
    public string Action
    {
      get { return (string)this["Action"]; }
      set { this["Action"] = value; }
    }


    [ConfigurationProperty("Controller")]
    public string Controller
    {
      get { return (string)this["Controller"]; }
      set { this["Controller"] = value; }
    }


    [ConfigurationProperty("Area")]
    public string Area
    {
      get { return (string)this["Area"]; }
      set { this["Area"] = value; }
    }


    public ActionResult Redirect()
    {
      RouteValueDictionary route = new RouteValueDictionary();

      if (!string.IsNullOrEmpty(Action))
        route["action"] = Action;
      if (!string.IsNullOrEmpty(Controller))
        route["controller"] = Controller;
      if (!string.IsNullOrEmpty(Area))
        route["area"] = Area;

      if (route.Count == 0)
        return new RedirectResult("~/");

      return new RedirectToRouteResult(route);
    }
  }
}
