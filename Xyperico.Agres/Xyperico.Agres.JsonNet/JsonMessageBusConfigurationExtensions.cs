using Xyperico.Agres.DocumentStore;
using Xyperico.Agres.MessageBus;
using Xyperico.Agres.Serialization;


namespace Xyperico.Agres.JsonNet
{
  public static class JsonMessageBusConfigurationExtensions
  {
    public static MessageBusConfiguration WithJsonMessageSerializer(this MessageBusConfiguration cfg)
    {
      ISerializer serializer = new JsonNetSerializer();
      Xyperico.Agres.MessageBus.MessageBusConfigurationExtensions.SetMessageSerializer(cfg, serializer);
      return cfg;
    }


    public static MessageBusConfiguration WithJsonSubscriptionSerializer(this MessageBusConfiguration cfg)
    {
      IDocumentSerializer serializer = new JsonNetDocumentSerializer();
      Xyperico.Agres.MessageBus.MessageBusConfigurationExtensions.SetSubscriptionSerializer(cfg, serializer);
      return cfg;
    }
  }
}
