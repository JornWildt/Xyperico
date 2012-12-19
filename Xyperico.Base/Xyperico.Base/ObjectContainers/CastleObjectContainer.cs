using System;
using Castle.Windsor;
using Castle.MicroKernel.Registration;


namespace Xyperico.Base.ObjectContainers
{
  public class CastleObjectContainer : IObjectContainer
  {
    private IWindsorContainer Container { get; set; }


    public CastleObjectContainer(IWindsorContainer container)
    {
      Container = container;
    }


    #region IObjectContainer Members

    public IObjectContainer AddComponent<I, T>() 
      where T : I
      where I : class
    {
      Container.Register(Component.For<I>().ImplementedBy<T>());
      return this;
    }


    public IObjectContainer AddComponent(string key, Type serviceType, Type classType)
    {
      Container.Register(Component.For(serviceType).ImplementedBy(classType).Named(key));
      return this;
    }


    public void AddTransientComponent(Type t1, Type t2)
    {
      Container.Register(
         Component
            .For(t1)
            .ImplementedBy(t2)
            .LifeStyle.Transient);
    }


    public void RegisterInstance<T1>(T1 instance) where T1 : class
    {
      Container.Register(Component.For<T1>().Instance(instance));
    }


    public bool HasComponent(Type t)
    {
      return Container.Kernel.HasComponent(t);
    }


    public T Resolve<T>()
    {
      return Container.Resolve<T>();
    }


    public object Resolve(Type service)
    {
      return Container.Resolve(service);
    }


    public object Resolve(Type service, object argumentsAsAnonymousType)
    {
      return Container.Resolve(service, argumentsAsAnonymousType);
    }


    public Array ResolveAll(Type service)
    {
      return Container.ResolveAll(service);
    }


    public T[] ResolveAll<T>()
    {
      return Container.ResolveAll<T>();
    }

    #endregion
  }
}
