using System;
using NUnit.Framework;
using Xyperico.Agres.ProtoBuf;


namespace Xyperico.Agres.Tests.Protobuf
{
  public abstract class ProtoBufSerializerTests<TId> : AbstractSerializerTests<TId> 
    where TId : IEquatable<TId>
  {
    protected override Serialization.ISerializer BuildSerializer()
    {
      return new ProtoBufSerializer();
    }


    //[Test]
    //public void CanRegisterIdentityType()
    //{
    //  //Xyperico.Agres.ProtoBuf.SerializerSetup.RegisterIdentity(typeof(TId));
    //}
  }


  [TestFixture]
  class ProtoBufSerializerTests_long : ProtoBufSerializerTests<long>
  {
  }


  [TestFixture]
  class ProtoBufSerializerTests_string : ProtoBufSerializerTests<string>
  {
  }


  [TestFixture]
  class ProtoBufSerializerTests_guid : ProtoBufSerializerTests<Guid>
  {
  }
}
