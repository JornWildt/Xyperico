using System;
using NServiceBus;


namespace Xyperico.Base.NServiceBus
{
  public static class NServiceBusAssert
  {
    public static void ExpectRaised<T>(this IBus bus, Action action, Action<T> tester) where T : class, IMessage
    {
      NServiceBusStub busStub = bus as NServiceBusStub;
      if (busStub == null)
        throw new ArgumentException("Could not convert IBus to NServiceBusStub.");
      T msg = null;
      using (busStub.RegisterScopedAction<T>(m => msg = m))
      {
        action();
      }
      if (msg != null)
        tester(msg);
      else
        throw new InvalidOperationException(string.Format("No {0} raised as expected", typeof(T)));
    }
  }
}
