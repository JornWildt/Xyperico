using System.Linq;
using NUnit.Framework;
using Xyperico.Agres.MessageBus;
using System.Collections.Generic;
using Xyperico.Base;
using Xyperico.Base.ObjectContainers;
using System;


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


    [Test]
    public void WhenInstantiatingMessageHandlerItPerformsDependencyInjection()
    {
      // Arrange
      DispatcherObjectContainer.AddComponent<IMyService, MyService>();
      Dispatcher.RegisterMessageHandlers(typeof(MyServiceMessage).Assembly);
      MyServiceMessage message = new MyServiceMessage();
      Count = 0;

      // Act
      Dispatcher.Dispatch(message);

      // Assert
      Assert.AreEqual(999, Count);
    }


    [TestCase(typeof(MyBaseMessage), "BASE:")]
    [TestCase(typeof(MyInheritedMessage1), "BASE:INH1:")]
    [TestCase(typeof(MyInheritedMessage2), "BASE:INH1:INH2:")]
    public void ItInvokesMessageHandlersOrderedByInheritance(Type msgType, string expectedOutput)
    {
      // Arrange
      Dispatcher.RegisterMessageHandlers(msgType.Assembly);
      IMessage message = (IMessage)Activator.CreateInstance(msgType);
      MyInheritanceMessageHandler.Sequence = "";

      // Act
      Dispatcher.Dispatch(message);

      // Assert
      Assert.AreEqual(expectedOutput, MyInheritanceMessageHandler.Sequence);
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


    public interface IMyService
    {
      int Value { get; }
    }


    public class MyService : IMyService
    {
      public int Value
      {
        get { return 999; }
      }
    }


    public class MyServiceMessage : IMessage
    {
    }


    public class MyServiceMessageHandler : IHandleMessage<MyServiceMessage>
    {
      public IMyService MyService { get; set; }

      public void Handle(MyServiceMessage message)
      {
        Assert.IsNotNull(MyService, "MyService must be set by dependency injection.");
        Count = MyService.Value;
      }
    }


    public class MyBaseMessage : IMessage
    {
    }


    public class MyInheritedMessage1 : MyBaseMessage
    {
    }


    public class MyInheritedMessage2 : MyInheritedMessage1
    {
    }


    public class MyInheritanceMessageHandler
      : IHandleMessage<MyBaseMessage>,
        IHandleMessage<MyInheritedMessage1>,
        IHandleMessage<MyInheritedMessage2>
    {
      public static string Sequence;

      public void Handle(MyBaseMessage message)
      {
        Sequence += "BASE:";
      }

      public void Handle(MyInheritedMessage1 message)
      {
        Sequence += "INH1:";
      }

      public void Handle(MyInheritedMessage2 message)
      {
        Sequence += "INH2:";
      }
    }

  }
}
