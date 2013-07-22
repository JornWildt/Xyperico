using System;
using NUnit.Framework;
using Xyperico.Agres.DocumentStore;


namespace Xyperico.Agres.Tests.DocumentStore
{
  public abstract class AbstractDocumentRepositoryTests : TestHelper
  {
    IDocumentStore<string, MyStoreData> MyDataStore;
    DocumentRepository Repository;


    protected override void SetUp()
    {
      base.SetUp();
      MyDataStore = BuildFactory().Create<string, MyStoreData>();
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


    [Test]
    public void CanClearDocumentStore()
    {
      // Arrange
      MyDataStore.Put("A", new MyStoreData { X = 1 });
      MyDataStore.Put("B", new MyStoreData { X = 2 });
      MyStoreData s11 = MyDataStore.Get("A");
      MyStoreData s12 = MyDataStore.Get("B");

      // Act
      MyDataStore.Clear();
      MyStoreData s21 = MyDataStore.Get("A");
      MyStoreData s22 = MyDataStore.Get("B");

      // Assert
      Assert.IsNotNull(s11);
      Assert.IsNotNull(s12);
      Assert.AreEqual(1, s11.X);
      Assert.AreEqual(2, s12.X);
      Assert.IsNull(s21);
      Assert.IsNull(s22);
    }


    [Serializable]
    class MyStoreData
    {
      public int X { get; set; }
    }
  }
}
