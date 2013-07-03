using NUnit.Framework;
using Xyperico.Agres.DocumentStore;


namespace Xyperico.Agres.Tests.DocumentStore
{
  [TestFixture]
  public class FileDocumentStoreTests : AbstractDocumentStoreTests
  {
    protected override IDocumentStore<string, long> BuildDocumentStore()
    {
      DotNetBinaryStreamSerializer serializer = new DotNetBinaryStreamSerializer();
      return new FileDocumentStore<string, long>(StorageBaseDir, serializer);
    }
  }
}
