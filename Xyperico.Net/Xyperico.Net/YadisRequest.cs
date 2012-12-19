using System;
using System.IO;
using System.Net;
using CuttingEdge.Conditions;


namespace Xyperico.Net
{
  public class YadisRequest
  {
    public Uri Url { get; private set; }

    public Uri DiscoveredUrl { get; private set; }

    public string Trace { get; private set; }


    public YadisRequest(string url)
      : this(new Uri(url))
    {
    }


    public YadisRequest(Uri url)
    {
      Condition.Requires(url, "url").IsNotNull();
      Url = url;
    }


    public XrdsDocument GetYadisDocument()
    {
      return GetYadisDocument(Url);
    }


    private XrdsDocument GetYadisDocument(Uri url)
    {
      Trace += string.Format("Lookup URL {0}. ", url);

      // GET directly from URL
      HttpWebRequest request;
      try
      {
        request = (HttpWebRequest)HttpWebRequest.Create(url);
      }
      catch (Exception ex)
      {
        throw new YadisRequestFailedException(string.Format("Unable to build Yadis request. Got '{0}'. Tried: {1}.", ex.Message, Trace));
      }

      try
      {
        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        {
          if (response.Headers["X-XRDS-Location"] != null)
          {
            Trace += string.Format("Got X-XRDS-Location header {0}. ", response.Headers["X-XRDS-Location"]);
            Uri location;
            try
            {
              location = new Uri(response.Headers["X-XRDS-Location"]);
            }
            catch (Exception ex)
            {
              throw new YadisRequestFailedException(string.Format("Unable to discover Yadis URL {0}. Got '{1}'. Tried: {2}.", Url, ex.Message, Trace));
            }
            return GetYadisDocument(location);
          }
          else if (response.ContentType.StartsWith("application/xrds+xml"))
          {
            Trace += string.Format("Found Yadis document at {0}. ", url);
            DiscoveredUrl = url;
            try
            {
              using (Stream s = response.GetResponseStream())
              {
                return XrdsDocument.Deserialize(s);
              }
            }
            catch (Exception ex)
            {
              throw new YadisRequestFailedException(string.Format("Unable to deserialize Yadis document. Got '{0}'. Tried: {1}.", ex.Message, Trace));
            }
          }
          else
          {
            Trace += string.Format("No X-XRDS-Location header or Yadis document found.");
            throw new YadisRequestFailedException(string.Format("Unable to discover Yadis URL {0}. Tried: {1}.", Url, Trace));
          }
        }
      }
      catch (WebException ex)
      {
        throw new YadisRequestFailedException(string.Format("Unable to get Yadis response. Got '{0}'. Tried: {1}.", ex.Message, Trace));
      }
    }
  }
}
