using Xyperico.Agres.EventStore;


namespace Xyperico.Agres.SQLite
{
  public static class SQLiteEventStoreConfigurationExtensions
  {
    //private static ILog Logger = LogManager.GetLogger(typeof(SQLiteEventStoreConfigurationExtensions));


    public static EventStoreConfiguration WithSQLiteEventStore(this EventStoreConfiguration cfg, string connectionString, bool createTable)
    {
      //Logger.Debug("Using SQLite as storage mechanism for event store");
      if (createTable)
        SQLiteAppendOnlyStore.CreateTableIfNotExists(connectionString);
      IAppendOnlyStore aStore = new SQLiteAppendOnlyStore(connectionString);
      Xyperico.Agres.EventStore.EventStoreConfigurationExtensions.SetAppendOnlyStore(cfg, aStore);
      return cfg;
    }
  }
}
