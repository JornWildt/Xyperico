using System.Configuration;

namespace Xyperico.Base.MongoDB
{
  public class ConfigurationSettings : ConfigurationSection
  {
    public class ConfigurationEntryElement : ConfigurationElement
    {
      [ConfigurationProperty("Database", IsRequired = true)]
      public string Database
      {
        get { return (string)this["Database"]; }
        set { this["Database"] = value; }
      }

      [ConfigurationProperty("Server", IsRequired = true)]
      public string Server
      {
        get { return (string)this["Server"]; }
        set { this["Server"] = value; }
      }

      [ConfigurationProperty("Port", IsRequired = true)]
      public string Port
      {
        get { return (string)this["Port"]; }
        set { this["Port"] = value; }
      }

      [ConfigurationProperty("Name", IsRequired = true, IsKey = true)]
      public string Name
      {
        get { return (string)this["Name"]; }
        set { this["Name"] = value; }
      }

      public override string ToString()
      {
        return Name;
      }
    }


    [ConfigurationProperty("ConfigurationEntries", IsRequired = true)]
    public ConfigurationElementCollection<ConfigurationEntryElement> ConfigurationEntries
    {
      get { return (ConfigurationElementCollection<ConfigurationEntryElement>)this["ConfigurationEntries"]; }
    }


    private static ConfigurationSettings _settings;

    public static ConfigurationSettings Settings
    {
      get
      {
        if (_settings == null)
          _settings = (ConfigurationSettings)ConfigurationManager.GetSection("MongoDBSettings");
        return _settings;
      }
    }
  }
}
