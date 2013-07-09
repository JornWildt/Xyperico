using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Xyperico.Agres.DocumentStore;


namespace Xyperico.Agres.JsonNet
{
  public class BsonNetDocumentSerializer : IDocumentSerializer
  {
    JsonSerializer Serializer = new JsonSerializer();// { TypeNameHandling = TypeNameHandling.Auto };


    public void Serialize(Stream s, object item)
    {
      using (var jw = new BsonWriter(s))
      {
        Serializer.Serialize(jw, item);
      }
    }


    public object Deserialize(Type t, Stream s)
    {
      using (var jr = new BsonReader(s))
      {
        return Serializer.Deserialize(jr, t);
      }
    }
  }
}
