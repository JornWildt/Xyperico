using System;
using System.Collections.Generic;
using System.IO;
using CuttingEdge.Conditions;


namespace Xyperico.Agres.Serialization
{
  public interface ISerializeWorker
  {
    void Serialize(Stream s, object o);
    object Deserialize(Stream s);
  }


  public abstract class AbstractSerializer : ISerializer
  {
    private static List<Type> KnownTypes = new List<Type>();

    private Dictionary<string, ISerializeWorker> Workers = new Dictionary<string, ISerializeWorker>();

    
    public static void RegisterKnownType(Type t)
    {
      KnownTypes.Add(t);
    }


    public static void RegisterKnownTypes(IEnumerable<Type> types)
    {
      foreach (Type t in types)
        RegisterKnownType(t);
    }

    
    public ISerializeWorker GetWorker(string key)
    {
      if (KnownTypes.Count == 0)
        throw new InvalidOperationException("There are no types registered for the serializer!");
      if (!Workers.ContainsKey(key))
        throw new InvalidOperationException(string.Format("Unknown type '{0}' - cannot (de)serialize it. Remember to register with AbstractSerializer before use.", key));

      return Workers[key];
    }


    protected virtual void Initialize()
    {
      if (KnownTypes.Count == 0)
        throw new InvalidOperationException("There are no types registered for the serializer!");

      foreach (Type t in KnownTypes)
      {
        ISerializeWorker w = CreateWorker(t);
        Workers.Add(t.FullName, w);
      }
    }


    protected abstract ISerializeWorker CreateWorker(Type t);


    public virtual byte[] Serialize(object item)
    {
      Condition.Requires(item, "item").IsNotNull();

      using (MemoryStream s = new MemoryStream())
      {
        Serialize(s, item);
        return s.ToArray();
      }
    }


    public virtual void Serialize(Stream s, object item)
    {
      Condition.Requires(s, "s").IsNotNull();
      Condition.Requires(item, "item").IsNotNull();

      using (BinaryWriter bw = new BinaryWriter(s))
      {
        string key = item.GetType().FullName;
        bw.Write(key);
        ISerializeWorker w = GetWorker(key);
        w.Serialize(s, item);
      }
    }


    public virtual object Deserialize(byte[] data)
    {
      Condition.Requires(data, "data").IsNotNull();

      using (MemoryStream s = new MemoryStream(data))
      {
        return Deserialize(s);
      }
    }


    public virtual object Deserialize(Stream s)
    {
      Condition.Requires(s, "s").IsNotNull();

      using (BinaryReader br = new BinaryReader(s))
      {
        string key = br.ReadString();
        ISerializeWorker w = GetWorker(key);
        return w.Deserialize(s);
      }
    }
  }
}
