using System;
using log4net;


namespace Xyperico.Agres.Configuration
{
  public static class LoggingConfigurationExtensions
  {
    private static readonly ILog Logger = LogManager.GetLogger(typeof(LoggingConfigurationExtensions));

    /// <summary>
    /// Activate default log4net behavior (which is log4net.Config.XmlConfigurator.Configure()).
    /// </summary>
    /// <param name="cfg"></param>
    /// <returns></returns>
    public static ObjectContainerConfiguration Log4Net(this LoggingConfiguration cfg)
    {
      log4net.Config.XmlConfigurator.Configure();
      Logger.Info("******************************************************************");
      Logger.Info("Starting application");
      Logger.Info("******************************************************************");
      Logger.Debug("Using default log4net behavior");
      return new ObjectContainerConfiguration(cfg);
    }


    /// <summary>
    /// Custom log4net activation - do what is required to configure log4net in "configurator".
    /// </summary>
    /// <param name="cfg"></param>
    /// <param name="a"></param>
    /// <returns></returns>
    public static ObjectContainerConfiguration Log4Net(this LoggingConfiguration cfg, Action configurator)
    {
      configurator();
      Logger.Info("******************************************************************");
      Logger.Info("Starting application");
      Logger.Info("******************************************************************");
      Logger.Debug("Using custom log4net behavior");
      return new ObjectContainerConfiguration(cfg);
    }
  }
}
