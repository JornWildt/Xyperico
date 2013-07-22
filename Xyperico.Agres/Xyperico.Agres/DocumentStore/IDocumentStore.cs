namespace Xyperico.Agres.DocumentStore
{
  public interface IDocumentStore<TKey, TValue>
  {
    void Put(TKey key, TValue value);
    TValue Get(TKey key);
    bool TryGet(TKey key, out TValue value);
    bool TryDelete(TKey key);
    void Clear();
  }
}
