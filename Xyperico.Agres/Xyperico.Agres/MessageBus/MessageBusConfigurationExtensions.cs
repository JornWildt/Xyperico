using System;
using System.Collections.Generic;
using System.Reflection;
using CuttingEdge.Conditions;
using log4net;
using Xyperico.Agres.Configuration;
using Xyperico.Agres.Serialization;
using Xyperico.Base;


namespace Xyperico.Agres.MessageBus
{
  public static class MessageBusConfigurationExtensions
  {
    private static readonly ILog Logger = LogManager.GetLogger(typeof(MessageBusConfigurationExtensions));

    private const string MessageDispatcher_SettingsKey = "MessageBusConfiguration_MessageDispatcher";
    private const string MessageSerializer_SettingsKey = "MessageBusConfiguration_MessageSerializer";
    private const string MessageSource_SettingsKey = "MessageBusConfiguration_MessageSource";
    private const string MessageBusHost_SettingsKey = "MessageBusConfiguration_MessageBusHost";


    public static MessageBusConfiguration ScanAssemblies(this MessageBusConfiguration cfg, IEnumerable<Assembly> assemblies, IMessageHandlerConvention messageHandlerConvention = null)
    {
      Condition.Requires(cfg, "cfg").IsNotNull();
      Condition.Requires(assemblies, "assemblies").IsNotNull();

      MessageDispatcher dispatcher = GetDispatcher(cfg);
      dispatcher.RegisterMessageHandlers(assemblies, messageHandlerConvention ?? new DefaultMessageHandlerConvention());

      return cfg;
    }


    public static BaseConfiguration Done(this MessageBusConfiguration cfg)
    {
      MessageBusHost busHost = new MessageBusHost(GetMessageSource(cfg), GetDispatcher(cfg));

      cfg.Set(MessageBusHost_SettingsKey, busHost);

      return new BaseConfiguration(cfg);
    }


    public static void SetMessageSerializer(MessageBusConfiguration cfg, ISerializer serializer)
    {
      Condition.Requires(serializer, "serializer").IsNotNull();

      ISerializer s;
      if (cfg.TryGet<ISerializer>(MessageSerializer_SettingsKey, out s))
        throw new InvalidOperationException(string.Format("Cannot set message serializer twice. Existing serializer is {0} - now got {1}.", s, serializer));
      
      cfg.Set(MessageSerializer_SettingsKey, serializer);
      Logger.DebugFormat("Using message serializer {0}", serializer);
    }


    public static ISerializer GetMessageSerializer(MessageBusConfiguration cfg)
    {
      ISerializer serializer = cfg.Get<ISerializer>(MessageSerializer_SettingsKey);
      if (serializer == null)
        throw new InvalidOperationException(string.Format("No message serializer has been configured."));
      return serializer;
    }


    public static void SetMessageSource(MessageBusConfiguration cfg, IMessageSource messageSource)
    {
      Condition.Requires(messageSource, "messageSource").IsNotNull();

      IMessageSource m;
      if (cfg.TryGet<IMessageSource>(MessageSource_SettingsKey, out m))
        throw new InvalidOperationException(string.Format("Cannot set message source twice. Existing source is {0} - now got {1}.", m, messageSource));

      cfg.Set(MessageSource_SettingsKey, messageSource);
      Logger.DebugFormat("Using message source {0}", messageSource);
    }


    public static IMessageSource GetMessageSource(MessageBusConfiguration cfg)
    {
      IMessageSource messageSource = cfg.Get<IMessageSource>(MessageSource_SettingsKey);
      if (messageSource == null)
        throw new InvalidOperationException(string.Format("No message source has been configured."));
      return messageSource;
    }


    public static MessageBusHost GetMessageBusHost(AbstractConfiguration cfg)
    {
      MessageBusHost busHost = cfg.Get<MessageBusHost>(MessageBusHost_SettingsKey);
      return busHost;
    }


    private static MessageDispatcher GetDispatcher(MessageBusConfiguration cfg)
    {
      MessageDispatcher dispatcher;
      if (!cfg.TryGet<MessageDispatcher>(MessageDispatcher_SettingsKey, out dispatcher))
      {
        IObjectContainer container = Xyperico.Agres.Configuration.ConfigurationExtensions.GetObjectContainer(cfg);
        dispatcher = new MessageDispatcher(container);
        cfg.Set(MessageDispatcher_SettingsKey, dispatcher);
      }
      return dispatcher;
    }
  }
}
