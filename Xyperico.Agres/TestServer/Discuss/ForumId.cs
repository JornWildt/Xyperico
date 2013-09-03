using System;
using System.Runtime.Serialization;
using Xyperico.Agres;


namespace TestServer.Discuss
{
  [DataContract]
  public class ForumId : Identity<Guid>
  {
    public ForumId()
      : base(Guid.NewGuid())
    {
    }


    protected override string Prefix
    {
      get { return ""; }
    }
  }
}
