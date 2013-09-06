using System;
using System.IO;
using System.Runtime.Serialization;
using NUnit.Framework;
using Xyperico.Agres.Serialization;


namespace Xyperico.Agres.Tests
{
  public abstract class AbstractSerializerTests<TId> : TestHelper where TId : IEquatable<TId>
  {
    [DataContract]
    class MyIdentity : Identity<TId> 
    {
      protected override string Prefix
      {
        get { return "X"; }
      }
    }


    [DataContract]
    public class MySerializationMessage<MyIdentity>
    {
      [DataMember(Order=1)]
      public MyIdentity Id { get; private set; }

      [DataMember(Order = 2)]
      public string Title { get; private set; }

      [DataMember(Order = 3)]
      public string Description { get; private set; }


      public MySerializationMessage() { }


      public MySerializationMessage(MyIdentity id, string title, string description)
      {
        Id = id;
        Title = title;
        Description = description;
      }
    }

    protected abstract ISerializer BuildSerializer();

    protected ISerializer Serializer { get; set; }


    protected override void TestFixtureSetUp()
    {
      base.TestFixtureSetUp();
      AbstractSerializer.RegisterKnownType(typeof(MyIdentity));
      AbstractSerializer.RegisterKnownType(typeof(MySerializationMessage<MyIdentity>));
    }


    protected override void SetUp()
    {
      base.SetUp();
      Serializer = BuildSerializer();
    }


    [Test]
    public void CanWriteReadByteArray()
    {
      // Arrange
      MySerializationMessage<MyIdentity> msg = new MySerializationMessage<MyIdentity>(new MyIdentity(), "Blah", "Blah blah ...");

      // Act
      byte[] data = Serializer.Serialize(msg);
      Assert.IsNotNull(data);
      Assert.Greater(data.Length, 0);

      MySerializationMessage<MyIdentity> result = (MySerializationMessage<MyIdentity>)Serializer.Deserialize(data);

      // Assert
      Assert.IsNotNull(result);
      Assert.IsNotNull(result.Id);
      Assert.IsNotNull(result.Title);
      Assert.IsNotNull(result.Description);
      Assert.AreEqual(msg.Id.Literal, result.Id.Literal);
      Assert.AreEqual(msg.Id, result.Id);
      Assert.AreEqual(msg.Title, result.Title);
      Assert.AreEqual(msg.Description, result.Description);
    }


    [Test]
    public void CanWriteReadStream()
    {
      // Arrange
      MySerializationMessage<MyIdentity> msg = new MySerializationMessage<MyIdentity>(new MyIdentity(), "Blah", "Blah blah ...");

      // Act
      using (var s1 = new MemoryStream())
      {
        Serializer.Serialize(s1, msg);
        using (var s2 = new MemoryStream(s1.ToArray()))
        {
          MySerializationMessage<MyIdentity> result = (MySerializationMessage<MyIdentity>)Serializer.Deserialize(s2);

          // Assert
          Assert.IsNotNull(result);
          Assert.IsNotNull(result.Id);
          Assert.IsNotNull(result.Title);
          Assert.IsNotNull(result.Description);
          Assert.AreEqual(msg.Id, result.Id);
          Assert.AreEqual(msg.Title, result.Title);
          Assert.AreEqual(msg.Description, result.Description);
        }
      }
    }
  }
}
