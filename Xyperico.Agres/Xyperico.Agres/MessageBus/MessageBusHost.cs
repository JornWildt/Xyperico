using System;
using CuttingEdge.Conditions;


namespace Xyperico.Agres.MessageBus
{
  public class MessageBusHost : IDisposable
  {
    protected IMessageSource MessageSource { get; set; }


    public MessageBusHost(IMessageSource messageSource)
    {
      Condition.Requires(messageSource, "messageSource").IsNotNull();
      
      MessageSource = messageSource;

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
      Console.WriteLine("Received message");
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
