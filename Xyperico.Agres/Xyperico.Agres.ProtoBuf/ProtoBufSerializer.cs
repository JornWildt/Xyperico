using System;
using System.IO;


namespace Xyperico.Agres.ProtoBuf
{
#if true

  public class ProtoBufSerializer : AbstractSerializer
  {
    protected override ISerializeWorker CreateWorker(Type t)
    {
      return new ProtoBufSerializerWorker(t);
    }


    public ProtoBufSerializer()
    {
      Initialize();
    }
  }

#else
  public class ProtoBufSerializer : ISerializer
  {
    public byte[] Serialize(object item)
    {
      //Condition.Requires(item, "item").IsNotNull();

      using (MemoryStream s = new MemoryStream())
      using (BinaryWriter bw = new BinaryWriter(s))
      {
        bw.Write(item.GetType().AssemblyQualifiedName);
        PBSerializer.Serialize(s, item);
        return s.ToArray();
      }
    }


    public object Deserialize(byte[] data)
    {
      //Condition.Requires(data, "data").IsNotNull();

      using (MemoryStream s = new MemoryStream(data))
      using (BinaryReader br = new BinaryReader(s))
      {
        string dataTypeName = br.ReadString();
        Type t = Type.GetType(dataTypeName); // FIXME: This is slow. Load instead initial set of data types to serialize
        return PBSerializer.Deserialize(t, s);
      }
    }
  }

#endif
}
