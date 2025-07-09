// *********************************************************
// Type: Microsoft.XboxLive.MathUtilities.DxtDecoder
// Assembly: Microsoft.XboxLive.Avatars.MathLib, Version=1.2.0.0, Culture=neutral, PublicKeyToken=7f5f5c78ffd609de
// MVID: 7604E429-D6C9-4874-8BC3-52338DCCDA22
// *********************************************************Microsoft.XboxLive.Avatars.MathLib.dll

using System;
using System.Runtime.InteropServices;


namespace Microsoft.XboxLive.MathUtilities
{
  public class DxtDecoder
  {
    private byte[] mInput;
    private int[] mOutput;
    private DxtDecoder.Colorb[] mClut = new DxtDecoder.Colorb[4];
    private DxtDecoder.Colorb[] mAlut = new DxtDecoder.Colorb[8];
    private DxtDecoder.Colorb[] mAlphas = new DxtDecoder.Colorb[16];
    private TextureDataFormat mFormat;
    private int mAlignedWidth;
    private int mAlignedHeight;
    private int m_Width;
    private int m_Height;
    private int mOffset;
    private int mBlockOffset;

    private void UnpackBlockColor(int outBlockW, int outBlockH)
    {
      ushort num1 = (ushort) ((uint) this.mInput[this.mOffset] | (uint) this.mInput[this.mOffset + 1] << 8);
      this.mOffset += 2;
      ushort num2 = (ushort) ((uint) this.mInput[this.mOffset] | (uint) this.mInput[this.mOffset + 1] << 8);
      this.mOffset += 2;
      uint num3 = (uint) (-16777216 | (int) num1 << 3 & 248 | (int) num1 << 5 & 64512 | (int) num1 << 8 & 16252928);
      uint num4 = num3 | num3 >> 5 & 458759U;
      this.mClut[0].argb = num4 | num4 >> 6 & 768U;
      uint num5 = (uint) (-16777216 | (int) num2 << 3 & 248 | (int) num2 << 5 & 64512 | (int) num2 << 8 & 16252928);
      uint num6 = num5 | num5 >> 5 & 458759U;
      this.mClut[1].argb = num6 | num6 >> 6 & 768U;
      if (this.mFormat == TextureDataFormat.Dxt1 && (int) num1 <= (int) num2)
      {
        this.mClut[2].r = (byte) ((int) this.mClut[0].r + (int) this.mClut[1].r + 1 >> 1);
        this.mClut[2].g = (byte) ((int) this.mClut[0].g + (int) this.mClut[1].g + 1 >> 1);
        this.mClut[2].b = (byte) ((int) this.mClut[0].b + (int) this.mClut[1].b + 1 >> 1);
        this.mClut[2].a = byte.MaxValue;
        this.mClut[3].argb = 0U;
      }
      else
      {
        this.mClut[2].r = (byte) ((4 * (int) this.mClut[0].r + 2 * (int) this.mClut[1].r + 3) / 6);
        this.mClut[2].g = (byte) ((4 * (int) this.mClut[0].g + 2 * (int) this.mClut[1].g + 3) / 6);
        this.mClut[2].b = (byte) ((4 * (int) this.mClut[0].b + 2 * (int) this.mClut[1].b + 3) / 6);
        this.mClut[2].a = byte.MaxValue;
        this.mClut[3].r = (byte) ((2 * (int) this.mClut[0].r + 4 * (int) this.mClut[1].r + 3) / 6);
        this.mClut[3].g = (byte) ((2 * (int) this.mClut[0].g + 4 * (int) this.mClut[1].g + 3) / 6);
        this.mClut[3].b = (byte) ((2 * (int) this.mClut[0].b + 4 * (int) this.mClut[1].b + 3) / 6);
        this.mClut[3].a = byte.MaxValue;
      }
      int num7 = 0;
      int mBlockOffset = this.mBlockOffset;
      if (this.mFormat == TextureDataFormat.Dxt2 || this.mFormat == TextureDataFormat.Dxt4)
      {
        for (int index1 = 0; index1 < outBlockH; ++index1)
        {
          int num8 = (int) this.mInput[this.mOffset++];
          for (int index2 = 0; index2 < outBlockW; ++index2)
          {
            int num9 = (int) this.mAlphas[num7 + index2].a + 1;
            DxtDecoder.Colorb colorb;
            colorb.argb = this.mClut[num8 & 3].argb;
            this.mOutput[mBlockOffset + index2] = (int) ((long) (colorb.argb >> 8 & 16711935U) * (long) num9 & 4278255360L | (long) (colorb.argb & 16711935U) * (long) num9 >> 8 & 16711935L);
            num8 >>= 2;
          }
          mBlockOffset += this.m_Width;
          num7 += 4;
        }
      }
      else
      {
        for (int index3 = 0; index3 < outBlockH; ++index3)
        {
          int num10 = (int) this.mInput[this.mOffset++];
          for (int index4 = 0; index4 < outBlockW; ++index4)
          {
            this.mOutput[mBlockOffset + index4] = (int) this.mClut[num10 & 3].argb & (int) this.mAlphas[num7 + index4].argb;
            num10 >>= 2;
          }
          mBlockOffset += this.m_Width;
          num7 += 4;
        }
      }
    }

