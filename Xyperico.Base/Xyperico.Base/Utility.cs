using System;
using System.Collections.Specialized;
using System.Reflection;


namespace Xyperico.Base
{
  public static class Utility
  {
    public static NameValueCollection GetNameValueCollection(object args)
    {
      NameValueCollection parameters = new NameValueCollection();

      if (args == null)
        return parameters;

      Type t = args.GetType();

      foreach (PropertyInfo p in t.GetProperties())
      {
        if (p.CanRead)
        {
          object argValue = p.GetValue(args, null);
          string argName = p.Name;
          if (argValue != null)
            parameters.Add(argName, argValue.ToString());
        }
      }

      return parameters;
    }
  }
}
