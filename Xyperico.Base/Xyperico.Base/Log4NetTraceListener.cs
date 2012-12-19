using System.Diagnostics;
using log4net;


namespace Xyperico.Base
{
  class Log4NetTraceListener : TraceListener
  {
    static ILog Logger = LogManager.GetLogger(typeof(TraceListener));


    public override void Write(string message)
    {
      Logger.Debug(message);
    }

    public override void WriteLine(string message)
    {
      Logger.Debug(message);
    }
  }
}
