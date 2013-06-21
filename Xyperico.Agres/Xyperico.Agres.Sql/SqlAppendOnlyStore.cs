using System.Data;
using System.Data.SqlClient;


namespace Xyperico.Agres.Sql
{
  public class SqlAppendOnlyStore : IAppendOnlyStore
  {
    const int SqlError_DuplicateKey = 2601;

    SqlConnection Connection;

    SqlCommand AppendCommand;
    SqlCommand ReadCommand;
    SqlTransaction Transaction;

    bool CommitOnClose;


    public SqlAppendOnlyStore(string connectionString, bool commitOnClose = true)
    {
      Connection = new SqlConnection(connectionString);
      CommitOnClose = commitOnClose;

      Connection.Open();
      Transaction = Connection.BeginTransaction();

      const string appendSql = @"
INSERT INTO EventStore
  (Name, Data, Version)
VALUES
  (@name, @data, @version)";

      AppendCommand = new SqlCommand(appendSql, Connection);
      AppendCommand.Parameters.Add("@name", SqlDbType.VarChar, 50);
      AppendCommand.Parameters.Add("@data", SqlDbType.Image);
      AppendCommand.Parameters.Add("@version", SqlDbType.BigInt);
      AppendCommand.Transaction = Transaction;

      const string readSql = @"
SELECT *
FROM EventStore
WHERE Name = @name
ORDER BY Version";
      
      ReadCommand = new SqlCommand(readSql, Connection);
      ReadCommand.Parameters.Add("@name", SqlDbType.VarChar, 50);
      ReadCommand.Transaction = Transaction;
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
      ReadCommand.Parameters["@name"].Value = name;
      using (SqlDataReader r = ReadCommand.ExecuteReader())
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

    
    public void Dispose()
    {
      if (CommitOnClose)
        Transaction.Commit();
      else
        Transaction.Rollback();
      Connection.Close();
    }
  }
}
