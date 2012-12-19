using System;
using NUnit.Framework;
using Xyperico.Web.Testing;


namespace Xyperico.Net.Tests.Tests
{
  [TestFixture]
  public class YadisTests : TestHelper
  {
    [Test]
    public void CanGetYadisDocument()
    {
      YadisRequest request = new YadisRequest(Utility.GetTestUri("yadisexamples/index"));
      XrdsDocument document = request.GetYadisDocument();
      
      Assert.IsNotNull(document);
      Assert.IsNotNull(document.XrdElements);
      Assert.AreEqual(1, document.XrdElements.Count);
      Assert.IsNotNull(document.XrdElements[0].Services);
      Assert.AreEqual(3, document.XrdElements[0].Services.Count);
      Assert.IsNotNull(document.XrdElements[0].Services[0].Types);
      Assert.AreEqual(5, document.XrdElements[0].Services[0].Types.Count);
      Assert.AreEqual("http://specs.openid.net/auth/2.0/signon", document.XrdElements[0].Services[0].Types[0]);
      Assert.AreEqual("http://jornwildt.myopenid.com/", document.XrdElements[0].Services[0].LocalID);
      Assert.IsNotNull(document.XrdElements[0].Services[0].Uris);
      Assert.AreEqual(1, document.XrdElements[0].Services[0].Uris.Count);
      Assert.AreEqual("http://www.myopenid.com/server", document.XrdElements[0].Services[0].Uris[0].Uri);
    }
  }
}
