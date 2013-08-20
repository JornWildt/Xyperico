using System;
using System.IO;
using DotNetXmlSerializer = System.Xml.Serialization.XmlSerializer;


namespace Xyperico.Agres.Serialization
{
  public class XmlSerializerWorker : ISerializeWorker
  {
    DotNetXmlSerializer Serializer;


    public XmlSerializerWorker(Type t)
    {
      Serializer = new DotNetXmlSerializer(t);
    }


    public void Serialize(Stream s, object o)
    {
      Serializer.Serialize(s, o);
    }

    
    public object Deserialize(Stream s)
    {
      return Serializer.Deserialize(s);
    }
  }
}
