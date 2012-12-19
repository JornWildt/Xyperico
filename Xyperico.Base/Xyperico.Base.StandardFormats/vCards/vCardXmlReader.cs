using System;
using System.IO;
using System.Xml;


namespace Xyperico.Base.StandardFormats.vCards
{
  public class vCardXmlReader
  {
    public delegate void ExtendedPropertiesReader(XmlElement xcard, XmlNamespaceManager nsm, vCard vcard);


    public vCard Read(Stream s, ExtendedPropertiesReader xreader = null)
    {
      XmlReaderSettings settings = new XmlReaderSettings();

      using (XmlReader r = XmlReader.Create(s, settings))
      {
        return Read(r, xreader);
      }
    }


    public vCard Read(StringReader r, ExtendedPropertiesReader xreader = null)
    {
      XmlReaderSettings settings = new XmlReaderSettings();

      using (XmlReader xr = XmlReader.Create(r, settings))
      {
        return Read(xr, xreader);
      }
    }


    public vCard Read(XmlReader r, ExtendedPropertiesReader xreader = null)
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(r);

      vCard c = ReadRootElement(doc, xreader);
      return c;
    }


    private static vCard ReadRootElement(XmlDocument doc, ExtendedPropertiesReader xreader = null)
    {
      vCard c = new vCard();
      XmlNamespaceManager nsm = new XmlNamespaceManager(doc.NameTable);
      nsm.AddNamespace("vc", vCard.vcard40NS);

      XmlNode xcards = doc.SelectSingleNode("/vc:vcards", nsm);
      if (!(xcards is XmlElement))
        throw new FormatException("Missing <vcards> element");

      XmlNode xcard = xcards.SelectSingleNode("vc:vcard", nsm);
      if (!(xcard is XmlElement))
        throw new FormatException("Missing /vcards/vcard element");

      string fn = xcard.SelectOptionalInnerText("vc:fn/vc:text", nsm);
      c.Fn.Items.Add(new vCardMultiString(fn));
      c.EMail.Items.Add(new vCardMultiString(xcard.SelectOptionalInnerText("vc:email/vc:text", nsm)));
      c.Name = xcard.SelectOptionalInnerText("vc:name/vc:text", nsm);
      c.Source.Items.Add(new vCardMultiString(xcard.SelectOptionalInnerText("vc:source/vc:uri", nsm)));

      XmlNode xn = xcard.SelectSingleNode("vc:n", nsm);
      if (xn is XmlElement)
      {
        c.N.FamilyName = xn.SelectOptionalInnerText("vc:surname/vc:text", nsm);
        c.N.GivenNames = xn.SelectOptionalInnerText("vc:given/vc:text", nsm);
      }

      if (xreader != null)
        xreader((XmlElement)xcard, nsm, c);

      return c;
    }
  }
}
