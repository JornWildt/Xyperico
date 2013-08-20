using System;
using CuttingEdge.Conditions;
using log4net;
using Xyperico.Agres.Configuration;
using Xyperico.Agres.DocumentStore;
using Xyperico.Agres.MessageBus.Subscription;
using Xyperico.Agres.Serialization;
using Xyperico.Base;


namespace Xyperico.Agres.MessageBus
{
  public static class MessageBusConfigurationExtensions
  {
    private static readonly ILog Logger = LogManager.GetLogger(typeof(MessageBusConfigurationExtensions));

    private const string MessageDispatcher_SettingsKey = "MessageBusConfiguration_MessageDispatcher";
    private const string MessageSerializer_SettingsKey = "MessageBusConfiguration_MessageSerializer";
    private const string MessageSource_SettingsKey = "MessageBusConfiguration_MessageSource";
    private const string MessageSink_SettingsKey = "MessageBusConfiguration_MessageSink";
    private const string MessageBusHost_SettingsKey = "MessageBusConfiguration_MessageBusHost";
    private const string SubscriptionSerializer_SettingsKey = "MessageBusConfiguration_SubscriptionSerializer";
    private const string SubscriptionStoreFactory_SettingsKey = "MessageBusConfiguration_SubscriptionStoreFactory";
    private const string SubscriptionService_SettingsKey = "MessageBusConfiguration_SubscriptionService";


    #region End user configuration methods

    /// <summary>
    /// Use file based storage for subscriptions.
    /// </summary>
    /// <param name="cfg"></param>
    /// <param name="baseDir"></param>
    /// <returns></returns>
    public static MessageBusConfiguration WithFileSubscriptionStore(this MessageBusConfiguration cfg, string baseDir)
    {
      if (cfg.ContainsKey(SubscriptionStoreFactory_SettingsKey))
        throw new InvalidOperationException("You should not configure subscription store for message bus twice.");

      Logger.Debug("Using plain files for storing subscription registrations.");
      Condition.Requires(cfg, "cfg").IsNotNull();
      Condition.Requires(baseDir, "baseDir").IsNotNull();

      IDocumentSerializer documentSerializer = GetSubscriptionSerializer(cfg);
      IDocumentStoreFactory docStoreFactory = new FileDocumentStoreFactory(baseDir, documentSerializer);
      cfg.Set(SubscriptionStoreFactory_SettingsKey, docStoreFactory);

      return cfg;
    }


    /// <summary>
    /// No more configuration needed for message bus - now configure something else or start the bus.
    /// </summary>
    /// <param name="cfg"></param>
    /// <returns></returns>
    public static BaseConfiguration Done(this MessageBusConfiguration cfg)
    {
      IObjectContainer container = ObjectContainerConfigurationExtensions.GetObjectContainer(cfg);
      IDocumentStoreFactory subscriptionStoreFactory = GetSubscriptionStore(cfg);
      ISubscriptionService subscriptionService = new SubscriptionService(subscriptionStoreFactory);
      cfg.Set(SubscriptionService_SettingsKey, subscriptionService);
      container.RegisterInstance<ISubscriptionService>(subscriptionService);

      return new BaseConfiguration(cfg);
    }

    #endregion

    
    #region Low level configuration methods

    public static ISubscriptionService GetSubscriptionService(AbstractConfiguration cfg)
    {
      ISubscriptionService service = cfg.Get<ISubscriptionService>(SubscriptionService_SettingsKey);
      if (service == null)
        throw new InvalidOperationException("Missing message bus subscription service configuration. Please make sure both subscription store and subscription serializer has been configured");
      return service;
    }


    public static void SetMessageSerializer(MessageBusConfiguration cfg, ISerializer serializer)
    {
      Condition.Requires(serializer, "serializer").IsNotNull();

      ISerializer s;
      if (cfg.TryGet<ISerializer>(MessageSerializer_SettingsKey, out s))
        throw new InvalidOperationException(string.Format("Cannot set message serializer twice. Existing serializer is {0} - now got {1}.", s, serializer));
      
      cfg.Set(MessageSerializer_SettingsKey, serializer);
      Logger.DebugFormat("Using {0} as serializer for messages on message bus", serializer);
    }


