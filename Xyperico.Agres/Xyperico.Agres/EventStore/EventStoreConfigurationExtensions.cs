using CuttingEdge.Conditions;
using log4net;
using Xyperico.Agres.Configuration;
using Xyperico.Agres.DocumentStore;
using Xyperico.Agres.Serialization;
using Xyperico.Base;
using System;


namespace Xyperico.Agres.EventStore
{
  public static class EventStoreConfigurationExtensions
  {
    private static ILog Logger = LogManager.GetLogger(typeof(EventStoreConfigurationExtensions));

    private const string AppendOnlyStore_SettingsKey = "EventStoreConfiguration_AppendOnlyStore";
    private const string MessageSerializer_SettingsKey = "EventStoreConfiguration_MessageSerializer";
    private const string DocumentSerializer_SettingsKey = "EventStoreConfiguration_DocumentSerializer";
    private const string DocumentStoreFactory_SettingsKey = "EventStoreConfiguration_DocumentStoreFactory";
    private const string EventPublisher_SettingsKey = "EventStoreConfiguration_EventPublisher";
    private const string EventStoreDB_SettingsKey = "EventStoreConfiguration_EventStoreDB";


    #region End user configuration methods

    public static EventStoreConfiguration WithFileDocumentStore(this EventStoreConfiguration cfg, string baseDir)
    {
      Logger.Debug("Using plain files for storing documents used in event store");
      Condition.Requires(cfg, "cfg").IsNotNull();
      Condition.Requires(baseDir, "baseDir").IsNotNull();

      IDocumentSerializer documentSerializer = cfg.Get<IDocumentSerializer>(DocumentSerializer_SettingsKey);
      IDocumentStoreFactory docStoreFactory = new FileDocumentStoreFactory(baseDir, documentSerializer);
      cfg.Set(DocumentStoreFactory_SettingsKey, docStoreFactory);

      return cfg;
    }


    public static BaseConfiguration Done(this EventStoreConfiguration cfg)
    {
      IAppendOnlyStore aStore = cfg.Get<IAppendOnlyStore>(AppendOnlyStore_SettingsKey);
      ISerializer messageSerializer = cfg.Get<ISerializer>(MessageSerializer_SettingsKey);

      if (aStore == null)
        throw new InvalidOperationException("Mising storage mechanism (IAppendOnlyStore) for event store.");
      if (messageSerializer == null)
        throw new InvalidOperationException("Missing event serializer for event store.");

      EventStoreDB eStore = new EventStoreDB(aStore, messageSerializer);
      cfg.Set(EventStoreDB_SettingsKey, eStore);

      IObjectContainer container = Xyperico.Agres.Configuration.ConfigurationExtensions.GetObjectContainer(cfg);
      container.RegisterInstance<IEventStore>(eStore);

      return new BaseConfiguration(cfg);
    }

    #endregion


    #region Low level configuration methods

    public static void SetAppendOnlyStore(EventStoreConfiguration cfg, IAppendOnlyStore store)
    {
      Condition.Requires(cfg, "cfg").IsNotNull();
      Condition.Requires(store, "store").IsNotNull();
      cfg.Set(AppendOnlyStore_SettingsKey, store);
      Logger.DebugFormat("Using {0} as storage engine for EventStore", store);
    }


    public static void SetMessageSerializer(EventStoreConfiguration cfg, ISerializer serializer)
    {
      Condition.Requires(cfg, "cfg").IsNotNull();
      Condition.Requires(serializer, "serializer").IsNotNull();
      cfg.Set(MessageSerializer_SettingsKey, serializer);
      Logger.DebugFormat("Using {0} as serializer for messages in event store", serializer);
    }


    public static ISerializer GetMessageSerializer(AbstractConfiguration cfg)
    {
      return cfg.Get<ISerializer>(MessageSerializer_SettingsKey);
    }


    public static void SetDocumentSerializer(EventStoreConfiguration cfg, IDocumentSerializer serializer)
    {
      Condition.Requires(cfg, "cfg").IsNotNull();
      Condition.Requires(serializer, "serializer").IsNotNull();
      cfg.Set(DocumentSerializer_SettingsKey, serializer);
    }


    public static void SetEventPublisher(EventStoreConfiguration cfg, IEventPublisher publisher)
    {
      Condition.Requires(cfg, "cfg").IsNotNull();
      Condition.Requires(publisher, "publisher").IsNotNull();
      cfg.Set(EventPublisher_SettingsKey, publisher);
    }


    public static IEventPublisher GetEventPublisher(AbstractConfiguration cfg, bool mustExists)
    {
      IEventPublisher eventPublisher = cfg.Get<IEventPublisher>(EventPublisher_SettingsKey);
      if (eventPublisher == null && mustExists)
        throw new InvalidOperationException("Missing event publisher for event store.");
      return eventPublisher;
    }


    public static IDocumentStoreFactory GetDocumentStoreFactory(AbstractConfiguration cfg)
    {
      IDocumentStoreFactory docStoreFactory = cfg.Get<IDocumentStoreFactory>(DocumentStoreFactory_SettingsKey);
      if (docStoreFactory == null)
        throw new InvalidOperationException("Missing document store factory for event store.");
      return docStoreFactory;
    }


    public static EventStoreDB GetEventStoreDB(AbstractConfiguration cfg)
    {
      EventStoreDB eStore = cfg.Get<EventStoreDB>(EventStoreDB_SettingsKey);
      if (eStore == null)
        throw new InvalidOperationException("Missing event store. Are you missing a call to Done() in the event store configuration code?");
      return eStore;
    }

    #endregion
  }
}
