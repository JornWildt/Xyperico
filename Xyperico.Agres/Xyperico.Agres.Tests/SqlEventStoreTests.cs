using NUnit.Framework;
using Xyperico.Agres.Sql;


namespace Xyperico.Agres.Tests
{
  [TestFixture]
  public class SqlEventStoreTests : EventStoreTests
  {
    const string SqlConnectionString = "Server=localhost;Database=CommunitySite;User Id=comsite;Password=123456;";


    protected override IAppendOnlyStore BuildAppendOnlyStore()
    {
      return new SqlAppendOnlyStore(SqlConnectionString, false);
    }
  }
}
