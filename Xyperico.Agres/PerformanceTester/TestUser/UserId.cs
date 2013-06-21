using System;
using ProtoBuf;
using Xyperico.Agres;


namespace PerformanceTestser.TestUser
{
  [Serializable]
  [ProtoContract]
  public class UserId : Identity<int>
  {
    public UserId()
    {
    }


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
