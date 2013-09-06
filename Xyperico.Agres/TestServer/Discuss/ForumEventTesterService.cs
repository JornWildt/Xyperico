using System;
using TestServer.Discuss.Events;
using Xyperico.Agres.EventStore;
using Xyperico.Agres.MessageBus;


namespace TestServer.Discuss
{
  public class ForumEventTesterService :
    GenericApplicationService<Forum, ForumData, ForumId>,
    IHandleMessage<ForumCreatedEvent>
  {
    public ForumEventTesterService(IEventStore eventStore)
      : base(eventStore)
    {
    }


    public void Handle(ForumCreatedEvent message)
    {
      Console.WriteLine("Handle ForumCreatedEvent.");
    }
  }
}
