using System.Xml;


namespace Xyperico.Base.StandardFormats.vCards
{
  public static class vCardExtensions
  {
    public static string SelectOptionalInnerText(this XmlNode xml, string path, XmlNamespaceManager ns)
    {
      if (xml == null)
        return null;
      XmlNode subnode = xml.SelectSingleNode(path + "/text()", ns);
      if (subnode == null)
        return null;
      return subnode.Value;
    }
  }
}
