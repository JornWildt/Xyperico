using Xyperico.Agres.EventStore;


namespace Xyperico.Agres.SQLite
{
  public static class SQLiteEventStoreConfigurationExtensions
  {
    /// <summary>
    /// Use SQLite for backend append-only store.
    /// </summary>
    /// <param name="cfg"></param>
    /// <param name="connectionString"></param>
    /// <param name="createTable"></param>
    /// <returns></returns>
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
