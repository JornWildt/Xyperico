using System;
using System.Reflection;


namespace Xyperico.Agres.MessageBus
{
  public interface IMessageHandlerConvention
  {
    bool IsMessageHandler(MethodInfo method, Type parameter);
  }
}
