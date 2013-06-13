using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Xyperico.Agres
{
  internal static class MethodInvoke
  {
    public static void InvokeMethodOptional(object instance, string methodName, object parameter)
    {
      MethodInfo info = MethodInfoCache.GetMethodInfo(instance.GetType(), methodName, parameter.GetType());
      if (info == null)
      {
        // we don't care if state does not consume events
        // they are persisted anyway
        return;
      }
      try
      {
        info.Invoke(instance, new[] { parameter });
      }
      catch (TargetInvocationException ex)
      {
        //if (null != InternalPreserveStackTraceMethod)
        //  InternalPreserveStackTraceMethod.Invoke(ex.InnerException, new object[0]);
        throw ex.InnerException;
      }
    }


    static class MethodInfoCache
    {
      // Mapping <handler class type, method parameter type> to method member info
      static readonly Dictionary<Type, Dictionary<string, MethodInfo>> Cache = new Dictionary<Type, Dictionary<string, MethodInfo>>();

      
      static string GetHashKey(string methodName, Type paramType)
      {
        return methodName + ":" + paramType.FullName;
      }


      public static MethodInfo GetMethodInfo(Type handlerType, string methodName, Type paramType)
      {
        Dictionary<string, MethodInfo> handlerMethods;
        if (!Cache.TryGetValue(handlerType, out handlerMethods))
        {
          Cache[handlerType] = handlerMethods = handlerType
            .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(m => m.GetParameters().Length == 1)
            .ToDictionary(m => GetHashKey(m.Name, m.GetParameters().First().ParameterType), m => m);
        }

        MethodInfo info = null;
        handlerMethods.TryGetValue(GetHashKey(methodName, paramType), out info);
        return info;
      }
    }
  }
}
