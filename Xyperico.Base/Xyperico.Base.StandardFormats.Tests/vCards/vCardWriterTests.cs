using System.IO;
using NUnit.Framework;
using Xyperico.Base.StandardFormats.vCards;


namespace Xyperico.Base.StandardFormats.Tests.vCards
{
  [TestFixture]
  class vCardWriterTests
  {
    [Test]
    public void CanAddExtendedTextProperty()
    {
      vCard v = new vCard();
      v.AddExtendedProperty("why", "http://because.net", new vCardTextProperty("It is red"));
      vCardTextProperty p = v.GetExtendedProperty("why", "http://because.net") as vCardTextProperty;
      Assert.IsNotNull(p);
      Assert.AreEqual("It is red", p.ToString());
      Assert.AreEqual("It is red", p.Text);
    }


    [Test]
    public void CanAddExtendedUriProperty()
    {
      vCard v = new vCard();
      v.AddExtendedProperty("where", "http://here.net", new vCardUriProperty("ldap://somefellow"));
      vCardUriProperty p = v.GetExtendedProperty("where", "http://here.net") as vCardUriProperty;
      Assert.IsNotNull(p);
      Assert.AreEqual("ldap://somefellow", p.ToString());
      Assert.AreEqual("ldap://somefellow", p.Uri);
    }


    [Test]
    public void CanWriteEmptyvCard()
    {
      vCard v = new vCard();
      v.Fn.Items.Add(new vCardMultiString("John Doe"));

      VerifyOutput(v, @"<?xml version=""1.0"" encoding=""utf-8""?>
<vcards xmlns=""urn:ietf:params:xml:ns:vcard-4.0"">
 <vcard>
  <fn>
   <text>John Doe</text>
  </fn>
 </vcard>
</vcards>");
    }


    [Test]
    public void CanWritevCardWithExtendedProperties()
    {
      vCard v = new vCard();
      v.Fn.Items.Add(new vCardMultiString("John Doe"));

      v.AddExtendedProperty("where", "http://here.net", new vCardUriProperty("ldap://somefellow"));
      v.AddExtendedProperty("why", "http://because.net", new vCardTextProperty("It is red"));
      VerifyOutput(v, @"<?xml version=""1.0"" encoding=""utf-8""?>
<vcards xmlns=""urn:ietf:params:xml:ns:vcard-4.0"">
 <vcard>
  <fn>
   <text>John Doe</text>
  </fn>
  <ex:where xmlns:ex=""http://here.net"">
   <ex:uri>ldap://somefellow</ex:uri>
  </ex:where>
  <ex:why xmlns:ex=""http://because.net"">
   <ex:text>It is red</ex:text>
  </ex:why>
 </vcard>
</vcards>");
    }


    private void VerifyOutput(vCard c, string expectedOutput)
    {
      using (MemoryStream s = new MemoryStream())
      {
        vCardXmlWriter writer = new vCardXmlWriter();
        writer.Write(s, c);
        s.Seek(0, SeekOrigin.Begin);
        StreamReader reader = new StreamReader(s);
        string output = reader.ReadToEnd();
        Assert.AreEqual(expectedOutput, output);
      }
    }
  }
}
