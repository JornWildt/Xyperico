using NUnit.Framework;
using Xyperico.Agres.EventStore;
using Xyperico.Agres.Sql;


namespace Xyperico.Agres.Tests
{
  [TestFixture]
  public class SqlEventStoreTests : AbstractEventStoreTests
  {
    const string SqlConnectionString = "Server=localhost;Database=CommunitySite;User Id=comsite;Password=123456;";

    
    protected override void SetUp()
    {
      base.SetUp();
      try { SqlAppendOnlyStore.DropTable(SqlConnectionString); }
      catch { }
      SqlAppendOnlyStore.CreateTable(SqlConnectionString);
    }


    protected override IAppendOnlyStore BuildAppendOnlyStore()
    {
      return new SqlAppendOnlyStore(SqlConnectionString, false);
    }
  }
}
