using System.Collections.Generic;
using System.Reflection;
using CuttingEdge.Conditions;
using Xyperico.Base;
using System;
using log4net;


namespace Xyperico.Agres.MessageBus
{
  public static class MessageBusConfigurationExtensions
  {
    private static readonly ILog Logger = LogManager.GetLogger(typeof(MessageBusConfigurationExtensions));

    private const string MessageDispatcher_SettingsKey = "MessageBusConfiguration_MessageDispatcher";
    private const string ObjectContainer_SettingsKey = "MessageBusConfiguration_ObjectContainer";


    /// <summary>
    /// Activate default log4net behavior (which is log4net.Config.XmlConfigurator.Configure()).
    /// </summary>
    /// <param name="cfg"></param>
    /// <returns></returns>
    public static Configuration WithLog4Net(this Configuration cfg)
    {
      log4net.Config.XmlConfigurator.Configure();
      Logger.Info("******************************************************************");
      Logger.Info("Starting application");
      Logger.Info("******************************************************************");
      Logger.Debug("Using default log4net behavior");
      return cfg;
    }


    /// <summary>
    /// Custom log4net activation - do what is required to configure log4net in "configurator".
    /// </summary>
    /// <param name="cfg"></param>
    /// <param name="a"></param>
    /// <returns></returns>
    public static Configuration WithLog4Net(this Configuration cfg, Action configurator)
    {
      configurator();
      Logger.Info("******************************************************************");
      Logger.Info("Starting application");
      Logger.Info("******************************************************************");
      Logger.Debug("Using custom log4net behavior");
      return cfg;
    }


    public static Configuration WithObjectContainer(this Configuration cfg, IObjectContainer container)
    {
      Condition.Requires(cfg, "cfg").IsNotNull();
      Condition.Requires(container, "container").IsNotNull();

      cfg.Set(ObjectContainer_SettingsKey, container);

      return cfg;
    }


    public static IObjectContainer GetObjectContainer(this Configuration cfg)
    {
      IObjectContainer container = cfg.Get<IObjectContainer>(ObjectContainer_SettingsKey);
      if (container == null)
        throw new InvalidOperationException(string.Format("No object container has been configured for dependency injection."));
      return container;
    }


    public static Configuration ScanAssemblies(this Configuration cfg, IEnumerable<Assembly> assemblies, IMessageHandlerConvention messageHandlerConvention = null)
    {
      Condition.Requires(cfg, "cfg").IsNotNull();
      Condition.Requires(assemblies, "assemblies").IsNotNull();

      MessageDispatcher dispatcher = Dispatcher(cfg, cfg.GetObjectContainer());
      dispatcher.RegisterMessageHandlers(assemblies, messageHandlerConvention ?? new DefaultMessageHandlerConvention());

      return cfg;
    }


    public static void Start(this Configuration cfg)
    {
      Msmq.IMessageFormatter messageFormater = new MSMQMessageFormatter(messageSerializer);
    }


    private static MessageDispatcher Dispatcher(Configuration cfg, IObjectContainer container)
    {
      MessageDispatcher dispatcher;
      if (!cfg.TryGet<MessageDispatcher>(MessageDispatcher_SettingsKey, out dispatcher))
      {
        dispatcher = new MessageDispatcher(container);
        cfg.Set(MessageDispatcher_SettingsKey, dispatcher);
      }
      return dispatcher;
    }
  }
}
