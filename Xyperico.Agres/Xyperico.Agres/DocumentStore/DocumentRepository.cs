using CuttingEdge.Conditions;
using System;


namespace Xyperico.Agres.DocumentStore
{
  public class DocumentRepository
  {
    protected IDocumentStoreFactory Factory { get; set; }


    public DocumentRepository(IDocumentStoreFactory factory)
    {
      Condition.Requires(factory, "factory").IsNotNull();
      Factory = factory;
    }


    public void PutSingleton<TValue>(TValue value)
    {
      Factory.Create<SingletonKey, TValue>().Put(SingletonKey.Key, value);
    }


    public bool TryGetSingleton<TValue>(out TValue value)
    {
      return Factory.Create<SingletonKey, TValue>().TryGet(SingletonKey.Key, out value);
    }


    public TValue GetSingleton<TValue>() where TValue : new()
    {
      return GetSingleton(() => new TValue());
    }


    public TValue GetSingleton<TValue>(Func<TValue> builder)
    {
      TValue value;
      if (Factory.Create<SingletonKey, TValue>().TryGet(SingletonKey.Key, out value))
        return value;
      return builder();
    }


    public bool TryDeleteSingleton<TValue>()
    {
      return Factory.Create<SingletonKey, TValue>().TryDelete(SingletonKey.Key);
    }


    class SingletonKey
    {
      public static readonly SingletonKey Key = new SingletonKey();

      public override string ToString()
      {
        return "Singleton";
      }
    }
  }
}
