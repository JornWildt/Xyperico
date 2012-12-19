using System.Web.Mvc;


namespace Xyperico.Web.Mvc
{
  public class PageLayoutAttribute : ActionFilterAttribute
  {
    private readonly string _masterName;
    public PageLayoutAttribute(string masterName)
    {
      _masterName = masterName;
    }

    public override void OnActionExecuted(ActionExecutedContext filterContext)
    {
      base.OnActionExecuted(filterContext);
      var result = filterContext.Result as ViewResult;
      if (result != null)
      {
        result.MasterName = _masterName + "Layout";
      }
    }
  }
}
