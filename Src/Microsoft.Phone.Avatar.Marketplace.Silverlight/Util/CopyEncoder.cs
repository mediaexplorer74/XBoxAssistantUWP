// *********************************************************
// Type: Microsoft.Phone.Marketplace.Util.CopyEncoder
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;


namespace Microsoft.Phone.Marketplace.Util
{
  internal class CopyEncoder
  {
    private const int PaddingSize = 5;
    private const int MaxUncompressedBlockSize = 65536;

    public void GetBlock(DeflateInput input, OutputBuffer output, bool isFinal)
    {
      int num = 0;
      if (input != null)
      {
        num = Math.Min(input.Count, output.FreeBytes - 5 - output.BitsInBuffer);
        if (num > 65531)
          num = 65531;
      }
      if (isFinal)
        output.WriteBits(3, 1U);
      else
        output.WriteBits(3, 0U);
      output.FlushBits();
      this.WriteLenNLen((ushort) num, output);
      if (input == null || num <= 0)
        return;
      output.WriteBytes(input.Buffer, input.StartIndex, num);
      input.ConsumeBytes(num);
    }

    private void WriteLenNLen(ushort len, OutputBuffer output)
    {
      output.WriteUInt16(len);
      ushort num = ~len;
      output.WriteUInt16(num);
    }
  }
}
