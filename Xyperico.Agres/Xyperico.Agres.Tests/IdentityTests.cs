using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Xyperico.Agres.Contract;


namespace Xyperico.Agres.Tests
{
  [TestFixture]
  public class IdentityTests : TestHelper
  {
    [Test]
    public void CanCompareIdentities()
    {
      // Arrange
      LongAId id1 = new LongAId(1);
      LongAId id2 = new LongAId(1);
      LongAId id3 = new LongAId(2);

      // Assert
      Assert.IsTrue(id1 == id2);
      Assert.IsFalse(id1 != id2);
      Assert.IsFalse(id1 == id3);
    }


    [Test]
    public void DifferentTypesOfIdentitiesAreNotEqual()
    {
      // Arrange
      LongAId id1 = new LongAId(1);
      LongBId id2 = new LongBId(1);

      // Assert
      Assert.IsFalse(id1 == id2);
      Assert.IsTrue(id1 != id2);
    }


    [Test]
    public void ItIncludesPrefixInLiteral()
    {
      // Arrange
      LongAId id1 = new LongAId(3);
      LongBId id2 = new LongBId(4);

      // Assert
      Assert.AreEqual("A3", id1.Literal);
      Assert.AreEqual("B4", id2.Literal);
    }
  }


  class LongAId : Identity<long>
  {
    public LongAId(long id)
      : base(id)
    {
    }

    protected override string Prefix
    {
      get { return "A"; }
    }
  }


  class LongBId : Identity<long>
  {
    public LongBId(long id)
      : base(id)
    {
    }

    protected override string Prefix
    {
      get { return "B"; }
    }
  }
}
