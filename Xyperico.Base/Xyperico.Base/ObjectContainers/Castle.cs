using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;


namespace Xyperico.Base.ObjectContainers
{
  public static class Castle
  {
    public static IWindsorContainer Container = new WindsorContainer();


    public static void RegisterDependency(Type t1, Type t2)
    {
      Container.Register(Component.For(t1).ImplementedBy(t2).Named(t1.ToString()));
    }


    public static T Instantiate<T>(object namedConstructorArgs = null, IWindsorContainer container = null)
    {
      Type t = typeof(T);
      object instance = Instantiate(typeof(T), namedConstructorArgs, container);
      return (T)instance;
    }


    public static object Instantiate(Type t, object namedConstructorArgs = null, IWindsorContainer container = null)
    {
      if (container == null)
        container = Container;

      object result = null;

      try
      {
        if (!container.Kernel.HasComponent(t))
        {
          container.Register(
             Component
                .For(t)
                .ImplementedBy(t)
                .LifeStyle.Transient);
        }

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
