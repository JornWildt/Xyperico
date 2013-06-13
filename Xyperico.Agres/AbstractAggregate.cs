﻿using System.Collections.Generic;
using Xyperico.Base;


namespace Xyperico.Agres
{
  public abstract class AbstractAggregate<TId> : IHaveIdentity<TId>
    where TId : IIdentity
  {
    const string RestoreMethodName = "RestoreFrom";

    protected List<IEvent> Changes { get; set; }

    
    public AbstractAggregate()
    {
      Changes = new List<IEvent>();
    }


    public abstract TId Id { get; protected set; }


    public IEnumerable<IEvent> GetChanges()
    {
      return Changes;
    }


    protected void Publish(IEvent e)
    {
      Changes.Add(e);
      Mutate(e);
    }


    protected void Mutate(IEnumerable<IEvent> events)
    {
      foreach (IEvent e in events)
        Mutate(e);
    }


    protected void Mutate(IEvent e)
    {
      MethodInvoke.InvokeMethodOptional(this, RestoreMethodName, e);
    }
  }
}
