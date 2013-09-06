using System;
using System.Collections.Generic;
using ProtoBuf.Meta;


namespace Xyperico.Agres.ProtoBuf
{
  public static class SerializerSetup
  {
    private static int IdentityTag = 10;


    public static void RegisterIdentity<TId,TCoreId>()
      where TId : Identity<TCoreId>
      where TCoreId : IEquatable<TCoreId>
    {
      MetaType meta = RuntimeTypeModel.Default.Add(typeof(Identity<TCoreId>), false);

      meta.AddSubType(IdentityTag++, typeof(TId));
    }
  }
}
