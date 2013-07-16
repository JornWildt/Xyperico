using System.Threading.Tasks;
using CuttingEdge.Conditions;
using Xyperico.Agres.DocumentStore;


namespace Xyperico.Agres.EventStore
{
  public class EventStoreHost
  {
    protected IEventStore EventStore { get; set; }
    protected IEventPublisher EventPublisher { get; set; }
    protected EventStorePublisher EventStorePublisher { get; set; }
    protected IDocumentStoreFactory DocumentStoreFactory { get; set; }

    public EventStoreHost(IEventStore store, IEventPublisher publisher, IDocumentStoreFactory documentStoreFactory)
    {
      Condition.Requires(store, "store").IsNotNull();
      Condition.Requires(publisher, "publisher").IsNotNull();

      EventStore = store;
      EventPublisher = publisher;
      DocumentStoreFactory = documentStoreFactory;
      EventStorePublisher = new EventStorePublisher(store, publisher, documentStoreFactory);
    }


    public void Start()
    {
      Task.Factory.StartNew(() => EventStorePublisher.Run());
    }
  }
}
