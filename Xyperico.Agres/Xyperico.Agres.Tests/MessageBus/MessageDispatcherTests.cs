using System.Linq;
using NUnit.Framework;
using Xyperico.Agres.MessageBus;
using System.Collections.Generic;


namespace Xyperico.Agres.Tests.MessageBus
{
  [TestFixture]
  public class MessageDispatcherTests : TestHelper
  {
    MessageDispatcher Dispatcher;


    protected override void SetUp()
    {
      base.SetUp();
      Dispatcher = new MessageDispatcher();
    }


    [Test]
    public void CanRegisterMessageHandler()
    {
      // Act
      Dispatcher.RegisterMessageHandlers(typeof(MyMessageHandler).Assembly);
      IList<MessageHandlerRegistration> handlers = Dispatcher.GetMessageHandlerRegistrations().ToList();

      // Assert
      MessageHandlerRegistration r1 = handlers.Where(h => h.MessageType == typeof(MyMessage)).FirstOrDefault();
      MessageHandlerRegistration r2 = handlers.Where(h => h.MessageType == typeof(MyOtherMessage)).FirstOrDefault();

      Assert.IsNotNull(r1, "Must find registration");
      Assert.AreEqual(typeof(MyMessage), r1.MessageType);
      Assert.AreEqual("Handle", r1.Method.Name);

      Assert.IsNull(r2);
    }


    class MyMessageHandler : IHandleMessage<MyMessage>
    {
      public void Handle(MyMessage m)
      {
      }

      // Not a handler (doesn't implement IHandleMessage<MyOtherMessage>)
      public void Handle(MyOtherMessage m)
      {
      }
    }


    class MyMessage : IMessage
    {
    }


    class MyOtherMessage : IMessage
    {
    }
  }
}
