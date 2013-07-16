using System.Collections.Generic;
using CuttingEdge.Conditions;
using Xyperico.Agres.Utility;


namespace Xyperico.Agres.EventStore
{
  public abstract class AbstractAggregate<TId> : IHaveIdentity<TId>
    where TId : IIdentity
  {
    const string RestoreMethodName = "RestoreFrom";

    public TId Id { get; protected set; }

    public int Version { get; private set; }

    protected List<IEvent> Changes { get; set; }


    protected AbstractAggregate()
    {
      Changes = new List<IEvent>();
    }


    protected AbstractAggregate(IEnumerable<IEvent> events)
      : this()
    {
      Condition.Requires(events, "events").IsNotNull();

      Mutate(events);
    }


    /// <summary>
    /// Verify incoming command - does it signal a valid state transition? To be implemented by inheriting aggregates.
    /// </summary>
    /// <remarks>Throw relevant exceptions on invalid state transitions.</remarks>
    /// <param name="c"></param>
    public virtual void VerifyCommand(ICommand<TId> c)
    {
    }


    public IEnumerable<IEvent> GetChanges()
    {
      return Changes;
    }


    protected void Publish(IEvent e)
    {
      Changes.Add(e);
      Mutate(e);
    }


    protected void Mutate(IEnumerable<IEvent> events)
    {
      foreach (IEvent e in events)
        Mutate(e);
    }


    protected void Mutate(IEvent e)
    {
      MethodInvoke.InvokeMethodOptional(this, RestoreMethodName, e);
      ++Version;
    }


    public override string ToString()
    {
      return string.Format("{0}({1})", GetType(), Id);
    }
  }
}
