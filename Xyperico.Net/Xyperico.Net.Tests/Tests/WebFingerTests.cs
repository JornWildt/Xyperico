using NUnit.Framework;
using System.Net;
using Xyperico.Net.WebFinger;
using System;
using System.Xml;


namespace Xyperico.Net.Tests.Tests
{
  [TestFixture]
  public class WebFingerTests : TestHelper
  {
    [Test]
    public void CanMakeWebFingerRequest()
    {
      string email = "someuser@" + TestHostname;
      WebFingerRequest request = new WebFingerRequest(email);
      XrdDocument document = request.GetAccountDocument();
      Assert.AreEqual("http://got.it/acct:" + email, document.Aliases[0]);
    }


    [Test]
    public void WhenCreatingWebFingerRequestItValidatesParameters()
    {
      AssertThrows<ArgumentNullException>(() => new WebFingerRequest(null),
                   ex => ex.ParamName == "email");
      AssertThrows<ArgumentException>(() => new WebFingerRequest(""),
                                      ex => ex.ParamName == "email");
      AssertThrows<ArgumentException>(() => new WebFingerRequest("x"),
                                      ex => ex.ParamName == "email");
      AssertThrows<ArgumentException>(() => new WebFingerRequest("@"),
                                      ex => ex.ParamName == "email");
      AssertThrows<ArgumentException>(() => new WebFingerRequest("a@"),
                                      ex => ex.ParamName == "email");
      AssertThrows<ArgumentException>(() => new WebFingerRequest("@a"),
                                      ex => ex.ParamName == "email");
    }
  }
}
