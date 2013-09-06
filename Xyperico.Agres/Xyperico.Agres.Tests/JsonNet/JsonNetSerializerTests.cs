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
  }


  [TestFixture]
  class JsonNetSerializerTests_string : JsonNetSerializerTests<string>
  {
  }


  [TestFixture]
  class JsonNetSerializerTests_guid : JsonNetSerializerTests<Guid>
  {
  }
}
