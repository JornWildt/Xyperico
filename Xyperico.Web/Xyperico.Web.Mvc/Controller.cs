using System.Web.Mvc;
using log4net;


namespace Xyperico.Web.Mvc
{
  public class Controller : System.Web.Mvc.Controller
  {
    static protected ILog Logger { get; set; }


    public Controller()
    {
      Logger = LogManager.GetLogger(this.GetType());
    }


    protected virtual ActionResult RedirectToHome()
    {
      return Redirect("~/");
    }


    protected string ErrorMessage
    {
      get { return (string)TempData["ErrorMessage"]; }
      set { TempData["ErrorMessage"] = value; }
    }


    protected virtual ActionResult Error(string errorMessage, ActionResult action)
    {
      ErrorMessage = errorMessage;
      return action;
    }
  }
}
