using System.Web.Mvc;


namespace Xyperico.TestWebServer.Controllers
{
  public class WellKnownController : Controller
  {
    [HttpGet]
    public ActionResult HostMeta()
    {
      Response.ContentType = "application/xrd+xml";
      return View();
    }
  }
}
