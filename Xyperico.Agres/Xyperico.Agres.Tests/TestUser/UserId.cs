using System;
using Xyperico.Agres.Contract;


namespace Xyperico.Agres.Tests.TestUser
{
  [Serializable]
  public class UserId : Identity<int>
  {
    public UserId(int id)
      : base(id)
    {
    }


    protected override string Prefix
    {
      get { return "user"; }
    }
  }
}
