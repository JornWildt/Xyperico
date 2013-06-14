using System.Collections.Generic;
using Xyperico.Base;
using Xyperico.Base.Exceptions;


namespace Xyperico.Agres.InMemoryEventStore
{
  public class InMemoryEventStore : IEventStore
  {
    protected object StreamsLock = new object();

    protected Dictionary<IIdentity, EventStream> Streams { get; set; }


    public InMemoryEventStore()
    {
      Streams = new Dictionary<IIdentity, EventStream>();
    }


    public EventStream Load(IIdentity id)
    {
      EventStream s;
      if (Streams.TryGetValue(id, out s))
        return s;
      return null;
    }


    public void Append(IIdentity id, int expectedVersion, IEnumerable<IEvent> events)
    {
      lock (StreamsLock) // Not the best performing thing to do, but who care with this not-for-production implementation ...
      {
        EventStream s;
        if (!Streams.TryGetValue(id, out s))
        {
          s = new EventStream();
          Streams[id] = s;
        }
        if (s.Version != expectedVersion)
          throw new VersionConflictException(id.ToString(), typeof(object), expectedVersion);
        s.Append(events);
      }
    }
  }
}