    private void UnpackBlockAlphaDXT23()
    {
      int num1 = 0;
      for (int index1 = 0; index1 < 8; ++index1)
      {
        uint num2 = (uint) this.mInput[this.mOffset++];
        DxtDecoder.Colorb[] mAlphas1 = this.mAlphas;
        int index2 = num1;
        int num3 = index2 + 1;
        mAlphas1[index2].a = (byte) ((int) num2 << 4 | (int) num2 & 15);
        DxtDecoder.Colorb[] mAlphas2 = this.mAlphas;
        int index3 = num3;
        num1 = index3 + 1;
        mAlphas2[index3].a = (byte) (num2 & 240U | num2 >> 4);
      }
    }

    private void UnpackBlockAlphaDXT45()
    {
      byte num1 = this.mInput[this.mOffset++];
      byte num2 = this.mInput[this.mOffset++];
      this.mAlut[0].a = num1;
      this.mAlut[1].a = num2;
      if ((int) num1 <= (int) num2)
      {
        this.mAlut[2].a = (byte) ((8 * (int) num1 + 2 * (int) num2 + 5) / 10);
        this.mAlut[3].a = (byte) ((6 * (int) num1 + 4 * (int) num2 + 5) / 10);
        this.mAlut[4].a = (byte) ((4 * (int) num1 + 6 * (int) num2 + 5) / 10);
        this.mAlut[5].a = (byte) ((2 * (int) num1 + 8 * (int) num2 + 5) / 10);
        this.mAlut[6].a = (byte) 0;
        this.mAlut[7].a = byte.MaxValue;
      }
      else
      {
        this.mAlut[2].a = (byte) ((12 * (int) num1 + 2 * (int) num2 + 7) / 14);
        this.mAlut[3].a = (byte) ((10 * (int) num1 + 4 * (int) num2 + 7) / 14);
        this.mAlut[4].a = (byte) ((8 * (int) num1 + 6 * (int) num2 + 7) / 14);
        this.mAlut[5].a = (byte) ((6 * (int) num1 + 8 * (int) num2 + 7) / 14);
        this.mAlut[6].a = (byte) ((4 * (int) num1 + 10 * (int) num2 + 7) / 14);
        this.mAlut[7].a = (byte) ((2 * (int) num1 + 12 * (int) num2 + 7) / 14);
      }
      int index1 = 0;
      for (int index2 = 0; index2 < 2; ++index2)
      {
        int num3 = (int) this.mInput[this.mOffset] | (int) this.mInput[this.mOffset + 1] << 8 | (int) this.mInput[this.mOffset + 2] << 16;
        this.mOffset += 3;
        this.mAlphas[index1].argb = this.mAlut[num3 & 7].argb;
        this.mAlphas[index1 + 1].argb = this.mAlut[num3 >> 3 & 7].argb;
        this.mAlphas[index1 + 2].argb = this.mAlut[num3 >> 6 & 7].argb;
        this.mAlphas[index1 + 3].argb = this.mAlut[num3 >> 9 & 7].argb;
        this.mAlphas[index1 + 4].argb = this.mAlut[num3 >> 12 & 7].argb;
        this.mAlphas[index1 + 5].argb = this.mAlut[num3 >> 15 & 7].argb;
        this.mAlphas[index1 + 6].argb = this.mAlut[num3 >> 18 & 7].argb;
        this.mAlphas[index1 + 7].argb = this.mAlut[num3 >> 21 & 7].argb;
        index1 += 8;
      }
    }

