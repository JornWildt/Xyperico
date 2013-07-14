using System;
using System.Collections.Generic;
using System.Reflection;
using CuttingEdge.Conditions;
using Xyperico.Base;


namespace Xyperico.Agres.MessageBus
{
  public class MessageDispatcher
  {
    public IObjectContainer ObjectContainer { get; set; }


    private List<MessageHandlerRegistration> MessageHandlers { get; set; }

    
    public MessageDispatcher(IObjectContainer objectContainer)
    {
      Condition.Requires(objectContainer, "objectContainer").IsNotNull();
      ObjectContainer = objectContainer;
      MessageHandlers = new List<MessageHandlerRegistration>();
    }


    public void RegisterMessageHandlers(IEnumerable<Assembly> assemblies)
    {
      RegisterMessageHandlers(assemblies, new DefaultMessageHandlerConvention());
    }


    public void RegisterMessageHandlers(IEnumerable<Assembly> assemblies, IMessageHandlerConvention messageHandlerLocator)
    {
      Condition.Requires(assemblies, "assemblies").IsNotNull();
      Condition.Requires(messageHandlerLocator, "messageHandlerLocator").IsNotNull();

      foreach (Assembly assembly in assemblies)
        RegisterMessageHandlers(assembly, messageHandlerLocator);
    }


    public void RegisterMessageHandlers(Assembly assembly)
    {
      RegisterMessageHandlers(assembly, new DefaultMessageHandlerConvention());
    }


    public void RegisterMessageHandlers(Assembly assembly, IMessageHandlerConvention messageHandlerLocator)
    {
      Condition.Requires(assembly, "assembly").IsNotNull();
      Condition.Requires(messageHandlerLocator, "messageHandlerLocator").IsNotNull();

      foreach (Type handler in assembly.GetTypes())
      {
        foreach (MethodInfo method in handler.GetMethods())
        {
          ParameterInfo[] parameters = method.GetParameters();
          if (parameters.Length == 1)
          {
            Type messageType = parameters[0].ParameterType;
            if (messageHandlerLocator.IsMessageHandler(method, messageType))
            {
              MessageHandlerRegistration registration = new MessageHandlerRegistration(messageType, method);
              MessageHandlers.Add(registration);
              if (!ObjectContainer.HasComponent(method.DeclaringType))
                ObjectContainer.AddComponent(method.DeclaringType, method.DeclaringType);
            }
          }
        }
      }
    }


    public void Dispatch(object message)
    {
      foreach (MessageHandlerRegistration handler in MessageHandlers)
      {
        if (handler.IsHandlerFor(message))
          handler.Invoke(ObjectContainer, message);
      }
    }


    public IEnumerable<MessageHandlerRegistration> GetMessageHandlerRegistrations()
    {
      return MessageHandlers;
    }
  }
}
