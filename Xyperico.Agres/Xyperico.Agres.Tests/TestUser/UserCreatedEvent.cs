using System;


namespace Xyperico.Agres.Tests.TestUser
{
  [Serializable]
  public class UserCreatedEvent : IEvent
  {
    public UserId Id { get; private set; }
    public string Name { get; private set; }

    public UserCreatedEvent(UserId id, string name)
    {
      Id = id;
      Name = name;
    }
  }
}
