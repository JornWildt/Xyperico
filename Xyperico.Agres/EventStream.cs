using System.Collections.Generic;
using CuttingEdge.Conditions;


namespace Xyperico.Agres
{
  public class EventStream
  {
    public int Version { get; protected set; }

    public List<IEvent> Events { get; protected set; }


    public EventStream()
      : this(0, new List<IEvent>())
    {
    }


    public EventStream(int version, List<IEvent> events)
    {
      Condition.Requires(version, "version").IsGreaterOrEqual(0);
      Condition.Requires(events, "events").IsNotNull();

      Version = version;
      Events = events;
    }


    public void Append(IEnumerable<IEvent> events)
    {
      Events.AddRange(events);
      ++Version;
    }
  }
}
