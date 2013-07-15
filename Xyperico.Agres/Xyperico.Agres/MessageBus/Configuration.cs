using System.Collections.Generic;


namespace Xyperico.Agres.MessageBus
{
  public class Configuration
  {
    private Dictionary<string, object> Settings { get; set; }

    
    public T Get<T>(string key)
    {
      T v;
      TryGet<T>(key, out v);
      return v;
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


    public Configuration()
    {
      Settings = new Dictionary<string, object>();
    }
  }
}
