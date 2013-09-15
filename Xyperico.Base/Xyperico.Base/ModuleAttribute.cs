using System;


namespace Xyperico.Base
{
  public class ModuleAttribute : Attribute
  {
    public string ModuleName { get; set; }


    public ModuleAttribute(string moduleName)
    {
      ModuleName = moduleName;
    }
  }
}
