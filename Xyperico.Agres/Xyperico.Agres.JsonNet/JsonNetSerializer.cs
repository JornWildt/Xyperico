using System.IO;
using System.Text;
using Newtonsoft.Json;
using Xyperico.Agres.Serialization;


namespace Xyperico.Agres.JsonNet
{
  public class JsonNetSerializer : ISerializer
  {
    JsonSerializer Serializer = new JsonSerializer() 
    { 
      TypeNameHandling = TypeNameHandling.Objects, 
      ContractResolver = new PrivateSetterContractResolver() 
    };


    public byte[] Serialize(object item)
    {
      using (var s = new MemoryStream())
      {
        using (var t = new StreamWriter(s, new UTF8Encoding(false)))
        using (var w = new JsonTextWriter(t))
        {
          Serializer.Serialize(w, item);
        }
        return s.ToArray();
      }
    }


    public void Serialize(Stream s, object item)
    {
      using (var t = new StreamWriter(s, new UTF8Encoding(false)))
      using (var w = new JsonTextWriter(t))
      {
        Serializer.Serialize(w, item);
      }
    }

    
    public object Deserialize(byte[] data)
    {
      using (var s = new MemoryStream(data))
      using (var t = new StreamReader(s, new UTF8Encoding(false)))
      using (var r = new JsonTextReader(t))
      {
        return Serializer.Deserialize(r);
      }
    }


    public object Deserialize(Stream s)
    {
      using (var t = new StreamReader(s, new UTF8Encoding(false)))
      using (var r = new JsonTextReader(t))
      {
        return Serializer.Deserialize(r);
      }
    }
  }
}
