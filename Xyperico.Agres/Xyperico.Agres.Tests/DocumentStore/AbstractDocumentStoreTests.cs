using System;
using NUnit.Framework;
using ProtoBuf;
using Xyperico.Agres.DocumentStore;


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

    protected virtual bool EnablePrimitiveTypeTests { get { return true; } }

    protected abstract IDocumentStore<string, long> BuildDocumentStore_Int();
    protected abstract IDocumentStore<string, MySerializableData> BuildDocumentStore_Class();


    [Test]
    public void CanPutGetAndDeletePrimitiveType()
    {
      if (!EnablePrimitiveTypeTests)
        Assert.Pass();

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
      MySerializableData data = new MySerializableData { Value = "15" };

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


    [Test]
    public void ItCanHandleWritingAndReadingDifferentSizesOfDataToTheSameDocument()
    {
      // Arrange
      MySerializableData data1 = new MySerializableData { Value = "Abcdefghijklmn" };
      MySerializableData data2 = new MySerializableData { Value = "Xyz" };

      // Act
      DocumentStore_Class.Put("rwxyz", data1);
      MySerializableData result1 = DocumentStore_Class.Get("rwxyz");
      
      DocumentStore_Class.Put("rwxyz", data2);
      MySerializableData result2 = DocumentStore_Class.Get("rwxyz");

      // Assert
      Assert.AreEqual(data1.Value, result1.Value);
      Assert.AreEqual(data2.Value, result2.Value);
    }


    [Serializable]
    [ProtoContract]
    public class MySerializableData
    {
      [ProtoMember(1)]
      public string Value { get; set; }
    }
  }
}
