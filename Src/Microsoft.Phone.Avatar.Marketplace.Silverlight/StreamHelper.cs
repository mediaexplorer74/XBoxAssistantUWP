// *********************************************************
// Type: Microsoft.Phone.Marketplace.StreamHelper
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System.IO;


namespace Microsoft.Phone.Marketplace
{
  internal static class StreamHelper
  {
    internal static void CopyStream(Stream source, Stream dest)
    {
      byte[] buffer = new byte[4096];
      while (true)
      {
        int count = source.Read(buffer, 0, buffer.Length);
        if (count != 0)
          dest.Write(buffer, 0, count);
        else
          break;
      }
    }
  }
}
