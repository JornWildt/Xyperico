using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using CuttingEdge.Conditions;


namespace Xyperico.Net
{
  [XmlRoot("XRD", Namespace="http://docs.oasis-open.org/ns/xri/xrd-1.0")]
  public class XrdDocument
  {
    public DateTime? Expires { get; set; }

    [XmlElement("Alias")]
    public List<string> Aliases { get; set; }

    public string Subject { get; set; }

    [XmlElement("Property")]
    public List<XrdProperty> Properties { get; set; }

    [XmlElement("Link")]
    public List<XrdLink> Links { get; set; }


    public XrdDocument()
    {
      Properties = new List<XrdProperty>();
      Links = new List<XrdLink>();
    }


    private static XmlSerializer XrdDocumentSerializer = new XmlSerializer(typeof(XrdDocument));


    public static XrdDocument GetFromUrl(string url)
    {
      Condition.Requires(url, "url").IsNotNullOrEmpty();

      return GetFromUrl(new Uri(url));
    }


    public static XrdDocument GetFromUrl(Uri url)
    {
      Condition.Requires(url, "url").IsNotNull();

      HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
      request.Accept = "application/xrd+xml, application/xml";
      HttpWebResponse response = (HttpWebResponse)request.GetResponse();
      using (XmlReader reader = XmlReader.Create(response.GetResponseStream()))
      {
        lock (XrdDocumentSerializer)
        {
          XrdDocument document = (XrdDocument)XrdDocumentSerializer.Deserialize(reader);
          return document;
        }
      }
    }
  }


  public class XrdProperty
  {
    [XmlAttribute("type")]
    public string Type { get; set; }

    [XmlText]
    public string Value { get; set; }
  }


  public class XrdLink
  {
    [XmlAttribute("type")]
    public string Type { get; set; }

    [XmlAttribute("rel")]
    public string Rel { get; set; }

    [XmlAttribute("href")]
    public string HRef { get; set; }

    [XmlAttribute("template")]
    public string Template { get; set; }

    public string Title { get; set; }

    [XmlElement("Property")]
    public List<XrdProperty> Properties { get; set; }

    public XrdLink()
    {
      Properties = new List<XrdProperty>();
    }
  }
}
