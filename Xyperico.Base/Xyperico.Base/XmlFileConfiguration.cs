using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using System.Configuration;


namespace Xyperico.Base
{
  public class XmlFileConfiguration<T>
  {
    private static XmlSerializer Serializer = new XmlSerializer(typeof(T));

    private static string ModuleDirectoryName;

    
    static XmlFileConfiguration()
    {
      if (ConfigurationManager.AppSettings["XmlFileConfiguration.ModuleDirectory"] != null)
        ModuleDirectoryName = ConfigurationManager.AppSettings["XmlFileConfiguration.ModuleDirectory"];
      else
        ModuleDirectoryName = "Areas";
    }


    private static T _settings;
    public static T Settings
    {
      get
      {
        if (_settings == null)
        {
          string xmlFileName = GetFileName();
          using (TextReader r = File.OpenText(xmlFileName))
          {
            _settings = (T)Serializer.Deserialize(r);
          }
        }
        return _settings;
      }
    }


    private static string GetFileName()
    {
      ModuleAttribute attr = typeof(T).GetCustomAttributes(typeof(ModuleAttribute), false).FirstOrDefault() as ModuleAttribute;
      if (attr == null)
        throw new InvalidOperationException(string.Format("Cannot get XML filename for configuration of type {0} since it has no Module attribute associated with it.", typeof(T)));
      if (string.IsNullOrEmpty(attr.ModuleName))
        throw new InvalidOperationException(string.Format("Cannot get XML filename for configuration of type {0} since the module name is empty.", typeof(T)));
      return FileUtility.MapPathToBaseDir(string.Format("~/{0}/{1}/module.config", ModuleDirectoryName, attr.ModuleName));
    }
  }
}
