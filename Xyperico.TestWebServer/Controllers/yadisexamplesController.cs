using System.Web.Mvc;


namespace Xyperico.TestWebServer.Controllers
{
  public class yadisexamplesController : Controller
  {
    [HttpGet]
    public string index()
    {
      string location = Url.Action("document", null, null, "http");
      Response.Headers.Add("X-XRDS-Location", location);
      return "";
    }


    [HttpGet]
    public ActionResult document()
    {
      Response.ContentType = "application/xrds+xml";
      return View();
    }
  }
}
