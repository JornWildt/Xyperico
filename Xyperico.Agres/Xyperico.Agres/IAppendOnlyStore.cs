using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;

namespace Xyperico.Agres
{
  public class NamedDataSet
  {
    public string Name { get; protected set; }

    public IList<byte[]> Data { get; set; }

    public long Version { get; set; }


    public NamedDataSet(string name)
    {
      Condition.Requires(name, "name").IsNotNullOrEmpty();

      Name = name;
      Data = new List<byte[]>();
    }
  }


  public interface IAppendOnlyStore : IDisposable
  {
    void Append(string name, byte[] data, long expectedVersion);

    NamedDataSet Load(string name);
  }
}
