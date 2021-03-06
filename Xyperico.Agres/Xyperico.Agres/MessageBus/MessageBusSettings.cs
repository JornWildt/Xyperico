﻿using System.Configuration;
using Xyperico.Base;


namespace Xyperico.Agres.MessageBus
{
  public class MessageBusSettings : ConfigurationSettingsBase<MessageBusSettings>
  {
    [ConfigurationProperty("InputQueue", IsRequired=true)]
    public string InputQueue
    {
      get { return (string)this["InputQueue"]; }
      set { this["InputQueue"] = value; }
    }


    [ConfigurationProperty("Routes", IsRequired = true)]
    public ConfigurationElementCollection<MessageRoute> Routes
    {
      get { return (ConfigurationElementCollection<MessageRoute>)this["Routes"]; }
    }


    public class MessageRoute : ConfigurationElement
    {
      [ConfigurationProperty("Messages", IsRequired = true, IsKey=true)]
      public string Messages
      {
        get { return (string)this["Messages"]; }
        set { this["Messages"] = value; }
      }


      [ConfigurationProperty("Destination", IsRequired = true)]
      public string Destination
      {
        get { return (string)this["Destination"]; }
        set { this["Destination"] = value; }
      }


      public override string ToString()
      {
        return Messages;
      }
    }
  }
}
