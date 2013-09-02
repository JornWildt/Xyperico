using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Xyperico.Agres.MessageBus.Subscription;
using Xyperico.Agres.DocumentStore;
using Xyperico.Agres.JsonNet;
using Xyperico.Agres.MessageBus;


namespace Xyperico.Agres.Tests.MessageBus
{
  [TestFixture]
  public class SubscriptionServiceTests : TestHelper
  {
    QueueName MyQueueName = "Wolla";
    ISubscriptionService Service;


    protected override void SetUp()
    {
      base.SetUp();
      IDocumentSerializer serializer = new JsonNetDocumentSerializer();
      IDocumentStoreFactory store = new FileDocumentStoreFactory(StorageBaseDir, serializer);
      Service = new SubscriptionService(store, MyQueueName);
      store.Create<Type, SubscriptionRegistration>().Clear();
    }


    [Test]
    public void CanAddAndGetSubscribers()
    {
      // Act
      IList<QueueName> subscribers11 = Service.GetSubscribers(typeof(MessageToSubscribe1)).ToList();
      IList<QueueName> subscribers12 = Service.GetSubscribers(typeof(MessageToSubscribe2)).ToList();

      Service.AddSubscriber(typeof(MessageToSubscribe1), new QueueName("beebob"));
      IList<QueueName> subscribers21 = Service.GetSubscribers(typeof(MessageToSubscribe1)).ToList();
      IList<QueueName> subscribers22 = Service.GetSubscribers(typeof(MessageToSubscribe2)).ToList();

      Service.AddSubscriber(typeof(MessageToSubscribe2), new QueueName("beebob"));
      IList<QueueName> subscribers31 = Service.GetSubscribers(typeof(MessageToSubscribe1)).ToList();
      IList<QueueName> subscribers32 = Service.GetSubscribers(typeof(MessageToSubscribe2)).ToList();

      Service.AddSubscriber(typeof(MessageToSubscribe1), new QueueName("other"));
      IList<QueueName> subscribers41 = Service.GetSubscribers(typeof(MessageToSubscribe1)).ToList();
      IList<QueueName> subscribers42 = Service.GetSubscribers(typeof(MessageToSubscribe2)).ToList();

      Service.AddSubscriber(typeof(MessageToSubscribe2), new QueueName("other"));
      IList<QueueName> subscribers51 = Service.GetSubscribers(typeof(MessageToSubscribe1)).ToList();
      IList<QueueName> subscribers52 = Service.GetSubscribers(typeof(MessageToSubscribe2)).ToList();

      // Assert
      Assert.AreEqual(0, subscribers11.Count);
      Assert.AreEqual(0, subscribers12.Count);
      
      Assert.AreEqual(1, subscribers21.Count);
      Assert.AreEqual(0, subscribers22.Count);
      Assert.AreEqual(new QueueName("beebob"), subscribers21[0]);

      Assert.AreEqual(1, subscribers31.Count);
      Assert.AreEqual(1, subscribers32.Count);
      Assert.AreEqual(new QueueName("beebob"), subscribers31[0]);
      Assert.AreEqual(new QueueName("beebob"), subscribers32[0]);

      Assert.AreEqual(2, subscribers41.Count);
      Assert.AreEqual(1, subscribers42.Count);
      Assert.AreEqual(new QueueName("beebob"), subscribers41[0]);
      Assert.AreEqual(new QueueName("other"), subscribers41[1]);
      Assert.AreEqual(new QueueName("beebob"), subscribers42[0]);

      Assert.AreEqual(2, subscribers51.Count);
      Assert.AreEqual(2, subscribers52.Count);
      Assert.AreEqual(new QueueName("beebob"), subscribers51[0]);
      Assert.AreEqual(new QueueName("other"), subscribers51[1]);
      Assert.AreEqual(new QueueName("beebob"), subscribers52[0]);
      Assert.AreEqual(new QueueName("other"), subscribers52[1]);
    }


