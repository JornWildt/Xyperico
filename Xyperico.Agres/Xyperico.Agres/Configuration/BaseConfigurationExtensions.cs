using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CuttingEdge.Conditions;
using log4net;
using Xyperico.Agres.DocumentStore;
using Xyperico.Agres.EventStore;
using Xyperico.Agres.MessageBus;
using Xyperico.Agres.MessageBus.Subscription;
using Xyperico.Agres.Serialization;
using Xyperico.Base;
using Xyperico.Agres.MessageBus.RouteHandling;


namespace Xyperico.Agres.Configuration
{
  public static class BaseConfigurationExtensions
  {
    private static readonly ILog Logger = LogManager.GetLogger(typeof(BaseConfigurationExtensions));

    private const string EventStore_SettingsKey = "BaseConfiguration_EventStore";
    private const string MessageBus_SettingsKey = "BaseConfiguration_MessageBus";


    #region End user configuration methods

    /// <summary>
    /// Register all serializable types - this is needed for some serializers (ProtoBuf for instance).
    /// </summary>
    /// <param name="cfg"></param>
    /// <param name="types"></param>
    /// <returns></returns>
    public static BaseConfiguration SerializableTypes(this BaseConfiguration cfg, IEnumerable<Type> types)
    {
      Condition.Requires(cfg, "cfg").IsNotNull();
      Condition.Requires(types, "types").IsNotNull();

      AbstractSerializer.RegisterKnownTypes(types);
      RegisterInternalSerializableTypes();

      return cfg;
    }


    /// <summary>
    /// Start configuration of event store.
    /// </summary>
    /// <param name="cfg"></param>
    /// <returns></returns>
    public static EventStoreConfiguration EventStore(this BaseConfiguration cfg)
    {
      if (cfg.ContainsKey(EventStore_SettingsKey))
        throw new InvalidOperationException("You should not configure EventStore twice.");
      cfg.Set(EventStore_SettingsKey, true);

      return new EventStoreConfiguration(cfg);
    }


    /// <summary>
    /// Start configuration of messages bus and scan for message handlers in the supplied assemblies.
    /// </summary>
    /// <param name="cfg"></param>
    /// <param name="assemblies"></param>
    /// <param name="messageHandlerConvention"></param>
    /// <returns></returns>
    public static MessageBusConfiguration MessageBus(this BaseConfiguration cfg, IEnumerable<Assembly> assemblies, IMessageHandlerConvention messageHandlerConvention = null)
    {
      if (cfg.ContainsKey(MessageBus_SettingsKey))
        throw new InvalidOperationException("You should not configure MessageBus twice.");
      cfg.Set(MessageBus_SettingsKey, true);

      Condition.Requires(assemblies, "assemblies").IsNotNull();

      // Also scan core message handlers
      assemblies = assemblies.Union(new Assembly[] { Assembly.GetExecutingAssembly() });

      MessageDispatcher dispatcher = MessageBusConfigurationExtensions.GetDispatcher(cfg);
      dispatcher.RegisterMessageHandlers(assemblies, messageHandlerConvention ?? new DefaultMessageHandlerConvention());

      return new MessageBusConfiguration(cfg);
    }


    /// <summary>
    /// No more configuration needed - start event store and message bus.
    /// </summary>
    /// <param name="cfg"></param>
    /// <returns></returns>
    public static IMessageBus Start(this BaseConfiguration cfg)
    {
      AutoSubscribeToHandledMessages(cfg);

      IMessageBus messageBus = BuildMessageBus(cfg);
      MessageBusHost busHost = BuildMessageBusHost(cfg);
      EventStoreHost eStoreHost = BuildEventStoreHost(cfg, messageBus);

      eStoreHost.Start();
      busHost.Start();

      return messageBus;
    }

    
    private static void RegisterInternalSerializableTypes()
    {
      var serializerTypes =
        typeof(BaseConfigurationExtensions).Assembly.GetTypes()
        .Where(t => typeof(Identity<>).IsAssignableFrom(t) || typeof(IMessage).IsAssignableFrom(t))
        .Where(t => !t.IsAbstract);

      AbstractSerializer.RegisterKnownTypes(serializerTypes);
    }

    #endregion


    private static IMessageBus BuildMessageBus(AbstractConfiguration cfg)
    {
      ISubscriptionService subscriptionService = MessageBusConfigurationExtensions.GetSubscriptionService(cfg);
      IMessageSink messageSink = MessageBusConfigurationExtensions.GetMessageSink(cfg);
      IObjectContainer container = ObjectContainerConfigurationExtensions.GetObjectContainer(cfg);
      IRouteManager routeManager = container.Resolve<IRouteManager>();

      Xyperico.Agres.MessageBus.Implementation.MessageBus bus = new Agres.MessageBus.Implementation.MessageBus(subscriptionService, routeManager, messageSink);
      container.RegisterInstance<IMessageBus>(bus);

      return bus;
    }


    private static MessageBusHost BuildMessageBusHost(AbstractConfiguration cfg)
    {
      MessageBusHost busHost = new MessageBusHost(MessageBusConfigurationExtensions.GetMessageSource(cfg), MessageBusConfigurationExtensions.GetDispatcher(cfg));
      return busHost;
    }

    
    private static EventStoreHost BuildEventStoreHost(BaseConfiguration cfg, IMessageBus messageBus)
    {
      IEventPublisher eventPublisher = EventStoreConfigurationExtensions.GetEventPublisher(cfg, false);
      if (eventPublisher == null)
        eventPublisher = new MessageBusEventPublisher(messageBus);
      IDocumentStoreFactory docStoreFactory = EventStoreConfigurationExtensions.GetDocumentStoreFactory(cfg);
      EventStoreDB eStore = EventStoreConfigurationExtensions.GetEventStoreDB(cfg);

      EventStoreHost host = new EventStoreHost(eStore, eventPublisher, docStoreFactory);

      return host;
    }


    private static void AutoSubscribeToHandledMessages(BaseConfiguration cfg)
    {
      ISubscriptionService subscriptionService = MessageBusConfigurationExtensions.GetSubscriptionService(cfg);
      IMessageSink messageSink = MessageBusConfigurationExtensions.GetMessageSink(cfg);

      MessageDispatcher dispatcher = MessageBusConfigurationExtensions.GetDispatcher(cfg);
      foreach (Type msg in dispatcher.GetHandledMessages())
      {
        if (typeof(IEvent).IsAssignableFrom(msg))
          subscriptionService.Subscribe(msg, messageSink);
      }
    }
  }
}
