using System;
using CuttingEdge.Conditions;
using Xyperico.Agres.EventStore;
using Msmq = System.Messaging;


namespace Xyperico.Agres.MSMQ
{
  /// <summary>
  /// MSMQ "Publisher" connected directly to EventStore (for debugging/development testing).
  /// </summary>
  public class MSMQEventPublisher : IEventPublisher, IDisposable
  {
    Msmq.MessageQueue Queue;

    protected Msmq.IMessageFormatter MessageFormater { get; set; }


    public MSMQEventPublisher(string destinationQueueName, Msmq.IMessageFormatter messageFormater)
    {
      Condition.Requires(destinationQueueName, "destinationQueueName").IsNotNull();
      Condition.Requires(messageFormater, "messageFormater").IsNotNull();
      MessageFormater = messageFormater;
      Queue = new Msmq.MessageQueue(destinationQueueName);
      Queue.Formatter = messageFormater;
    }


    public void Publish(IEvent e)
    {
      Queue.Send(e);
    }


    public void Dispose()
    {
      Queue.Dispose();
    }
  }
}
