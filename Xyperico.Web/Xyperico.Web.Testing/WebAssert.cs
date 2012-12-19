using System;
using System.IO;
using System.Net;
using System.Xml;
using NUnit.Framework;
using Xyperico.Base.Testing;
using HtmlAgilityPack;


namespace Xyperico.Web.Testing
{
  public static class WebAssert
  {
    public static void NoWebExceptions(Action a)
    {
      NoWebExceptions<object>(() => { a(); return null; });
    }


    public static T NoWebExceptions<T>(Func<T> a)
    {
      try
      {
        return a();
      }
      catch (WebException ex)
      {
        string url = ((HttpWebResponse)ex.Response).ResponseUri.ToString();
        using (StreamReader r = new StreamReader(((HttpWebResponse)ex.Response).GetResponseStream()))
        {
          Console.Write(r.ReadToEnd());
        }
        Assert.Fail(string.Format("Got WebException for URL {0}: {1}", url, ex.Message));
        return default(T);
      }
    }


    public static void ExpectErrorCode(Action a, HttpStatusCode expectedCode)
    {
      try
      {
        a();
        Assert.Fail("Missing WebException");
      }
      catch (WebException ex)
      {
        HttpWebResponse response = (HttpWebResponse)ex.Response;
        if (response.StatusCode != expectedCode)
        {
          string url = ((HttpWebResponse)ex.Response).ResponseUri.ToString();
          using (StreamReader r = new StreamReader(((HttpWebResponse)ex.Response).GetResponseStream()))
          {
            Console.Write(r.ReadToEnd());
          }
          Assert.Fail(string.Format("Got WebException with unexpected status code {0} for URL {1}. Exception text: {2}.", response.StatusCode, url, ex.Message));
        }
      }
    }


    public static XmlDocumentAssertion CanGetBasicXHtmlDocument(string path)
    {
      Uri url = Utility.GetTestUri(path);
      return CanGetBasicXHtmlDocument(url);
    }


    public static XmlDocumentAssertion CanGetBasicXHtmlDocument(Uri url)
    {
      return WebAssert.NoWebExceptions(() =>
      {
        XmlDocument xml = url.GetXmlDocument();
        Assert.IsNotNull(xml);

        XmlNode titleNode = xml.SelectSingleNode("/html/head/title");
        Assert.IsInstanceOf<XmlElement>(titleNode, "Page must contain title element");
        Assert.IsNotNullOrEmpty(((XmlElement)titleNode).InnerText, "Page title must not be empty");

        return new XmlDocumentAssertion(xml);
      });
    }


    public static HtmlDocumentAssertion CanGetBasicHtmlDocument(string path)
    {
      Uri url = Utility.GetTestUri(path);
      return CanGetBasicHtmlDocument(url);
    }


    public static HtmlDocumentAssertion CanGetBasicHtmlDocument(Uri url)
    {
      return WebAssert.NoWebExceptions(() =>
      {
        HtmlDocument html = url.GetHtmlDocument();
        Assert.IsNotNull(html);

        HtmlNode titleNode = html.DocumentNode.SelectSingleNode("/html/head/title");
        Assert.IsNotNull(titleNode, "Page must contain title element");
        Assert.IsNotNullOrEmpty(titleNode.InnerText, "Page title must not be empty");

        return new HtmlDocumentAssertion(html);
      });
    }


    public static void Redirects(Uri source, Uri destination)
    {
      using (HttpWebResponse response = source.GetHttpResponse())
      {
        Assert.AreEqual(HttpStatusCode.Found, response.StatusCode);

        Uri absoluteLocation = new Uri(source, response.Headers["Location"]);
        Assert.AreEqual(absoluteLocation, destination);
      }
    }


    public static void IsRedirection(HttpWebResponse response)
    {
      Assert.AreEqual(HttpStatusCode.Found, response.StatusCode);
      Assert.IsNotNull(response.Headers["Location"]);
    }
  }
}
