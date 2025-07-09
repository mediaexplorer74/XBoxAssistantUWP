// *********************************************************
// Type: Microsoft.Phone.Marketplace.DownloadVerifier
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using Microsoft.Phone.Marketplace.Util;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;


namespace Microsoft.Phone.Marketplace
{
  public static class DownloadVerifier
  {
    public static void VerifyAsync(
      Uri downloadUrl,
      Stream downloadStream,
      Action<bool> resultCallback)
    {
      new Thread((ParameterizedThreadStart) (s => resultCallback(DownloadVerifier.Verify(downloadUrl, downloadStream))))
      {
        IsBackground = true
      }.Start();
    }

    public static bool Verify(Uri downloadUrl, Stream downloadStream)
    {
      try
      {
        byte[] key = DownloadVerifier.GetKey(downloadUrl);
        if (key == null)
          return false;
        byte[] hash1 = DownloadVerifier.GetHash(downloadUrl);
        if (hash1 == null)
          return false;
        byte[] hash2 = DownloadVerifier.ComputeHash((Stream) new ConcatStream((Stream) new MemoryStream(key), downloadStream));
        return DownloadVerifier.CompareHashes(hash1, hash2);
      }
      catch
      {
        return false;
      }
    }

    public static bool IsValidDownloadUri(Uri downloadUrl)
    {
      try
      {
        byte[] key = DownloadVerifier.GetKey(downloadUrl);
        if (key == null)
          return false;
        byte[] hash1 = DownloadVerifier.GetHash(downloadUrl);
        if (hash1 == null)
          return false;
        MemoryStream secondStream = new MemoryStream();
        byte[] hash2 = DownloadVerifier.ComputeHash((Stream) new ConcatStream((Stream) new MemoryStream(key), (Stream) secondStream));
        return !DownloadVerifier.CompareHashes(hash1, hash2);
      }
      catch
      {
        return false;
      }
    }

    private static byte[] GetKey(Uri downloadUrl)
    {
      string str = downloadUrl.ToString();
      int length = str.IndexOf('?');
      if (length != -1)
        str = str.Substring(0, length);
      string[] strArray = str.Split(new char[1]{ '/' }, StringSplitOptions.RemoveEmptyEntries);
      return strArray.Length < 2 ? (byte[]) null : Encoding.UTF8.GetBytes(strArray[strArray.Length - 2] + (object) '\\' + strArray[strArray.Length - 1]);
    }

    private static byte[] GetHash(Uri downloadUrl)
    {
      string str1 = downloadUrl.Query;
      if (str1.StartsWith("?"))
        str1 = str1.Substring(1);
      string str2 = str1;
      char[] separator1 = new char[1]{ '&' };
      foreach (string str3 in str2.Split(separator1, StringSplitOptions.RemoveEmptyEntries))
      {
        char[] separator2 = new char[1]{ '=' };
        string[] strArray = str3.Split(separator2, StringSplitOptions.RemoveEmptyEntries);
        if (strArray.Length > 1 && strArray[0] == "hash")
          return Convert.FromBase64String(Uri.UnescapeDataString(strArray[1]));
      }
      return (byte[]) null;
    }

    private static byte[] ComputeHash(Stream stream)
    {
      SHA256Managed shA256Managed = new SHA256Managed();
      shA256Managed.ComputeHash(stream);
      return shA256Managed.Hash;
    }

    private static bool CompareHashes(byte[] hash1, byte[] hash2)
    {
      if (hash1.Length != hash2.Length)
        return false;
      byte num = 0;
      int index = 0;
      for (int length = hash1.Length; index < length; ++index)
        num |= (byte) ((uint) hash1[index] ^ (uint) hash2[index]);
      return num == (byte) 0;
    }
  }
}
