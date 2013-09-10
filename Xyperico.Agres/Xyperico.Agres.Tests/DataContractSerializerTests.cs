using System;
using NUnit.Framework;
using Xyperico.Agres.Serialization;


namespace Xyperico.Agres.Tests.DataContract
{
  public abstract class DataContractSerializerTests<TId> : AbstractSerializerTests<TId> 
    where TId : IEquatable<TId>
  {
    protected override Serialization.ISerializer BuildSerializer()
    {
      return new DataContractSerializer();
    }
  }


  [TestFixture]
  class DataContractSerializerTests_long : DataContractSerializerTests<long>
  {
    protected override AbstractSerializerTests<long>.MyIdentity BuildId()
    {
      return new AbstractSerializerTests<long>.MyIdentity(12234L);
    }
  }


  [TestFixture]
  class DataContractSerializerTests_string : DataContractSerializerTests<string>
  {
    protected override AbstractSerializerTests<string>.MyIdentity BuildId()
    {
      return new AbstractSerializerTests<string>.MyIdentity("ABC");
    }
  }


  [TestFixture]
  class DataContractSerializerTests_guid : DataContractSerializerTests<Guid>
  {
    protected override AbstractSerializerTests<Guid>.MyIdentity BuildId()
    {
      return new AbstractSerializerTests<Guid>.MyIdentity(Guid.NewGuid());
    }
  }
}
