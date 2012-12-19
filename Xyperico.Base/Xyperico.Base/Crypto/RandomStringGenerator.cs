using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Xyperico.Base.Crypto
{
  public static class RandomStringGenerator
  {
    public const string ALPHA = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public const string NUMERIC = "0123456789";
    public const string ALPHA_NUMERIC = ALPHA + NUMERIC;

    public static string GenerateRandomString(int length, string characterSet = ALPHA_NUMERIC)
    {
      byte[] data = GenerateRandomBytes(length);

      string randomString = "";
      int characterSetLength = characterSet.Length;

      for (int i = 0; i < length; i++)
      {
        int position = data[i];
        position = (position % characterSetLength);
        randomString = (randomString + characterSet.Substring(position, 1));
      }

      return randomString;
    }


    public static byte[] GenerateRandomBytes(int length)
    {
      RandomNumberGenerator randomizer = RandomNumberGenerator.Create();
      byte[] data = new byte[length];
      randomizer.GetBytes(data);
      return data;
    }
  }
}
