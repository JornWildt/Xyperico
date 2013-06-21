using System;
using System.Collections.Generic;
using ProtoBuf.Meta;


namespace Xyperico.Agres.ProtoBuf
{
  public static class SerializerSetup
  {
    private static MetaType IdentityMeta = null;
    private static int IdentityTag = 10;


    public static void RegisterIdentity(Type t)
    {
      if (IdentityMeta == null)
      {
        IdentityMeta = RuntimeTypeModel.Default.Add(typeof(Identity<Guid>), false).Add(1, "Id");
      }

      IdentityMeta.AddSubType(IdentityTag++, t);
    }
  }
}
