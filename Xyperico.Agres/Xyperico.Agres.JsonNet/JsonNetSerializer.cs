using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System.Text;


namespace Xyperico.Agres.JsonNet
{
  public class JsonNetSerializer : ISerializer
  {
    JsonSerializer Serializer = new JsonSerializer() { TypeNameHandling = TypeNameHandling.Auto };


    public byte[] Serialize(object item)
    {
      using (var s = new MemoryStream())
      using (var t = new StreamWriter(s, Encoding.UTF8))
      using (var w = new JsonTextWriter(t))
      {
        Serializer.Serialize(w, item);
        return s.ToArray();
      }
    }


    public void Serialize(Stream s, object item)
    {
      using (var t = new StreamWriter(s, Encoding.UTF8))
      using (var w = new JsonTextWriter(t))
      {
        Serializer.Serialize(w, item);
      }
    }

    
    public object Deserialize(byte[] data)
    {
      using (var s = new MemoryStream())
      using (var t = new StreamReader(s, Encoding.UTF8))
      using (var r = new JsonTextReader(t))
      {
        return Serializer.Deserialize(r);
      }
    }


    public object Deserialize(Stream s)
    {
      using (var t = new StreamReader(s, Encoding.UTF8))
      using (var r = new JsonTextReader(t))
      {
        return Serializer.Deserialize(r);
      }
    }
  }
}
