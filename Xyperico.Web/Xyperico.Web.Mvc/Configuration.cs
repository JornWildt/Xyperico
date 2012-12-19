using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xyperico.Base;
using System.Configuration;

namespace Xyperico.Web.Mvc
{
  public class Configuration : ConfigurationSettingsBase<Configuration>
  {
    [ConfigurationProperty("ActivateRouteDebugging")]
    public bool ActivateRouteDebugging
    {
      get { return (bool)this["ActivateRouteDebugging"]; }
      set { this["ActivateRouteDebugging"] = value; }
    }


    [ConfigurationProperty("Theme")]
    public string Theme
    {
      get { return (string)this["Theme"]; }
      set { this["Theme"] = value; }
    }
  }
}
