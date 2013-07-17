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
  }
}
