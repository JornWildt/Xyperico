using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Xyperico.Agres.InMemoryEventStore
{
  public class InMemoryAppendOnlyStore : IAppendOnlyStore
  {
    protected object StreamsLock = new object();

    protected Dictionary<string, NamedDataSet> Streams { get; set; }


    public InMemoryAppendOnlyStore()
    {
      Streams = new Dictionary<string, NamedDataSet>();
    }


    public void Append(string name, byte[] data, long expectedVersion)
    {
      lock (StreamsLock) // Not the best performing thing to do, but who care with this not-for-production implementation ...
      {
        NamedDataSet entry;
        if (!Streams.TryGetValue(name, out entry))
        {
          entry = new NamedDataSet(name);
          Streams[name] = entry;
        }
        if (entry.Version != expectedVersion)
          throw new VersionConflictException(expectedVersion, entry.Version, name);
        entry.Data.Add(data);
        entry.Version++;
      }
    }


    public NamedDataSet Load(string name)
    {
      NamedDataSet entry;
      if (Streams.TryGetValue(name, out entry))
        return entry;
      else
        return new NamedDataSet(name);
    }

    
    public void Dispose()
    {
      Streams.Clear();
    }
  }
}
