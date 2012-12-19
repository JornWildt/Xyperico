using System;
using System.Collections.Generic;
using NServiceBus;
using System.Reflection;


namespace Xyperico.Base.NServiceBus
{
  public class NServiceBusStub : IBus
  {
    #region IBus Members

    public IMessageContext CurrentMessageContext
    {
      get { throw new NotImplementedException(); }
    }

    public void DoNotContinueDispatchingCurrentMessageToHandlers()
    {
      throw new NotImplementedException();
    }

    public void ForwardCurrentMessageTo(string destination)
    {
      throw new NotImplementedException();
    }

    public void HandleCurrentMessageLater()
    {
      throw new NotImplementedException();
    }

    public IDictionary<string, string> OutgoingHeaders
    {
      get { throw new NotImplementedException(); }
    }

    public void Publish<T>(Action<T> messageConstructor) where T : IMessage
    {
      throw new NotImplementedException();
    }

    public void Publish<T>(params T[] messages) where T : IMessage
    {
      foreach (IMessage msg in messages)
        Raise(msg);
    }

    public void Reply<T>(Action<T> messageConstructor) where T : IMessage
    {
      throw new NotImplementedException();
    }

    public void Reply(params IMessage[] messages)
    {
      throw new NotImplementedException();
    }

    public void Return(int errorCode)
    {
      throw new NotImplementedException();
    }

    public void Send<T>(string destination, string correlationId, Action<T> messageConstructor) where T : IMessage
    {
      throw new NotImplementedException();
    }

    public void Send(string destination, string correlationId, params IMessage[] messages)
    {
      throw new NotImplementedException();
    }

    public ICallback Send<T>(string destination, Action<T> messageConstructor) where T : IMessage
    {
      throw new NotImplementedException();
    }

    public ICallback Send(string destination, params IMessage[] messages)
    {
      throw new NotImplementedException();
    }

    public ICallback Send<T>(Action<T> messageConstructor) where T : IMessage
    {
      throw new NotImplementedException();
    }

    public ICallback Send(params IMessage[] messages)
    {
      foreach (IMessage m in messages)
        Raise(m);
      return null;
    }

    public void SendLocal<T>(Action<T> messageConstructor) where T : IMessage
    {
      throw new NotImplementedException();
    }

    public void SendLocal(params IMessage[] messages)
    {
      throw new NotImplementedException();
    }

    public void Subscribe<T>(Predicate<T> condition) where T : IMessage
    {
      throw new NotImplementedException();
    }

    public void Subscribe(Type messageType, Predicate<IMessage> condition)
    {
      throw new NotImplementedException();
    }

    public void Subscribe<T>() where T : IMessage
    {
      throw new NotImplementedException();
    }

    public void Subscribe(Type messageType)
    {
      throw new NotImplementedException();
    }

    public void Unsubscribe<T>() where T : IMessage
    {
      throw new NotImplementedException();
    }

    public void Unsubscribe(Type messageType)
    {
      throw new NotImplementedException();
    }

    #endregion

    #region IMessageCreator Members

    public object CreateInstance(Type messageType)
    {
      throw new NotImplementedException();
    }

    public T CreateInstance<T>(Action<T> action) where T : IMessage
    {
      throw new NotImplementedException();
    }

    public T CreateInstance<T>() where T : IMessage
    {
      throw new NotImplementedException();
    }

    #endregion


    private void Raise(IMessage msg)
    {
      if (Actions != null)
      {
        Type messageActionGenericType = typeof(Action<>);
        foreach (Delegate action in Actions)
        {
          Type actionType = action.GetType();
          Type messageActionType = messageActionGenericType.MakeGenericType(msg.GetType());
          if (actionType == messageActionType)
            messageActionType.InvokeMember("Invoke", BindingFlags.InvokeMethod, null, action, new object[] { msg });
        }
      }
    }


    private List<Delegate> Actions = new List<Delegate>();


    public IDisposable RegisterScopedAction<T>(Action<T> callback) where T : IMessage
    {
      Actions.Add(callback);
      return new DisposableRegistration<T>(callback, Actions);
    }


    private class DisposableRegistration<T> : IDisposable
    {
      Action<T> Callback;
      List<Delegate> Actions;

      public DisposableRegistration(Action<T> callback, List<Delegate> actions)
      {
        Callback = callback;
        Actions = actions;
      }

      void IDisposable.Dispose()
      {
        Actions.Remove(Callback);
      }
    }
  }
}
