using Xyperico.Agres.MessageBus;
using Xyperico.Agres.Serialization;


namespace Xyperico.Agres.JsonNet
{
  public static class JsonMessageBusConfigurationExtensions
  {
    public static Configuration WithJsonMessageSerializer(this Configuration cfg)
    {
      ISerializer serializer = new JsonNetSerializer();
      cfg.WithMessageSerializer(serializer);
      return cfg;
    }
  }
}
