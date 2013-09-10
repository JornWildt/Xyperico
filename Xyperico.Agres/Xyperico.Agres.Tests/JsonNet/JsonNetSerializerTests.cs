using System;
using NUnit.Framework;
using Xyperico.Agres.JsonNet;


namespace Xyperico.Agres.Tests.JsonNet
{
  public abstract class JsonNetSerializerTests<TId> : AbstractSerializerTests<TId> 
    where TId : IEquatable<TId>
  {
    protected override Serialization.ISerializer BuildSerializer()
    {
      return new JsonNetSerializer();
    }
  }


  [TestFixture]
  class JsonNetSerializerTests_long : JsonNetSerializerTests<long>
  {
    protected override AbstractSerializerTests<long>.MyIdentity BuildId()
    {
      return new AbstractSerializerTests<long>.MyIdentity(12234L);
    }
  }


  [TestFixture]
  class JsonNetSerializerTests_string : JsonNetSerializerTests<string>
  {
    protected override AbstractSerializerTests<string>.MyIdentity BuildId()
    {
      return new AbstractSerializerTests<string>.MyIdentity("ABC");
    }
  }


  [TestFixture]
  class JsonNetSerializerTests_guid : JsonNetSerializerTests<Guid>
  {
    protected override AbstractSerializerTests<Guid>.MyIdentity BuildId()
    {
      return new AbstractSerializerTests<Guid>.MyIdentity(Guid.NewGuid());
    }
  }
}
