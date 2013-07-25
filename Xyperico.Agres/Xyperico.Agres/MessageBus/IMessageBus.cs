using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xyperico.Agres.MessageBus
{
  public interface IMessageBus
  {
    void Publish(IEvent e);
    void Subscribe(Type eventType);
    void Subscribe<T>() where T : IEvent;
    void Unsubscribe(Type eventType);
    void Unsubscribe<T>() where T : IEvent;
    void Send(IMessage msg);
    void Send(QueueName destination, IMessage msg);
    
    // These works on the message in the "current" context
    // void Reply(IMessage msg);
    // Get Message context information
    // Handle current message later
  }
}
