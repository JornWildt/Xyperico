using System;


namespace Xyperico.Agres.MessageBus.Subscription
{
  public class SubscriptionMessageHandlers : IHandleMessage<SubscribeCommand>
  {
    #region Dependencies

    public ISubscriptionService SubscriptionService { get; set; }

    #endregion


    public void Handle(SubscribeCommand message)
    {
      Type messageType = Type.GetType(message.SubscribedMessagesTypeName);
      if (messageType == null)
        throw new InvalidOperationException(string.Format("Could not reflect properties of message type '{0}' while handling incoming subscribe command. Is the associated assembly loaded?", message.SubscribedMessagesTypeName));
      SubscriptionService.AddSubscriber(messageType, new QueueName(message.SubscriberQueueName));
    }
  }
}
