namespace Xyperico.Base.Testing
{
  public interface IDisposingBuilder
  {
    void DisposeInstances();
  }

  public interface IDisposingBuilder<T> : IDisposingBuilder
  {
    T RegisterInstance(T item);
  }
}
