using System.Xml;
namespace Xyperico.Base.StandardFormats.vCards
{
  public abstract class vCardProperty
  {
    public abstract void WriteXml(XmlWriter w, string propertyName, string propertyNS);


    protected void WriteTypedElement(XmlWriter w, string propertyName, string propertyNS, string type, string content)
    {
      w.WriteStartElement("ex", propertyName, propertyNS);
      w.WriteElementString(type, propertyNS, content);
      w.WriteEndElement();
    }
  }
}
