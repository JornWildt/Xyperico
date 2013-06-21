using System;
using ProtoBuf;
using Xyperico.Agres;


namespace PerformanceTestser.TestUser
{
  [Serializable]
  [ProtoContract]
  public class UserCreatedEvent : IEvent
  {
    [ProtoMember(1)]
    public UserId Id { get; private set; }

    [ProtoMember(2)]
    public string Name { get; private set; }

    public UserCreatedEvent()
    {
    }

    public UserCreatedEvent(UserId id, string name)
    {
      Id = id;
      Name = name;
    }
  }
}
