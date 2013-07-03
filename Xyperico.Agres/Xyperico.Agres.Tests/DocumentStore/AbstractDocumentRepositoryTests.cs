using NUnit.Framework;
using Xyperico.Agres.DocumentStore;


namespace Xyperico.Agres.Tests.DocumentStore
{
  public abstract class AbstractDocumentRepositoryTests : TestHelper
  {
    DocumentRepository Repository;


    protected override void SetUp()
    {
      base.SetUp();
      Repository = new DocumentRepository(BuildFactory());
    }


    protected abstract IDocumentStoreFactory BuildFactory();


    [Test]
    public void CanPutGetDeleteSingleton()
    {
      // Act
      Repository.PutSingleton(101112L);

      long result1;
      bool ok1 = Repository.TryGetSingleton(out result1);

      bool ok2 = Repository.TryDeleteSingleton<long>();

      long result2;
      bool ok3 = Repository.TryGetSingleton(out result2);

      bool ok4 = Repository.TryDeleteSingleton<long>();

      // Assert
      Assert.IsTrue(ok1);
      Assert.AreEqual(101112L, result1);
      Assert.IsTrue(ok2);
      Assert.IsFalse(ok3);
      Assert.AreEqual(0L, result2);
      Assert.IsFalse(ok4);
    }
  }
}
