// *********************************************************
// Type: Microsoft.Phone.Marketplace.Util.IFileFormatReader
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll


namespace Microsoft.Phone.Marketplace.Util
{
  internal interface IFileFormatReader
  {
    bool ReadHeader(InputBuffer input);

    bool ReadFooter(InputBuffer input);

    void UpdateWithBytesRead(byte[] buffer, int offset, int bytesToCopy);

    void Validate();
  }
}
