using System;
using CuttingEdge.Conditions;
using log4net;


namespace Xyperico.Agres.MessageBus
{
  public class MessageBusHost : IDisposable
  {
    protected IMessageSource MessageSource { get; set; }

    protected MessageDispatcher Dispatcher { get; set; }


    public MessageBusHost(IMessageSource messageSource, MessageDispatcher dispatcher)
    {
      Condition.Requires(messageSource, "messageSource").IsNotNull();
      Condition.Requires(dispatcher, "dispatcher").IsNotNull();
      
      MessageSource = messageSource;
      Dispatcher = dispatcher;

      Initialize();
    }


    public void Start()
    {
      MessageSource.Start();
    }


    protected void Initialize()
    {
      MessageSource.MessageReceived += MessageSource_MessageReceived;
    }


    void MessageSource_MessageReceived(object sender, MessageReceivedEventArgs e)
    {
      Dispatcher.Dispatch(e.Message.Body);
    }

    
    public void Dispose()
    {
      if (MessageSource != null)
      {
        MessageSource.MessageReceived -= MessageSource_MessageReceived;
        MessageSource.Dispose();
      }
    }
  }
}
