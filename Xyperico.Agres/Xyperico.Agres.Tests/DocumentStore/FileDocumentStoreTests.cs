using NUnit.Framework;
using Xyperico.Agres.DocumentStore;
using Xyperico.Agres.JsonNet;


namespace Xyperico.Agres.Tests.DocumentStore
{
  [TestFixture]
  public class FileDocumentStoreTests : AbstractDocumentStoreTests
  {
    protected override IDocumentStore<string, long> BuildDocumentStore_Int()
    {
      IDocumentSerializer serializer = new JsonNetDocumentSerializer();
      return new FileDocumentStore<string, long>(StorageBaseDir, serializer);
    }


    protected override IDocumentStore<string, MySerializableData> BuildDocumentStore_Class()
    {
      IDocumentSerializer serializer = new JsonNetDocumentSerializer();
      return new FileDocumentStore<string, MySerializableData>(StorageBaseDir, serializer);
    }
  }
}
