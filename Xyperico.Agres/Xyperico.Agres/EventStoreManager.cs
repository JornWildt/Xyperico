using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CuttingEdge.Conditions;
using System.Threading.Tasks;

namespace Xyperico.Agres
{
  public class EventStoreManager
  {
    protected IEventStore EventStore { get; set; }
    protected IEventPublisher EventPublisher { get; set; }
    protected EventStorePublisher EventStorePublisher { get; set; }

    public EventStoreManager(IEventStore store, IEventPublisher publisher)
    {
      Condition.Requires(store, "store").IsNotNull();
      Condition.Requires(publisher, "publisher").IsNotNull();

      EventStore = store;
      EventPublisher = publisher;
      EventStorePublisher = new EventStorePublisher(store, publisher);
    }


    public void Start()
    {
      Task.Factory.StartNew(() => EventStorePublisher.Run());
    }
  }
}
