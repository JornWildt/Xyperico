﻿using System;
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
      IList<string> subscribers11 = Service.GetSubscribers(typeof(MessageToSubscribe1)).ToList();
      IList<string> subscribers12 = Service.GetSubscribers(typeof(MessageToSubscribe2)).ToList();

      Service.AddSubscriber(typeof(MessageToSubscribe1), new QueueName("beebob"));
      IList<string> subscribers21 = Service.GetSubscribers(typeof(MessageToSubscribe1)).ToList();
      IList<string> subscribers22 = Service.GetSubscribers(typeof(MessageToSubscribe2)).ToList();

      Service.AddSubscriber(typeof(MessageToSubscribe2), new QueueName("beebob"));
      IList<string> subscribers31 = Service.GetSubscribers(typeof(MessageToSubscribe1)).ToList();
      IList<string> subscribers32 = Service.GetSubscribers(typeof(MessageToSubscribe2)).ToList();

      Service.AddSubscriber(typeof(MessageToSubscribe1), new QueueName("other"));
      IList<string> subscribers41 = Service.GetSubscribers(typeof(MessageToSubscribe1)).ToList();
      IList<string> subscribers42 = Service.GetSubscribers(typeof(MessageToSubscribe2)).ToList();

      Service.AddSubscriber(typeof(MessageToSubscribe2), new QueueName("other"));
      IList<string> subscribers51 = Service.GetSubscribers(typeof(MessageToSubscribe1)).ToList();
      IList<string> subscribers52 = Service.GetSubscribers(typeof(MessageToSubscribe2)).ToList();

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


    [Test]
    public void ItCanHandleSubscribeMessage()
    {
      // Arrange
      SubscriptionMessageHandlers handlers = new SubscriptionMessageHandlers { SubscriptionService = Service };
      SubscribeCommand cmd = new SubscribeCommand(typeof(MessageToSubscribe1), "WhollyBob");

      // Act
      handlers.Handle(cmd);

      // Assert
      IList<string> subscribers = Service.GetSubscribers(typeof(MessageToSubscribe1)).ToList();
      Assert.AreEqual(1, subscribers.Count);
      Assert.AreEqual("WhollyBob", subscribers[0]);
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
