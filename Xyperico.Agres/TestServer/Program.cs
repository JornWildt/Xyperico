using System;
using System.Linq;
using System.Reflection;
using log4net;
using TestServer.Discuss;
using TestServer.Discuss.Commands;
using Xyperico.Agres;
using Xyperico.Agres.Configuration;
using Xyperico.Agres.EventStore;
using Xyperico.Agres.JsonNet;
using Xyperico.Agres.MessageBus;
using Xyperico.Agres.MSMQ;
using Xyperico.Agres.ProtoBuf;
using Xyperico.Agres.SQLite;


namespace TestServer
{
  class Program
  {
    const string SQLiteConnectionString = "Data Source=C:\\tmp\\Xyperico.Discuss.TestStorage\\Xyperico.Discuss.Tests.db";
    const string SqlConnectionString = "Server=localhost;Database=CommunitySite;User Id=comsite;Password=123456;";

    protected static readonly ILog Logger = LogManager.GetLogger((typeof(Program)));


    static void Main(string[] args)
    {
      var serializerTypes =
        typeof(ForumId).Assembly.GetTypes()
        .Where(t => typeof(Identity<>).IsAssignableFrom(t) || typeof(IMessage).IsAssignableFrom(t))
        .Where(t => !t.IsAbstract);

      Assembly[] handlerAssemblies = new Assembly[] { typeof(ForumApplicationService).Assembly };

      IMessageBus bus = Configure.With()
        .Log4Net()
        .ObjectContainer(Xyperico.Base.ObjectContainer.Container)
        .SerializableTypes(serializerTypes)
        .MessageBus(handlerAssemblies)
          .WithJsonSubscriptionSerializer()
          //.WithDataContractSubscriptionSerializer()
          //.WithProtoBufSubscriptionSerializer()
          //.WithBsonSubscriptionSerializer()
          .WithFileSubscriptionStore("C:\\tmp\\Xyperico.Discuss.TestStorage")
          .WithJsonMessageSerializer()
          //.WithDataContractMessageSerializer()
          //.WithProtoBufMessageSerializer()
          //.WithBsonMessageSerializer()
          .WithMSMQ(".\\private$\\comsite")
          .Done()
        .EventStore()
          .WithSQLiteEventStore(SQLiteConnectionString, true)
          //.WithSqlServerEventStore(SqlConnectionString, true)
          .WithJsonEventSerializer()
          //.WithDataContractEventSerializer()
          //.WithProtoBufEventSerializer()
          //.WithBsonEventSerializer()
          //.WithJsonDocumentSerializer()
          //.WithDataContractDocumentSerializer()
          .WithProtoBufDocumentSerializer()
          //.WithBsonDocumentSerializer()
          .WithFileDocumentStore("C:\\tmp\\Xyperico.Discuss.TestStorage")
          .Done()
        .Start();

      // Supported serializers: JSON, BSON, DataContract, ProtoBuf

      do
      {
        CreateForumCommand cmd = new CreateForumCommand(new ForumId(), "Test forum", "Blah blah");
        bus.Send(cmd);
      }
      while (Console.ReadLine() == "");
    }
  }


  [Serializable]
  public class TestMessage
  {
    public int X { get; set; }
  }
}
