using System.Web.Mvc;


namespace Xyperico.TestWebServer.Controllers
{
  public class xrdExamplesController : Controller
  {
    [HttpGet]
    public ActionResult Index()
    {
      Response.ContentType = "application/xrd+xml";
      return View();
    }
  }
}
