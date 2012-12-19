using System;
using System.IO;
using System.Reflection;
using System.Web.Compilation;
using log4net;


namespace Xyperico.Web.Mvc
{
  public class PluginAreaBootstrapper
  {
    static readonly ILog Logger = LogManager.GetLogger(typeof(PluginAreaBootstrapper));

    public const string AreaDirectory = @"bin\Areas";


    public static void Init()
    {
      log4net.Config.XmlConfigurator.Configure();

      string subDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AreaDirectory);
      try
      {
        Logger.DebugFormat("*** Loading assemblies from '{0}'.", subDir);
        foreach (string filename in Directory.EnumerateFiles(subDir, "*.dll"))
        {
          Logger.DebugFormat("Loading assembly '{0}'.", filename);
          // Load into memory to avoid locking the assembly file
          //byte[] assemblyData = File.ReadAllBytes(filename);
          //BuildManager.AddReferencedAssembly(Assembly.Load(assemblyData));
          BuildManager.AddReferencedAssembly(Assembly.LoadFrom(filename));
        }
        Logger.DebugFormat("*** Finished loading assemblies.");
      }
      catch (Exception ex)
      {
        Logger.Info(string.Format("Failed to load areas from '{0}'.", subDir), ex);
      }
    }
  }
}
