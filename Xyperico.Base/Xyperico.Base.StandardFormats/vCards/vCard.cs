using System.Collections.Generic;
using System.Linq;


namespace Xyperico.Base.StandardFormats.vCards
{
  public enum vCardSex { NotKnown = 0, Male = 1, Female = 2, NotApplicable = 9 }


  // http://tools.ietf.org/html/draft-ietf-vcarddav-vcardrev-11#section-6.1.4
  // http://tools.ietf.org/html/draft-ietf-vcarddav-vcardxml-05
  public class vCard
  {
    internal const string vcard40NS = "urn:ietf:params:xml:ns:vcard-4.0";

    /// <summary>
    /// Identify the source of directory information contained in the content type.
    /// </summary>
    public vCardMultiValueSet<vCardMultiString> Source { get; set; }

    /// <summary>
    /// Identify the displayable name of the directory entity to which information in the vCard pertains.
    /// </summary>
    /// <remarks>The NAME property is used to convey the display name of the entity to 
    /// which the directory information pertains.  Its value is the displayable, 
    /// presentation text associated with the source for the vCard, as specified in 
    /// the SOURCE property.</remarks>
    public string Name { get; set; }

    /// <summary>
    /// Specify the kind of object the vCard represents.
    /// </summary>
    /// <remarks>The value may be one of: "individual" for a single person, "group" for a group of people, 
    /// "org" for an organization, "location" for a named geographical place, "thing" for an inanimate 
    /// object (e.g. a device, a server, etc.), an x-name or an iana-token.  If this property is absent, 
    /// "individual" MUST be assumed as default.</remarks>
    public string Kind { get; set; }

    /// <summary>
    /// Specify the formatted text corresponding to the name of the object the vCard represents.
    /// </summary>
    /// <remarks>This property is based on the semantics of the X.520
    /// Common Name attribute. The property MUST be present in the vCard object..</remarks>
    public vCardMultiValueSet<vCardMultiString> Fn { get; set; }

    /// <summary>
    /// Specify the components of the name of the object the vCard represents.
    /// </summary>
    public vCardN N { get; set; }

    /// <summary>
    /// Specify the text corresponding to the nickname of the object the vCard represents.
    /// </summary>
    public vCardMultiValueSet<vCardMultiString> Nickname { get; set; }

    /// <summary>
    /// Specify an image or photograph information that annotates some aspect of the object the vCard represents.
    /// </summary>
    public vCardMultiValueSet<vCardMultiPhoto> Photo { get; set; }

    /// <summary>
    /// Specify the sex of the object the vCard represents.
    /// </summary>
    /// <remarks>The value 0 stands for "not known", 1 stands for "male", 2 stands for "female", 
    /// and 9 stands for "not applicable".</remarks>
    public vCardSex Sex { get; set; }

    /// <summary>
    /// Specify the components of the delivery address for the vCard object.
    /// </summary>
    public vCardMultiValueSet<vCardMultiAdr> Adr { get; set; }

    /// <summary>
    /// Specify the telephone number for telephony communication with the object the vCard represents.
    /// </summary>
    public vCardMultiValueSet<vCardMultiString> Tel { get; set; }

    /// <summary>
    /// Specify the electronic mail address for communication with the object the vCard represents.
    /// </summary>
    public vCardMultiValueSet<vCardMultiString> EMail { get; set; }

    /// <summary>
    /// Specify the position or job of the object the vCard represents.
    /// </summary>
    public vCardMultiValueSet<vCardMultiString> Title { get; set; }

    /// <summary>
    /// Specify a graphic image of a logo associated with the object the vCard represents.
    /// </summary>
    public vCardMultiValueSet<vCardMultiPhoto> Logo { get; set; }

    /// <summary>
    /// Specify the organizational name and units associated with the vCard.
    /// </summary>
    public vCardMultiValueSet<vCardMultiOrg> Org { get; set; }

    /// <summary>
    /// Specify supplemental information or a comment that is associated with the vCard.
    /// </summary>
    public vCardMultiValueSet<vCardMultiString> Note { get; set; }

    /// <summary>
    /// Specify a uniform resource locator associated with the object that the vCard refers to.
    /// </summary>
    public vCardMultiValueSet<vCardMultiString> Url { get; set; }


    public vCard()
    {
      Kind = "individual";
      Source = new vCardMultiValueSet<vCardMultiString>();
      Fn = new vCardMultiValueSet<vCardMultiString>();
      Nickname = new vCardMultiValueSet<vCardMultiString>();
      Fn = new vCardMultiValueSet<vCardMultiString>();
      Adr = new vCardMultiValueSet<vCardMultiAdr>();
      Tel = new vCardMultiValueSet<vCardMultiString>();
      EMail = new vCardMultiValueSet<vCardMultiString>();
      Title = new vCardMultiValueSet<vCardMultiString>();
      Logo = new vCardMultiValueSet<vCardMultiPhoto>();
      Org = new vCardMultiValueSet<vCardMultiOrg>();
      Note = new vCardMultiValueSet<vCardMultiString>();
      Url = new vCardMultiValueSet<vCardMultiString>();
    }


    #region Extended properties

    internal class PropertyRegistration
    {
      public string PropertyName;
      public string PropertyNS;
      public vCardProperty Property;
    }

    internal List<PropertyRegistration> PropertyRegistrations = new List<PropertyRegistration>();


    public void AddExtendedProperty(string propertyName, string propertyNS, vCardProperty property)
    {
      PropertyRegistrations.Add(new PropertyRegistration()
      {
        PropertyName = propertyName,
        PropertyNS = propertyNS,
        Property = property
      });
    }


    public vCardProperty GetExtendedProperty(string propertyName, string propertyNS)
    {
      PropertyRegistration reg = PropertyRegistrations.FirstOrDefault(pr => pr.PropertyName == propertyName && pr.PropertyNS == propertyNS);
      if (reg == null)
        return null;
      return reg.Property;
    }

    #endregion
  }
}
