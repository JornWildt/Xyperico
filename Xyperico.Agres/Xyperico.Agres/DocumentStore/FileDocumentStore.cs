using System.IO;
using CuttingEdge.Conditions;


namespace Xyperico.Agres.DocumentStore
{
  public class FileDocumentStore<TKey,TValue> : IDocumentStore<TKey, TValue>
  {
    protected IDocumentSerializer Serializer { get; set; }

    protected string BaseDirectory { get; set; }


    public FileDocumentStore(string baseDir, IDocumentSerializer serializer)
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


    public TValue Get(TKey key)
    {
      TValue result;
      TryGet(key, out result);
      return result;
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
        object result = Serializer.Deserialize(typeof(TValue), s);
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


    public void Clear()
    {
      string path = typeof(TValue).Name;
      string fullPath = Path.Combine(BaseDirectory, path);
      IOException error = null;
      foreach (string filename in Directory.EnumerateFiles(fullPath))
      {
        try
        {
          File.Delete(filename);
        }
        catch (IOException ex)
        {
          error = ex;
        }
      }

      if (error != null)
        throw new IOException(string.Format("Failed to delete one or more files in '{0}'.", fullPath), error);
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
