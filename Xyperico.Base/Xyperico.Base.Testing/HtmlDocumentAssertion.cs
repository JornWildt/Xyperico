using System;
using HtmlAgilityPack;


namespace Xyperico.Base.Testing
{
  public class HtmlDocumentAssertion
  {
    private HtmlDocument Html;

    public HtmlDocumentAssertion(HtmlDocument html)
    {
      Html = html;
    }

    public HtmlDocumentAssertion Where(Action<HtmlDocument> a)
    {
      a(Html);
      return this;
    }
  }
}
