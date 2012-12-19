using System.Web.Mvc;
using System.Web;


namespace Xyperico.Web.Mvc
{
  public static class LocalizationExtension
  {
    public static IHtmlString Format(this HtmlHelper html, string format, params object[] args)
    {
      for (int i = 0; i < args.Length; ++i)
        if (args[i] != null)
          args[i] = html.Encode(args[i]);
        
      return new MvcHtmlString(string.Format(format, args));
    }
  }
}
