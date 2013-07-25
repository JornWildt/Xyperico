using CuttingEdge.Conditions;
using log4net;
using Xyperico.Agres.Configuration;
using Xyperico.Agres.DocumentStore;
using Xyperico.Agres.Serialization;
using Xyperico.Base;


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
    private const string EventStoreHost_SettingsKey = "EventStoreConfiguration_EventStoreHost";


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
      EventStoreDB eStore = new EventStoreDB(aStore, messageSerializer);

      IObjectContainer container = Xyperico.Agres.Configuration.ConfigurationExtensions.GetObjectContainer(cfg);
      container.RegisterInstance<IEventStore>(eStore);

      IDocumentStoreFactory docStoreFactory = cfg.Get<IDocumentStoreFactory>(DocumentStoreFactory_SettingsKey);
      IEventPublisher eventPublisher = cfg.Get<IEventPublisher>(EventPublisher_SettingsKey);

      EventStoreHost host = new EventStoreHost(eStore, eventPublisher, docStoreFactory);
      cfg.Set(EventStoreHost_SettingsKey, host);

      return new BaseConfiguration(cfg);
    }


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


    public static EventStoreHost GetEventStoreHost(BaseConfiguration cfg)
    {
      return cfg.Get<EventStoreHost>(EventStoreHost_SettingsKey);
    }
  }
}
