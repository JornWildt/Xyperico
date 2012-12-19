using System.IO;
using System.Xml;
using NUnit.Framework;
using Xyperico.Base.StandardFormats.vCards;


namespace Xyperico.Base.StandardFormats.Tests.vCards
{
  [TestFixture]
  class vCardXmlReaderTests
  {
    [Test]
    public void CanReadXmlvCard()
    {
      string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<vcards xmlns=""urn:ietf:params:xml:ns:vcard-4.0"">
 <vcard>
  <fn>
   <text>John Doe</text>
  </fn>
  <n>
    <surname><text>Doe</text></surname>
    <given><text>J.</text></given>
  </n>
  <email><text>jw@me.my</text></email>
  <name><text>JohnDoe</text></name>
  <source><uri>http://www.somewhere.com</uri></source>
  <ex:why xmlns:ex=""http://here.net"">It is red</ex:why>
  <ex:where xmlns:ex=""http://here.net"">
   <uri>ldap://somefellow</uri>
  </ex:where>
 </vcard>
</vcards>";

      using (StringReader r = new StringReader(xml))
      {
        vCardXmlReader reader = new vCardXmlReader();
        vCard c = reader.Read(r, ParseExtensions);
        Assert.IsNotNull(c);
        Assert.AreEqual("John Doe", c.Fn.Default.Value);
        Assert.AreEqual("jw@me.my", c.EMail.Default.Value);
        Assert.AreEqual("JohnDoe", c.Name);
        Assert.AreEqual("Doe", c.N.FamilyName);
        Assert.AreEqual("J.", c.N.GivenNames);
        Assert.AreEqual("http://www.somewhere.com", c.Source.Default.Value);

        vCardProperty p = c.GetExtendedProperty("why", "http://here.net");
        Assert.IsNotNull(p);
        Assert.AreEqual("It is red", p.ToString());
      }
    }


    void ParseExtensions(XmlElement xcard, XmlNamespaceManager nsm, vCard vcard)
    {
      nsm.AddNamespace("ex", "http://here.net");
      XmlNode xwhy = xcard.SelectSingleNode("ex:why/text()", nsm);
      if (xwhy != null)
        vcard.AddExtendedProperty("why", "http://here.net", new vCardTextProperty(xwhy.Value));
    }
  }
}
