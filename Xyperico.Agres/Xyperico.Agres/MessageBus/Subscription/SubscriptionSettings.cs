using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xyperico.Base;
using System.Configuration;

namespace Xyperico.Agres.MessageBus.Subscription
{
  public class SubscriptionSettings : ConfigurationSettingsBase<SubscriptionSettings>
  {
    [ConfigurationProperty("ActivateRouteDebugging")]
    public bool ActivateRouteDebugging
    {
      get { return (bool)this["ActivateRouteDebugging"]; }
      set { this["ActivateRouteDebugging"] = value; }
    }
  }
}
