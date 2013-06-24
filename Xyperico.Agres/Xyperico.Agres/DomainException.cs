using System;


namespace Xyperico.Agres
{
  public class DomainException : Exception
  {
    public string Name { get; protected set; }

    public IIdentity Id { get; protected set; }

    
    public DomainException(string name, IIdentity id, ICommand c, string format, params object[] p)
      : base(string.Format("Domain exception for ID {0}, command {1}: {2}", id, c, string.Format(format, p)))
    {
      Name = name;
      Id = id;
    }
  }
}
