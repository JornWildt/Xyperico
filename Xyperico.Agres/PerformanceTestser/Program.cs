using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xyperico.Agres;
using Xyperico.Agres.InMemoryEventStore;
using Xyperico.Agres.Sql;
using Xyperico.Agres.JsonNet;
using Xyperico.Agres.Serializer;
using PerformanceTestser.TestUser;
using Xyperico.Agres.Contract;

namespace PerformanceTestser
{
  public class Program
  {
    const string SqlConnectionString = "Server=localhost;Database=CommunitySite;User Id=comsite;Password=123456;";


    static Func<IAppendOnlyStore>[] AppendOnlyStores =
    {
      () => new InMemoryAppendOnlyStore(),
      () => new SqlAppendOnlyStore(SqlConnectionString, false)
    };


    public static void Main(string[] args)
    {
      AbstractSerializer.RegisterKnownType(typeof(UserCreatedEvent));

      ISerializer[] Serializers = 
      {
        new DataContractSerializer(),
        new ProtoBufSerializer(),
        new DotNetBinaryFormaterSerializer(),
        new JsonNetSerializer()
      };

      UserId id = new UserId(1);
      List<IEvent> events = new List<IEvent>() { new UserCreatedEvent(id, "John") };

      foreach (Func<IAppendOnlyStore> aStoreBuilder in AppendOnlyStores)
      {
        foreach (ISerializer serializer in Serializers)
        {
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
              EventStream s = eStore.Load(id);
              eStore.Append(id, s.Version, events);
              ++count;
            }
            while ((DateTime.Now - t1) < TimeSpan.FromSeconds(1));

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
