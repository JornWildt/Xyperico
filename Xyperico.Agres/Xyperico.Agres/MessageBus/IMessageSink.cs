using System;


namespace Xyperico.Agres.MessageBus
{
  public interface IMessageSink : IDisposable
  {
    void Send(QueueName destination, Message m);
  }
}
