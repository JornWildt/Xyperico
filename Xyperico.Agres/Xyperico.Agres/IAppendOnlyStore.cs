using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;


namespace Xyperico.Agres
{
  public class NamedDataSet
  {
    public string Name { get; private set; }

    public IList<byte[]> Data { get; set; }

    public long Version { get; set; }


    public NamedDataSet(string name)
    {
      Condition.Requires(name, "name").IsNotNullOrEmpty();

      Name = name;
      Data = new List<byte[]>();
    }
  }


  public class DataItem
  {
    public long Id { get; private set; }
    
    public string Name { get; private set; }

    public byte[] Data { get; set; }


    public DataItem(long id, string name, byte[] data)
    {
      Condition.Requires(name, "name").IsNotNullOrEmpty();
      Condition.Requires(data, "data").IsNotNull();

      Id = id;
      Name = name;
      Data = data;
    }
  }


  public interface IAppendOnlyStore : IDisposable
  {
    void Append(string name, byte[] data, long expectedVersion);

    NamedDataSet Load(string name);

    // ID starts from offset 1
    IEnumerable<DataItem> ReadFrom(long id, int count);

    long LastUsedId { get; }
  }
}
