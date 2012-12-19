namespace Xyperico.Base.StandardFormats.vCards
{
  public class vCardMultiString : vCardMultiValue<vCardMultiString>
  {
    public string Value { get; set; }

    public vCardMultiString()
    {
    }

    public vCardMultiString(string s)
    {
      Value = s;
    }
  }
}
