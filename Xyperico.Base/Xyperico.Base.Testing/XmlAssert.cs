using System.Xml;
using NUnit.Framework;


namespace Xyperico.Base.Testing
{
  public static class XmlAssert
  {
    public static void ContainsSingleXPathNode(XmlDocument xml, string xpath)
    {
      XmlNode foundXml = xml.SelectSingleNode(xpath);
      Assert.IsNotNull(foundXml, string.Format("XML must contain the XPath node '{0}'.", xpath));
    }


    public static void ValueOfSingleXPathNodeEquals(XmlDocument xml, string xpath, string expectedValue, XmlNamespaceManager nsmgr = null)
    {
      XmlNode foundXml = xml.SelectSingleNode(xpath, nsmgr);
      Assert.IsNotNull(foundXml, string.Format("XML must contain the XPath node '{0}'.", xpath));
      string containedValue = foundXml.InnerText;
      Assert.AreEqual(expectedValue, containedValue);
    }
  }
}
