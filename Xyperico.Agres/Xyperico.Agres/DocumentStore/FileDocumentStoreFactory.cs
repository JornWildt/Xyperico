﻿using CuttingEdge.Conditions;


namespace Xyperico.Agres.DocumentStore
{
  public class FileDocumentStoreFactory : IDocumentStoreFactory
  {
    protected string BaseDir { get; set; }

    protected IDocumentSerializer Serializer { get; set; }


    public FileDocumentStoreFactory(string baseDir, IDocumentSerializer serializer)
    {
      Condition.Requires(baseDir, "baseDir").IsNotNull();
      Condition.Requires(serializer, "serializer").IsNotNull();

      BaseDir = baseDir;
      Serializer = serializer;
    }


    public IDocumentStore<TKey, TValue> Create<TKey, TValue>()
    {
      return new FileDocumentStore<TKey, TValue>(BaseDir, Serializer);
    }
  }
}
