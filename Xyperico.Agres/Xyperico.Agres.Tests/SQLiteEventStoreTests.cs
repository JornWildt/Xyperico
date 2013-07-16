using NUnit.Framework;
using Xyperico.Agres.EventStore;
using Xyperico.Agres.Sql;


namespace Xyperico.Agres.Tests
{
  [TestFixture]
  public class SQLiteEventStoreTests : AbstractEventStoreTests
  {
    const string SqlConnectionString = "Data Source=C:\\tmp\\Xyperico.Agres.Tests.db";

    protected override void SetUp()
    {
      base.SetUp();
      try { SQLiteAppendOnlyStore.DropTable(SqlConnectionString); } catch { }
      SQLiteAppendOnlyStore.CreateTable(SqlConnectionString);
    }


    protected override IAppendOnlyStore BuildAppendOnlyStore()
    {
      return new SQLiteAppendOnlyStore(SqlConnectionString, false);
    }
  }
}
