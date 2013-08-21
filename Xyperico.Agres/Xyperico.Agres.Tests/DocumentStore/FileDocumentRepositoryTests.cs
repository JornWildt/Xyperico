using NUnit.Framework;
using Xyperico.Agres.DocumentStore;
using Xyperico.Agres.JsonNet;


namespace Xyperico.Agres.Tests.DocumentStore
{
  [TestFixture]
  public class FileDocumentRepositoryTests : AbstractDocumentRepositoryTests
  {
    protected override IDocumentStoreFactory BuildFactory()
    {
      IDocumentSerializer serializer = new JsonNetDocumentSerializer();
      return new FileDocumentStoreFactory(StorageBaseDir, serializer);
    }
  }
}
