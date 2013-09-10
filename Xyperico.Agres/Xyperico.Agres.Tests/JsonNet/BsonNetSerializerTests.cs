using System;
using NUnit.Framework;
using Xyperico.Agres.JsonNet;


namespace Xyperico.Agres.Tests.JsonNet
{
  public abstract class BsonNetSerializerTests<TId> : AbstractSerializerTests<TId> 
    where TId : IEquatable<TId>
  {
    protected override Serialization.ISerializer BuildSerializer()
    {
      return new BsonNetSerializer();
    }
  }


  [TestFixture]
  class BsonNetSerializerTests_long : BsonNetSerializerTests<long>
  {
    protected override AbstractSerializerTests<long>.MyIdentity BuildId()
    {
      return new AbstractSerializerTests<long>.MyIdentity(12234L);
    }
  }


  [TestFixture]
  class BsonNetSerializerTests_string : BsonNetSerializerTests<string>
  {
    protected override AbstractSerializerTests<string>.MyIdentity BuildId()
    {
      return new AbstractSerializerTests<string>.MyIdentity("ABC");
    }
  }


  [TestFixture]
  class BsonNetSerializerTests_guid : BsonNetSerializerTests<Guid>
  {
    protected override AbstractSerializerTests<Guid>.MyIdentity BuildId()
    {
      return new AbstractSerializerTests<Guid>.MyIdentity(Guid.NewGuid());
    }
  }
}
