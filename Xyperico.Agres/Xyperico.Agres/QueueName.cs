using CuttingEdge.Conditions;
using System;


namespace Xyperico.Agres
{
  public class QueueName
  {
    public string Name { get; private set; }

    public QueueName(string name)
    {
      Condition.Requires(name, "name").IsNotNullOrEmpty();
      Name = name;
    }


    public override string ToString()
    {
      return Name;
    }


    public override bool Equals(object obj)
    {
      QueueName other = obj as QueueName;
      if (other == null)
        return false;
      return Name == other.Name;
    }


    public override int GetHashCode()
    {
      return Name.GetHashCode();
    }


    public static implicit operator QueueName(string name)
    {
      return new QueueName(name);
    }


    public static bool operator ==(QueueName a, QueueName b)
    {
      // If both are null, or both are same instance, return true.
      if (System.Object.ReferenceEquals(a, b))
        return true;

      // If one is null, but not both, return false.
      if ((object)a == null || (object)b == null)
        return false;

      return a.Equals(b);
    }
    
    
    public static bool operator !=(QueueName a, QueueName b)
    {
      return !(a == b);
    }
  }
}
