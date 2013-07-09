using System;
using CuttingEdge.Conditions;
using Xyperico.Agres.MessageBus;
using Msmq = System.Messaging;


namespace Xyperico.Agres.MSMQ
{
  public class MSMQMessageSource : IMessageSource, IDisposable
  {
    protected string QueueName { get; set; }

    protected Msmq.IMessageFormatter MessageFormater { get; set; }

    protected Msmq.MessageQueue Queue { get; set; }

    protected TimeSpan ReceiveTimeout { get; set; }


    public event EventHandler<MessageReceivedEventArgs> MessageReceived;


    public MSMQMessageSource(string queueName, Msmq.IMessageFormatter messageFormater)
    {
      Condition.Requires(queueName, "queueName").IsNotNull();
      Condition.Requires(messageFormater, "messageFormater").IsNotNull();
      QueueName = queueName;
      MessageFormater = messageFormater;
      Initialize();
    }


    public void Start()
    {
      Queue.BeginPeek(ReceiveTimeout, new object(), HandleMessageReceived);
    }


    protected void Initialize()
    {
      ReceiveTimeout = TimeSpan.FromSeconds(1000);
      Queue = new Msmq.MessageQueue(QueueName);
      Queue.Formatter = MessageFormater;
    }


    protected void HandleMessageReceived(IAsyncResult result)
    {
      try
      {
        Msmq.Message mm = Queue.EndPeek(result);
        Message m = new Message(mm.Id, mm.Body);
        OnMessageReceived(new MessageReceivedEventArgs(m));

        // Success - remove message
        Queue.ReceiveById(m.Id);
      }
      finally
      {
        // Failure - try again
        Queue.BeginPeek(ReceiveTimeout, new object(), HandleMessageReceived);
      }
    }


    protected void OnMessageReceived(MessageReceivedEventArgs args)
    {
      if (MessageReceived != null)
        MessageReceived(this, args);
    }

    
    public void Dispose()
    {
      if (Queue != null)
        Queue.Dispose();
    }
  }
}
