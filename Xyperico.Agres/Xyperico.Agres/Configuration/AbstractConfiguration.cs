using System.Collections.Generic;


namespace Xyperico.Agres.Configuration
{
  public abstract class AbstractConfiguration
  {
    private Dictionary<string, object> Settings { get; set; }


    public AbstractConfiguration()
    {
      Settings = new Dictionary<string, object>();
    }


    public AbstractConfiguration(AbstractConfiguration src)
    {
      Settings = src.Settings;
    }


    public bool ContainsKey(string key)
    {
      return Settings.ContainsKey(key);
    }


    public T Get<T>(string key)
    {
      if (Settings.ContainsKey(key))
        return (T)Settings[key];
      return default(T);
    }


    public bool TryGet<T>(string key, out T value)
    {
      object v;
      if (Settings.TryGetValue(key, out v))
      {
        value = (T)v;
        return true;
      }
      else
      {
        value = default(T);
        return false;
      }
    }


    public void Set<T>(string key, T value)
    {
      Settings[key] = value;
    }
  }
}
