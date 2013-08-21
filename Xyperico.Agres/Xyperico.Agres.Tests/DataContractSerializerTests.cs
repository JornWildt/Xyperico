using NUnit.Framework;
using Xyperico.Agres.Serialization;


namespace Xyperico.Agres.Tests
{
  [TestFixture]
  public class DataContractSerializerTests : AbstractSerializerTests
  {
    protected override Serialization.ISerializer BuildSerializer()
    {
      return new DataContractSerializer();
    }
  }
}
