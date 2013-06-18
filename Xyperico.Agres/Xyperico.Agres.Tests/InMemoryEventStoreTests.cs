using NUnit.Framework;
using Xyperico.Agres.InMemoryEventStore;


namespace Xyperico.Agres.Tests
{
  [TestFixture]
  public class InMemoryEventStoreTests : EventStoreTests
  {
    protected override IAppendOnlyStore BuildAppendOnlyStore()
    {
      return new InMemoryAppendOnlyStore();
    }
  }
}