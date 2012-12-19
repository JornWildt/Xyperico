using System;


namespace Xyperico.Net.Tests
{
  public class TestHelper : Xyperico.Base.Testing.TestHelper
  {
    public static string TestHostname 
    { 
      get 
      {
        return new Uri(Xyperico.Base.Testing.TestHelper.GetMachineSpecificConfigurationSetting("BaseUri")).Authority;
      } 
    }
  }
}
