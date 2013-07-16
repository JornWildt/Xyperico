using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Xyperico.Agres.Serialization;


namespace Xyperico.Agres.JsonNet
{
  public class BsonNetSerializer : ISerializer
  {
    JsonSerializer Serializer = new JsonSerializer() { TypeNameHandling = TypeNameHandling.Auto };


    public byte[] Serialize(object item)
    {
      using (var s = new MemoryStream())
      using (var w = new BsonWriter(s))
      {
        Serializer.Serialize(w, item);
        return s.ToArray();
      }
    }


    public void Serialize(Stream s, object item)
    {
      //Condition.Requires(s, "s").IsNotNull();
      //Condition.Requires(item, "item").IsNotNull();

      using (var w = new BsonWriter(s))
      {
        Serializer.Serialize(w, item);
      }
    }

    
    public object Deserialize(byte[] data)
    {
      using (var s = new MemoryStream())
      using (var r = new BsonReader(s))
      {
        return Serializer.Deserialize(r);
      }
    }


    public object Deserialize(Stream s)
    {
      using (var r = new BsonReader(s))
      {
        return Serializer.Deserialize(r);
      }
    }
  }
}
