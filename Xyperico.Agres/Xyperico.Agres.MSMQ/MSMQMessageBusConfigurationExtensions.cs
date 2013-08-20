using Xyperico.Agres.MessageBus;
using Xyperico.Agres.Serialization;
using Msmq = System.Messaging;


namespace Xyperico.Agres.MSMQ
{
  public static class MSMQMessageBusConfigurationExtensions
  {
    /// <summary>
    /// Use MSMQ for message transport.
    /// </summary>
    /// <param name="cfg"></param>
    /// <param name="queueName"></param>
    /// <returns></returns>
    public static MessageBusConfiguration WithMSMQ(this MessageBusConfiguration cfg, string queueName)
    {
      ISerializer serializer = Xyperico.Agres.MessageBus.MessageBusConfigurationExtensions.GetMessageSerializer(cfg);
      Msmq.IMessageFormatter messageFormater = new MSMQMessageFormatter(serializer);

      IMessageSource messageSource = new MSMQMessageSource(queueName, messageFormater);
      Xyperico.Agres.MessageBus.MessageBusConfigurationExtensions.SetMessageSource(cfg, messageSource);

      IMessageSink messageSink = new MSMQMessageSink(messageFormater);
      Xyperico.Agres.MessageBus.MessageBusConfigurationExtensions.SetMessageSink(cfg, messageSink);

      return cfg;
    }
  }
}
