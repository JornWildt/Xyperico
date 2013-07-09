﻿using System;
using System.IO;


namespace Xyperico.Agres.ProtoBuf
{
  public class ProtoBufSerializer : AbstractSerializer
  {
    protected override ISerializeWorker CreateWorker(Type t)
    {
      return new ProtoBufSerializerWorker(t);
    }


    public ProtoBufSerializer()
    {
      Initialize();
    }
  }
}
