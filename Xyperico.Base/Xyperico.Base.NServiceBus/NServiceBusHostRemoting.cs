using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using log4net;
using NServiceBus;
using Castle.Windsor;


namespace Xyperico.Base.NServiceBus
{
  public class NServiceBusHostRemoting
  {
    public IWindsorContainer ObjectContainer { get; set; }

    public string ExecutablePath { get; set; }

    public string ProcessName { get; set; }

    public string Arguments { get; set; }


    public NServiceBusHostRemoting()
    {
      ProcessName = "NServiceBus.Host.exe";
      Arguments = "NServiceBus.Integration";
    }

    
    public void Start()
    {
      CheckForServerAlreadyRunning();
      ServerProcess = Process.Start(ExecutablePath, Arguments);
      WaitForServerToBeReady();
    }


    public void Stop()
    {
      try
      {
        Logger.Debug("Closing Server");
        ServerProcess.Kill();
        Logger.Debug("Waiting for exit of Server");
        ServerProcess.WaitForExit();
        Logger.Debug("Server ended");
      }
      catch (Exception ex)
      {
        Logger.Debug("Got exception during server stop.", ex);
      }
    }


    #region --- Internals ---

    private ILog Logger = LogManager.GetLogger(typeof(NServiceBusHostRemoting));

    private Process ServerProcess;
    private bool PingIsPending = false;


    void CheckForServerAlreadyRunning()
    {
      Process[] existingServers = Process.GetProcessesByName(ProcessName);
      if (existingServers.Length > 0)
        throw new ApplicationException(string.Format("There is already one instance of {0} running!", ProcessName));
    }


    void WaitForServerToBeReady()
    {
      IBus bus = ObjectContainer.Resolve<IBus>();
      PingIsPending = true;
      bus.Send(new PingCommand()).Register(ASyncWaitHandler, null);
      int i = 0;
      do
      {
        System.Threading.Thread.Sleep(1000);
      }
      while (PingIsPending && ++i < 3000);
      if (PingIsPending)
      {
        Stop();
        throw new ApplicationException("Timeout while waiting for server to start.");
      }
    }


    public void ASyncWaitHandler(IAsyncResult asyncResult)
    {
      PingIsPending = false;
    }

    #endregion
  }
}
