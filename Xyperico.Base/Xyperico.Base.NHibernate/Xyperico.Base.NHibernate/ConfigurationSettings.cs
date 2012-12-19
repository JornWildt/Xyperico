using System.Configuration;

namespace Xyperico.Base.NHibernate
{
  public class ConfigurationSettings : ConfigurationSection
  {
    public class ConfigurationFileElement : ConfigurationElement
    {
      [ConfigurationProperty("Filename", IsRequired = true)]
      public string Filename
      {
        get { return (string)this["Filename"]; }
        set { this["Filename"] = value; }
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


    public class MappingAssemblyElement : ConfigurationElement
    {
      [ConfigurationProperty("Assembly", IsRequired = true, IsKey = true)]
      public string Assembly
      {
        get { return (string)this["Assembly"]; }
        set { this["Assembly"] = value; }
      }

      public override string ToString()
      {
        return Assembly;
      }
    }


    [ConfigurationProperty("ConfigurationFiles", IsRequired = true)]
    public ConfigurationElementCollection<ConfigurationFileElement> ConfigurationFiles
    {
      get { return (ConfigurationElementCollection<ConfigurationFileElement>)this["ConfigurationFiles"]; }
    }


    [ConfigurationProperty("MappingAssemblies", IsRequired = true)]
    public ConfigurationElementCollection<MappingAssemblyElement> MappingAssemblies
    {
      get { return (ConfigurationElementCollection<MappingAssemblyElement>)this["MappingAssemblies"]; }
    }


    private static ConfigurationSettings _settings;

    public static ConfigurationSettings Settings
    {
      get
      {
        if (_settings == null)
          _settings = (ConfigurationSettings)ConfigurationManager.GetSection("NHibernateSettings");
        return _settings;
      }
    }
  }
}
