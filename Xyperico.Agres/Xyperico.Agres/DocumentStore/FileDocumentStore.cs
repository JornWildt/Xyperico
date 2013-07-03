using System.IO;
using CuttingEdge.Conditions;


namespace Xyperico.Agres.DocumentStore
{
  public class FileDocumentStore<TKey,TValue> : IDocumentStore<TKey, TValue>
  {
    protected IStreamSerializer Serializer { get; set; }

    protected string BaseDirectory { get; set; }


    public FileDocumentStore(string baseDir, IStreamSerializer serializer)
    {
      Condition.Requires(baseDir, "baseDir").IsNotNull();
      Condition.Requires(serializer, "serializer").IsNotNull();

      BaseDirectory = baseDir;
      Serializer = serializer;
    }


    #region IDocumentStore

    public void Put(TKey key, TValue value)
    {
      string filename = GetFileName(key);
      using (Stream s = File.Open(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
      {
        Serializer.Serialize(s, value);
      }
    }


    public bool TryGet(TKey key, out TValue value)
    {
      string filename = GetFileName(key);
      
      if (!File.Exists(filename))
      {
        value = default(TValue);
        return false;
      }

      using (Stream s = File.Open(filename, FileMode.Open, FileAccess.Read))
      {
        object result = Serializer.Deserialize(s);
        value = (TValue)result;
      }

      return true;
    }


    public bool TryDelete(TKey key)
    {
      string filename = GetFileName(key);
      if (!File.Exists(filename))
        return false;
      File.Delete(filename);
      return true;
    }

    #endregion


    protected string GetFileName(TKey key)
    {
      string path = typeof(TValue).Name;
      string file = key.ToString() + ".dat";
      string fullPath = Path.Combine(BaseDirectory, path);
      Directory.CreateDirectory(fullPath);
      return Path.Combine(fullPath, file);
    }
  }
}
