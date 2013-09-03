using System;
using TestServer.Discuss.Commands;
using Xyperico.Agres.EventStore;
using Xyperico.Agres.MessageBus;


namespace TestServer.Discuss
{
  public class ForumApplicationService : 
    GenericApplicationService<Forum, ForumId>,
    IHandleMessage<CreateForumCommand>,
    IHandleMessage<UpdateForumCommand>
  {
    public ForumApplicationService(IEventStore eventStore)
      : base(eventStore)
    {
    }


    public void Handle(CreateForumCommand cmd)
    {
      Console.WriteLine("Received CreateForumCommand");
      Update(cmd, f => f.Create(cmd));
    }
    
    
    public void Handle(UpdateForumCommand cmd)
    {
      Update(cmd, f => f.Update(cmd));
    }
  }
}
