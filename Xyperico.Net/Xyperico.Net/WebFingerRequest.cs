using CuttingEdge.Conditions;
using System;
using System.Xml;
using System.Linq;
using Xyperico.Base.Exceptions;
using System.Web;


namespace Xyperico.Net.WebFinger
{
  public class WebFingerRequest
  {
    public string EMail { get; private set; }

    public string AccountName { get; private set; }

    public string HostName { get; private set; }


    public WebFingerRequest(string email)
    {
      Condition.Requires(email, "email").IsNotNullOrEmpty();
      Xyperico.Base.CommonDomainTypes.EMail.ValidateEMail(email, "email");

      EMail = email;

      string[] emailParts = email.Trim().Split('@');
      AccountName = emailParts[0];
      HostName = emailParts[1];
    }


    public XrdDocument GetAccountDocument()
    {
      XrdDocument hostMeta = GetHostMeta();
      XrdLink LRDDLink = GetLRDDLink(hostMeta);
      string emailUrl = "acct:" + EMail;
      string fingerUrl = LRDDLink.Template.Replace("{uri}", HttpUtility.UrlEncode(emailUrl));
      XrdDocument accountDocument = XrdDocument.GetFromUrl(fingerUrl);
      return accountDocument;
    }


    private HostMetaDocument GetHostMeta()
    {
      HostMetaDocumentRequest request = new HostMetaDocumentRequest(HostName);
      HostMetaDocument hostMeta = request.GetDocument();
      return hostMeta;
    }


    private XrdLink GetLRDDLink(XrdDocument hostMeta)
    {
      XrdLink LRDDLink = hostMeta.Links.Where(l => l.Rel == "lrdd").FirstOrDefault();
      if (LRDDLink == null)
        throw new MissingResourceException(string.Format("No 'lrdd' link found in host-meta file for host {0}.", HostName));
      return LRDDLink;
    }
  }
}
