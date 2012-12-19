using System.Linq;
using NUnit.Framework;
using Xyperico.Web.Testing;
using System.Net;
using System;


namespace Xyperico.Net.Tests.Tests
{
  [TestFixture]
  public class XrdDocumentTests : TestHelper
  {
    [Test]
    public void CanGetAndParseXrdDocument()
    {
      XrdDocument doc = XrdDocument.GetFromUrl(Utility.GetTestUri("xrdexamples/index"));
      Assert.IsNotNull(doc);

      Assert.AreEqual("testserver.xyperico.dk", doc.Subject);

      XrdProperty p1 = doc.Properties.First();
      Assert.AreEqual("http://test", p1.Type);
      Assert.AreEqual("Blah", p1.Value);

      XrdLink vCard = doc.Links.Where(l => l.Rel == "http://microformats/vCard").First();
      Assert.AreEqual("http://test/vCard", vCard.HRef);
      Assert.AreEqual("Host vCard", vCard.Title);

      XrdLink describedBy = doc.Links.Where(l => l.Rel == "lrdd").First();
      Assert.AreEqual("http://test?uri={uri}", describedBy.Template);
      Assert.AreEqual("Resource Template", describedBy.Title);
    }


    [Test]
    public void WhenGettingUnknownDocumentItThrows()
    {
      AssertThrows<WebException>(() => XrdDocument.GetFromUrl(Utility.GetTestUri("unknown")));
    }


    [Test]
    public void WhenGettingDocumentItValidatesParameters()
    {
      AssertThrows<ArgumentNullException>(() => XrdDocument.GetFromUrl((string)null),
                   ex => ex.ParamName == "url");
      AssertThrows<ArgumentNullException>(() => XrdDocument.GetFromUrl((Uri)null),
                   ex => ex.ParamName == "url");
    }
  }
}
