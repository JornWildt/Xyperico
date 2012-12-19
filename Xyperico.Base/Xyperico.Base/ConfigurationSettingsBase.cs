using System;
using System.Configuration;


namespace Xyperico.Base
{
  public class ConfigurationSettingsBase<T> : ConfigurationSection
  {
    protected static T _settings;
    public static T Settings
    {
      get
      {
        T settings;
        if (!TryGetSettings(out settings, false))
        {
          if (!TryGetSettings(out settings, true))
            throw new ArgumentException(string.Format("Missing {0} or {1} setting in config file.", typeof(T).Name, typeof(T).FullName));
        }
        return settings;
      }
    }

    public static bool TryGetSettings(out T settings, bool useFullName)
    {
      if (_settings == null)
      {
        if (useFullName)
          _settings = (T)ConfigurationManager.GetSection(typeof(T).FullName);
        else
          _settings = (T)ConfigurationManager.GetSection(typeof(T).Name);
      }

      settings = _settings;
      return (settings != null);
    }
  }

  public class ValueElement<T> : ConfigurationElement
  {
    [ConfigurationProperty("Value")]
    public T Value
    {
      get { return (T)this["Value"]; }
      set { this["Value"] = value; }
    }
  }

}
