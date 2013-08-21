using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Xyperico.Agres.MessageBus.Subscription
{
  //[Serializable]
  [DataContract]
  public class SubscriptionRegistration
  {
    [DataMember(Order=1)]
    public List<string> SubscriberQueueNames { get; set; }

    public SubscriptionRegistration()
    {
      SubscriberQueueNames = new List<string>();
    }
  }
}
