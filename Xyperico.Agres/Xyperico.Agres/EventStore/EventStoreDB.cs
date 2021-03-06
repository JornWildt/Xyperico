﻿using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using Xyperico.Agres.Serialization;


namespace Xyperico.Agres.EventStore
{
  public class EventStoreDB : IEventStore
  {
    protected IAppendOnlyStore AppendOnlyStore { get; set; }

    protected ISerializer Serializer { get; set; }


    public EventStoreDB(IAppendOnlyStore store, ISerializer serializer)
    {
      Condition.Requires(store, "store").IsNotNull();
      Condition.Requires(serializer, "serializer").IsNotNull();

      AppendOnlyStore = store;
      Serializer = serializer;
    }


    public EventStream Load(IIdentity id)
    {
      Condition.Requires(id, "id").IsNotNull();

      NamedDataSet data = AppendOnlyStore.Load(id.Literal);
      if (data == null)
        return null;

      List<IEvent> events = new List<IEvent>(data.Data.Select(d => (IEvent)Serializer.Deserialize(d)));
      EventStream s = new EventStream(data.Version, events);

      return s;
    }

    
    public void Append(IIdentity id, long expectedVersion, IEnumerable<IEvent> events)
    {
      Condition.Requires(id, "id").IsNotNull();
      Condition.Requires(expectedVersion, "expectedVersion").IsGreaterOrEqual(0);
      Condition.Requires(events, "events").IsNotNull();

      foreach (IEvent e in events)
      {
        byte[] data = Serializer.Serialize(e);
        AppendOnlyStore.Append(id.Literal, data, expectedVersion);
        ++expectedVersion;
      }
    }


    public IEnumerable<EventStoreItem> ReadFrom(long id, int count)
    {
      IEnumerable<DataItem> data = AppendOnlyStore.ReadFrom(id, count);

      return data.Select(d => new EventStoreItem(d.Id, d.Name, (IEvent)Serializer.Deserialize(d.Data)));
    }
  }
}
