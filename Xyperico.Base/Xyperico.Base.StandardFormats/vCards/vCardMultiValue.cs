using System;
using System.Collections.Generic;


namespace Xyperico.Base.StandardFormats.vCards
{
  public class vCardMultiValue<T>
  {
    public string Type { get; set; }

    public List<string> Pid { get; set; }

    public string Language { get; set; }

    public short Pref { get; set; }

    //public T Value { get; set; }
  }
}
