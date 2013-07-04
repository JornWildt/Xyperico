﻿using NUnit.Framework;
using Xyperico.Agres.DocumentStore;


namespace Xyperico.Agres.Tests.DocumentStore
{
  [TestFixture]
  public class FileDocumentStoreTests : AbstractDocumentStoreTests
  {
    protected override IDocumentStore<string, long> BuildDocumentStore_Int()
    {
      DotNetBinaryStreamSerializer serializer = new DotNetBinaryStreamSerializer();
      return new FileDocumentStore<string, long>(StorageBaseDir, serializer);
    }


    protected override IDocumentStore<string, MySerializableData> BuildDocumentStore_Class()
    {
      DotNetBinaryStreamSerializer serializer = new DotNetBinaryStreamSerializer();
      return new FileDocumentStore<string, MySerializableData>(StorageBaseDir, serializer);
    }
  }
}
