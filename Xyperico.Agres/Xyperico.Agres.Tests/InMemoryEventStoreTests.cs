using NUnit.Framework;
using Xyperico.Agres.InMemoryEventStore;


namespace Xyperico.Agres.Tests
{
  [TestFixture]
  public class InMemoryEventStoreTests : AbstractEventStoreTests
  {
    protected override IAppendOnlyStore BuildAppendOnlyStore()
    {
      return new InMemoryAppendOnlyStore();
    }
  }
}