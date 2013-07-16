using System.Collections.Generic;


namespace Xyperico.Agres.EventStore
{
  public interface IEventStore
  {
    EventStream Load(IIdentity id);
    void Append(IIdentity id, long expectedVersion, IEnumerable<IEvent> events);
    IEnumerable<EventStoreItem> ReadFrom(long id, int count);
  }
}
