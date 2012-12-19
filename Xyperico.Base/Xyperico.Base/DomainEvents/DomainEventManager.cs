using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;


namespace Xyperico.Base.DomainEvents
{
  // Thanks to Udi Dahan for original implementation
  // See http://www.udidahan.com/2009/06/14/domain-events-salvation/
  public static class DomainEventManager
  {
    static ILog Logger = LogManager.GetLogger(typeof(DomainEventManager));


    [ThreadStatic] // So that each thread has its own callbacks
    private static List<Delegate> Actions;


    public static IObjectContainer Container { get; set; }


    // Registers a callback for the given domain event
    public static void Register<T>(Action<T> callback) where T : IDomainEvent
    {
      if (Actions == null)
        Actions = new List<Delegate>();

      Actions.Add(callback);
    }


    // Registers a scoped callback for the given domain event
    public static IDisposable RegisterScopedAction<T>(Action<T> callback) where T : IDomainEvent
    {
      if (Actions == null)
        Actions = new List<Delegate>();
      Actions.Add(callback);
      return new DisposableRegistration<T>(callback, Actions);
    }


    // Clears callbacks passed to Register on the current thread
    public static void ClearCallbacks()
    {
      Actions = null;
    }


    // Raises the given domain event
    public static void Raise<T>(T args) where T : IDomainEvent
    {
      if (Container != null)
        foreach (var handler in Container.ResolveAll<IDomainEventHandler<T>>())
          handler.Handle(args);

      if (Actions != null)
        foreach (var action in Actions)
          if (action is Action<T>)
            ((Action<T>)action)(args);
    }


    public static void RegisterAllEventHandlers(IEnumerable<Assembly> assemblies)
    {
      if (Container == null)
        throw new ArgumentNullException("No container has yet been registered for DomainEvents");

      foreach (Assembly a in assemblies)
      {
        foreach (Type t in a.GetTypes())
        {
          foreach (Type handlerInterface in GetDomainEventHandlerInterfaces(t))
          {
            Logger.DebugFormat("Registering handler {0} for {1}", t, handlerInterface);
            Container.AddComponent(handlerInterface.ToString() + t.ToString(), handlerInterface, t);
          }
        }
      }
    }


    public static void ExpectRaised<T>(Action action, Action<T> tester) where T : class, IDomainEvent
    {
      T msg = null;
      using (RegisterScopedAction<T>(m => msg = m))
      {
        action();
      }
      if (msg != null)
        tester(msg);
      else
        throw new InvalidOperationException(string.Format("No {0} raised as expected", typeof(T)));
    }


    private static IEnumerable<Type> GetDomainEventHandlerInterfaces(Type t)
    {
      if (!t.IsAbstract)
      {
        foreach (Type interfaceType in t.GetInterfaces())
        {
          if (interfaceType.IsGenericType)
          {
            Type[] args = interfaceType.GetGenericArguments();
            if (args.Length == 1)
            {
              if (typeof(IDomainEvent).IsAssignableFrom(args[0]))
              {
                Type handlerType = typeof(IDomainEventHandler<>).MakeGenericType(args[0]);
                if (handlerType.IsAssignableFrom(t))
                  yield return interfaceType;
              }
            }
          }
        }
      }
    }


    private class DisposableRegistration<T> : IDisposable
    {
      Action<T> Callback;
      List<Delegate> Actions;

      public DisposableRegistration(Action<T> callback, List<Delegate> actions)
      {
        Callback = callback;
        Actions = actions;
      }

      void IDisposable.Dispose()
      {
        Actions.Remove(Callback);
      }
    }
  }
}
