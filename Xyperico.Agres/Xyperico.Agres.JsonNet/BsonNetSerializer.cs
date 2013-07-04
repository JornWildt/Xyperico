using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;


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

    public object Deserialize(byte[] data)
    {
      using (var s = new MemoryStream())
      using (var r = new BsonReader(s))
      {
        return Serializer.Deserialize(r);
      }
    }
  }
}
