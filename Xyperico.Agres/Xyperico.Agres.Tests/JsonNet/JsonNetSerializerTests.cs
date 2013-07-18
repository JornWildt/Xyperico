using NUnit.Framework;
using Xyperico.Agres.JsonNet;


namespace Xyperico.Agres.Tests.JsonNet
{
  [TestFixture]
  public class JsonNetSerializerTests : AbstractSerializerTests
  {
    protected override Serialization.ISerializer BuildSerializer()
    {
      return new JsonNetSerializer();
    }
  }
}
