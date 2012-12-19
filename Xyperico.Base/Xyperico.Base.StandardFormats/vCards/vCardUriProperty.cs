using System.Xml;


namespace Xyperico.Base.StandardFormats.vCards
{
  public class vCardUriProperty : vCardProperty
  {
    public string Uri { get; private set; }

    
    public vCardUriProperty(string uri)
    {
      Uri = uri;
    }


    public override void WriteXml(XmlWriter w, string propertyName, string propertyNS)
    {
      WriteTypedElement(w, propertyName, propertyNS, "uri", Uri);
    }


    public override string ToString()
    {
      return Uri;
    }
  }
}
