using System;
using System.Xml;


namespace Xyperico.Base.Testing
{
  public class XmlDocumentAssertion
  {
    private XmlDocument Xml;

    public XmlDocumentAssertion(XmlDocument xml)
    {
      Xml = xml;
    }

    public XmlDocumentAssertion Where(Action<XmlDocument> a)
    {
      a(Xml);
      return this;
    }
  }
}
