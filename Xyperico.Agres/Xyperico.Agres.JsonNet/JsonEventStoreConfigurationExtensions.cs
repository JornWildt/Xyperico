using Xyperico.Agres.DocumentStore;
using Xyperico.Agres.EventStore;
using Xyperico.Agres.Serialization;


namespace Xyperico.Agres.JsonNet
{
  public static class JsonEventStoreConfigurationExtensions
  {
    public static EventStoreConfiguration WithJsonMessageSerializer(this EventStoreConfiguration cfg)
    {
      ISerializer serializer = new JsonNetSerializer();
      Xyperico.Agres.EventStore.EventStoreConfigurationExtensions.SetMessageSerializer(cfg, serializer);
      return cfg;
    }


    public static EventStoreConfiguration WithJsonDocumentSerializer(this EventStoreConfiguration cfg)
    {
      IDocumentSerializer serializer = new JsonNetDocumentSerializer();
      Xyperico.Agres.EventStore.EventStoreConfigurationExtensions.SetDocumentSerializer(cfg, serializer);
      return cfg;
    }
  }
}
