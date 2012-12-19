using System.Web.Mvc;


namespace Xyperico.TestWebServer.Controllers
{
  public class DescribeController : Controller
  {
    [HttpGet]
    public ActionResult Index(string uri)
    {
      Response.ContentType = "application/xrd+xml";
      return View((object)uri);
    }
  }
}