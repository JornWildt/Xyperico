using System;
using CuttingEdge.Conditions;
using Xyperico.Agres.Contract;
using Xyperico.Base.Exceptions;


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
      if (s == null)
        throw new MissingResourceException(string.Format("No event stream found for ID {0} of type {1}", id, id.GetType()));
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
