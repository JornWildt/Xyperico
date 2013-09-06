using System.Collections.Generic;
using CuttingEdge.Conditions;
using TestServer.Discuss.Commands;
using TestServer.Discuss.Events;
using Xyperico.Agres;
using Xyperico.Agres.EventStore;


namespace TestServer.Discuss
{
  public class ForumData : IHaveIdentity<ForumId>
  {
    public ForumId Id { get; protected set; }

    public string Title { get; protected set; }

    public string Description { get; protected set; }


    #region Restore state

    protected void RestoreFrom(ForumCreatedEvent e)
    {
      Id = e.Id;
      Title = e.Title;
      Description = e.Description;
    }


    protected void RestoreFrom(ForumUpdatedEvent e)
    {
      Title = e.Title;
      Description = e.Description;
    }

    #endregion
  }


  public class Forum : AbstractAggregate<ForumId, ForumData>
  {
    public Forum(IEnumerable<IEvent> events)
      : base(events)
    {
    }


    public override void VerifyCommand(ICommand<ForumId> c)
    {
      if (Version == 0)
      {
        if (!(c is CreateForumCommand))
          throw new DomainException("NotCreated", Id, c, "Cannot modify non-existing forum");
      }
      else
      {
        if (c is CreateForumCommand)
          throw new DomainException("Recreated", Id, c, "Cannot re-create forum");
      }
    }


    public void Create(CreateForumCommand cmd)
    {
      Condition.Requires(cmd, "cmd").IsNotNull();

      Publish(new ForumCreatedEvent(cmd.Id, cmd.Title, cmd.Description));
    }


    public void Update(UpdateForumCommand cmd)
    {
      Condition.Requires(cmd, "cmd").IsNotNull();

      Publish(new ForumUpdatedEvent(cmd.Id, cmd.Title, cmd.Description));
    }
  }
}
