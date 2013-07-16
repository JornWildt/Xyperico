using System;
using System.Collections.Generic;
using System.Reflection;
using CuttingEdge.Conditions;
using log4net;
using Xyperico.Agres.Serialization;
using Xyperico.Base;


namespace Xyperico.Agres.MessageBus
{
  public static class MessageBusConfigurationExtensions
  {
    private static readonly ILog Logger = LogManager.GetLogger(typeof(MessageBusConfigurationExtensions));

    private const string MessageDispatcher_SettingsKey = "MessageBusConfiguration_MessageDispatcher";
    private const string ObjectContainer_SettingsKey = "MessageBusConfiguration_ObjectContainer";
    private const string MessageSerializer_SettingsKey = "MessageBusConfiguration_MessageSerializer";
    private const string MessageSource_SettingsKey = "MessageBusConfiguration_MessageSource";


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

      IObjectContainer c;
      if (cfg.TryGet<IObjectContainer>(ObjectContainer_SettingsKey, out c))
        throw new InvalidOperationException(string.Format("Cannot set object container twice. Existing container is {0} - now got {1}.", c, container));

      cfg.Set(ObjectContainer_SettingsKey, container);
      Logger.DebugFormat("Using object container {0}", container);

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

      MessageDispatcher dispatcher = GetDispatcher(cfg);
      dispatcher.RegisterMessageHandlers(assemblies, messageHandlerConvention ?? new DefaultMessageHandlerConvention());

      return cfg;
    }


    public static Configuration WithMessageSerializer(this Configuration cfg, ISerializer serializer)
    {
      Condition.Requires(serializer, "serializer").IsNotNull();

      ISerializer s;
      if (cfg.TryGet<ISerializer>(MessageSerializer_SettingsKey, out s))
        throw new InvalidOperationException(string.Format("Cannot set message serializer twice. Existing serializer is {0} - now got {1}.", s, serializer));
      
      cfg.Set(MessageSerializer_SettingsKey, serializer);
      Logger.DebugFormat("Using message serializer {0}", serializer);
      
      return cfg;
    }


    public static ISerializer GetMessageSerializer(this Configuration cfg)
    {
      ISerializer serializer = cfg.Get<ISerializer>(MessageSerializer_SettingsKey);
      if (serializer == null)
        throw new InvalidOperationException(string.Format("No message serializer has been configured."));
      return serializer;
    }


    public static Configuration WithMessageSource(this Configuration cfg, IMessageSource messageSource)
    {
      Condition.Requires(messageSource, "messageSource").IsNotNull();

      IMessageSource m;
      if (cfg.TryGet<IMessageSource>(MessageSource_SettingsKey, out m))
        throw new InvalidOperationException(string.Format("Cannot set message source twice. Existing source is {0} - now got {1}.", m, messageSource));

      cfg.Set(MessageSource_SettingsKey, messageSource);
      Logger.DebugFormat("Using message source {0}", messageSource);

      return cfg;
    }


    public static IMessageSource GetMessageSource(this Configuration cfg)
    {
      IMessageSource messageSource = cfg.Get<IMessageSource>(MessageSource_SettingsKey);
      if (messageSource == null)
        throw new InvalidOperationException(string.Format("No message source has been configured."));
      return messageSource;
    }


    public static void Start(this Configuration cfg)
    {
      MessageBusHost busHost = new MessageBusHost(cfg.GetMessageSource(), cfg.GetDispatcher());
      busHost.Start();
    }


    private static MessageDispatcher GetDispatcher(this Configuration cfg)
    {
      MessageDispatcher dispatcher;
      if (!cfg.TryGet<MessageDispatcher>(MessageDispatcher_SettingsKey, out dispatcher))
      {
        IObjectContainer container = cfg.GetObjectContainer();
        dispatcher = new MessageDispatcher(container);
        cfg.Set(MessageDispatcher_SettingsKey, dispatcher);
      }
      return dispatcher;
    }
  }
}
