using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Xyperico.Agres.MessageBus.Subscription;
using Xyperico.Agres.DocumentStore;
using Xyperico.Agres.JsonNet;


namespace Xyperico.Agres.Tests.MessageBus
{
  [TestFixture]
  public class SubscriptionServiceTests : TestHelper
  {
    [Test]
    public void CanAddAndGetSubscribers()
    {
      // Arrange
      IDocumentSerializer serializer = new JsonNetDocumentSerializer();
      IDocumentStoreFactory store = new FileDocumentStoreFactory(StorageBaseDir, serializer);
      ISubscriptionService service = new SubscriptionService(store);

      store.Create<Type, SubscriptionRegistration>().Clear();

      // Act
      IList<string> subscribers11 = service.GetSubscribers(typeof(MessageToSubscribe1)).ToList();
      IList<string> subscribers12 = service.GetSubscribers(typeof(MessageToSubscribe2)).ToList();

      service.AddSubscriber(typeof(MessageToSubscribe1), new QueueName("beebob"));
      IList<string> subscribers21 = service.GetSubscribers(typeof(MessageToSubscribe1)).ToList();
      IList<string> subscribers22 = service.GetSubscribers(typeof(MessageToSubscribe2)).ToList();

      service.AddSubscriber(typeof(MessageToSubscribe2), new QueueName("beebob"));
      IList<string> subscribers31 = service.GetSubscribers(typeof(MessageToSubscribe1)).ToList();
      IList<string> subscribers32 = service.GetSubscribers(typeof(MessageToSubscribe2)).ToList();

      service.AddSubscriber(typeof(MessageToSubscribe1), new QueueName("other"));
      IList<string> subscribers41 = service.GetSubscribers(typeof(MessageToSubscribe1)).ToList();
      IList<string> subscribers42 = service.GetSubscribers(typeof(MessageToSubscribe2)).ToList();

      service.AddSubscriber(typeof(MessageToSubscribe2), new QueueName("other"));
      IList<string> subscribers51 = service.GetSubscribers(typeof(MessageToSubscribe1)).ToList();
      IList<string> subscribers52 = service.GetSubscribers(typeof(MessageToSubscribe2)).ToList();

      // Assert
      Assert.AreEqual(0, subscribers11.Count);
      Assert.AreEqual(0, subscribers12.Count);
      
      Assert.AreEqual(1, subscribers21.Count);
      Assert.AreEqual(0, subscribers22.Count);
      Assert.AreEqual("beebob", subscribers21[0]);

      Assert.AreEqual(1, subscribers31.Count);
      Assert.AreEqual(1, subscribers32.Count);
      Assert.AreEqual("beebob", subscribers31[0]);
      Assert.AreEqual("beebob", subscribers32[0]);

      Assert.AreEqual(2, subscribers41.Count);
      Assert.AreEqual(1, subscribers42.Count);
      Assert.AreEqual("beebob", subscribers41[0]);
      Assert.AreEqual("other", subscribers41[1]);
      Assert.AreEqual("beebob", subscribers42[0]);

      Assert.AreEqual(2, subscribers51.Count);
      Assert.AreEqual(2, subscribers52.Count);
      Assert.AreEqual("beebob", subscribers51[0]);
      Assert.AreEqual("other", subscribers51[1]);
      Assert.AreEqual("beebob", subscribers52[0]);
      Assert.AreEqual("other", subscribers52[1]);
    }
  }


  public class MessageToSubscribe1
  {
  }


  public class MessageToSubscribe2
  {
  }
}
