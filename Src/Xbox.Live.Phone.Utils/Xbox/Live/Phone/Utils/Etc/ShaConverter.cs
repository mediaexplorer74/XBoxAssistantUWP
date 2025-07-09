// *********************************************************
// Type: Xbox.Live.Phone.Utils.Etc.ShaConverter
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using System;


namespace Xbox.Live.Phone.Utils.Etc
{
  public class ShaConverter
  {
    public const int ShortManifestLength = 1000;
    public const int LongManifestLength = 1021;

    public static byte[] Sha1000to1021(byte[] input)
    {
      if (input == null)
        return (byte[]) null;
      byte[] destinationArray = new byte[1021];
      destinationArray[0] = (byte) 1;
      Array.Copy((Array) input, 0, (Array) destinationArray, 1, 1000);
      for (int index = 1001; index < destinationArray.Length; ++index)
        destinationArray[index] = (byte) 0;
      return destinationArray;
    }

    public static byte[] Sha1021to1000(byte[] input)
    {
      if (input == null)
        return (byte[]) null;
      byte[] destinationArray = new byte[1000];
      Array.Copy((Array) input, 1, (Array) destinationArray, 0, 1000);
      return destinationArray;
    }
  }
}
