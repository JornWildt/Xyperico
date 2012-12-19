using System;
using NServiceBus;


namespace Xyperico.Base.NServiceBus
{
  [Serializable]
  public class PingReplyMessage : IMessage
  {
    public PingReplyMessage()
    {
    }
  }
}
