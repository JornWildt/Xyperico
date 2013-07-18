using NUnit.Framework;
using Xyperico.Agres.Serialization;


namespace Xyperico.Agres.Tests
{
  [TestFixture]
  public class DotNetBinarySerializerTests : AbstractSerializerTests
  {
    protected override Serialization.ISerializer BuildSerializer()
    {
      return new DotNetBinarySerializer();
    }
  }
}
