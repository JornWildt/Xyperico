using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace Xyperico.Agres.DocumentStore
{
  public class DotNetBinaryDocumentSerializer : IDocumentSerializer
  {
    protected BinaryFormatter Formatter { get; set; }


    public DotNetBinaryDocumentSerializer()
    {
      Formatter = new BinaryFormatter();
    }


    public void Serialize(Stream s, object item)
    {
      Formatter.Serialize(s, item);
    }

    
    public object Deserialize(Type t, Stream s)
    {
      return Formatter.Deserialize(s);
    }
  }
}
