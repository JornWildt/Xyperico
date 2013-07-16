using System;
using CuttingEdge.Conditions;


namespace Xyperico.Agres.EventStore
{
  public class GenericApplicationService<TAggregate, TId>
    where TAggregate : AbstractAggregate<TId>
    where TId : IIdentity
  {
    protected IEventStore EventStore { get; set; }

    protected GenericRepository<TAggregate,TId> Repository { get; set; }


    public GenericApplicationService(IEventStore eventStore)
    {
      Condition.Requires(eventStore, "eventStore").IsNotNull();

      EventStore = eventStore;
      Repository = new GenericRepository<TAggregate, TId>(eventStore);
    }


    /// <summary>
    /// Load aggregate, call update action for modifying aggregate and write events back.
    /// </summary>
    /// <remarks>Expects event store to return empty/uninitialized aggregate when not found in event stream.</remarks>
    /// <param name="cmd">Command containing ID for loading and storing aggregate.</param>
    /// <param name="update">Update action.</param>
    protected void Update(ICommand<TId> cmd, Action<TAggregate> update)
    {
      var item = Repository.Get(cmd.Id);
      item.Aggregate.VerifyCommand(cmd);
      update(item.Aggregate);
      Repository.Update(item);
    }
  }
}
