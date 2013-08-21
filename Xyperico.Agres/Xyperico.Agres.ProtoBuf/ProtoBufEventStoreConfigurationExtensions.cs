using Xyperico.Agres.EventStore;
using Xyperico.Agres.Serialization;
using Xyperico.Agres.DocumentStore;


namespace Xyperico.Agres.ProtoBuf
{
  public static class ProtoBufEventStoreConfigurationExtensions
  {
    /// <summary>
    /// Use ProtoBuf for event serialization.
    /// </summary>
    /// <param name="cfg"></param>
    /// <returns></returns>
    public static EventStoreConfiguration WithProtoBufEventSerializer(this EventStoreConfiguration cfg)
    {
      ISerializer serializer = new ProtoBufSerializer();
      Xyperico.Agres.EventStore.EventStoreConfigurationExtensions.SetMessageSerializer(cfg, serializer);
      return cfg;
    }


    /// <summary>
    /// Use ProtoBuf for document serialization.
    /// </summary>
    /// <param name="cfg"></param>
    /// <returns></returns>
    public static EventStoreConfiguration WithProtoBufDocumentSerializer(this EventStoreConfiguration cfg)
    {
      IDocumentSerializer serializer = new ProtoBufDocumentSerializer();
      Xyperico.Agres.EventStore.EventStoreConfigurationExtensions.SetDocumentSerializer(cfg, serializer);
      return cfg;
    }
  }
}
