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
  }


  [TestFixture]
  class DataContractSerializerTests_string : DataContractSerializerTests<string>
  {
  }


  [TestFixture]
  class DataContractSerializerTests_guid : DataContractSerializerTests<Guid>
  {
  }
}
