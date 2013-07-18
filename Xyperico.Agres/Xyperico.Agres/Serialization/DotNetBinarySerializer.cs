using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using CuttingEdge.Conditions;


namespace Xyperico.Agres.Serialization
{
  public class DotNetBinarySerializer : ISerializer
  {
    protected BinaryFormatter Formatter { get; set; }


    public DotNetBinarySerializer()
    {
      Formatter = new BinaryFormatter();
    }


    public byte[] Serialize(object item)
    {
      Condition.Requires(item, "item").IsNotNull();

      using (MemoryStream s = new MemoryStream())
      {
        Formatter.Serialize(s, item);
        return s.ToArray();
      }
    }


    public void Serialize(Stream s, object item)
    {
      Condition.Requires(s, "s").IsNotNull();
      Condition.Requires(item, "item").IsNotNull();
      Formatter.Serialize(s, item);
    }


    public object Deserialize(byte[] data)
    {
      Condition.Requires(data, "data").IsNotNull();

      using (MemoryStream s = new MemoryStream(data))
      {
        return Formatter.Deserialize(s);
      }
    }


    public object Deserialize(Stream s)
    {
      Condition.Requires(s, "s").IsNotNull();
      return Formatter.Deserialize(s);
    }
  }
}
