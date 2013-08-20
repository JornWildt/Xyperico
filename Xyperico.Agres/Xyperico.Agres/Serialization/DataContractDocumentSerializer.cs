using System;
using System.IO;
using Xyperico.Agres.DocumentStore;
using DCSerializer = System.Runtime.Serialization.DataContractSerializer;


namespace Xyperico.Agres.Serialization
{
  public class DataContractDocumentSerializer : IDocumentSerializer
  {
    void IDocumentSerializer.Serialize(Stream s, object item)
    {
      DCSerializer serializer = new DCSerializer(item.GetType());
      serializer.WriteObject(s, item);
    }


    object IDocumentSerializer.Deserialize(Type t, Stream s)
    {
      DCSerializer serializer = new DCSerializer(t);
      return serializer.ReadObject(s);
    }
  }
}
