using System;
using System.Net;
using System.Xml;
using System.IO;
using HtmlAgilityPack;


namespace Xyperico.Web.Testing
{
  public static class Utility
  {
    public static Uri BaseUri
    {
      get
      {
        return new Uri(Xyperico.Base.Testing.TestHelper.GetMachineSpecificConfigurationSetting("BaseUri"));
      }
    }


    public static CookieContainer Cookies
    {
      get;
      set;
    }


    public static Uri AsTestUri(this string path)
    {
      return new Uri(BaseUri, path);
    }


    public static Uri GetTestUri(string path)
    {
      return new Uri(BaseUri, path);
    }


    public static XmlDocument GetXmlDocumentFromResponse(HttpWebResponse response)
    {
      XmlReaderSettings settings = new XmlReaderSettings
      {
        DtdProcessing = DtdProcessing.Ignore
      };

      using (XmlReader reader = XmlReader.Create(response.GetResponseStream(), settings))
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(reader);
        return doc;
      }
    }


    public static HtmlDocument GetHtmlDocumentFromResponse(HttpWebResponse response)
    {
      using (TextReader reader = new StreamReader(response.GetResponseStream()))
      {
        HtmlDocument doc = new HtmlDocument();
        doc.Load(reader);
        return doc;
      }
    }
  }
}
