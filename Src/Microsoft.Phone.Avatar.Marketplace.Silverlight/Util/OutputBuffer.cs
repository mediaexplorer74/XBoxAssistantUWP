// *********************************************************
// Type: Microsoft.Phone.Marketplace.Util.OutputBuffer
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;


namespace Microsoft.Phone.Marketplace.Util
{
  internal class OutputBuffer
  {
    private byte[] byteBuffer;
    private int pos;
    private uint bitBuf;
    private int bitCount;

    internal void UpdateBuffer(byte[] output)
    {
      this.byteBuffer = output;
      this.pos = 0;
    }

    internal int BytesWritten => this.pos;

    internal int FreeBytes => this.byteBuffer.Length - this.pos;

    internal void WriteUInt16(ushort value)
    {
      this.byteBuffer[this.pos++] = (byte) value;
      this.byteBuffer[this.pos++] = (byte) ((uint) value >> 8);
    }

    internal void WriteBits(int n, uint bits)
    {
      this.bitBuf |= bits << this.bitCount;
      this.bitCount += n;
      if (this.bitCount < 16)
        return;
      this.byteBuffer[this.pos++] = (byte) this.bitBuf;
      this.byteBuffer[this.pos++] = (byte) (this.bitBuf >> 8);
      this.bitCount -= 16;
      this.bitBuf >>= 16;
    }

    internal void FlushBits()
    {
      while (this.bitCount >= 8)
      {
        this.byteBuffer[this.pos++] = (byte) this.bitBuf;
        this.bitCount -= 8;
        this.bitBuf >>= 8;
      }
      if (this.bitCount <= 0)
        return;
      this.byteBuffer[this.pos++] = (byte) this.bitBuf;
      this.bitBuf = 0U;
      this.bitCount = 0;
    }

    internal void WriteBytes(byte[] byteArray, int offset, int count)
    {
      if (this.bitCount == 0)
      {
        Array.Copy((Array) byteArray, offset, (Array) this.byteBuffer, this.pos, count);
        this.pos += count;
      }
      else
        this.WriteBytesUnaligned(byteArray, offset, count);
    }

    private void WriteBytesUnaligned(byte[] byteArray, int offset, int count)
    {
      for (int index = 0; index < count; ++index)
        this.WriteByteUnaligned(byteArray[offset + index]);
    }

    private void WriteByteUnaligned(byte b) => this.WriteBits(8, (uint) b);

    internal int BitsInBuffer => this.bitCount / 8 + 1;

    internal OutputBuffer.BufferState DumpState()
    {
      OutputBuffer.BufferState bufferState;
      bufferState.pos = this.pos;
      bufferState.bitBuf = this.bitBuf;
      bufferState.bitCount = this.bitCount;
      return bufferState;
    }

    internal void RestoreState(OutputBuffer.BufferState state)
    {
      this.pos = state.pos;
      this.bitBuf = state.bitBuf;
      this.bitCount = state.bitCount;
    }

    internal struct BufferState
    {
      internal int pos;
      internal uint bitBuf;
      internal int bitCount;
    }
  }
}
