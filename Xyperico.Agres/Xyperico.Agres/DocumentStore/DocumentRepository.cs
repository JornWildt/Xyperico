using CuttingEdge.Conditions;


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