    [Test]
    public void WhenCreatingWithDefaultConstructorValuesItInitializesFromConfigFile()
    {
      // Arrange
      IDocumentSerializer serializer = new JsonNetDocumentSerializer();
      IDocumentStoreFactory store = new FileDocumentStoreFactory(StorageBaseDir, serializer);
      SubscriptionService service = new SubscriptionService(store);

      Assert.AreEqual("Zebra", service.InputQueueName.Name);

      IList<RouteRegistration> routes = service.GetRoutes().ToList();
      Assert.AreEqual(2, routes.Count);
      Assert.AreEqual("Abc.Def", routes[0].MessageFilter);
      Assert.AreEqual("Alibaba", routes[0].Destination.Name);
      Assert.AreEqual("Xyz.Qwe", routes[1].MessageFilter);
      Assert.AreEqual("RobinHat", routes[1].Destination.Name);
    }


    [Test]
    public void WhenSubscribingItFollowsMatchingRoutes()
    {
      // Arrange
      MessageSinkStub sink = new MessageSinkStub();
      Service.AddRoute("Xyperico.Agres.Tests.MessageBus", "Trilian");
      Service.AddRoute("Rofl.abc", "Max");

      // Act
      Service.Subscribe(typeof(MessageToSubscribe1), sink);

      // Assert
      Assert.AreEqual("Trilian", sink.LastDestination.Name);
      Assert.IsInstanceOf<SubscribeCommand>(sink.LastMessage.Body);
      SubscribeCommand sc = (SubscribeCommand)sink.LastMessage.Body;
      Assert.AreEqual(MyQueueName.Name, sc.SubscriberQueueName);
      Assert.AreEqual(typeof(MessageToSubscribe1).AssemblyQualifiedName, sc.SubscribedMessagesTypeName);
    }


    [TestCase("Xyperico.Agres.Tests", "Trillian", typeof(MessageToSubscribe1))]
    public void WhenSubscribingItChecksPrefixOfMessageFilter(string filter, string destination, Type message)
    {
      MessageSinkStub sink = new MessageSinkStub();
      Service.AddRoute(filter, destination);

      // Act
      Service.Subscribe(message, sink);

      // Assert
      Assert.AreEqual(destination, sink.LastDestination.Name);
      Assert.IsInstanceOf<SubscribeCommand>(sink.LastMessage.Body);
      SubscribeCommand sc = (SubscribeCommand)sink.LastMessage.Body;
      Assert.AreEqual(message.AssemblyQualifiedName, sc.SubscribedMessagesTypeName);
    }



    [Test]
    public void ItCanHandleSubscribeMessage()
    {
      // Arrange
      SubscriptionMessageHandlers handlers = new SubscriptionMessageHandlers { SubscriptionService = Service };
      SubscribeCommand cmd = new SubscribeCommand(typeof(MessageToSubscribe1), "WhollyBob");

      // Act
      handlers.Handle(cmd);

      // Assert
      IList<QueueName> subscribers = Service.GetSubscribers(typeof(MessageToSubscribe1)).ToList();
      Assert.AreEqual(1, subscribers.Count);
      Assert.AreEqual(new QueueName("WhollyBob"), subscribers[0]);
    }


    [Test]
    public void WhenAddingSameRouteTwiceItThrows()
    {
      // Arrange
      Service.AddRoute("Rofl.abc", "Max");

      // Act + Assert
      AssertThrows<InvalidOperationException>(() => Service.AddRoute("Rofl.abc", "Max"));
    }


    [Test]
    public void WhenRegisteringExistingSubscriberItDoesNotCreateMultipleRegistrations()
    {
      // Arrange
      Service.AddSubscriber(typeof(MessageToSubscribe1), "Mamma");
      IList<QueueName> s1 = Service.GetSubscribers(typeof(MessageToSubscribe1)).ToList();

      // Act
      Service.AddSubscriber(typeof(MessageToSubscribe1), "Mamma");
      IList<QueueName> s2 = Service.GetSubscribers(typeof(MessageToSubscribe1)).ToList();

      // Assert
      Assert.AreEqual(1, s1.Count);
      Assert.AreEqual(1, s2.Count);
    }


    private class MessageSinkStub : IMessageSink
    {
      public QueueName LastDestination;
      public Message LastMessage;

      public void Send(QueueName destination, Message m)
      {
        LastDestination = destination;
        LastMessage = m;
      }

      public void Dispose()
      {
      }
    }

  }


  public class MessageToSubscribe1
  {
  }


  public class MessageToSubscribe2
  {
  }
}
