using System;
using System.IO;
using Xyperico.Agres.DocumentStore;
using PBSerializer = ProtoBuf.Serializer.NonGeneric;


namespace Xyperico.Agres.ProtoBuf
{
  public class ProtoBufDocumentSerializer : IDocumentSerializer
  {
    public void Serialize(Stream s, object item)
    {
      PBSerializer.Serialize(s, item);
      // Make sure nothing from any previous serialization is left in the stream
      s.SetLength(s.Position);
    }


    public object Deserialize(Type t, Stream s)
    {
      return PBSerializer.Deserialize(t, s);
    }
  }
}
