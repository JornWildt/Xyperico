using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using CuttingEdge.Conditions;


namespace Xyperico.Agres.Serializer
{
  public class DotNetBinaryFormaterSerializer : ISerializer
  {
    protected BinaryFormatter Formatter { get; set; }


    public DotNetBinaryFormaterSerializer()
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


    public object Deserialize(byte[] data)
    {
      Condition.Requires(data, "data").IsNotNull();

      using (MemoryStream s = new MemoryStream(data))
      {
        return Formatter.Deserialize(s);
      }
    }
  }
}
