using System.IO;
using NUnit.Framework;
using Xyperico.Base.StandardFormats.vCards;
using System.Xml;
using System.Text;

namespace Xyperico.Base.StandardFormats.Tests.vCards
{
  [TestFixture]
  public class hCardReaderTests
  {
    // Missing tests
    // - organization-name and units
    // - Variationer over <abbr title="..."> med og uden title=""
    // - Embedded vcards
    // - Name
    // - Photo
    // - Logo


    [Test]
    public void CanReadOrganizationHCard()
    {
      // This example card contains quite a variety of properties, so being able to parse is seems like a good place to start
      string hCard = @"
<html>
<head>
  <title>CommerceNet Title</title>
</head>
<body>
  <div class=""vcard"">
    <a class=""fn org url"" href=""http://www.commerce.net/"">CommerceNet</a>
    <div class=""tel""><span class=""type"">Fax</span>+1-650-289-4041</div>
    <div class=""adr"">
      <span class=""type"">Work</span>:
      <div class=""street-address"">169 University Avenue</div>
      <span class=""locality"">Palo Alto</span>,  
      <abbr class=""region"" title=""California"">CA</abbr>&nbsp;&nbsp;
      <span class=""postal-code"">94301</span>
      <div class=""country-name"">USA</div>
    </div>
    <div class=""tel""><span class=""type"">Work</span>+1-650-289-4040</div>
    <div>Email: 
     <span class=""email"">info@commerce.net</span>
    </div>
    <img class=""logo"" src=""http://www.elfisk.dk/image.png""/>
  </div>
</body>
</html>";

      using (StringReader r = new StringReader(hCard))
      {
        hCardReader reader = new hCardReader();
        vCard c = reader.Read(r);
        Assert.IsNotNull(c);
        Assert.AreEqual("CommerceNet Title", c.Name);
        Assert.AreEqual("org", c.Kind);
        Assert.AreEqual("CommerceNet", c.Fn.Default.Value);
        Assert.AreEqual("CommerceNet", c.Org.Default.Name);
        Assert.AreEqual("http://www.commerce.net/", c.Url.Default.Value);
        Assert.AreEqual("Work", c.Adr.Default.Type);
        Assert.AreEqual("169 University Avenue", c.Adr.Default.StreetAddress);
        Assert.AreEqual("Palo Alto", c.Adr.Default.Locality);
        Assert.AreEqual("California", c.Adr.Default.Region);
        Assert.AreEqual("94301", c.Adr.Default.PostalCode);
        Assert.AreEqual("USA", c.Adr.Default.Country);
        Assert.AreEqual("Work", c.Tel["Work"].Type);
        Assert.AreEqual("+1-650-289-4040", c.Tel["Work"].Value);
        Assert.AreEqual("Fax", c.Tel["Fax"].Type);
        Assert.AreEqual("+1-650-289-4041", c.Tel["Fax"].Value);
        Assert.AreEqual("info@commerce.net", c.EMail.Default.Value);
        Assert.AreEqual("http://www.elfisk.dk/image.png", c.Logo.Default.Source);
      }
    }


    [Test]
    public void CanReadPersonHCard()
    {
      string hCard = @"
<div class=""vcard"">
  <div class=""fn""><span class=""value"">John Petersen</span></div>
  <div class=""n"">
    <span class=""family-name"">Stevenson</span>
    <span class=""given-name"">John Phillip</span>
    <span class=""honorific-prefix"">Dr.</span>
    <span class=""honorific-suffix"">Jr.</span>
  </div>
  <div class=""nickname"">Johny<span>Not used</span> Johnson</div>
  <div class=""sex"">Male</div>
  <img class=""photo"" src=""http://www.elfisk.dk/image.png""/>
  <div class=""note""><span class=""type"">about</span>About me blah ...</div>
</div>
";
      using (StringReader r = new StringReader(hCard))
      {
        hCardReader reader = new hCardReader();
        vCard c = reader.Read(r);
        Assert.IsNotNull(c);
        Assert.AreEqual("individual", c.Kind);
        Assert.AreEqual("John Petersen", c.Fn.Default.Value);
        Assert.AreEqual("Johny Johnson", c.Nickname.Default.Value);
        Assert.AreEqual(vCardSex.Male, c.Sex);
        Assert.AreEqual("http://www.elfisk.dk/image.png", c.Photo.Default.Source);
        Assert.AreEqual("About me blah ...", c.Note["about"].Value);
        Assert.AreEqual("Stevenson", c.N.FamilyName);
        Assert.AreEqual("John Phillip", c.N.GivenNames);
        Assert.AreEqual("Dr.", c.N.HonorificPrefixes);
        Assert.AreEqual("Jr.", c.N.HonorificSuffixes);
      }
    }


    [Test]
    public void CanReadImpliedNOptimization()
    {
      string hCard = @"
<div class=""vcard"">
  <div class=""fn"">Maria Dal</div>
</div>
";
      using (StringReader r = new StringReader(hCard))
      {
        hCardReader reader = new hCardReader();
        vCard c = reader.Read(r);
        Assert.IsNotNull(c);
        Assert.AreEqual("Maria Dal", c.Fn.Default.Value);
        Assert.AreEqual("Maria", c.N.GivenNames);
        Assert.AreEqual("Dal", c.N.FamilyName);
      }
    }


    [Test]
    public void CanReadKind()
    {
      string hCard = @"
<div class=""vcard"">
  <div class=""kind"">thing</div>
</div>
";
      using (StringReader r = new StringReader(hCard))
      {
        hCardReader reader = new hCardReader();
        vCard c = reader.Read(r);
        Assert.IsNotNull(c);
        Assert.AreEqual("thing", c.Kind);
      }
    }


    [Test]
    public void CanReadMultipleValuesInOneProperty()
    {
      string hCard = @"
<div class=""vcard"">
  <div class=""fn""><span class=""value"">Mr. </span> unused <span class=""value"">Joshua </span><span class=""value"">Nelson</span></div>
</div>
";
      using (StringReader r = new StringReader(hCard))
      {
        hCardReader reader = new hCardReader();
        vCard c = reader.Read(r);
        Assert.IsNotNull(c);
        Assert.AreEqual("Mr. Joshua Nelson", c.Fn.Default.Value);
      }
    }
  }
}
