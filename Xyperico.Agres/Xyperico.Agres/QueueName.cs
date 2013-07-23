using CuttingEdge.Conditions;


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
  }
}
