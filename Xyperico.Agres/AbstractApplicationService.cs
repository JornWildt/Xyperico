using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CuttingEdge.Conditions;
using Xyperico.Base;


namespace Xyperico.Agres
{
  public abstract class AbstractApplicationService<TAggregate, TId>
    where TAggregate : AbstractAggregate<TId>
    where TId : IIdentity
  {
    protected IEventStore EventStore { get; set; }

    protected AbstractRepository<TAggregate,TId> Repository { get; set; }


    public AbstractApplicationService(IEventStore eventStore)
    {
      Condition.Requires(eventStore, "eventStore").IsNotNull();

      EventStore = eventStore;
      Repository = new AbstractRepository<TAggregate, TId>(eventStore);
    }


    /// <summary>
    /// Calls factory method for creating aggregate and stores events from it afterwards.
    /// </summary>
    /// <param name="cmd">Command containing ID for storing new aggregate.</param>
    /// <param name="func">Factory function.</param>
    protected void Create(ICommand<TId> cmd, Func<TAggregate> func)
    {
      TAggregate a = func();
      EventStore.Append(cmd.Id, 0, a.GetChanges());
    }


    /// <summary>
    /// Load aggregate, call update action for modifying aggregate and write events back.
    /// </summary>
    /// <param name="cmd">Command containing ID for loading and storing aggregate.</param>
    /// <param name="update">Update action.</param>
    protected void Update(ICommand<TId> cmd, Action<TAggregate> update)
    {
      var item = Repository.Get(cmd.Id);
      update(item.Aggregate);
      EventStore.Append(cmd.Id, item.EventStream.Version, item.Aggregate.GetChanges());
    }
  }
}
