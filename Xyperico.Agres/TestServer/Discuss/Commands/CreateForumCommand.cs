using System.Runtime.Serialization;
using CuttingEdge.Conditions;
using Xyperico.Agres;


namespace TestServer.Discuss.Commands
{
  [DataContract]
  public class CreateForumCommand : ICommand<ForumId>
  {
    [DataMember(Order = 1)]
    public ForumId Id { get; set; }

    [DataMember(Order = 2)]
    public string Title { get; set; }

    [DataMember(Order = 3)]
    public string Description { get; set; }

    
    public CreateForumCommand() { }

    
    public CreateForumCommand(ForumId id, string title, string description)
    {
      Condition.Requires(id, "id").IsNotNull();
      Condition.Requires(title, "title").IsNotNullOrEmpty();
      Condition.Requires(description, "description").IsNotNull();

      Id = id;
      Title = title;
      Description = description;
    }
  }
}
