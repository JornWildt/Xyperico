using System;
using System.Collections.Generic;


namespace Xyperico.Base.StandardFormats.vCards
{
  public class vCardMultiAdr : vCardMultiValue<vCardMultiAdr>
  {
    public string PostOfficeBox { get; set; }
    public string ExtendedAddress { get; set; }
    public string StreetAddress { get; set; }
    public string Locality { get; set; }
    public string PostalCode { get; set; }
    public string Region { get; set; }
    public string Country { get; set; }
  }
}
