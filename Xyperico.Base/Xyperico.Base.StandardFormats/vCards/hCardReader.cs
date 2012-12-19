using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using HtmlAgilityPack;
using System.Net;


namespace Xyperico.Base.StandardFormats.vCards
{
  public class hCardReader
  {
    public vCard Read(Uri url)
    {
      WebClient client = new WebClient();
      using (Stream s = client.OpenRead(url))
      {
        return Read(s);
      }
    }


    public vCard Read(Stream s)
    {
      using (TextReader reader = new StreamReader(s))
      {
        return Read(reader);
      }
    }


    public vCard Read(TextReader reader)
    {
      HtmlDocument doc = new HtmlDocument();
      doc.Load(reader);
      return Read(doc);
    }


    public vCard Read(HtmlDocument doc)
    {
      vCard c = ReadRootElement(doc);
      return c;
    }


    private vCard ReadRootElement(HtmlDocument doc)
    {
      HtmlNode hCardNode = doc.DocumentNode.SelectSingleNode("//" + hCardReaderExtensions.GetClassSelector("vcard"));
      if (hCardNode == null)
        return null;

      vCard c = new vCard();
      c.Name = doc.DocumentNode.SelectSingleNode("//html/head/title").SafeDot(n => n.InnerText);
      c.Kind = ReadTextProperty(hCardNode, "kind");
      c.Fn = ReadMultiString(hCardNode, "fn");
      c.N = ReadN(hCardNode);
      c.Nickname = ReadMultiString(hCardNode, "nickname");
      c.Photo = ReadPhoto(hCardNode, "photo");
      c.Sex = ReadSex(hCardNode);
      c.EMail = ReadEMail(hCardNode);
      c.Logo = ReadPhoto(hCardNode, "logo");
      c.Org = ReadOrg(hCardNode);
      c.Url = ReadUrl(hCardNode);
      c.Adr = ReadAdr(hCardNode);
      c.Tel = ReadTel(hCardNode);
      c.Note = ReadMultiString(hCardNode, "note");

      PerformImplied_N_Optimization(c);

      if (c.Org != null && c.Org.Default != null && c.Kind == null)
        c.Kind = "org";
      else if (c.Kind == null)
        c.Kind = "individual";

      return c;
    }


    private void PerformImplied_N_Optimization(vCard c)
    {
      if (c.Fn.Default != null)
      {
        string[] words = c.Fn.Default.Value.Split(' ');
        if (words.Length == 2 && (c.N == null || c.N.FamilyName == null || c.N.GivenNames == null))
        {
          c.N = new vCardN
          {
            GivenNames = words[0],
            FamilyName = words[1]
          };
        }
      }
    }


    private vCardN ReadN(HtmlNode hCardNode)
    {
      HtmlNode nNode = hCardNode.SelectSingleNodeFromClass("n");
      if (nNode == null)
        return null;

      return new vCardN
      {
        FamilyName = ReadTextProperty(nNode, "family-name"),
        GivenNames = ReadTextProperty(nNode, "given-name"),
        HonorificPrefixes = ReadTextProperty(nNode, "honorific-prefix"),
        HonorificSuffixes = ReadTextProperty(nNode, "honorific-suffix")
      };
    }


    private vCardMultiValueSet<vCardMultiPhoto> ReadPhoto(HtmlNode hCardNode, string className)
    {
      return ReadMultiValue<vCardMultiPhoto>(hCardNode, className,
        (node, item) =>
        {
          HtmlAttribute srcAttribute = node.Attributes["src"];
          if (srcAttribute != null)
          {
            item.Source = srcAttribute.Value;
          }
          else
          {
            item.Source = ReadTextValue(node);
          }
        });
    }


    private vCardSex ReadSex(HtmlNode node)
    {
      string s = ReadTextProperty(node, "sex");
      if (s == null)
        return vCardSex.NotKnown;

      vCardSex vsex = vCardSex.NotKnown;
      Enum.TryParse(s, out vsex);
      return vsex;
    }


    private vCardMultiValueSet<vCardMultiOrg> ReadOrg(HtmlNode hCardNode)
    {
      return ReadMultiValue<vCardMultiOrg>(hCardNode, "org",
        (node, item) =>
        {
          // (Implied "organization-name" Optimization)
          // Get "organization-name" node - if not available, assume inner HTML is the complete name
          HtmlNode organizationNameNode = node.SelectSingleNodeFromClass("organization-name");
          if (organizationNameNode == null)
          {
            item.Name = ReadTextValue(node);
          }
          else
          {
            item.Name = ReadTextValue(organizationNameNode);
          }
        });
    }


    private vCardMultiValueSet<vCardMultiString> ReadUrl(HtmlNode hCardNode)
    {
      return ReadMultiValue<vCardMultiString>(hCardNode, "url",
        (node, item) => item.Value = node.Attributes["href"].SafeDot(a => a.Value));
    }


    private vCardMultiValueSet<vCardMultiAdr> ReadAdr(HtmlNode hCardNode)
    {
      return ReadMultiValue<vCardMultiAdr>(hCardNode, "adr",
        (node, item) =>
        {
          item.Country = ReadTextProperty(node, "country-name");
          item.ExtendedAddress = ReadTextProperty(node, "extended-address");
          item.Locality = ReadTextProperty(node, "locality");
          item.PostalCode = ReadTextProperty(node, "postal-code");
          item.PostOfficeBox = ReadTextProperty(node, "post-office-box");
          item.Region = ReadTextProperty(node, "region");
          item.StreetAddress = ReadTextProperty(node, "street-address");
        });
    }


