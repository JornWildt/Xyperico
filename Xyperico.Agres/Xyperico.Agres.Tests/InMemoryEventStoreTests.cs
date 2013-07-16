using NUnit.Framework;
using Xyperico.Agres.EventStore;


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