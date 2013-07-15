using System;
using System.Collections.Generic;
using System.Reflection;
using CuttingEdge.Conditions;
using Xyperico.Base;
using log4net;


namespace Xyperico.Agres.MessageBus
{
  public class MessageDispatcher
  {
    private static ILog Logger = LogManager.GetLogger(typeof(MessageDispatcher));

    public IObjectContainer ObjectContainer { get; set; }


    private List<MessageHandlerRegistration> MessageHandlers { get; set; }
    private Dictionary<Type, List<MessageHandlerRegistration>> MessageHandlerIndex { get; set; }

    
    public MessageDispatcher(IObjectContainer objectContainer)
    {
      Condition.Requires(objectContainer, "objectContainer").IsNotNull();
      ObjectContainer = objectContainer;
      MessageHandlers = new List<MessageHandlerRegistration>();
      MessageHandlerIndex = new Dictionary<Type, List<MessageHandlerRegistration>>();
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


    public void RegisterMessageHandlers(Assembly assembly, IMessageHandlerConvention messageHandlerConvention)
    {
      Condition.Requires(assembly, "assembly").IsNotNull();
      Condition.Requires(messageHandlerConvention, "messageHandlerLocator").IsNotNull();

      Logger.DebugFormat("Scanning assembly '{0}' for message handlers. Using message handler convention '{1}'.", assembly, messageHandlerConvention);

      foreach (Type handler in assembly.GetTypes())
      {
        foreach (MethodInfo method in handler.GetMethods())
        {
          ParameterInfo[] parameters = method.GetParameters();
          if (parameters.Length == 1)
          {
            Type messageType = parameters[0].ParameterType;
            if (messageHandlerConvention.IsMessageHandler(method, messageType))
            {
              Logger.DebugFormat("Found message handler '{0}' on '{1}' for message type '{2}'.", method, method.DeclaringType, messageType);
              MessageHandlerRegistration registration = new MessageHandlerRegistration(messageType, method);
              MessageHandlers.Add(registration);
              if (!MessageHandlerIndex.ContainsKey(messageType))
                MessageHandlerIndex.Add(messageType, new List<MessageHandlerRegistration>());
              MessageHandlerIndex[messageType].Add(registration);
              if (!ObjectContainer.HasComponent(method.DeclaringType))
                ObjectContainer.AddComponent(method.DeclaringType, method.DeclaringType);
            }
          }
        }
      }
    }


    public void Dispatch(object message)
    {
      Condition.Requires(message, "message").IsNotNull();
      List<MessageHandlerRegistration> registrations;
      if (MessageHandlerIndex.TryGetValue(message.GetType(), out registrations))
      {
        foreach (MessageHandlerRegistration handler in registrations)
        {
          Logger.DebugFormat("Dispatching message '{0}' to '{1}'.", message, handler);
          handler.Invoke(ObjectContainer, message);
        }
      }
    }


    public IEnumerable<MessageHandlerRegistration> GetMessageHandlerRegistrations()
    {
      return MessageHandlers;
    }
  }
}