    private void UnpackBlock(int outBlockW, int outBlockH)
    {
      switch (this.mFormat)
      {
        case TextureDataFormat.Dxt2:
        case TextureDataFormat.Dxt3:
          this.UnpackBlockAlphaDXT23();
          break;
        case TextureDataFormat.Dxt4:
        case TextureDataFormat.Dxt5:
          this.UnpackBlockAlphaDXT45();
          break;
      }
      this.UnpackBlockColor(outBlockW, outBlockH);
    }

    public int[] UnpackImage(byte[] image, int width, int height, TextureDataFormat format)
    {
      this.mInput = image;
      this.mAlignedWidth = width + 3 & -4;
      this.mAlignedHeight = height + 3 & -4;
      this.mFormat = format;
      this.mOffset = 0;
      if (format >= TextureDataFormat.RGBA)
        throw new ArgumentException("Invalid data format", nameof (format));
      if (format == TextureDataFormat.Dxt1)
      {
        if (this.mAlignedWidth * this.mAlignedHeight != image.Length * 2)
          throw new ArgumentException("Compressed data length does not match", nameof (image));
      }
      else if (this.mAlignedWidth * this.mAlignedHeight != image.Length)
        throw new ArgumentException("Compressed data length does not match", nameof (image));
      this.mOutput = new int[width * height];
      this.m_Width = width;
      this.m_Height = height;
      switch (this.mFormat)
      {
        case TextureDataFormat.Dxt1:
          for (int index = 0; index < 16; ++index)
            this.mAlphas[index].argb = uint.MaxValue;
          break;
        case TextureDataFormat.Dxt4:
        case TextureDataFormat.Dxt5:
          for (int index = 0; index < 8; ++index)
            this.mAlut[index].argb = uint.MaxValue;
          break;
      }
      int num1 = this.mAlignedHeight - 4;
      int num2 = this.mAlignedWidth - 4;
      int num3;
      for (num3 = 0; num3 < num1; num3 += 4)
      {
        int num4;
        for (num4 = 0; num4 < num2; num4 += 4)
        {
          this.mBlockOffset = num3 * this.m_Width + num4;
          this.UnpackBlock(4, 4);
        }
        this.mBlockOffset = num3 * this.m_Width + num4;
        this.UnpackBlock(4 + this.m_Width - this.mAlignedWidth, 4);
      }
      int num5 = 0;
      int outBlockH = 4 + this.m_Height - this.mAlignedHeight;
      for (; num5 < num2; num5 += 4)
      {
        this.mBlockOffset = num3 * this.m_Width + num5;
        this.UnpackBlock(4, outBlockH);
      }
      this.mBlockOffset = num3 * this.m_Width + this.mAlignedWidth - 4;
      this.UnpackBlock(4 + this.m_Width - this.mAlignedWidth, outBlockH);
      return this.mOutput;
    }

    [StructLayout(LayoutKind.Explicit, Size = 4)]
    private struct Colorb
    {
      [FieldOffset(0)]
      public byte b;
      [FieldOffset(1)]
      public byte g;
      [FieldOffset(2)]
      public byte r;
      [FieldOffset(3)]
      public byte a;
      [FieldOffset(0)]
      public uint argb;
    }
  }
}
