using System;


namespace Xyperico.Agres.EventStore
{
  public class ConsolePublisher : IEventPublisher
  {
    public void Publish(IEvent e)
    {
      Console.WriteLine("Publish: " + e);
    }
  }
}
