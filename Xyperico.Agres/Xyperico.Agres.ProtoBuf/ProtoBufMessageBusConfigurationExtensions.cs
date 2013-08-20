using Xyperico.Agres.DocumentStore;
using Xyperico.Agres.MessageBus;
using Xyperico.Agres.Serialization;


namespace Xyperico.Agres.ProtoBuf
{
  public static class ProtoBufMessageBusConfigurationExtensions
  {
    /// <summary>
    /// Use ProtoBuf for message serialization.
    /// </summary>
    /// <param name="cfg"></param>
    /// <returns></returns>
    public static MessageBusConfiguration WithProtoBufMessageSerializer(this MessageBusConfiguration cfg)
    {
      ISerializer serializer = new ProtoBufSerializer();
      Xyperico.Agres.MessageBus.MessageBusConfigurationExtensions.SetMessageSerializer(cfg, serializer);
      return cfg;
    }


    /// <summary>
    /// Use ProtoBuf for subscription serialization.
    /// </summary>
    /// <param name="cfg"></param>
    /// <returns></returns>
    public static MessageBusConfiguration WithProtoBufSubscriptionSerializer(this MessageBusConfiguration cfg)
    {
      IDocumentSerializer serializer = new ProtoBufDocumentSerializer();
      Xyperico.Agres.MessageBus.MessageBusConfigurationExtensions.SetSubscriptionSerializer(cfg, serializer);
      return cfg;
    }
  }
}
