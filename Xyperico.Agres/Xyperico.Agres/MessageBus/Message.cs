using System;


namespace Xyperico.Agres.MessageBus
{
  public class Message
  {
    public string Id { get; private set; }
    public object Body { get; private set; }

    
    public Message(object body)
      : this(Guid.NewGuid().ToString(), body)
    {
    }


    public Message(string id, object body)
    {
      Id = id;
      Body = body;
    }
  }
}
