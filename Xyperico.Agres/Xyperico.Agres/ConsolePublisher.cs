using System;


namespace Xyperico.Agres
{
  public class ConsolePublisher : IEventPublisher
  {
    public void Publish(IEvent e)
    {
      Console.WriteLine("Publish: " + e);
    }
  }
}
