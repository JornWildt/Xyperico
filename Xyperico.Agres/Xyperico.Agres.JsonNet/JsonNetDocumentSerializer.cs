using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Xyperico.Agres.DocumentStore;


namespace Xyperico.Agres.JsonNet
{
  public class JsonNetDocumentSerializer : IDocumentSerializer
  {
    JsonSerializer Serializer = new JsonSerializer();


    public void Serialize(Stream s, object item)
    {
      using (var tw = new StreamWriter(s, Encoding.UTF8))
      using (var jw = new JsonTextWriter(tw))
      {
        Serializer.Serialize(jw, item);
      }
    }


    public object Deserialize(Type t, Stream s)
    {
      using (var tr = new StreamReader(s, Encoding.UTF8))
      using (var jr = new JsonTextReader(tr))
      {
        return Serializer.Deserialize(jr, t);
      }
    }
  }
}
