using System;
using System.IO;
using Xyperico.Agres.DocumentStore;
using DotNetXmlSerializer = System.Xml.Serialization.XmlSerializer;


namespace Xyperico.Agres.Serialization
{
  public class XmlDocumentSerializer : IDocumentSerializer
  {
    public void Serialize(Stream s, object item)
    {
      DotNetXmlSerializer serializer = new DotNetXmlSerializer(item.GetType());
      serializer.Serialize(s, item);
    }


    public object Deserialize(Type t, Stream s)
    {
      DotNetXmlSerializer serializer = new DotNetXmlSerializer(t);
      return serializer.Deserialize(s);
    }
  }
}
