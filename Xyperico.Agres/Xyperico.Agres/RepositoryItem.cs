using CuttingEdge.Conditions;


namespace Xyperico.Agres
{
  public class RepositoryItem<TAggregate>
    where TAggregate : class
  {
    public TAggregate Aggregate { get; private set; }
    
    public EventStream EventStream { get; private set; }

    
    public RepositoryItem(TAggregate a, EventStream s)
    {
      Condition.Requires(a, "a").IsNotNull();
      Condition.Requires(s, "s").IsNotNull();

      Aggregate = a;
      EventStream = s;
    }
  }
}
