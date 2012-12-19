using Castle.Windsor;
using Xyperico.Base.ObjectContainers;
using System;


namespace Xyperico.Base
{
  public static class ObjectContainer
  {
    public static IObjectContainer Container = new CastleObjectContainer(Xyperico.Base.ObjectContainers.Castle.Container);


    public static T Instantiate<T>(this IObjectContainer container, object namedConstructorArgs = null)
    {
      return Instantiate<T>(namedConstructorArgs, container);
    }


    public static T Instantiate<T>(object namedConstructorArgs = null, IObjectContainer container = null)
    {
      return (T)Instantiate(typeof(T), namedConstructorArgs, container);
    }


    public static object Instantiate(this IObjectContainer container, Type t, object namedConstructorArgs = null)
    {
      return Instantiate(t, namedConstructorArgs, container);
    }


    public static object Instantiate(Type t, object namedConstructorArgs = null, IObjectContainer container = null)
    {
      if (container == null)
        container = Container;

      object result = null;

      try
      {
        if (!container.HasComponent(t))
          container.AddTransientComponent(t, t);

        result = container.Resolve(t, namedConstructorArgs ?? new { });
      }
      catch (Exception ex)
      {
        throw new ArgumentException(string.Format("Could not instantiate type '{0}'. Got error: {1}.", t, ex.Message), ex);
      }

      if (result == null)
        throw new ArgumentException(string.Format("Could not instantiate type '{0}'. No internal error message available.", t));

      if (!t.IsInstanceOfType(result))
        throw new ArgumentException(string.Format("Could not use instantiated type '{0}' since it does not inherit from '{1}'.", result.GetType(), t));

      return result;
    }
  }
}
