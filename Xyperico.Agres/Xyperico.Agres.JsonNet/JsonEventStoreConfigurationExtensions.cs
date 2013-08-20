using Xyperico.Agres.DocumentStore;
using Xyperico.Agres.EventStore;
using Xyperico.Agres.Serialization;


namespace Xyperico.Agres.JsonNet
{
  public static class JsonEventStoreConfigurationExtensions
  {
    /// <summary>
    /// Use JSON for event serialization.
    /// </summary>
    /// <param name="cfg"></param>
    /// <returns></returns>
    public static EventStoreConfiguration WithJsonEventSerializer(this EventStoreConfiguration cfg)
    {
      ISerializer serializer = new JsonNetSerializer();
      Xyperico.Agres.EventStore.EventStoreConfigurationExtensions.SetMessageSerializer(cfg, serializer);
      return cfg;
    }


    /// <summary>
    /// Use JSON for document serialization.
    /// </summary>
    /// <param name="cfg"></param>
    /// <returns></returns>
    public static EventStoreConfiguration WithJsonDocumentSerializer(this EventStoreConfiguration cfg)
    {
      IDocumentSerializer serializer = new JsonNetDocumentSerializer();
      Xyperico.Agres.EventStore.EventStoreConfigurationExtensions.SetDocumentSerializer(cfg, serializer);
      return cfg;
    }


    /// <summary>
    /// Use BSON (binary JSON) for event serialization.
    /// </summary>
    /// <param name="cfg"></param>
    /// <returns></returns>
    public static EventStoreConfiguration WithBsonEventSerializer(this EventStoreConfiguration cfg)
    {
      ISerializer serializer = new BsonNetSerializer();
      Xyperico.Agres.EventStore.EventStoreConfigurationExtensions.SetMessageSerializer(cfg, serializer);
      return cfg;
    }


    /// <summary>
    /// Use BSON (binary JSON) for document serialization.
    /// </summary>
    /// <param name="cfg"></param>
    /// <returns></returns>
    public static EventStoreConfiguration WithBsonDocumentSerializer(this EventStoreConfiguration cfg)
    {
      IDocumentSerializer serializer = new BsonNetDocumentSerializer();
      Xyperico.Agres.EventStore.EventStoreConfigurationExtensions.SetDocumentSerializer(cfg, serializer);
      return cfg;
    }
  }
}
