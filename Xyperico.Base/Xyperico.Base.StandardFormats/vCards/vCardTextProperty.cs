using System.Xml;


namespace Xyperico.Base.StandardFormats.vCards
{
  public class vCardTextProperty : vCardProperty
  {
    public string Text { get; private set; }

    public vCardTextProperty(string text)
    {
      Text = text;
    }


    public override void WriteXml(XmlWriter w, string propertyName, string propertyNS)
    {
      WriteTypedElement(w, propertyName, propertyNS, "text", Text);
    }


    public override string ToString()
    {
      return Text;
    }
  }
}
