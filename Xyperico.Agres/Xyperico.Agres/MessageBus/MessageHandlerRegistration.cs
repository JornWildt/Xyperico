using System;
using System.Reflection;
using Xyperico.Base;
using CuttingEdge.Conditions;


namespace Xyperico.Agres.MessageBus
{
  public class MessageHandlerRegistration
  {
    public Type MessageType { get; protected set; }
    public MethodInfo Method { get; protected set; }


    public MessageHandlerRegistration(Type messageType, MethodInfo method)
    {
      Condition.Requires(messageType, "messageType").IsNotNull();
      Condition.Requires(method, "method").IsNotNull();
      MessageType = messageType;
      Method = method;
    }


    public bool IsHandlerFor(object message)
    {
      return MessageType.IsAssignableFrom(message.GetType());
    }


    public void Invoke(IObjectContainer dependencyResolver, object message)
    {
      object handlerInstance = dependencyResolver.Resolve(Method.DeclaringType);
      Method.Invoke(handlerInstance, new object[] { message });
    }
  }
}
