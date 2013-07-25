using System.Collections.Generic;


namespace Xyperico.Agres.MessageBus.Subscription
{
  public class SubscriptionRegistration
  {
    public List<QueueName> SubscriberQueueNames { get; set; }

    public SubscriptionRegistration()
    {
      SubscriberQueueNames = new List<QueueName>();
    }
  }
}
