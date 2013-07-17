using Xyperico.Agres.EventStore;


namespace Xyperico.Agres.SQLite
{
  public static class SQLiteEventStoreConfigurationExtensions
  {
    public static EventStoreConfiguration WithSQLiteEventStore(this EventStoreConfiguration cfg, string connectionString, bool createTable)
    {
      if (createTable)
        SQLiteAppendOnlyStore.CreateTableIfNotExists(connectionString);
      IAppendOnlyStore aStore = new SQLiteAppendOnlyStore(connectionString);
      Xyperico.Agres.EventStore.EventStoreConfigurationExtensions.SetAppendOnlyStore(cfg, aStore);
      return cfg;
    }
  }
}
