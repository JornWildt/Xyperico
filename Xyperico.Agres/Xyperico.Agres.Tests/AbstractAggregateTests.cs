using System;
using NUnit.Framework;


namespace Xyperico.Agres.Tests
{
  [TestFixture]
  public class AbstractAggregateTests : TestHelper
  {
    [Test]
    public void WhenNoTypesAreRegisteredInSerializerItThrowsOnSerialization()
    {
      AbstractSerializer s = new MySerializer();
      AssertThrows<InvalidOperationException>(() => s.Serialize(new { }));
    }


    [Test]
    public void WhenSerializingUnknownTypeItThrowsInvalidOperation()
    {
      AbstractSerializer.RegisterKnownType(typeof(object));

      AbstractSerializer s = new MySerializer();
      AssertThrows<InvalidOperationException>(() => s.Serialize(new { }));
    }


    class MySerializer : AbstractSerializer
    {
      protected override ISerializeWorker CreateWorker(Type t)
      {
        return null;
      }
    }
  }
}
