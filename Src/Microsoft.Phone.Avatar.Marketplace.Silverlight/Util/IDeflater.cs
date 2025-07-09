// *********************************************************
// Type: Microsoft.Phone.Marketplace.Util.IDeflater
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;


namespace Microsoft.Phone.Marketplace.Util
{
  internal interface IDeflater : IDisposable
  {
    bool NeedsInput();

    void SetInput(byte[] inputBuffer, int startIndex, int count);

    int GetDeflateOutput(byte[] outputBuffer);

    bool Finish(byte[] outputBuffer, out int bytesRead);
  }
}
