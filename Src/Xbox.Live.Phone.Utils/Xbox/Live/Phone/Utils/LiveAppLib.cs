// *********************************************************
// Type: Xbox.Live.Phone.Utils.LiveAppLib
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using System;
using System.Globalization;
using System.Text.RegularExpressions;


namespace Xbox.Live.Phone.Utils
{
  public static class LiveAppLib
  {
    private const string GamertagExpression = "^([A-Za-z][A-Za-z0-9 ]*)?$";
    private const int MaxGamertagLength = 15;

    public static bool CompareByteArray(byte[] a, byte[] b)
    {
      if (a == b)
        return true;
      if (a == null || b == null || a.Length != b.Length)
        return false;
      for (int index = 0; index < a.Length; ++index)
      {
        if ((int) a[index] != (int) b[index])
          return false;
      }
      return true;
    }

    public static byte[] ManifestStringToByteArray(string manifestString)
    {
      manifestString = !string.IsNullOrEmpty(manifestString) ? manifestString.Replace("0x", string.Empty) : throw new ArgumentNullException(nameof (manifestString));
      byte[] byteArray = new byte[manifestString.Length / 2];
      for (int index = 0; index < manifestString.Length / 2; ++index)
        byteArray[index] = byte.Parse(manifestString.Substring(index * 2, 2), NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture);
      return byteArray;
    }

    public static bool IsValidGamertag(string gamertag)
    {
      bool flag = false;
      if (!string.IsNullOrEmpty(gamertag))
      {
        gamertag = gamertag.Trim();
        if (gamertag.Length > 0 && gamertag.Length <= 15)
          flag = Regex.IsMatch(gamertag, "^([A-Za-z][A-Za-z0-9 ]*)?$");
      }
      return flag;
    }
  }
}
