using NUnit.Framework;
using Xyperico.Agres.ProtoBuf;


namespace Xyperico.Agres.Tests
{
  [TestFixture]
  public class ProtoBufSerializerTests : AbstractSerializerTests
  {
    protected override Serialization.ISerializer BuildSerializer()
    {
      return new ProtoBufSerializer();
    }
  }
}
