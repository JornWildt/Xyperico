using System;
using CuttingEdge.Conditions;
using Xyperico.Base;
using System.Collections.Generic;


namespace Xyperico.Agres
{
  public class AbstractRepository<TAggregate, TId>
    where TAggregate : AbstractAggregate<TId>
    where TId : IIdentity
  {
    protected IEventStore EventStore { get; set; }


    public AbstractRepository(IEventStore eventStore)
    {
      Condition.Requires(eventStore, "eventStore").IsNotNull();
      EventStore = eventStore;
    }


    /// <summary>
    /// Get events from event store and restore aggregate from them.
    /// </summary>
    /// <param name="id">Aggregate ID.</param>
    /// <returns>Aggregate and associated event stream.</returns>
    public RepositoryItem<TAggregate> Get(IIdentity id)
    {
      EventStream s = EventStore.Load(id);
      TAggregate a = (TAggregate)Activator.CreateInstance(typeof(TAggregate), s.Events);
      return new RepositoryItem<TAggregate>(a, s);
    }


    /// <summary>
    /// Append events from aggregate to event store.
    /// </summary>
    /// <param name="item"></param>
    public void Update(RepositoryItem<TAggregate> item)
    {
      EventStore.Append(item.Aggregate.Id, item.EventStream.Version, item.Aggregate.GetChanges());
    }
  }
}
