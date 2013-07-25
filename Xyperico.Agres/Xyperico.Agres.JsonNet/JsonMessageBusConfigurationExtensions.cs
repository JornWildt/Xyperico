using Xyperico.Agres.MessageBus;
using Xyperico.Agres.Serialization;
using Xyperico.Agres.DocumentStore;


namespace Xyperico.Agres.JsonNet
{
  public static class JsonMessageBusConfigurationExtensions
  {
    //private static ILog Logger = LogManager.GetLogger(typeof(JsonMessageBusConfigurationExtensions));


    public static MessageBusConfiguration WithJsonMessageSerializer(this MessageBusConfiguration cfg)
    {
      //Logger.Debug("Using JSON.NET for serializing messages on message bus");
      ISerializer serializer = new JsonNetSerializer();
      Xyperico.Agres.MessageBus.MessageBusConfigurationExtensions.SetMessageSerializer(cfg, serializer);
      return cfg;
    }


    public static MessageBusConfiguration WithJsonSubscriptionSerializer(this MessageBusConfiguration cfg)
    {
      //Logger.Debug("Using JSON.NET for serializing subscriber registrations");
      IDocumentSerializer serializer = new JsonNetDocumentSerializer();
      Xyperico.Agres.MessageBus.MessageBusConfigurationExtensions.SetSubscriptionSerializer(cfg, serializer);
      return cfg;
    }
  }
}
