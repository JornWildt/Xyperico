using System.Web.Mvc;
using System.Web;


namespace Xyperico.Web.Mvc
{
  public static class Themes
  {
    public static ThemeableRazorViewEngine RazorViewEngine { get; set; }

    
    public static void SetupThemes()
    {
      RazorViewEngine = new ThemeableRazorViewEngine(c => Xyperico.Web.Mvc.Configuration.Settings.Theme);

      ViewEngines.Engines.Clear();
      ViewEngines.Engines.Add(RazorViewEngine);
    }


    public static IHtmlString Stylesheet(this HtmlHelper html, string stylesheet)
    {
      string cssPath = RazorViewEngine.FindStylesheet(html.ViewContext.Controller.ControllerContext, stylesheet, true);
      if (cssPath == null)
        cssPath = RazorViewEngine.FindStylesheet(html.ViewContext.Controller.ControllerContext, stylesheet, false);
      
      if (!string.IsNullOrEmpty(cssPath))
        return new MvcHtmlString(string.Format("<link href=\"{0}\" rel=\"stylesheet\"/>", UrlHelper.GenerateContentUrl(cssPath, html.ViewContext.HttpContext)));
      else
        return new MvcHtmlString(string.Format("<!-- Stylesheet '{0}' not found -->", stylesheet));
    }


    public static IHtmlString ImageUrl(this HtmlHelper html, string image)
    {
      string imagePath = RazorViewEngine.FindImageUrl(html.ViewContext.Controller.ControllerContext, image, true);
      if (imagePath == null)
        imagePath = RazorViewEngine.FindImageUrl(html.ViewContext.Controller.ControllerContext, image, false);

      return new MvcHtmlString(UrlHelper.GenerateContentUrl(imagePath, html.ViewContext.HttpContext));
    }
  }
}
