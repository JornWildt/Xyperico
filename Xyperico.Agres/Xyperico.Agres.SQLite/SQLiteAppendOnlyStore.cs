using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Collections.Generic;


namespace Xyperico.Agres.Sql
{
  public class SQLiteAppendOnlyStore : IAppendOnlyStore
  {
    // Abort due to constraint violation (see http://www.sqlite.org/c3ref/c_abort.html)
    const int SqlError_DuplicateKey = 19;

    SQLiteConnection Connection;

    SQLiteCommand AppendCommand;
    SQLiteCommand LoadCommand;
    SQLiteCommand ReadFromCommand;
    SQLiteCommand LastUsedIdCommand;
    SQLiteTransaction Transaction;

    bool CommitOnClose;


    public SQLiteAppendOnlyStore(string connectionString, bool commitOnClose = true)
    {
      Connection = new SQLiteConnection(connectionString);
      CommitOnClose = commitOnClose;

      Connection.Open();
      Transaction = Connection.BeginTransaction();

      const string appendSql = @"
INSERT INTO EventStore
  (Name, Data, Version)
VALUES
  (@name, @data, @version)";

      AppendCommand = new SQLiteCommand(appendSql, Connection);
      AppendCommand.Parameters.Add("@name", DbType.String, 50);
      AppendCommand.Parameters.Add("@data", DbType.Binary);
      AppendCommand.Parameters.Add("@version", DbType.Int64);
      AppendCommand.Transaction = Transaction;

      const string loadSql = @"
SELECT *
FROM EventStore
WHERE Name = @name
ORDER BY Version";
      
      LoadCommand = new SQLiteCommand(loadSql, Connection);
      LoadCommand.Parameters.Add("@name", DbType.String);
      LoadCommand.Transaction = Transaction;

      const string readFromSql = @"
SELECT *
FROM EventStore
WHERE @id <= Id
ORDER BY Id";

      ReadFromCommand = new SQLiteCommand(readFromSql, Connection);
      ReadFromCommand.Parameters.Add("@id", DbType.Int64);
      ReadFromCommand.Transaction = Transaction;

      const string lastUsedIdSql = @"
SELECT IFNULL(MAX(seq),0)
FROM SQLITE_SEQUENCE
WHERE name = 'EventStore'";

      LastUsedIdCommand = new SQLiteCommand(lastUsedIdSql, Connection);
      LastUsedIdCommand.Transaction = Transaction;
    }


    public void Append(string name, byte[] data, long expectedVersion)
    {
      AppendCommand.Parameters["@name"].Value = name;
      AppendCommand.Parameters["@data"].Value = data;
      AppendCommand.Parameters["@version"].Value = expectedVersion+1; // FIXME: calculate differently

      try
      {
        AppendCommand.ExecuteNonQuery();
      }
      catch (SQLiteException ex)
      {
        if (ex.ErrorCode == SqlError_DuplicateKey)
          throw new VersionConflictException(expectedVersion, name);
        throw;
      }
    }

    
    public NamedDataSet Load(string name)
    {
      NamedDataSet result = new NamedDataSet(name);
      LoadCommand.Parameters["@name"].Value = name;
      using (SQLiteDataReader r = LoadCommand.ExecuteReader())
      {
        while (r.Read())
        {
          byte[] data = (byte[])r["Data"];
          long version = (long)r["Version"];
          result.Data.Add(data);
          result.Version = version; // Ends up getting the last (biggest) value
        }
      }
      return result;
    }


    public IEnumerable<DataItem> ReadFrom(long id, int count)
    {
      ReadFromCommand.Parameters["@id"].Value = id;
      using (SQLiteDataReader r = ReadFromCommand.ExecuteReader())
      {
        while (r.Read() && --count >= 0)
        {
          long rid = (long)r["Id"];
          byte[] data = (byte[])r["Data"];
          string name = (string)r["Name"];
          yield return new DataItem(rid, name, data);
        }
      }
    }


    public long LastUsedId
    {
      get
      {
        return (long)LastUsedIdCommand.ExecuteScalar();
      }
    }

    
    public void Dispose()
    {
      if (CommitOnClose)
        Transaction.Commit();
      else
        Transaction.Rollback();
      AppendCommand.Dispose(); // FIXME: error handling here
      LoadCommand.Dispose();
      Connection.Close();
    }


    public static void CreateTable(string connectionString)
    {
      using (var connection = new SQLiteConnection(connectionString))
      {
        connection.Open();

        string createTableSQL = @"CREATE TABLE EventStore (
	Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name VARCHAR(50) NOT NULL,
	Data IMAGE NOT NULL,
	Version BIGINT NOT NULL
)";
        using (SQLiteCommand cmd = new SQLiteCommand(createTableSQL, connection))
        {
          cmd.ExecuteNonQuery();
        }

        string indexSQL = @"CREATE UNIQUE INDEX IX_EventStore_NameVersion ON EventStore
(
	Name ASC,
	Version ASC
)";
        using (SQLiteCommand cmd = new SQLiteCommand(indexSQL, connection))
        {
          cmd.ExecuteNonQuery();
        }
      }
    }
    
    
    public static void DropTable(string connectionString)
    {
      using (var connection = new SQLiteConnection(connectionString))
      {
        connection.Open();

        using (SQLiteCommand cmd = new SQLiteCommand("DROP TABLE EventStore", connection))
        {
          cmd.ExecuteNonQuery();
        }
      }
    }


    public static bool TableExists(string connectionString)
    {
      using (var connection = new SQLiteConnection(connectionString))
      {
        connection.Open();

        using (SQLiteCommand cmd = new SQLiteCommand("SELECT 1 FROM sqlite_master WHERE type='table' AND name='EventStore'", connection))
        {
          object nameo = cmd.ExecuteScalar();
          return nameo != null;
        }
      }
    }


    public static void CreateTableIfNotExists(string connectionString)
    {
      if (!TableExists(connectionString))
        CreateTable(connectionString);
    }
  }
}
