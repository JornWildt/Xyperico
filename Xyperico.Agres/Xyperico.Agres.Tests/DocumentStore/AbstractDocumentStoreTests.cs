using NUnit.Framework;
using Xyperico.Agres.DocumentStore;
using System;
using ProtoBuf;


namespace Xyperico.Agres.Tests.DocumentStore
{
  public abstract class AbstractDocumentStoreTests : TestHelper
  {
    protected IDocumentStore<string, long> DocumentStore_Int { get; set; }
    protected IDocumentStore<string, MySerializableData> DocumentStore_Class { get; set; }

    
    protected override void SetUp()
    {
      base.SetUp();
      DocumentStore_Int = BuildDocumentStore_Int();
      DocumentStore_Class = BuildDocumentStore_Class();
    }


    protected abstract IDocumentStore<string, long> BuildDocumentStore_Int();
    protected abstract IDocumentStore<string, MySerializableData> BuildDocumentStore_Class();


    [Test]
    public void CanPutGetAndDeletePrimitiveType()
    {
      // Act
      DocumentStore_Int.Put("abc", 102030);
      
      long result1;
      bool ok1 = DocumentStore_Int.TryGet("abc", out result1);

      DocumentStore_Int.TryDelete("abc");

      long result2;
      bool ok2 = DocumentStore_Int.TryGet("abc", out result2);

      // Assert
      Assert.IsTrue(ok1);
      Assert.AreEqual(102030, result1);
      Assert.IsFalse(ok2);
      Assert.AreEqual(0, result2);
    }


    [Test]
    public void CanPutGetAndDeleteClass()
    {
      // Arrange
      MySerializableData data = new MySerializableData { Value = 15 };

      // Act
      DocumentStore_Class.Put("abc", data);

      MySerializableData result1;
      bool ok1 = DocumentStore_Class.TryGet("abc", out result1);

      DocumentStore_Class.TryDelete("abc");

      MySerializableData result2;
      bool ok2 = DocumentStore_Class.TryGet("abc", out result2);

      // Assert
      Assert.IsTrue(ok1);
      Assert.AreEqual(data.Value, result1.Value);
      Assert.IsFalse(ok2);
      Assert.IsNull(result2);
    }


    [Serializable]
    [ProtoContract]
    public class MySerializableData
    {
      [ProtoMember(1)]
      public long Value { get; set; }
    }
  }
}
