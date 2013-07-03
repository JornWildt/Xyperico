using NUnit.Framework;
using Xyperico.Agres.DocumentStore;


namespace Xyperico.Agres.Tests.DocumentStore
{
  public abstract class AbstractDocumentStoreTests : TestHelper
  {
    protected IDocumentStore<string, long> DocumentStore { get; set; }

    
    protected override void SetUp()
    {
      base.SetUp();
      DocumentStore = BuildDocumentStore();
    }


    protected abstract IDocumentStore<string, long> BuildDocumentStore();


    [Test]
    public void CanPutGetAndDeleteItem()
    {
      // Act
      DocumentStore.Put("abc", 102030);
      
      long result1;
      bool ok1 = DocumentStore.TryGet("abc", out result1);

      DocumentStore.TryDelete("abc");

      long result2;
      bool ok2 = DocumentStore.TryGet("abc", out result2);

      // Assert
      Assert.IsTrue(ok1);
      Assert.AreEqual(result1, 102030);
      Assert.IsFalse(ok2);
      Assert.AreEqual(result2, 0);
    }
  }
}
