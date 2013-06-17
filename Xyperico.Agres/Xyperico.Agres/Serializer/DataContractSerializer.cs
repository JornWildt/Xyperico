using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCSerializer = System.Runtime.Serialization.DataContractSerializer;
using CuttingEdge.Conditions;
using System.IO;


namespace Xyperico.Agres.Serializer
{
#if true
  public class DataContractSerializer : AbstractSerializer
  {
    protected class DataContractSerializerWorker : ISerializeWorker
    {
      DCSerializer Serializer;

      public DataContractSerializerWorker(DCSerializer s)
      {
        Serializer = s;
      }

      public void Serialize(Stream s, object o)
      {
        Serializer.WriteObject(s, o);
      }

      public object Deserialize(Stream s)
      {
        return Serializer.ReadObject(s);
      }
    }


    protected override ISerializeWorker CreateWorker(Type t)
    {
      DCSerializer s = new DCSerializer(t);
      return new DataContractSerializerWorker(s);
    }


    public DataContractSerializer()
    {
      Initialize();
    }
  }

#else

  public class DataContractSerializer : ISerializer
  {
    Dictionary<Type, DCSerializer> Serializers = new Dictionary<Type, DCSerializer>();


    public byte[] Serialize(object item)
    {
      Condition.Requires(item, "item").IsNotNull();

      using (MemoryStream s = new MemoryStream())
      using (BinaryWriter bw = new BinaryWriter(s))
      {
        bw.Write(item.GetType().AssemblyQualifiedName);
        DCSerializer dcs = GetSerializer(item.GetType());
        dcs.WriteObject(s, item);
        return s.ToArray();
      }
    }

    
    public object Deserialize(byte[] data)
    {
      Condition.Requires(data, "data").IsNotNull();

      using (MemoryStream s = new MemoryStream(data))
      using (BinaryReader br = new BinaryReader(s))
      {
        string dataTypeName = br.ReadString();
        Type t = Type.GetType(dataTypeName); // FIXME: This is slow. Load instead initial set of data types to serialize
        DCSerializer dcs = GetSerializer(t);
        return dcs.ReadObject(s);
      }
    }


    DCSerializer GetSerializer(Type t)
    {
      DCSerializer s;
      if (Serializers.TryGetValue(t, out s))
        return s;

      s = new DCSerializer(t);
      Serializers[t] = s;
      return s;
    }
  }
#endif
}
