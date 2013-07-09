using NUnit.Framework;
using Xyperico.Agres.DocumentStore;


namespace Xyperico.Agres.Tests.DocumentStore
{
  [TestFixture]
  public class FileDocumentRepositoryTests : AbstractDocumentRepositoryTests
  {
    protected override IDocumentStoreFactory BuildFactory()
    {
      IDocumentSerializer serializer = new DotNetBinaryDocumentSerializer();
      return new FileDocumentStoreFactory(StorageBaseDir, serializer);
    }
  }
}
