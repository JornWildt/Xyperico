using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace Xyperico.Base.CommonDomainTypes
{
  [Serializable]
  [DataContract]
  public class EMail : IEquatable<EMail>
  {
    [DataMember(Order = 1)]
    private string _email { get; set; }


    public EMail(string email)
    {
      ValidateEMail(email, "email");
      _email = email;
    }


    public EMail()
    {
    }


    public static void ValidateEMail(string email, string parameterName)
    {
      if (email == null)
        throw new ArgumentNullException(parameterName, "Got null argument for EMail constructor.");
      if (!email.Contains("@"))
        throw new ArgumentException(string.Format("The e-mail '{0}' does not contain any @-character.", email), parameterName);
      if (email.Length < 3)
        throw new ArgumentException(string.Format("The e-mail '{0}' is too short.", email), parameterName);
    }


    public override string ToString()
    {
      return _email;
    }


    public override bool Equals(object obj)
    {
      EMail op = obj as EMail;
      if (obj == null)
        return false;
      return Equals(op);
    }


    public override int GetHashCode()
    {
      return _email.GetHashCode();
    }


    #region IEquatable<EMail> Members

    public bool Equals(EMail other)
    {
      return other != null && _email == other._email;
    }

    #endregion
  }
}
