using Xyperico.Agres.MessageBus;
using Xyperico.Agres.Serialization;
using CuttingEdge.Conditions;
using Msmq = System.Messaging;


namespace Xyperico.Agres.MSMQ
{
  public class MSMQMessageSink : IMessageSink
  {
    private Msmq.IMessageFormatter MessageFormater { get; set; }


    public MSMQMessageSink(Msmq.IMessageFormatter messageFormater)
    {
      Condition.Requires(messageFormater, "messageFormater").IsNotNull();
      MessageFormater = messageFormater;
    }


    public void Send(QueueName destination, Message msg)
    {
      using (Msmq.MessageQueue q = new Msmq.MessageQueue(destination.Name))
      {
        Msmq.Message m = new Msmq.Message();
        m.Label = msg.Body != null ? msg.Body.GetType().ToString() : "MSG";
        m.Recoverable = true;
        m.Formatter = MessageFormater;
        m.Body = msg.Body;
        q.Send(m);
      }
    }


    public void Dispose()
    {
    }
  }
}
