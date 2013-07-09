using NUnit.Framework;
using Xyperico.Agres.DocumentStore;


namespace Xyperico.Agres.Tests.DocumentStore
{
  [TestFixture]
  public class FileDocumentStoreTests : AbstractDocumentStoreTests
  {
    protected override IDocumentStore<string, long> BuildDocumentStore_Int()
    {
      DotNetBinaryDocumentSerializer serializer = new DotNetBinaryDocumentSerializer();
      return new FileDocumentStore<string, long>(StorageBaseDir, serializer);
    }


    protected override IDocumentStore<string, MySerializableData> BuildDocumentStore_Class()
    {
      DotNetBinaryDocumentSerializer serializer = new DotNetBinaryDocumentSerializer();
      return new FileDocumentStore<string, MySerializableData>(StorageBaseDir, serializer);
    }
  }
}
