using System;
using NUnit.Framework;
using Xyperico.Agres.ProtoBuf;
using ProtoBuf.Meta;


namespace Xyperico.Agres.Tests.Protobuf
{
  public abstract class ProtoBufSerializerTests<TId> : AbstractSerializerTests<TId> 
    where TId : IEquatable<TId>
  {
    protected override Serialization.ISerializer BuildSerializer()
    {
      return new ProtoBufSerializer();
    }
  }


  [TestFixture]
  class ProtoBufSerializerTests_long : ProtoBufSerializerTests<long>
  {
    protected override AbstractSerializerTests<long>.MyIdentity BuildId()
    {
      return new AbstractSerializerTests<long>.MyIdentity(12234L);
    }
  }


  [TestFixture]
  class ProtoBufSerializerTests_string : ProtoBufSerializerTests<string>
  {
    protected override AbstractSerializerTests<string>.MyIdentity BuildId()
    {
      return new AbstractSerializerTests<string>.MyIdentity("ABC");
    }
  }


  [TestFixture]
  class ProtoBufSerializerTests_guid : ProtoBufSerializerTests<Guid>
  {
    protected override AbstractSerializerTests<Guid>.MyIdentity BuildId()
    {
      return new AbstractSerializerTests<Guid>.MyIdentity(Guid.NewGuid());
    }
  }
}
