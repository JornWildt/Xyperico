using System.Collections.Generic;
using Xyperico.Base;


namespace Xyperico.Agres
{
  public interface IEventStore
  {
    EventStream Load(IIdentity id);
    void Append(IIdentity id, int expectedVersion, IEnumerable<IEvent> events);
  }
}
