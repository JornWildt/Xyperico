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

    protected List<DataItem> Events { get; set; }


    public InMemoryAppendOnlyStore()
    {
      Streams = new Dictionary<string, NamedDataSet>();
      Events = new List<DataItem>();
    }


    public void Append(string name, byte[] data, long expectedVersion)
    {
      lock (StreamsLock) // Not the best performing thing to do, but who cares with this not-for-production implementation ...
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

        Events.Add(new DataItem(Events.Count+1, name, data));
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


    public IEnumerable<DataItem> ReadFrom(long id, int count)
    {
      for (long i=id-1, stop=Math.Min(id-1+count, Events.Count); i<stop; ++i)
      {
        // Hacking "long" to "int" (not for production, right?)
        yield return Events[(int)i];
      }
    }


    public long LastUsedId
    {
      get { return Events.Count; }
    }


    public void Dispose()
    {
      Streams.Clear();
      Events.Clear();
    }
  }
}
