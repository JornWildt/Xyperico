using System.Collections.Generic;
using Xyperico.Agres.Contract;


namespace Xyperico.Agres
{
  public interface IEventStore
  {
    EventStream Load(IIdentity id);
    void Append(IIdentity id, int expectedVersion, IEnumerable<IEvent> events);
  }
}
