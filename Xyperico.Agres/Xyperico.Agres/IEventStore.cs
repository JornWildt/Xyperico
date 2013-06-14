using System.Collections.Generic;
using Xyperico.Agres.Contract;


namespace Xyperico.Agres
{
  public interface IEventStore
  {
    EventStream Load(IIdentity id);
    void Append(IIdentity id, long expectedVersion, IEnumerable<IEvent> events);
  }
}
