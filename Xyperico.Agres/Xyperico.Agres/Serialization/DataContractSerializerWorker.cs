using System;
using System.IO;
using DCSerializer = System.Runtime.Serialization.DataContractSerializer;


namespace Xyperico.Agres.Serialization
{
  class DataContractSerializerWorker : ISerializeWorker
  {
    DCSerializer Serializer;

    
    public DataContractSerializerWorker(Type t)
    {
      Serializer = new DCSerializer(t);
    }

    
    public void Serialize(Stream s, object o)
    {
      Serializer.WriteObject(s, o);
    }

    
    public object Deserialize(Stream s)
    {
      return Serializer.ReadObject(s);
    }
  }
}
