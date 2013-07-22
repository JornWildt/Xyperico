using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;
using log4net;
using Xyperico.Agres.EventStore;
using Xyperico.Agres.MessageBus;
using Xyperico.Agres.Serialization;
using Xyperico.Base;


namespace Xyperico.Agres.Configuration
{
  public static class ConfigurationExtensions
  {
    private static readonly ILog Logger = LogManager.GetLogger(typeof(ConfigurationExtensions));

    private const string ObjectContainer_SettingsKey = "BaseConfiguration_ObjectContainer";


    /// <summary>
    /// Activate default log4net behavior (which is log4net.Config.XmlConfigurator.Configure()).
    /// </summary>
    /// <param name="cfg"></param>
    /// <returns></returns>
    public static ObjectContainerConfiguration WithLog4Net(this LoggingConfiguration cfg)
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
    public static ObjectContainerConfiguration WithLog4Net(this LoggingConfiguration cfg, Action configurator)
    {
      configurator();
      Logger.Info("******************************************************************");
      Logger.Info("Starting application");
      Logger.Info("******************************************************************");
      Logger.Debug("Using custom log4net behavior");
      return new ObjectContainerConfiguration(cfg);
    }


    public static BaseConfiguration WithObjectContainer(this ObjectContainerConfiguration cfg, IObjectContainer container)
    {
      Condition.Requires(cfg, "cfg").IsNotNull();
      Condition.Requires(container, "container").IsNotNull();

      IObjectContainer c;
      if (cfg.TryGet<IObjectContainer>(ObjectContainer_SettingsKey, out c))
        throw new InvalidOperationException(string.Format("Cannot set object container twice. Existing container is {0} - now got {1}.", c, container));

      cfg.Set(ObjectContainer_SettingsKey, container);
      Logger.DebugFormat("Using object container {0}", container);

      return new BaseConfiguration(cfg);
    }


    public static IObjectContainer GetObjectContainer(AbstractConfiguration cfg)
    {
      IObjectContainer container = cfg.Get<IObjectContainer>(ObjectContainer_SettingsKey);
      if (container == null)
        throw new InvalidOperationException(string.Format("No object container has been configured for dependency injection."));
      return container;
    }


    public static BaseConfiguration RegisterSerializableTypes(this BaseConfiguration cfg, IEnumerable<Type> types)
    {
      Condition.Requires(cfg, "cfg").IsNotNull();
      Condition.Requires(types, "types").IsNotNull();

      AbstractSerializer.RegisterKnownTypes(types);
      return cfg;
    }


    public static EventStoreConfiguration EventStore(this BaseConfiguration cfg)
    {
      return new EventStoreConfiguration(cfg);
    }


    public static MessageBusConfiguration MessageBus(this BaseConfiguration cfg)
    {
      return new MessageBusConfiguration(cfg);
    }


    public static void Start(this BaseConfiguration cfg)
    {
      MessageBusHost busHost = MessageBusConfigurationExtensions.GetMessageBusHost(cfg);
      if (busHost == null)
        throw new InvalidOperationException("Cannot start application: no MessageBusHost has been configured.");

      EventStoreHost eStoreHost = EventStoreConfigurationExtensions.GetEventStoreHost(cfg);
      if (eStoreHost == null)
        throw new InvalidOperationException("Cannot start application: no EventStoreHost has been configured.");

      eStoreHost.Start();
      busHost.Start();
    }
  }
}
