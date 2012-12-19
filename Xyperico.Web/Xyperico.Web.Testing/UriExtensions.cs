using System;
using System.Net;
using System.Xml;
using HtmlAgilityPack;


namespace Xyperico.Web.Testing
{
  public static class UriExtensions
  {
    public static HttpWebResponse GetHttpResponse(this Uri url, string accept = "*/*", bool allowRedirect = false)
    {
      try
      {
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        req.Accept = accept;
        req.AllowAutoRedirect = allowRedirect;
        req.CookieContainer = Utility.Cookies;
        HttpWebResponse response = (HttpWebResponse)req.GetResponse();
        return response;
      }
      catch (Exception)
      {
        Console.WriteLine(string.Format("Got exception while reading from {0}.", url));
        throw;
      }
    }


    public static XmlDocument GetXmlDocument(this Uri url, string accept = "*/*")
    {
      using (HttpWebResponse response = url.GetHttpResponse(accept))
      {
        return Utility.GetXmlDocumentFromResponse(response);
      }
    }


    public static HtmlDocument GetHtmlDocument(this Uri url, string accept = "*/*")
    {
      using (HttpWebResponse response = url.GetHttpResponse(accept))
      {
        return Utility.GetHtmlDocumentFromResponse(response);
      }
    }
  }
}
