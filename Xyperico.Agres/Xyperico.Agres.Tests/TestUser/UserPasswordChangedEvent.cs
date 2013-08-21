using System;


namespace Xyperico.Agres.Tests.TestUser
{
  //[Serializable]
  public class UserPasswordChangedEvent : IEvent
  {
    public UserId Id { get; private set; }
    public string NewPassword { get; private set; }

    public UserPasswordChangedEvent(UserId id, string newPassword)
    {
      Id = id;
      NewPassword = newPassword;
    }
  }
}
