using System;
using System.IO;
using PBSerializer = ProtoBuf.Serializer.NonGeneric;


namespace Xyperico.Agres.ProtoBuf
{
  class ProtoBufSerializerWorker : ISerializeWorker
  {
    Type ObjectType;

    public ProtoBufSerializerWorker(Type t)
    {
      ObjectType = t;
    }

    public void Serialize(Stream s, object o)
    {
      PBSerializer.Serialize(s, o);
    }

    public object Deserialize(Stream s)
    {
      return PBSerializer.Deserialize(ObjectType, s);
    }
  }
}
