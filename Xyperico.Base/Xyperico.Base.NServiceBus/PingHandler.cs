using NServiceBus;


namespace Xyperico.Base.NServiceBus
{
  public class PingHandler : IMessageHandler<PingCommand>
  {
    public IBus Bus { get; set; }


    #region IMessageHandler<PingMessage> Members

    public void Handle(PingCommand message)
    {
      Bus.Reply(new PingReplyMessage());
    }

    #endregion
  }
}
