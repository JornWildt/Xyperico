using NUnit.Framework;
using Xyperico.Agres.DocumentStore;
using Xyperico.Agres.ProtoBuf;


namespace Xyperico.Agres.Tests.DocumentStore
{
  [TestFixture]
  public class ProtoBufFileDocumentStoreTests : AbstractDocumentStoreTests
  {
    protected override IDocumentStore<string, long> BuildDocumentStore_Int()
    {
      ProtoBufStreamSerializer serializer = new ProtoBufStreamSerializer();
      return new FileDocumentStore<string, long>(StorageBaseDir, serializer);
    }


    protected override IDocumentStore<string, MySerializableData> BuildDocumentStore_Class()
    {
      ProtoBufStreamSerializer serializer = new ProtoBufStreamSerializer();
      return new FileDocumentStore<string, MySerializableData>(StorageBaseDir, serializer);
    }
  }
}
