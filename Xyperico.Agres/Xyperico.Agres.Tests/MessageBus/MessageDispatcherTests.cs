using System.Linq;
using NUnit.Framework;
using Xyperico.Agres.MessageBus;
using System.Collections.Generic;
using Xyperico.Base;
using Xyperico.Base.ObjectContainers;


namespace Xyperico.Agres.Tests.MessageBus
{
  [TestFixture]
  public class MessageDispatcherTests : TestHelper
  {
    MessageDispatcher Dispatcher;
    IObjectContainer DispatcherObjectContainer;


    protected override void SetUp()
    {
      base.SetUp();
      DispatcherObjectContainer = new CastleObjectContainer(new global::Castle.Windsor.WindsorContainer());
      Dispatcher = new MessageDispatcher(DispatcherObjectContainer);
    }


    [Test]
    public void CanRegisterMessageHandler()
    {
      // Act
      Dispatcher.RegisterMessageHandlers(typeof(MyMessageHandler).Assembly);
      IList<MessageHandlerRegistration> handlers = Dispatcher.GetMessageHandlerRegistrations().ToList();

      // Assert
      MessageHandlerRegistration r1 = handlers.Where(h => h.MessageType == typeof(MyMessage1)).FirstOrDefault();
      MessageHandlerRegistration r2 = handlers.Where(h => h.MessageType == typeof(MyMessage2)).FirstOrDefault();
      MessageHandlerRegistration ro = handlers.Where(h => h.MessageType == typeof(MyOtherMessage)).FirstOrDefault();

      Assert.IsNotNull(r1, "Must find registration for MyMessage1");
      Assert.AreEqual(typeof(MyMessage1), r1.MessageType);
      Assert.AreEqual("Handle", r1.Method.Name);

      Assert.IsNotNull(r1, "Must find registration for MyMessage2");
      Assert.AreEqual(typeof(MyMessage2), r2.MessageType);
      Assert.AreEqual("Handle", r2.Method.Name);

      Assert.IsNull(ro, "Must not choose MyOtherMessage method - its not a handler");
    }


    [Test]
    public void CanRegisterMultipleHandlersForOneMessageType()
    {
      // Act
      Dispatcher.RegisterMessageHandlers(typeof(MyMultiMessage).Assembly);
      
      IEnumerable<MessageHandlerRegistration> handlers = Dispatcher.GetMessageHandlerRegistrations();
      IList<MessageHandlerRegistration> registrations = handlers.Where(h => h.MessageType == typeof(MyMultiMessage)).ToList();

      MessageHandlerRegistration r1 = handlers.Where(h => h.Method.DeclaringType == typeof(MyMultiMessageHandler1)).FirstOrDefault();

      // Assert
      Assert.AreEqual(2, registrations.Count);
      Assert.IsNotNull(r1);
    }


    [Test]
    public void CanInvokeMessageHandler()
    {
      // Arrange
      Dispatcher.RegisterMessageHandlers(typeof(MyMessageHandler).Assembly);
      MyMessage1 message = new MyMessage1();
      Count = 0;

      // Act
      Dispatcher.Dispatch(message);

      // Assert
      Assert.AreEqual(1, Count);
    }


    [Test]
    public void CanInvokeMultipleMessageHandlers()
    {
      // Arrange
      Dispatcher.RegisterMessageHandlers(typeof(MyMultiMessage).Assembly);
      MyMultiMessage message = new MyMultiMessage();
      Count = 0;

      // Act
      Dispatcher.Dispatch(message);

      // Assert
      Assert.AreEqual(11, Count);
    }


    static int Count = 0;

    class MyMessageHandler
      : IHandleMessage<MyMessage1>, 
        IHandleMessage<MyMessage2>
    {
      public void Handle(MyMessage1 m)
      {
        Count++;
      }

      public void Handle(MyMessage2 message)
      {
      }

      // Not a handler (doesn't implement IHandleMessage<MyOtherMessage>)
      public void Handle(MyOtherMessage m)
      {
      }
    }


    class MyMessage1 : IMessage
    {
    }


    class MyMessage2 : IMessage
    {
    }


    class MyOtherMessage : IMessage
    {
    }


    class MyMultiMessage : IMessage
    {
    }

    
    class MyMultiMessageHandler1 : IHandleMessage<MyMultiMessage>
    {
      public void Handle(MyMultiMessage message)
      {
        Count += 1;
      }
    }


    class MyMultiMessageHandler2 : IHandleMessage<MyMultiMessage>
    {
      public void Handle(MyMultiMessage message)
      {
        Count += 10;
      }
    }
  }
}
