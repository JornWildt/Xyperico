using System;


namespace Xyperico.Base
{
  /// <summary>
  /// For use in environments that do not support dependency injection.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public struct LazyObjectResolver<T>
  {
    private T _value;

    public T Value
    {
      get
      {
        if (_value == null)
          _value = ObjectContainer.Container.Resolve<T>();
        return _value;
      }
      set
      {
        _value = value;
      }
    }
  }
}
