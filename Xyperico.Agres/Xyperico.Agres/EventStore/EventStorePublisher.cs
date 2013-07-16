using System;
using CuttingEdge.Conditions;
using log4net;
using System.Threading;
using Xyperico.Agres.DocumentStore;


namespace Xyperico.Agres.EventStore
{
  public class EventStorePublisher
  {
    static private ILog Logger = LogManager.GetLogger(typeof(EventStorePublisher));

    private IEventStore EventStore;
    private DocumentRepository LastPublishedIdRepository;
    private IEventPublisher EventPublisher;


    public EventStorePublisher(IEventStore store, IEventPublisher publisher, IDocumentStoreFactory documentStoreFactory)
    {
      Condition.Requires(store, "store").IsNotNull();
      Condition.Requires(publisher, "publisher").IsNotNull();

      EventStore = store;
      EventPublisher = publisher;
      LastPublishedIdRepository = new DocumentRepository(documentStoreFactory);
    }


    public void Run()
    {
      long? lastPublishedEventId = null;
      while (true)
      {
        try
        {
          if (lastPublishedEventId == null)
            lastPublishedEventId = ReadLastPublishedEventId();

          bool mayHaveMoreEventsToPublish;
          lastPublishedEventId = PublishEvents(lastPublishedEventId.Value, 3, out mayHaveMoreEventsToPublish);

          StoreLastPublishedEventId(lastPublishedEventId.Value);

          if (!mayHaveMoreEventsToPublish)
            Thread.Sleep(1000);
        }
        catch (Exception ex)
        {
          // Reset latest ID
          Logger.Warn("Got exception in main event store publisher loop (ignored).", ex);
          Thread.Sleep(500);
        }
      }
    }


    protected long ReadLastPublishedEventId()
    {
      LastPublishedEventIdItem item = LastPublishedIdRepository.GetSingleton<LastPublishedEventIdItem>();
      return item.Id;
    }


    protected void StoreLastPublishedEventId(long lastPublishedEventId)
    {
      LastPublishedIdRepository.PutSingleton(new LastPublishedEventIdItem { Id = lastPublishedEventId });
    }


    [Serializable]
    class LastPublishedEventIdItem
    {
      public long Id;
    }


    protected long PublishEvents(long lastPublishedEventId, int count, out bool mayHaveMoreEventsToPublish)
    {
      var events = EventStore.ReadFrom(lastPublishedEventId+1, count);
      int foundCount = 0;
      foreach (EventStoreItem item in events)
      {
        if (item.Id < lastPublishedEventId)
        {
          throw new InvalidOperationException(string.Format("Unexpected event ID {0} (should be >= than latest published ID {1}).", item.Id, lastPublishedEventId));
        }

        EventPublisher.Publish(item.Event);
        lastPublishedEventId = item.Id;
        ++foundCount;
      }
      mayHaveMoreEventsToPublish = (foundCount == count);
      return lastPublishedEventId;
    }
  }
}
