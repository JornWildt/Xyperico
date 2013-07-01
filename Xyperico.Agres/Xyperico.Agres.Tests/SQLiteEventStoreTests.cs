using NUnit.Framework;
using Xyperico.Agres.Sql;


namespace Xyperico.Agres.Tests
{
  [TestFixture]
  public class SQLiteEventStoreTests : AbstractEventStoreTests
  {
    const string SqlConnectionString = "Data Source=C:\\tmp\\Xyperico.Agres.Tests.db";

    protected override void TestFixtureSetUp()
    {
      base.TestFixtureSetUp();
      SQLiteAppendOnlyStore.CreateTable(SqlConnectionString);
    }


    protected override void TestFixtureTearDown()
    {
      SQLiteAppendOnlyStore.DropTable(SqlConnectionString);
      base.TestFixtureTearDown();
    }


    protected override IAppendOnlyStore BuildAppendOnlyStore()
    {
      return new SQLiteAppendOnlyStore(SqlConnectionString, false);
    }
  }
}
