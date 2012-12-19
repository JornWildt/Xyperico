namespace Xyperico.Base.StandardFormats.vCards
{
  /// <summary>
  /// Specify the components of the name of the object the vCard represents.
  /// </summary>
  public class vCardN
  {
    public string FamilyName { get; set; }
    public string GivenNames { get; set; }
    public string HonorificPrefixes { get; set; }
    public string HonorificSuffixes { get; set; }
  }
}
