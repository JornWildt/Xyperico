using System.IO;
using System.Messaging;
using CuttingEdge.Conditions;
using Xyperico.Agres.Serialization;


namespace Xyperico.Agres.MSMQ
{
  public class MSMQMessageFormatter : IMessageFormatter
  {
    protected ISerializer Serializer { get; set; }


    public MSMQMessageFormatter(ISerializer serializer)
    {
      Condition.Requires(serializer, "serializer").IsNotNull();
      Serializer = serializer;
    }


    public bool CanRead(Message message)
    {
      return true;
    }


    public object Read(Message message)
    {
      return Serializer.Deserialize(message.BodyStream);
    }

    
    public void Write(Message message, object obj)
    {
      using (MemoryStream s = new MemoryStream())
      {
        Serializer.Serialize(s, obj);

        // Stupid writers close the stream - make sure to "reopen" it (by copying)
        message.BodyStream = new MemoryStream(s.ToArray());
      }
    }

    
    public object Clone()
    {
      return new MSMQMessageFormatter(Serializer);
    }
  }
}
