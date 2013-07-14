using System;


namespace Xyperico.Base
{
  public interface IObjectResolver
  {
    bool HasComponent(Type t);

    T Resolve<T>();
    object Resolve(Type service);
    object Resolve(Type service, object argumentsAsAnonymousType);

    Array ResolveAll(Type service);
    T[] ResolveAll<T>();
  }


  public interface IObjectContainer : IObjectResolver
  {
    IObjectContainer AddComponent<T1, T2>() where T2 : T1 where T1 : class;
    IObjectContainer AddComponent(Type serviceType, Type classType);
    IObjectContainer AddComponent(string key, Type serviceType, Type classType);

    void AddTransientComponent(Type t1, Type t2);

    void RegisterInstance<T1>(T1 instance) where T1 : class;
  }
}
