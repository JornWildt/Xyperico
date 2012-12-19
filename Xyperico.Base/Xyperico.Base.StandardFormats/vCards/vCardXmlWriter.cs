using System;
using System.IO;
using System.Text;
using System.Xml;


namespace Xyperico.Base.StandardFormats.vCards
{
  public class vCardXmlWriter
  {
    public void Write(Stream s, vCard c)
    {
      XmlWriterSettings settings = new XmlWriterSettings();
      settings.Encoding = Encoding.UTF8;
      settings.Indent = true;
      settings.IndentChars = " ";

      using (XmlWriter w = XmlWriter.Create(s, settings))
      {
        Write(w, c);
      }
    }


    public void Write(XmlWriter w, vCard c)
    {
      if (c.Fn.Items.Count == 0)
        throw new ArgumentException("vCard must have non-empty FN property", "FN");

      w.WriteStartDocument();

      w.WriteStartElement("vcards", vCard.vcard40NS);
      w.WriteAttributeString("xmlns", "", null, vCard.vcard40NS);
      w.WriteStartElement("vcard");

      if (c.Source.Default != null)
        WriteUriElement(w, "source", c.Source.Default.Value.ToString());
      if (c.Name != null)
        WriteTextElement(w, "name", c.Name);
      WriteTextElement(w, "fn", c.Fn.Default.Value);
      //WriteTextElement(w, "email", c.EMail);

      if (c.N != null && (!string.IsNullOrEmpty(c.N.FamilyName) || !string.IsNullOrEmpty(c.N.GivenNames)))
      {
        w.WriteStartElement("n");
        if (c.N.FamilyName != null)
          WriteTextElement(w, "surname", c.N.FamilyName);
        if (c.N.GivenNames != null)
          WriteTextElement(w, "given", c.N.GivenNames);
        w.WriteEndElement();
      }

      foreach (vCard.PropertyRegistration reg in c.PropertyRegistrations)
      {
        reg.Property.WriteXml(w, reg.PropertyName, reg.PropertyNS);
      }

      w.WriteEndElement(); // vcard
      w.WriteEndElement(); // vcards

      w.WriteEndDocument();
    }


    private static void WriteTextElement(XmlWriter w, string elementName, string text)
    {
      if (text == null)
        return;
      WriteSomeElement(w, elementName, "text", text);
    }


    private static void WriteUriElement(XmlWriter w, string elementName, string uri)
    {
      if (uri == null)
        return;
      WriteSomeElement(w, elementName, "uri", uri);
    }


    private static void WriteSomeElement(XmlWriter w, string elementName, string type, string content)
    {
      if (content == null)
        return;
      w.WriteStartElement(elementName);
      w.WriteElementString(type, content);
      w.WriteEndElement();
    }
  }
}
