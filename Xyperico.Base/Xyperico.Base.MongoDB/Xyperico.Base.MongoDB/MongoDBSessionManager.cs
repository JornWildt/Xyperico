using System;
using System.Collections.Generic;
using Xyperico.Base.Collections;
using MongoDB.Driver;
using log4net;


namespace Xyperico.Base.MongoDB
{
  public class MongoDBSessionManager
  {
    static ILog Logger = LogManager.GetLogger(typeof(MongoDBSessionManager));

    #region Thread-safe, lazy Singleton

    /// <summary>
    /// This is a thread-safe, lazy singleton.  See http://www.yoda.arachsys.com/csharp/singleton.html
    /// for more details about its implementation.
    /// </summary>
    public static MongoDBSessionManager Instance
    {
      get
      {
        return Nested.MongoDBSessionManager;
      }
    }

    /// <summary>
    /// Private constructor to enforce singleton
    /// </summary>
    private MongoDBSessionManager() { }

    /// <summary>
    /// Assists with ensuring thread-safe, lazy singleton
    /// </summary>
    private class Nested
    {
      static Nested() { }
      internal static readonly MongoDBSessionManager MongoDBSessionManager = new MongoDBSessionManager();
    }

    #endregion


    public static ConfigurationSettings.ConfigurationEntryElement GetMongoDBConfig(string configEntry)
    {
      if (Xyperico.Base.MongoDB.ConfigurationSettings.Settings != null)
      {
        // Try "<MachineName>_<entry>"
        string localConfigName = System.Environment.MachineName + "_" + configEntry;
        if (Xyperico.Base.MongoDB.ConfigurationSettings.Settings.ConfigurationEntries[localConfigName] != null)
          return Xyperico.Base.MongoDB.ConfigurationSettings.Settings.ConfigurationEntries[localConfigName];

        // Try "<entry>"
        if (Xyperico.Base.MongoDB.ConfigurationSettings.Settings.ConfigurationEntries[configEntry] != null)
          return Xyperico.Base.MongoDB.ConfigurationSettings.Settings.ConfigurationEntries[configEntry];

        // Try "<MachineName>_Default"
        localConfigName = System.Environment.MachineName + "_Default";
        if (Xyperico.Base.MongoDB.ConfigurationSettings.Settings.ConfigurationEntries[localConfigName] != null)
          return Xyperico.Base.MongoDB.ConfigurationSettings.Settings.ConfigurationEntries[localConfigName];

        // Try "Default"
        if (Xyperico.Base.MongoDB.ConfigurationSettings.Settings.ConfigurationEntries["Default"] != null)
          return Xyperico.Base.MongoDB.ConfigurationSettings.Settings.ConfigurationEntries["Default"];
      }

      string msg = "MongoDB configuration entry '" + configEntry + "' (and it's alternatives) does not exist.";
      Logger.Error(msg);
      throw new ArgumentException(msg);
    }


    public MongoDatabase GetMongoDBFor(string configEntry)
    {
      ConfigurationSettings.ConfigurationEntryElement config = GetMongoDBConfig(configEntry);

      MongoDatabase mdb = ContextDatabases.ContainsKey(configEntry)
                          ? ContextDatabases[configEntry]
                          : null;

      if (mdb == null)
      {
        mdb = GetMongoDBFor(config);
        ContextDatabases[configEntry] = mdb;
      }

      if (mdb == null)
        throw new ApplicationException("Unable to create Mongo database");

      return mdb;
    }


    private MongoDatabase GetMongoDBFor(ConfigurationSettings.ConfigurationEntryElement config)
    {
      var connectionString = "mongodb://localhost/?safe=true";
      var client = new MongoClient(connectionString);
      var server = client.GetServer();
      var database = server.GetDatabase(config.Database);

      return database;
      //new Mongo(config.Database, config.Server, config.Port, "");
    }


    private INameValueContextCollection _contextStore;

    protected INameValueContextCollection ContextStore
    {
      get
      {
        if (_contextStore == null)
        {
          _contextStore = Xyperico.Base.ObjectContainer.Container.Resolve<INameValueContextCollection>();
        }
        return _contextStore;
      }
    }


    private Dictionary<string, MongoDatabase> ContextDatabases
    {
      get
      {
        if (ContextStore == null)
          throw new ApplicationException("No ContextStore has been configured for MongoDBSessionManager. Please make sure the object container contains an implementation of IMongoDBContextStore.");
        if (ContextStore.GetData(SESSION_KEY) == null)
          ContextStore.SetData(SESSION_KEY, new Dictionary<string, MongoDatabase>());

        return (Dictionary<string, MongoDatabase>)ContextStore.GetData(SESSION_KEY);
      }
    }


    private const string SESSION_KEY = "MONGODB_SESSIONS";


    //public void CloseAllConnections()
    //{
    //  foreach (KeyValuePair<string, MongoDatabase> connection in ContextConnections)
    //  {
    //    connection.Value.
    //  }
    //  ContextConnections.Clear();
    //}
  }
}
