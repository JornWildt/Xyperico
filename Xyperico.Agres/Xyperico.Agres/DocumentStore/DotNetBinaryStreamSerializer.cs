using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace Xyperico.Agres.DocumentStore
{
  public class DotNetBinaryStreamSerializer : IStreamSerializer
  {
    protected BinaryFormatter Formatter { get; set; }


    public DotNetBinaryStreamSerializer()
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
