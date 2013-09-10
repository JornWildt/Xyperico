using System;
using System.Collections.Generic;
using ProtoBuf.Meta;


namespace Xyperico.Agres.ProtoBuf
{
  public static class SerializerSetup
  {
    private class TypeInfo
    {
      MetaType Meta;
      int Tag;

      public TypeInfo(MetaType meta)
      {
        Meta = meta;
        Tag = 10;
      }

      public void RegisterInheritance(Type derivedType)
      {
        Meta.AddSubType(Tag++, derivedType);
      }
    }

    static Dictionary<Type, TypeInfo> MetaTypes = new Dictionary<Type, TypeInfo>();


    public static void RegisterInheritance<TBase, TDerived>()
      where TDerived : TBase
    {
      TypeInfo info;
      if (!MetaTypes.ContainsKey(typeof(TBase)))
      {
        MetaType meta = RuntimeTypeModel.Default.Add(typeof(TBase), true);
        info = MetaTypes[typeof(TBase)] = new TypeInfo(meta);
      }
      else
        info = MetaTypes[typeof(TBase)];

      info.RegisterInheritance(typeof(TDerived));
    }
  }
}
