using System;
using System.Collections.Generic;


namespace Xyperico.Agres.MessageBus.Subscription
{
  [Serializable]
  public class SubscriptionRegistration
  {
    public List<string> SubscriberQueueNames { get; set; }

    public SubscriptionRegistration()
    {
      SubscriberQueueNames = new List<string>();
    }
  }
}
