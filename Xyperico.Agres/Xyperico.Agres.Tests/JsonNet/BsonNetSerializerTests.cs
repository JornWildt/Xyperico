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
  }


  [TestFixture]
  class BsonNetSerializerTests_string : BsonNetSerializerTests<string>
  {
  }


  [TestFixture]
  class BsonNetSerializerTests_guid : BsonNetSerializerTests<Guid>
  {
  }
}
