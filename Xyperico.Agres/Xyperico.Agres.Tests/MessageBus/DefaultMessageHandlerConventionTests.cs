using NUnit.Framework;
using Xyperico.Agres.MessageBus;
using System.Reflection;
using System;


namespace Xyperico.Agres.Tests.MessageBus
{
  [TestFixture]
  public class DefaultMessageHandlerConventionTests : TestHelper
  {
    [Test]
    public void CanIdentifyMessageHandler()
    {
      // Arrange
      DefaultMessageHandlerConvention c = new DefaultMessageHandlerConvention();
      MethodInfo m_Handle_MyMessage = typeof(MyMessageHandler).GetMethod("Handle", new Type[] { typeof(MyMessage) });
      MethodInfo m_Handle_MyOtherMessage = typeof(MyMessageHandler).GetMethod("Handle", new Type[] { typeof(MyOtherMessage) });
      MethodInfo m_Use_MyMessage = typeof(MyMessageHandler).GetMethod("Use", new Type[] { typeof(MyMessage) });
      MethodInfo m_Handle_MyNonMessage = typeof(MyMessageHandler).GetMethod("Handle", new Type[] { typeof(MyNonMessage) });
      MethodInfo m_Handle_int = typeof(MyMessageHandler).GetMethod("Handle", new Type[] { typeof(int) });

      // Assert
      Assert.IsNotNull(m_Handle_MyMessage);
      Assert.IsNotNull(m_Handle_MyOtherMessage);
      Assert.IsNotNull(m_Use_MyMessage);
      Assert.IsNotNull(m_Handle_MyNonMessage);
      Assert.IsNotNull(m_Handle_int);

      Assert.IsTrue(c.IsMessageHandler(m_Handle_MyMessage, m_Handle_MyMessage.GetParameters()[0].ParameterType));
      Assert.IsFalse(c.IsMessageHandler(m_Handle_MyOtherMessage, m_Handle_MyOtherMessage.GetParameters()[0].ParameterType));
      Assert.IsFalse(c.IsMessageHandler(m_Use_MyMessage, m_Use_MyMessage.GetParameters()[0].ParameterType));
      Assert.IsFalse(c.IsMessageHandler(m_Handle_MyNonMessage, m_Handle_MyNonMessage.GetParameters()[0].ParameterType));
      Assert.IsFalse(c.IsMessageHandler(m_Handle_int, m_Handle_int.GetParameters()[0].ParameterType));
    }


    class MyMessageHandler : IHandleMessage<MyMessage>
    {
      public void Handle(MyMessage m)
      {
      }

      public void Handle(MyOtherMessage m)
      {
      }

      public void Use(MyMessage m)
      {
      }

      public void Handle(MyNonMessage m)
      {
      }

      public void Handle(int i)
      {
      }
    }


    class MyMessage : IMessage
    {
    }


    class MyOtherMessage : IMessage
    {
    }


    class MyNonMessage
    {
    }
  }
}
