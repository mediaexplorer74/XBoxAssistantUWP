﻿// *********************************************************
// Type: Microsoft.Phone.Marketplace.Util.InputBuffer
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;


namespace Microsoft.Phone.Marketplace.Util
{
  internal class InputBuffer
  {
    private byte[] buffer;
    private int start;
    private int end;
    private uint bitBuffer;
    private int bitsInBuffer;

    public int AvailableBits => this.bitsInBuffer;

    public int AvailableBytes => this.end - this.start + this.bitsInBuffer / 8;

    public bool EnsureBitsAvailable(int count)
    {
      if (this.bitsInBuffer < count)
      {
        if (this.NeedsInput())
          return false;
        this.bitBuffer |= (uint) this.buffer[this.start++] << this.bitsInBuffer;
        this.bitsInBuffer += 8;
        if (this.bitsInBuffer < count)
        {
          if (this.NeedsInput())
            return false;
          this.bitBuffer |= (uint) this.buffer[this.start++] << this.bitsInBuffer;
          this.bitsInBuffer += 8;
        }
      }
      return true;
    }

    public uint TryLoad16Bits()
    {
      if (this.bitsInBuffer < 8)
      {
        if (this.start < this.end)
        {
          this.bitBuffer |= (uint) this.buffer[this.start++] << this.bitsInBuffer;
          this.bitsInBuffer += 8;
        }
        if (this.start < this.end)
        {
          this.bitBuffer |= (uint) this.buffer[this.start++] << this.bitsInBuffer;
          this.bitsInBuffer += 8;
        }
      }
      else if (this.bitsInBuffer < 16 && this.start < this.end)
      {
        this.bitBuffer |= (uint) this.buffer[this.start++] << this.bitsInBuffer;
        this.bitsInBuffer += 8;
      }
      return this.bitBuffer;
    }

    private uint GetBitMask(int count) => (uint) ((1 << count) - 1);

    public int GetBits(int count)
    {
      if (!this.EnsureBitsAvailable(count))
        return -1;
      int bits = (int) this.bitBuffer & (int) this.GetBitMask(count);
      this.bitBuffer >>= count;
      this.bitsInBuffer -= count;
      return bits;
    }

    public int CopyTo(byte[] output, int offset, int length)
    {
      int num1 = 0;
      while (this.bitsInBuffer > 0 && length > 0)
      {
        output[offset++] = (byte) this.bitBuffer;
        this.bitBuffer >>= 8;
        this.bitsInBuffer -= 8;
        --length;
        ++num1;
      }
      if (length == 0)
        return num1;
      int num2 = this.end - this.start;
      if (length > num2)
        length = num2;
      Array.Copy((Array) this.buffer, this.start, (Array) output, offset, length);
      this.start += length;
      return num1 + length;
    }

    public bool NeedsInput() => this.start == this.end;

    public void SetInput(byte[] buffer, int offset, int length)
    {
      this.buffer = buffer;
      this.start = offset;
      this.end = offset + length;
    }

    public void SkipBits(int n)
    {
      this.bitBuffer >>= n;
      this.bitsInBuffer -= n;
    }

    public void SkipToByteBoundary()
    {
      this.bitBuffer >>= this.bitsInBuffer % 8;
      this.bitsInBuffer -= this.bitsInBuffer % 8;
    }
  }
}
