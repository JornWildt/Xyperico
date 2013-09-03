using System;
using NUnit.Framework;
using Xyperico.Agres.MessageBus.RouteHandling;


namespace Xyperico.Agres.Tests.MessageBus
{
  [TestFixture]
  public class RouteManagerTests : TestHelper
  {
    QueueName MyQueueName = "Wolla";
    IRouteManager RouteManager;


    protected override void SetUp()
    {
      base.SetUp();
      RouteManager = new RouteManager();
    }


    [Test]
    public void WhenAddingSameRouteTwiceItThrows()
    {
      // Arrange
      RouteManager.AddRoute("Rofl.abc", "Max");

      // Act + Assert
      AssertThrows<InvalidOperationException>(() => RouteManager.AddRoute("Rofl.abc", "Max"));
    }
  }
}
