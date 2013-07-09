using NUnit.Framework;
using Xyperico.Agres.DocumentStore;
using Xyperico.Agres.JsonNet;


namespace Xyperico.Agres.Tests.DocumentStore
{
  [TestFixture]
  public class BsonNetFileDocumentStoreTests : AbstractDocumentStoreTests
  {
    protected override bool EnablePrimitiveTypeTests { get { return false; } }


    protected override IDocumentStore<string, long> BuildDocumentStore_Int()
    {
      BsonNetDocumentSerializer serializer = new BsonNetDocumentSerializer();
      return new FileDocumentStore<string, long>(StorageBaseDir, serializer);
    }


    protected override IDocumentStore<string, MySerializableData> BuildDocumentStore_Class()
    {
      BsonNetDocumentSerializer serializer = new BsonNetDocumentSerializer();
      return new FileDocumentStore<string, MySerializableData>(StorageBaseDir, serializer);
    }
  }
}
