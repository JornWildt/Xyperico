using System;
using System.Collections.Generic;
using PerformanceTestser.TestUser;
using Xyperico.Agres;
using Xyperico.Agres.Contract;
using Xyperico.Agres.InMemoryEventStore;
using Xyperico.Agres.JsonNet;
using Xyperico.Agres.ProtoBuf;
using Xyperico.Agres.Serializer;
using Xyperico.Agres.Sql;
using System.IO;

namespace PerformanceTestser
{
  public class Program
  {
    const string SqlConnectionString = "Server=localhost;Database=CommunitySite;User Id=comsite;Password=123456;";
    const string SQLiteConnectionString = "Data Source=C:\\tmp\\AgresEventStore.db";


    static Func<IAppendOnlyStore>[] AppendOnlyStores =
    {
      () => new SQLiteAppendOnlyStore(SQLiteConnectionString, false),
      () => new InMemoryAppendOnlyStore(),
      () => new SqlAppendOnlyStore(SqlConnectionString, false)
    };


    public static void Main(string[] args)
    {
      //SQLiteAppendOnlyStore.CreateTable(SQLiteConnectionString);

      AbstractSerializer.RegisterKnownType(typeof(UserCreatedEvent));

      // Create serializers after registering known types
      ISerializer[] Serializers = 
      {
        new DataContractSerializer(),
        new ProtoBufSerializer(),
        new DotNetBinaryFormaterSerializer(),
        new JsonNetSerializer()
      };

      foreach (Func<IAppendOnlyStore> aStoreBuilder in AppendOnlyStores)
      {
        foreach (ISerializer serializer in Serializers)
        {
          UserId id = new UserId(1);
          List<IEvent> events = new List<IEvent>() { new UserCreatedEvent(id, "John") };

          IAppendOnlyStore aStore = aStoreBuilder();
          try
          {
            EventStore eStore = new EventStore(aStore, serializer);
            eStore.Append(id, 0, events);

            // Warm up various caches
            for (int i = 0; i < 10; ++i)
            {
              EventStream s = eStore.Load(id);
              eStore.Append(id, s.Version, events);
            }

            int count = 0;
            DateTime t1 = DateTime.Now;
            do
            {
              id = new UserId(100 + count);
              //EventStream s = eStore.Load(id);
              eStore.Append(id, 0, new IEvent[] { new UserCreatedEvent(id, "John") });
              ++count;
            }
            while ((DateTime.Now - t1) < TimeSpan.FromMilliseconds(1000));

            Console.WriteLine("{0} + {1}: {2}", aStore, serializer, count);
          }
          finally
          {
            aStore.Dispose();
          }
        }
      }
    }
  }
}
