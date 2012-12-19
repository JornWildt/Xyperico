using System.Collections.Generic;


namespace Xyperico.Base.Testing
{
  public abstract class DisposingBuilder<T> : IDisposingBuilder<T>
    where T : class
  {
    private List<T> CreatedItems = new List<T>();


    public virtual T RegisterInstance(T item)
    {
      CreatedItems.Add(item);
      return item;
    }


    protected abstract void DisposeInstance(T item);


    #region IDisposingBuilder Members

    public void DisposeInstances()
    {
      foreach (T item in CreatedItems)
        DisposeInstance(item);
    }

    #endregion
  }
}
