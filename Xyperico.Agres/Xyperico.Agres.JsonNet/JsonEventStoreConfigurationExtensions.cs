using Xyperico.Agres.DocumentStore;
using Xyperico.Agres.EventStore;
using Xyperico.Agres.Serialization;


namespace Xyperico.Agres.JsonNet
{
  public static class JsonEventStoreConfigurationExtensions
  {
    //private static ILog Logger = LogManager.GetLogger(typeof(JsonEventStoreConfigurationExtensions));


    public static EventStoreConfiguration WithJsonEventSerializer(this EventStoreConfiguration cfg)
    {
      //Logger.Debug("Using JSON.NET for serializing events in event store");
      ISerializer serializer = new JsonNetSerializer();
      Xyperico.Agres.EventStore.EventStoreConfigurationExtensions.SetMessageSerializer(cfg, serializer);
      return cfg;
    }


    public static EventStoreConfiguration WithJsonDocumentSerializer(this EventStoreConfiguration cfg)
    {
      //Logger.Debug("Using JSON.NET for serializing documents in event store");
      IDocumentSerializer serializer = new JsonNetDocumentSerializer();
      Xyperico.Agres.EventStore.EventStoreConfigurationExtensions.SetDocumentSerializer(cfg, serializer);
      return cfg;
    }
  }
}
