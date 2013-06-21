using System;
using CuttingEdge.Conditions;
using Xyperico.Agres.Contract;


namespace Xyperico.Agres
{
  public class GenericRepository<TAggregate, TId>
    where TAggregate : AbstractAggregate<TId>
    where TId : IIdentity
  {
    protected IEventStore EventStore { get; set; }


    public GenericRepository(IEventStore eventStore)
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
      Condition.Requires(id, "id").IsNotNull();

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
