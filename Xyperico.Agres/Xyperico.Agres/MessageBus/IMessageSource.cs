using System;


namespace Xyperico.Agres.MessageBus
{
  public class MessageReceivedEventArgs : EventArgs
  {
    public Message Message { get; private set; }

    public MessageReceivedEventArgs(Message message)
    {
      Message = message;
    }
  }


  public interface IMessageSource : IDisposable
  {
    void Start();
    event EventHandler<MessageReceivedEventArgs> MessageReceived;
  }
}
