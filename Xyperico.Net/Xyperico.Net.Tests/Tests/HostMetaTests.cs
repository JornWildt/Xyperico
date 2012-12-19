using System;
using NUnit.Framework;


namespace Xyperico.Net.Tests.Tests
{
  [TestFixture]
  public class HostMetaTests : TestHelper
  {
    [Test]
    public void CanMakeHostMetaTypedRequest()
    {
      HostMetaDocumentRequest request = new HostMetaDocumentRequest(TestHostname);
      HostMetaDocument hostMeta = request.GetDocument();
      Assert.IsNotNull(hostMeta);

      Assert.AreEqual("testserver.xyperico.dk", hostMeta.Host);
    }


    [Test]
    public void WhenGettingDocumentItValidatesParameters()
    {
      AssertThrows<ArgumentNullException>(() => new HostMetaDocumentRequest(null),
                   ex => ex.ParamName == "hostname");
    }
  }
}
