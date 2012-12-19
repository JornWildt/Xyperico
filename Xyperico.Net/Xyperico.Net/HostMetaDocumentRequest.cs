using System;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using CuttingEdge.Conditions;


namespace Xyperico.Net
{
  public class HostMetaDocumentRequest
  {
    private static XmlSerializer HostMetaDocumentSerializer = new XmlSerializer(typeof(HostMetaDocument));

    public string Hostname { get; private set; }


    public HostMetaDocumentRequest(string hostname)
    {
      Condition.Requires(hostname, "hostname").IsNotNullOrEmpty();
      Hostname = hostname;
    }


    public HostMetaDocument GetDocument()
    {
      string url = string.Format("http://{0}/.well-known/host-meta", Hostname);
      HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
      request.Accept = "application/xrd+xml, application/xml";
      HttpWebResponse response = (HttpWebResponse)request.GetResponse();
      using (XmlReader reader = XmlReader.Create(response.GetResponseStream()))
      {
        lock (HostMetaDocumentSerializer)
        {
          HostMetaDocument document = (HostMetaDocument)HostMetaDocumentSerializer.Deserialize(reader);
          return document;
        }
      }
    }
  }
}
