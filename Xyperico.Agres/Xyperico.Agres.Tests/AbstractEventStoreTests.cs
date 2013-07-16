using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using Xyperico.Agres.EventStore;
using Xyperico.Agres.Serialization;
using Xyperico.Agres.Tests.TestUser;


namespace Xyperico.Agres.Tests
{
  public abstract class AbstractEventStoreTests : TestHelper
  {
    protected IAppendOnlyStore AppendOnlyStore { get; set; }
    protected EventStoreDB EventStore { get; set; }


    protected override void SetUp()
    {
      base.SetUp();

      // Get concrete implementation from inheriting test class
      AppendOnlyStore = BuildAppendOnlyStore();
      
      // Use a simple serializer that works for just about everything
      ISerializer serializer = new DotNetBinaryFormaterSerializer();
      
      EventStore = new EventStoreDB(AppendOnlyStore, serializer);
    }


    protected override void TearDown()
    {
      AppendOnlyStore.Dispose();
      base.TearDown();
    }


    protected abstract IAppendOnlyStore BuildAppendOnlyStore();


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


    [Test]
    public void CanReadRangeOfEvents()
    {
      // Arrange (insert a few events)
      long startId = AppendOnlyStore.LastUsedId + 1;

      UserId id1 = new UserId(5);
      List<IEvent> events = new List<IEvent>() { new UserCreatedEvent(id1, "John"), new UserPasswordChangedEvent(id1, "zoa") };
      EventStore.Append(id1, 0, events);
      
      UserId id2 = new UserId(23);
      events = new List<IEvent>() { new UserCreatedEvent(id2, "Amy"), new UserPasswordChangedEvent(id2, "zope") };
      EventStore.Append(id2, 0, events);

      // Act
      var items1 = EventStore.ReadFrom(startId, 10).ToList();
      var items2 = EventStore.ReadFrom(startId+1, 1).ToList();
      var items3 = EventStore.ReadFrom(startId+2, 2).ToList();

      // Assert
      Assert.AreEqual(4, items1.Count);
      Assert.AreEqual(id1.Literal, items1[0].Key);
      Assert.IsInstanceOf<UserCreatedEvent>(items1[0].Event);
      Assert.IsInstanceOf<UserPasswordChangedEvent>(items1[1].Event);

      Assert.AreEqual(1, items2.Count);
      Assert.AreEqual(id1.Literal, items2[0].Key);
      Assert.AreEqual(startId+1, items2[0].Id);
      Assert.IsInstanceOf<UserPasswordChangedEvent>(items2[0].Event);

      Assert.AreEqual(2, items3.Count);
      Assert.AreEqual(id2.Literal, items3[0].Key);
      Assert.AreEqual(startId + 2, items3[0].Id);
      Assert.IsInstanceOf<UserCreatedEvent>(items3[0].Event);
      Assert.AreEqual(id2.Literal, items3[1].Key);
      Assert.AreEqual(startId + 3, items3[1].Id);
      Assert.IsInstanceOf<UserPasswordChangedEvent>(items3[1].Event);
    }

    // Not really a serious test - just playing around with concurrency to see what happens

    class TestLoadData
    {
      public int N;
      public IAppendOnlyStore Store;
    }

    [Test]
    public void CanHandleMultipleParallelWorkers()
    {
      for (int i = 0; i < 10; ++i)
      {
        ThreadPool.QueueUserWorkItem(TestLoad, new TestLoadData { N = i, Store = AppendOnlyStore });
        System.Threading.Thread.Sleep(Rand.GetInts(50, 200, 1)[0]);
      }

      // Wait somehow for the threads to finish
      System.Threading.Thread.Sleep(2000);
    }

    static Randomizer Rand = new Randomizer();

    static void TestLoad(object o)
    {
      TestLoadData data = (TestLoadData)o;

      IAppendOnlyStore appendOnlyStore = data.Store;
      {
        ISerializer serializer = new DotNetBinaryFormaterSerializer();
        EventStoreDB store = new EventStoreDB(appendOnlyStore, serializer);

        UserId id = new UserId(data.N);
        EventStream s = store.Load(id);
        System.Threading.Thread.Sleep(Rand.GetInts(100, 500, 1)[0]);
        Assert.AreEqual(s.Version, 0);
        List<IEvent> events = new List<IEvent>() { new UserCreatedEvent(id, "John") };
        store.Append(id, 0, events);
      }
    }
  }
}
