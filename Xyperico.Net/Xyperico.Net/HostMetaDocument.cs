using System.Xml;
using System.Xml.Serialization;


namespace Xyperico.Net
{
  [XmlInclude(typeof(XrdDocument))]
  [XmlRoot("XRD", Namespace = "http://docs.oasis-open.org/ns/xri/xrd-1.0")]
  public class HostMetaDocument : XrdDocument
  {
    [XmlElement(Namespace = "http://host-meta.net/xrd/1.0")]
    public string Host { get; set; }
  }
}
