using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using CuttingEdge.Conditions;


namespace Xyperico.Net
{
  [XmlRoot("XRDS", Namespace = "xri://$xrds")]
  public class XrdsDocument
  {
    [XmlElement("XRD", Namespace="xri://$xrd*($v*2.0)")]
    public List<XrdsXrd> XrdElements { get; set; }

    public XrdsDocument()
    {
      XrdElements = new List<XrdsXrd>();
    }


    private static XmlSerializer XrdsDocumentSerializer = new XmlSerializer(typeof(XrdsDocument));


    public static XrdsDocument Deserialize(Stream s)
    {
      Condition.Requires(s, "s").IsNotNull();

      lock (XrdsDocumentSerializer)
      {
        XrdsDocument document = (XrdsDocument)XrdsDocumentSerializer.Deserialize(s);
        return document;
      }
    }


    public static void Serialize(Stream s, XrdsDocument document)
    {
      Condition.Requires(s, "s").IsNotNull();
      Condition.Requires(document, "document").IsNotNull();

      lock (XrdsDocumentSerializer)
      {
        XrdsDocumentSerializer.Serialize(s, document);
      }
    }


    public XrdsService FindService(string type)
    {
      var q = from e in XrdElements
              from s in e.Services
              where s.Types.Contains(type)
              orderby s.Priority
              select s;
      return q.FirstOrDefault();
    }


    public XrdsService GetService(string type)
    {
      XrdsService service = FindService(type);
      if (service == null)
        throw new ArgumentOutOfRangeException("type", string.Format("No XRDS service found for type '{0}'.", type));
      return service;
    }
  }


  public class XrdsXrd
  {
    [XmlElement("Service")]
    public List<XrdsService> Services { get; set; }
  }


  public class XrdsService
  {
    [XmlAttribute("priority")]
    public int Priority { get; set; }

    [XmlElement("Type")]
    public List<string> Types { get; set; }

    public string MediaType { get; set; }

    [XmlElement("URI")]
    public List<XrdsUri> Uris { get; set; }

    public string LocalID { get; set; }

    public Uri FindBestUri()
    {
      return Uris.OrderBy(u => u.Priority).Select(u => new Uri(u.Uri)).FirstOrDefault();
    }

    public Uri getBestUri()
    {
      Uri u = FindBestUri();
      if (u == null)
        throw new ArgumentOutOfRangeException("none", "No URIs found in XRDS service.");
      return u;
    }

    public XrdsService()
    {
      Priority = 10000;
    }
  }


  public class XrdsUri
  {
    [XmlAttribute("priority")]
    public int Priority { get; set; }

    [XmlText]
    public string Uri { get; set; }
  }
}
