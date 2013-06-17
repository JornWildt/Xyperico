using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using Xyperico.Agres.Contract;
using Xyperico.Agres.InMemoryEventStore;
using Xyperico.Agres.Serializer;
using Xyperico.Agres.Sql;
using Xyperico.Agres.Tests.TestUser;
using Xyperico.Base.Exceptions;
using Xyperico.Agres.JsonNet;


namespace Xyperico.Agres.Tests
{
  [TestFixture]
  public class SqlEventStoreTests : TestHelper
  {
    const string SqlConnectionString = "Server=localhost;Database=CommunitySite;User Id=comsite;Password=123456;";
    IAppendOnlyStore AppendOnlyStore;
    EventStore EventStore;


    protected override void SetUp()
    {
      base.SetUp();
      //AppendOnlyStore = new SqlAppendOnlyStore(SqlConnectionString, false);
      AppendOnlyStore = new InMemoryAppendOnlyStore();
      //ISerializer serializer = new DotNetBinaryFormaterSerializer();
      ISerializer serializer = new JsonNetSerializer();
      EventStore = new EventStore(AppendOnlyStore, serializer);
    }


    protected override void TearDown()
    {
      base.TearDown();
      AppendOnlyStore.Dispose();
    }


    [Test]
    public void CanSaveAndLoadEvents()
    {
      // Arrange
      UserId id = new UserId(1);
      List<IEvent> events = new List<IEvent>() { new UserCreatedEvent(id, "John") };

      // Act
      EventStore.Append(id, 0, events);
      EventStream s = EventStore.Load(id);

      // Assert
      Assert.IsNotNull(s);
      Assert.AreEqual(1, s.Version);
      Assert.AreEqual(1, s.Events.Count);
      Assert.IsInstanceOf<UserCreatedEvent>(s.Events[0]);

      UserCreatedEvent e = (UserCreatedEvent)s.Events[0];
      Assert.AreEqual(id, e.Id);
      Assert.AreEqual("John", e.Name);
    }


    [Test]
    public void WhenLoadingEmptyStreamItReturnsEmptyStream()
    {
      // Act
      EventStream s = EventStore.Load(new UserId(99999));

      // Assert
      Assert.IsNotNull(s.Events);
      Assert.AreEqual(0, s.Events.Count);
      Assert.AreEqual(0, s.Version);
    }


    [Test]
    public void WhenInsertingExistingIdAndVersionItThrowsConcurrencyException()
    {
      // Arrange
      UserId id = new UserId(1);
      List<IEvent> events = new List<IEvent>() { new UserCreatedEvent(id, "John") };

      EventStore.Append(id, 0, events);

      // Act + Assert
      AssertThrows<VersionConflictException>(() => EventStore.Append(id, 0, events));
    }


    // Not really a serious test - just playing around with concurrency to see what happens
    [Test]
    public void CanHandleMultipleParallelWorkers()
    {
      for (int i = 0; i < 10; ++i)
      {
        ThreadPool.QueueUserWorkItem(TestLoad, i);
        System.Threading.Thread.Sleep(Rand.GetInts(50, 200, 1)[0]);
      }
    }

    static Randomizer Rand = new Randomizer();

    static void TestLoad(object o)
    {
      int n = (int)o;

      using (SqlAppendOnlyStore appendOnlyStore = new SqlAppendOnlyStore(SqlConnectionString, false))
      {
        ISerializer serializer = new DotNetBinaryFormaterSerializer();
        EventStore store = new EventStore(appendOnlyStore, serializer);

        UserId id = new UserId(n);
        EventStream s = store.Load(id);
        System.Threading.Thread.Sleep(Rand.GetInts(100, 500, 1)[0]);
        Assert.AreEqual(s.Version, 0);
        List<IEvent> events = new List<IEvent>() { new UserCreatedEvent(id, "John") };
        store.Append(id, 0, events);
      }
    }
  }
}
