using System.Web.Mvc;


namespace Xyperico.Web.Mvc.Html
{
  public static class ErrorMessageExtension
  {
    public static MvcHtmlString ErrorMessage<TModel>
      (this HtmlHelper<TModel> html)
    {
      string msg = (string)html.ViewContext.TempData["ErrorMessage"];
      if (msg != null)
      {
        return new MvcHtmlString("<div class=\"validation-summary-errors\">" + html.Encode(msg) + "</div>");
      }
      return null;
    }
  }
}
