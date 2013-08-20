using Xyperico.Agres.EventStore;


namespace Xyperico.Agres.Sql
{
  public static class SqlEventStoreConfigurationExtensions
  {
    /// <summary>
    /// Use SQL server for backend append-only store.
    /// </summary>
    /// <param name="cfg"></param>
    /// <param name="connectionString"></param>
    /// <param name="createTable"></param>
    /// <returns></returns>
    public static EventStoreConfiguration WithSqlServerEventStore(this EventStoreConfiguration cfg, string connectionString, bool createTable)
    {
      if (createTable)
        SqlAppendOnlyStore.CreateTableIfNotExists(connectionString);
      IAppendOnlyStore aStore = new SqlAppendOnlyStore(connectionString);
      Xyperico.Agres.EventStore.EventStoreConfigurationExtensions.SetAppendOnlyStore(cfg, aStore);
      return cfg;
    }
  }
}
