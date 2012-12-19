using System;
using System.Web;


namespace Xyperico.Web.Mvc.Implementation
{
  public class WebContextApplicationBaseUrl : IApplicationBaseAddress
  {
    #region IApplicationBaseUrl Members

    public string Url
    {
      get { return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority); }
    }


    public Uri Uri
    {
      get { return new Uri(Url); }
    }

    #endregion
  }
}
