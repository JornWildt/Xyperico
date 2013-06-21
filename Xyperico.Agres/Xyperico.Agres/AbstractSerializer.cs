using System;
using System.Collections.Generic;
using System.IO;
using CuttingEdge.Conditions;


namespace Xyperico.Agres
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
      return Workers[key];
    }


    protected virtual void Initialize()
    {
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
      using (BinaryWriter bw = new BinaryWriter(s))
      {
        string key = item.GetType().FullName;
        bw.Write(key);
        ISerializeWorker w = GetWorker(key);
        w.Serialize(s, item);
        return s.ToArray();
      }
    }

    public virtual object Deserialize(byte[] data)
    {
      Condition.Requires(data, "data").IsNotNull();

      using (MemoryStream s = new MemoryStream(data))
      using (BinaryReader br = new BinaryReader(s))
      {
        string key = br.ReadString();
        ISerializeWorker w = GetWorker(key);
        return w.Deserialize(s);
      }
    }
  }
}
