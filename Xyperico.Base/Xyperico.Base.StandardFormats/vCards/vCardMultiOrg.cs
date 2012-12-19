using System.Collections.Generic;


namespace Xyperico.Base.StandardFormats.vCards
{
  public class vCardMultiOrg : vCardMultiValue<vCardMultiOrg>
  {
    public string Name { get; set; }

    public List<string> UnitNames { get; set; }


    public vCardMultiOrg()
    {
      UnitNames = new List<string>();
    }
  }
}
