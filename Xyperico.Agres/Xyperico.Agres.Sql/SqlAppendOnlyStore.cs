using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Xyperico.Agres.EventStore;


namespace Xyperico.Agres.Sql
{
  public class SqlAppendOnlyStore : IAppendOnlyStore
  {
    const int SqlError_DuplicateKey = 2601;

    SqlConnection Connection;

    SqlCommand AppendCommand;
    SqlCommand LoadCommand;
    SqlCommand ReadFromCommand;
    SqlCommand LastUsedIdCommand;

    bool CommitOnClose;


    public SqlAppendOnlyStore(string connectionString, bool commitOnClose = true)
    {
      Connection = new SqlConnection(connectionString);
      CommitOnClose = commitOnClose;

      Connection.Open();

      const string appendSql = @"
INSERT INTO EventStore
  (Name, Data, Version)
VALUES
  (@name, @data, @version)";

      AppendCommand = new SqlCommand(appendSql, Connection);
      AppendCommand.Parameters.Add("@name", SqlDbType.VarChar, 50);
      AppendCommand.Parameters.Add("@data", SqlDbType.Image);
      AppendCommand.Parameters.Add("@version", SqlDbType.BigInt);

      const string loadSql = @"
SELECT *
FROM EventStore
WHERE Name = @name
ORDER BY Version";
      
      LoadCommand = new SqlCommand(loadSql, Connection);
      LoadCommand.Parameters.Add("@name", SqlDbType.VarChar, 50);

      const string readFromSql = @"
SELECT *
FROM EventStore
WHERE @id <= Id
ORDER BY Id";

      ReadFromCommand = new SqlCommand(readFromSql, Connection);
      ReadFromCommand.Parameters.Add("@id", SqlDbType.BigInt);

      const string lastUsedIdSql = @"
SELECT IDENT_CURRENT('EventStore') - 1";

      LastUsedIdCommand = new SqlCommand(lastUsedIdSql, Connection);
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
      catch (SqlException ex)
      {
        if (ex.Number == SqlError_DuplicateKey)
          throw new VersionConflictException(expectedVersion, name);
        throw;
      }
    }

    
    public NamedDataSet Load(string name)
    {
      NamedDataSet result = new NamedDataSet(name);
      LoadCommand.Parameters["@name"].Value = name;
      using (SqlDataReader r = LoadCommand.ExecuteReader())
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
      using (SqlDataReader r = ReadFromCommand.ExecuteReader())
      {
        while (r.Read() && --count >= 0)
        {
          int rid = (int)r["Id"];
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
        object result = LastUsedIdCommand.ExecuteScalar();
        return (long)(decimal)result;
      }
    }

    
    public void Dispose()
    {
      //if (CommitOnClose)
      //  Transaction.Commit();
      //else
      //  Transaction.Rollback();
      Connection.Close();
    }


    public static void CreateTable(string connectionString)
    {
      using (var connection = new SqlConnection(connectionString))
      {
        connection.Open();

        string createTableSQL = @"CREATE TABLE EventStore (
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    Name VARCHAR(50) NOT NULL,
	Data IMAGE NOT NULL,
	Version BIGINT NOT NULL
)";
        using (SqlCommand cmd = new SqlCommand(createTableSQL, connection))
        {
          cmd.ExecuteNonQuery();
        }

        string indexSQL = @"CREATE UNIQUE INDEX IX_EventStore_NameVersion ON EventStore
(
	Name ASC,
	Version ASC
)";
        using (SqlCommand cmd = new SqlCommand(indexSQL, connection))
        {
          cmd.ExecuteNonQuery();
        }
      }
    }


    public static void DropTable(string connectionString)
    {
      using (var connection = new SqlConnection(connectionString))
      {
        connection.Open();

        using (SqlCommand cmd = new SqlCommand("DROP TABLE EventStore", connection))
        {
          cmd.ExecuteNonQuery();
        }
      }
    }


    public static bool TableExists(string connectionString)
    {
      using (var connection = new SqlConnection(connectionString))
      {
        connection.Open();

        using (SqlCommand cmd = new SqlCommand("SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'EventStore'", connection))
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


    public static void DropTableIfExists(string connectionString)
    {
      if (TableExists(connectionString))
        DropTable(connectionString);
    }
  }
}
