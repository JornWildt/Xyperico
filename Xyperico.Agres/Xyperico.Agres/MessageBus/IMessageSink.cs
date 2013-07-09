using System;


namespace Xyperico.Agres.MessageBus
{
  public interface IMessageSink : IDisposable
  {
    void Send(Message m);
  }
}
