using NUnit.Framework;
using Xyperico.Base.CommonDomainTypes;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace Xyperico.Base.Tests.CommonDomainTypes
{
  [TestFixture]
  public class PasswordTests : TestHelper
  {
    [Test]
    public void CanCheckPassword()
    {
      Password p = new Password("1", "alibalibi");
      Assert.IsTrue(p.Matches("1", "alibalibi"));
      Assert.IsFalse(p.Matches("1", "alibalibi1"));
      Assert.IsFalse(p.Matches("1", " alibalibi"));
    }


    [Test]
    public void CanCompare()
    {
      Password p1a = new Password("1", "alibalibi");
      Password p1b = new Password("1", "alibalibi");
      Password p2 = new Password("1", "qwerty");

      Assert.AreEqual(p1a, p1b);
      Assert.AreNotEqual(p1a, p2);
    }


    [Test]
    public void ErrorChecks()
    {
      Password p = new Password("1", "alibalibi");
      AssertThrows<ArgumentNullException>(() => new Password(null, null));
      AssertThrows<ArgumentNullException>(() => p.Matches(null, "11"));
      AssertThrows<ArgumentNullException>(() => p.Matches("1", null));
    }


    [Test]
    public void CanSerialize()
    {
      IFormatter formatter = new BinaryFormatter();
      Password p1 = new Password("1", "alibalibi");
      using (MemoryStream s = new MemoryStream())
      {
        formatter.Serialize(s, p1);
        s.Seek(0, SeekOrigin.Begin);

        Password p2 = (Password)formatter.Deserialize(s);

        Assert.IsTrue(p2.Matches("1", "alibalibi"));
        Assert.IsFalse(p2.Matches("1", "alibalibi1"));
      }
    }
  }
}
