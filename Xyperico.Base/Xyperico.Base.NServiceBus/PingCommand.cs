using System;
using NServiceBus;


namespace Xyperico.Base.NServiceBus
{
  [Serializable]
  public class PingCommand : IMessage
  {
    public PingCommand()
    {
    }
  }
}
