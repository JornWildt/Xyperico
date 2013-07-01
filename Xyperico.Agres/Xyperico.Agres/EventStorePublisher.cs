using System;
using CuttingEdge.Conditions;
using log4net;
using System.Threading;


namespace Xyperico.Agres
{
  public class EventStorePublisher
  {
    static private ILog Logger = LogManager.GetLogger(typeof(EventStorePublisher));

    private IEventStore EventStore;

    private IEventPublisher EventPublisher;


    public EventStorePublisher(IEventStore store, IEventPublisher publisher)
    {
      Condition.Requires(store, "store").IsNotNull();
      Condition.Requires(publisher, "publisher").IsNotNull();

      EventStore = store;
      EventPublisher = publisher;
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

          lastPublishedEventId = PublishEvents(lastPublishedEventId.Value);

          StoreLastPublishedEventId(lastPublishedEventId.Value);

          Thread.Sleep(1000);


          // Get lock on unpublished event list
          // Get ID of last published events (if needed)
          // Read events since last published event
          // Publish events externally
          // Register new latests event
          // Unlock
          // If not more to do - then wait some time
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
      return 0;
    }


    protected void StoreLastPublishedEventId(long lastPublishedEventId)
    {
    }


    protected long PublishEvents(long lastPublishedEventId, int count = 10)
    {
      var events = EventStore.ReadFrom(lastPublishedEventId, count);
      foreach (EventStoreItem item in events)
      {
        if (item.Id < lastPublishedEventId)
        {
          throw new InvalidOperationException(string.Format("Unexpected event ID {0} (should be >= than latest published ID {1}).", item.Id, lastPublishedEventId));
        }

        EventPublisher.Publish(item.Event);
        lastPublishedEventId = item.Id;
      }
      return lastPublishedEventId;
    }
  }
}