    public static ISerializer GetMessageSerializer(AbstractConfiguration cfg)
    {
      ISerializer serializer = cfg.Get<ISerializer>(MessageSerializer_SettingsKey);
      if (serializer == null)
        throw new InvalidOperationException(string.Format("No message serializer has been configured."));
      return serializer;
    }


    public static void SetSubscriptionSerializer(MessageBusConfiguration cfg, IDocumentSerializer serializer)
    {
      Condition.Requires(serializer, "serializer").IsNotNull();

      IDocumentSerializer s;
      if (cfg.TryGet<IDocumentSerializer>(SubscriptionSerializer_SettingsKey, out s))
        throw new InvalidOperationException(string.Format("Cannot set message bus subscription serializer twice. Existing serializer is {0} - now got {1}.", s, serializer));

      cfg.Set(SubscriptionSerializer_SettingsKey, serializer);
      Logger.DebugFormat("Using {0} as serializer for subscription registrations", serializer);
    }


    public static IDocumentSerializer GetSubscriptionSerializer(AbstractConfiguration cfg)
    {
      IDocumentSerializer serializer = cfg.Get<IDocumentSerializer>(SubscriptionSerializer_SettingsKey);
      if (serializer == null)
        throw new InvalidOperationException(string.Format("No message bus subscription serializer has been configured."));
      return serializer;
    }


    public static IDocumentStoreFactory GetSubscriptionStore(AbstractConfiguration cfg)
    {
      IDocumentStoreFactory store = cfg.Get<IDocumentStoreFactory>(SubscriptionStoreFactory_SettingsKey);
      if (store == null)
        throw new InvalidOperationException(string.Format("No message bus subscription store has been configured."));
      return store;
    }


    public static void SetMessageSource(MessageBusConfiguration cfg, IMessageSource messageSource)
    {
      Condition.Requires(messageSource, "messageSource").IsNotNull();

      IMessageSource s;
      if (cfg.TryGet<IMessageSource>(MessageSource_SettingsKey, out s))
        throw new InvalidOperationException(string.Format("Cannot set message source twice. Existing source is {0} - now got {1}.", s, messageSource));

      cfg.Set(MessageSource_SettingsKey, messageSource);
      Logger.DebugFormat("Using message source {0}", messageSource);
    }


    public static IMessageSource GetMessageSource(AbstractConfiguration cfg)
    {
      IMessageSource messageSource = cfg.Get<IMessageSource>(MessageSource_SettingsKey);
      if (messageSource == null)
        throw new InvalidOperationException(string.Format("No message source has been configured for message bus."));
      return messageSource;
    }


    public static void SetMessageSink(MessageBusConfiguration cfg, IMessageSink messageSink)
    {
      Condition.Requires(messageSink, "messageSink").IsNotNull();

      IMessageSink s;
      if (cfg.TryGet<IMessageSink>(MessageSink_SettingsKey, out s))
        throw new InvalidOperationException(string.Format("Cannot set message sink twice. Existing sink is {0} - now got {1}.", s, messageSink));

      cfg.Set(MessageSink_SettingsKey, messageSink);
      Logger.DebugFormat("Using message sink {0}", messageSink);
    }


    public static IMessageSink GetMessageSink(AbstractConfiguration cfg)
    {
      IMessageSink messageSink = cfg.Get<IMessageSink>(MessageSink_SettingsKey);
      if (messageSink == null)
        throw new InvalidOperationException(string.Format("No message sink has been configured."));
      return messageSink;
    }


    public static MessageBusHost GetMessageBusHost(AbstractConfiguration cfg)
    {
      MessageBusHost busHost = cfg.Get<MessageBusHost>(MessageBusHost_SettingsKey);
      return busHost;
    }


    public static MessageDispatcher GetDispatcher(AbstractConfiguration cfg)
    {
      MessageDispatcher dispatcher;
      if (!cfg.TryGet<MessageDispatcher>(MessageDispatcher_SettingsKey, out dispatcher))
      {
        IObjectContainer container = Xyperico.Agres.Configuration.ObjectContainerConfigurationExtensions.GetObjectContainer(cfg);
        dispatcher = new MessageDispatcher(container);
        cfg.Set(MessageDispatcher_SettingsKey, dispatcher);
      }
      return dispatcher;
    }

    #endregion
  }
}
