using Xyperico.Agres.MessageBus;
using Msmq = System.Messaging;


namespace Xyperico.Agres.MSMQ
{
  public static class MSMQMessageBusConfigurationExtensions
  {
    public static Configuration WithMSMQ(this Configuration cfg, string queueName)
    {
      ISerializer serializer = cfg.GetMessageSerializer();
      Msmq.IMessageFormatter messageFormater = new MSMQMessageFormatter(serializer);

      IMessageSource messageSource = new MSMQMessageSource(queueName, messageFormater);
      cfg.WithMessageSource(messageSource);

      return cfg;
    }
  }
}
