using NUnit.Framework;
using Xyperico.Agres.MessageBus;


namespace Xyperico.Agres.Tests.MessageBus
{
  [TestFixture]
  public class ConfigurationSettingsTests : TestHelper
  {
    [Test]
    public void CanReadConfigurationSettings()
    {
      Assert.AreEqual("Zebra", MessageBusSettings.Settings.InputQueue);
      Assert.AreEqual(2, MessageBusSettings.Settings.Routes.Count);
      Assert.AreEqual("Abc.Def", MessageBusSettings.Settings.Routes[0].Messages);
      Assert.AreEqual("Alibaba", MessageBusSettings.Settings.Routes[0].Destination);
      Assert.AreEqual("Xyz.Qwe", MessageBusSettings.Settings.Routes[1].Messages);
      Assert.AreEqual("RobinHat", MessageBusSettings.Settings.Routes[1].Destination);
    }
  }
}
