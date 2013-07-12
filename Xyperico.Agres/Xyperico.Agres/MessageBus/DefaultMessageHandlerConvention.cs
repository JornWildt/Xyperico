using System;


namespace Xyperico.Agres.MessageBus
{
  public class DefaultMessageHandlerConvention : IMessageHandlerConvention
  {
    public bool IsMessageHandler(System.Reflection.MethodInfo method, Type parameter)
    {
      if (method.Name != "Handle" || !typeof(IMessage).IsAssignableFrom(parameter))
        return false;

      Type expectedHandlerInterfaceType = typeof(IHandleMessage<>).MakeGenericType(new Type[] { parameter });

      foreach (Type inf in method.DeclaringType.GetInterfaces())
      {
        if (expectedHandlerInterfaceType.IsAssignableFrom(inf))
          return true;
      }

      return false;
    }
  }
}
