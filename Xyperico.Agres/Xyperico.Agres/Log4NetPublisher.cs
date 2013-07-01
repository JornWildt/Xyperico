using log4net;


namespace Xyperico.Agres
{
  public class Log4NetPublisher : IEventPublisher
  {
    static ILog Logger = LogManager.GetLogger(typeof(Log4NetPublisher));


    public void Publish(IEvent e)
    {
      Logger.DebugFormat("Publishing {0}.", e);
    }
  }
}
