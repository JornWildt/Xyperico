﻿using Xyperico.Agres.DocumentStore;
using Xyperico.Agres.MessageBus;
using Xyperico.Agres.Serialization;


namespace Xyperico.Agres.JsonNet
{
  public static class JsonMessageBusConfigurationExtensions
  {
    /// <summary>
    /// Use JSON for message serialization.
    /// </summary>
    /// <param name="cfg"></param>
    /// <returns></returns>
    public static MessageBusConfiguration WithJsonMessageSerializer(this MessageBusConfiguration cfg)
    {
      ISerializer serializer = new JsonNetSerializer();
      Xyperico.Agres.MessageBus.MessageBusConfigurationExtensions.SetMessageSerializer(cfg, serializer);
      return cfg;
    }


    /// <summary>
    /// Use JSON for subscription serialization.
    /// </summary>
    /// <param name="cfg"></param>
    /// <returns></returns>
    public static MessageBusConfiguration WithJsonSubscriptionSerializer(this MessageBusConfiguration cfg)
    {
      IDocumentSerializer serializer = new JsonNetDocumentSerializer();
      Xyperico.Agres.MessageBus.MessageBusConfigurationExtensions.SetSubscriptionSerializer(cfg, serializer);
      return cfg;
    }
  }
}
