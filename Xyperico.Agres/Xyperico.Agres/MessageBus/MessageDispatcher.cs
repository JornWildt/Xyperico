using System;
using System.Collections.Generic;
using System.Linq;
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
        InternalRegisterMessageHandlers(assembly, messageHandlerLocator);

      SortRegisteredHandlers();
    }


    public void RegisterMessageHandlers(Assembly assembly)
    {
      InternalRegisterMessageHandlers(assembly, new DefaultMessageHandlerConvention());

      SortRegisteredHandlers();
    }


    public void RegisterMessageHandlers(Assembly assembly, IMessageHandlerConvention messageHandlerConvention)
    {
      InternalRegisterMessageHandlers(assembly, messageHandlerConvention);
    }


    internal void InternalRegisterMessageHandlers(Assembly assembly, IMessageHandlerConvention messageHandlerConvention)
    {
      Condition.Requires(assembly, "assembly").IsNotNull();
      Condition.Requires(messageHandlerConvention, "messageHandlerLocator").IsNotNull();

      Logger.DebugFormat("Scanning assembly '{0}' for message handlers. Using message handler convention '{1}'.", assembly, messageHandlerConvention);

      // Go through all types and all their methods looking for message handlers
      foreach (Type handler in assembly.GetTypes())
      {
        foreach (MethodInfo method in handler.GetMethods())
        {
          // Only test single-parameter methods for handlers
          ParameterInfo[] parameters = method.GetParameters();
          if (parameters.Length == 1)
          {
            Type baseType = parameters[0].ParameterType;

            if (messageHandlerConvention.IsMessageHandler(method, baseType))
            {
              // Go through all other messages that inherit from the handlers message parameter
              foreach (Type messageType in GetAllInheritedClasses(baseType))
              {
                Logger.DebugFormat("Register message handler '{0}' on '{1}' for message type '{2}'.", method, method.DeclaringType, messageType);
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
    }

    
    private IEnumerable<Type> GetAllInheritedClasses(Type baseType)
    {
      return baseType.Assembly.GetTypes().Where(t => t.IsSubclassOf(baseType))
             .Union(new Type[] { baseType });
    }


    void SortRegisteredHandlers()
    {
      foreach (var entry in MessageHandlerIndex)
      {
        entry.Value.Sort(new InheritanceComparerer());
      }
    }

    
    private class InheritanceComparerer : IComparer<MessageHandlerRegistration>
    {
      public int Compare(MessageHandlerRegistration x, MessageHandlerRegistration y)
      {
        if (x.ParameterType == y.ParameterType)
          return 0;
        if (x.ParameterType.IsSubclassOf(y.ParameterType))
          return 1;
        return -1;
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
