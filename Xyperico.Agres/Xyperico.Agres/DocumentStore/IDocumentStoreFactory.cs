namespace Xyperico.Agres.DocumentStore
{
  public interface IDocumentStoreFactory
  {
    IDocumentStore<TKey, TValue> Create<TKey, TValue>();
  }
}
