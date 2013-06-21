using System.Collections.Generic;
using CuttingEdge.Conditions;


namespace Xyperico.Agres
{
  public abstract class AbstractAggregate<TId> : IHaveIdentity<TId>
    where TId : IIdentity
  {
    const string RestoreMethodName = "RestoreFrom";

    public TId Id { get; protected set; }

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
    }


    public override string ToString()
    {
      return string.Format("{0}({1})", GetType(), Id);
    }
  }
}