    private vCardMultiValueSet<vCardMultiString> ReadTel(HtmlNode hCardNode)
    {
      return ReadMultiString(hCardNode, "tel");
    }


    private vCardMultiValueSet<vCardMultiString> ReadEMail(HtmlNode hCardNode)
    {
      return ReadMultiValue<vCardMultiString>(hCardNode, "email",
        (node, item) => 
          {
            if (node.Attributes.Contains("href"))
              item.Value = ReduceUrlToValue(node.Attributes["href"].Value);
            else
              item.Value = ReduceUrlToValue(ReadTextValue(node));
          });
    }


    private vCardMultiValueSet<vCardMultiString> ReadMultiString(HtmlNode hCardNode, string className)
    {
      return ReadMultiValue<vCardMultiString>(hCardNode, className,
        (node, item) => item.Value = ReadTextValue(node));
    }


    private vCardMultiValueSet<T> ReadMultiValue<T>(HtmlNode hCardNode, string className, Action<HtmlNode,T> reader)
      where T : vCardMultiValue<T>, new()
    {
      vCardMultiValueSet<T> result = new vCardMultiValueSet<T>();
      HtmlNodeCollection classNodes = hCardNode.SelectNodesFromClass(className);
      if (classNodes != null)
      {
        foreach (HtmlNode classNode in classNodes)
        {
          T item = new T();
          item.Type = ReadTextProperty(classNode, "type");
          reader(classNode, item);
          result.Items.Add(item);
        }
      }
      return result;
    }


    private string ReadTextProperty(HtmlNode node, string className)
    {
      HtmlNode subNode = node.SelectSingleNodeFromClass(className);
      if (subNode == null)
        return null;
      return ReadTextValue(subNode);
    }


    private string ReadTextValue(HtmlNode node)
    {
      HtmlNodeCollection valueNodes = node.SelectNodesFromClass("value");
      if (valueNodes != null && valueNodes.Count > 0)
      {
        string result = "";
        foreach (HtmlNode valueNode in valueNodes)
          result += ReadTextValue(valueNode);
        return result;
      }
      else if (node.Name.ToLower() == "abbr")
      {
        HtmlAttribute a = node.Attributes["title"];
        if (a != null)
          return a.Value;
      }

      string text = "";
      foreach (HtmlNode textNode in node.ChildNodes)
        if (textNode.NodeType == HtmlNodeType.Text)
          text += textNode.InnerText;
      return text;
    }


    private string ReduceUrlToValue(string url)
    {
      int colonPos = url.IndexOf(":");
      if (colonPos >= 0)
        url = url.Substring(colonPos + 1);

      int questionmarkPos = url.IndexOf("?");
      if (questionmarkPos >= 0)
        url = url.Substring(0, questionmarkPos);

      return url;
    }
  }


  internal static class hCardReaderExtensions
  {
    public static HtmlNodeCollection SelectNodesFromClass(this HtmlNode node, string className)
    {
      HtmlNodeCollection selectedNodes = new HtmlNodeCollection(node);

      // Iterate through all immediate child nodes
      foreach (HtmlNode subNode in node.ChildNodes)
      {
        // Skip nodes containing an embedded card
        if (!subNode.ContainsClassName("vcard"))
        {
          // If sub-node contains class name then add it to the list
          if (subNode.ContainsClassName(className))
          {
            selectedNodes.Add(subNode);
          }
          // Otherwise select nodes recursively from descendants
          else
          {
            HtmlNodeCollection selectedSubNodes = SelectNodesFromClass(subNode, className);
            foreach (HtmlNode sn in selectedSubNodes)
              selectedNodes.Add(sn);
          }
        }
      }

      return selectedNodes;
    }


    public static HtmlNode SelectSingleNodeFromClass(this HtmlNode node, string className)
    {
      HtmlNode subNode = node.SelectNodesFromClass(className).FirstOrDefault();
      return subNode;
    }


    public static bool ContainsClassName(this HtmlNode node, string className)
    {
      HtmlAttribute classAttribute = node.Attributes["class"];
      if (classAttribute != null)
      {
        string classValue = classAttribute.Value;
        int classPos = classValue.IndexOf(className);
        int classPosEnd = classPos + className.Length;

        // Check for class name 1) at beginning, 2) between spaces or, 3) at end
        if ((classPos == 0 || classPos > 0 && classValue[classPos - 1] == ' ')
            && (classPosEnd == classValue.Length || classPosEnd > 0 && classValue[classPosEnd] == ' '))
        {
          return true;
        }
      }
      return false;
    }


    public static string GetClassSelector(string className)
    {
      return "*[@class='" + className + "' or contains(@class,' " + className + " ') or contains(@class,' " + className + "') or contains(@class,'" + className + " ')]";
    }


    public static TProperty SafeDot<TClass, TProperty>(this TClass instance, Func<TClass, TProperty> getter)
      where TProperty : class
    {
      if (instance == null)
        return null;
      return getter(instance);
    }
  }
}
