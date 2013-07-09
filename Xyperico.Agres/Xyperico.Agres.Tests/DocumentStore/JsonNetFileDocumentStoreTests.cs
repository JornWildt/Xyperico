using NUnit.Framework;
using Xyperico.Agres.DocumentStore;
using Xyperico.Agres.JsonNet;


namespace Xyperico.Agres.Tests.DocumentStore
{
  [TestFixture]
  public class JsonNetFileDocumentStoreTests : AbstractDocumentStoreTests
  {
    protected override IDocumentStore<string, long> BuildDocumentStore_Int()
    {
      JsonNetDocumentSerializer serializer = new JsonNetDocumentSerializer();
      return new FileDocumentStore<string, long>(StorageBaseDir, serializer);
    }


    protected override IDocumentStore<string, MySerializableData> BuildDocumentStore_Class()
    {
      JsonNetDocumentSerializer serializer = new JsonNetDocumentSerializer();
      return new FileDocumentStore<string, MySerializableData>(StorageBaseDir, serializer);
    }
  }
}
