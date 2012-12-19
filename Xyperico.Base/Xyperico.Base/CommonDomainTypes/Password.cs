using System;
using System.Security.Cryptography;
using System.Text;
using CuttingEdge.Conditions;


namespace Xyperico.Base.CommonDomainTypes
{
  [Serializable]
  public class Password : IEquatable<Password>
  {
    private string HashedPassword;

    public enum PasswordArgumentType { PlainText, Hashed }


    public Password(string prefix, string p)
      : this(prefix, p, PasswordArgumentType.PlainText)
    {
    }


    public Password(string prefix, string p, PasswordArgumentType passwordType)
    {
      Condition.Requires(p, "p").IsNotNull();
      if (passwordType == PasswordArgumentType.PlainText)
        HashedPassword = Hash(prefix +":" + p);
      else
        HashedPassword = p;
    }


    public virtual bool Matches(string prefix, string p)
    {
      Condition.Requires(prefix, "prefix").IsNotNull();
      Condition.Requires(p, "p").IsNotNull();
      return Hash(prefix + ":" + p) == HashedPassword;
    }


    protected virtual string Hash(string p)
    {
      if (p == null)
        return null;

      byte[] data = Encoding.UTF8.GetBytes(p);
      byte[] result;
      SHA256 shaM = new SHA256Managed();
      result = shaM.ComputeHash(data);
      return Convert.ToBase64String(result);
    }


    public override bool Equals(object obj)
    {
      Password op = obj as Password;
      if (obj == null)
        return false;
      return Equals(op);
    }


    public override int GetHashCode()
    {
      return HashedPassword.GetHashCode();
    }


    public override string ToString()
    {
      return HashedPassword;
    }


    #region IEquatable<Password> Members

    public bool Equals(Password other)
    {
      return HashedPassword == other.HashedPassword;
    }

    #endregion
  }
}
