using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;
using Xyperico.Base.CommonDomainTypes;


namespace Xyperico.Base.Tests.CommonDomainTypes
{
  [TestFixture]
  public class EMailTests : TestHelper
  {
    [Test]
    public void CanCreateAndDisplayEMail()
    {
      EMail e1 = new EMail("abc@def.eu");
      Assert.AreEqual("abc@def.eu", e1.ToString());
    }


    [Test]
    public void CanCompare()
    {
      EMail e1a = new EMail("abc@def.eu");
      EMail e1b = new EMail("abc@def.eu");
      EMail e2 = new EMail("lkj@oi.dk");

      Assert.AreEqual(e1a, e1b);
      Assert.AreNotEqual(e1a, e2);
    }


    [Test]
    public void ErrorChecks()
    {
      AssertThrows<ArgumentNullException>(() => new EMail(null));
      AssertThrows<ArgumentException>(() => new EMail(""));
      AssertThrows<ArgumentException>(() => new EMail("lkjl"));
      AssertThrows<ArgumentException>(() => new EMail("@"));
      AssertThrows<ArgumentException>(() => new EMail("a@"));
      AssertThrows<ArgumentException>(() => new EMail("@b"));
    }


    [Test]
    public void CanSerialize()
    {
      IFormatter formatter = new BinaryFormatter();
      EMail p1 = new EMail("qqq@www");
      using (MemoryStream s = new MemoryStream())
      {
        formatter.Serialize(s, p1);
        s.Seek(0, SeekOrigin.Begin);

        EMail p2 = (EMail)formatter.Deserialize(s);

        Assert.AreEqual(p1, p2);
      }
    }
  }
}
