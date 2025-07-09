// *********************************************************
// Type: Microsoft.Phone.Marketplace.Util.GZipDecoder
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;


namespace Microsoft.Phone.Marketplace.Util
{
  internal class GZipDecoder : IFileFormatReader
  {
    private GZipDecoder.GzipHeaderState gzipHeaderSubstate;
    private GZipDecoder.GzipHeaderState gzipFooterSubstate;
    private int gzip_header_flag;
    private int gzip_header_xlen;
    private uint expectedCrc32;
    private uint expectedOutputStreamSize;
    private int loopCounter;
    private uint actualCrc32;
    private long actualStreamSize;

    public GZipDecoder() => this.Reset();

    public void Reset()
    {
      this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingID1;
      this.gzipFooterSubstate = GZipDecoder.GzipHeaderState.ReadingCRC;
      this.expectedCrc32 = 0U;
      this.expectedOutputStreamSize = 0U;
    }

    public bool ReadHeader(InputBuffer input)
    {
      int num;
      switch (this.gzipHeaderSubstate)
      {
        case GZipDecoder.GzipHeaderState.ReadingID1:
          int bits1 = input.GetBits(8);
          if (bits1 < 0)
            return false;
          if (bits1 != 31)
            throw new InvalidDataException("Corrupted GZip header");
          this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingID2;
          goto case GZipDecoder.GzipHeaderState.ReadingID2;
        case GZipDecoder.GzipHeaderState.ReadingID2:
          int bits2 = input.GetBits(8);
          if (bits2 < 0)
            return false;
          if (bits2 != 139)
            throw new InvalidDataException("Corrupted GZip header");
          this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingCM;
          goto case GZipDecoder.GzipHeaderState.ReadingCM;
        case GZipDecoder.GzipHeaderState.ReadingCM:
          int bits3 = input.GetBits(8);
          if (bits3 < 0)
            return false;
          if (bits3 != 8)
            throw new InvalidDataException("Unknown compression mode");
          this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingFLG;
          goto case GZipDecoder.GzipHeaderState.ReadingFLG;
        case GZipDecoder.GzipHeaderState.ReadingFLG:
          int bits4 = input.GetBits(8);
          if (bits4 < 0)
            return false;
          this.gzip_header_flag = bits4;
          this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingMMTime;
          this.loopCounter = 0;
          goto case GZipDecoder.GzipHeaderState.ReadingMMTime;
        case GZipDecoder.GzipHeaderState.ReadingMMTime:
          num = 0;
          for (; this.loopCounter < 4; ++this.loopCounter)
          {
            if (input.GetBits(8) < 0)
              return false;
          }
          this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingXFL;
          this.loopCounter = 0;
          goto case GZipDecoder.GzipHeaderState.ReadingXFL;
        case GZipDecoder.GzipHeaderState.ReadingXFL:
          if (input.GetBits(8) < 0)
            return false;
          this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingOS;
          goto case GZipDecoder.GzipHeaderState.ReadingOS;
        case GZipDecoder.GzipHeaderState.ReadingOS:
          if (input.GetBits(8) < 0)
            return false;
          this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingXLen1;
          goto case GZipDecoder.GzipHeaderState.ReadingXLen1;
        case GZipDecoder.GzipHeaderState.ReadingXLen1:
          if ((this.gzip_header_flag & 4) != 0)
          {
            int bits5 = input.GetBits(8);
            if (bits5 < 0)
              return false;
            this.gzip_header_xlen = bits5;
            this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingXLen2;
            goto case GZipDecoder.GzipHeaderState.ReadingXLen2;
          }
          else
            goto case GZipDecoder.GzipHeaderState.ReadingFileName;
        case GZipDecoder.GzipHeaderState.ReadingXLen2:
          int bits6 = input.GetBits(8);
          if (bits6 < 0)
            return false;
          this.gzip_header_xlen |= bits6 << 8;
          this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingXLenData;
          this.loopCounter = 0;
          goto case GZipDecoder.GzipHeaderState.ReadingXLenData;
        case GZipDecoder.GzipHeaderState.ReadingXLenData:
          num = 0;
          for (; this.loopCounter < this.gzip_header_xlen; ++this.loopCounter)
          {
            if (input.GetBits(8) < 0)
              return false;
          }
          this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingFileName;
          this.loopCounter = 0;
          goto case GZipDecoder.GzipHeaderState.ReadingFileName;
        case GZipDecoder.GzipHeaderState.ReadingFileName:
          if ((this.gzip_header_flag & 8) == 0)
          {
            this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingComment;
            goto case GZipDecoder.GzipHeaderState.ReadingComment;
          }
          else
          {
            int bits7;
            do
            {
              bits7 = input.GetBits(8);
              if (bits7 < 0)
                return false;
            }
            while (bits7 != 0);
            this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingComment;
            goto case GZipDecoder.GzipHeaderState.ReadingComment;
          }
        case GZipDecoder.GzipHeaderState.ReadingComment:
          if ((this.gzip_header_flag & 16) == 0)
          {
            this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingCRC16Part1;
            goto case GZipDecoder.GzipHeaderState.ReadingCRC16Part1;
          }
          else
          {
            int bits8;
            do
            {
              bits8 = input.GetBits(8);
              if (bits8 < 0)
                return false;
            }
            while (bits8 != 0);
            this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingCRC16Part1;
            goto case GZipDecoder.GzipHeaderState.ReadingCRC16Part1;
          }
        case GZipDecoder.GzipHeaderState.ReadingCRC16Part1:
          if ((this.gzip_header_flag & 2) == 0)
          {
            this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.Done;
            goto case GZipDecoder.GzipHeaderState.Done;
          }
          else
          {
            if (input.GetBits(8) < 0)
              return false;
            this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingCRC16Part2;
            goto case GZipDecoder.GzipHeaderState.ReadingCRC16Part2;
          }
        case GZipDecoder.GzipHeaderState.ReadingCRC16Part2:
          if (input.GetBits(8) < 0)
            return false;
          this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.Done;
          goto case GZipDecoder.GzipHeaderState.Done;
        case GZipDecoder.GzipHeaderState.Done:
          return true;
        default:
          throw new InvalidDataException("Unknown state");
      }
    }

    public bool ReadFooter(InputBuffer input)
    {
      input.SkipToByteBoundary();
      if (this.gzipFooterSubstate == GZipDecoder.GzipHeaderState.ReadingCRC)
      {
        for (; this.loopCounter < 4; ++this.loopCounter)
        {
          int bits = input.GetBits(8);
          if (bits < 0)
            return false;
          this.expectedCrc32 |= (uint) (bits << 8 * this.loopCounter);
        }
        this.gzipFooterSubstate = GZipDecoder.GzipHeaderState.ReadingFileSize;
        this.loopCounter = 0;
      }
      if (this.gzipFooterSubstate == GZipDecoder.GzipHeaderState.ReadingFileSize)
      {
        if (this.loopCounter == 0)
          this.expectedOutputStreamSize = 0U;
        for (; this.loopCounter < 4; ++this.loopCounter)
        {
          int bits = input.GetBits(8);
          if (bits < 0)
            return false;
          this.expectedOutputStreamSize |= (uint) (bits << 8 * this.loopCounter);
        }
      }
      return true;
    }

    public void UpdateWithBytesRead(byte[] buffer, int offset, int copied)
    {
      this.actualCrc32 = Crc32Helper.UpdateCrc32(this.actualCrc32, buffer, offset, copied);
      long num = this.actualStreamSize + (long) (uint) copied;
      if (num > 4294967296L)
        num %= 4294967296L;
      this.actualStreamSize = num;
    }

    public void Validate()
    {
      if ((int) this.expectedCrc32 != (int) this.actualCrc32)
        throw new InvalidDataException("Invalid CRC");
      if (this.actualStreamSize != (long) this.expectedOutputStreamSize)
        throw new InvalidDataException("Invalid stream size");
    }

    internal enum GzipHeaderState
    {
      ReadingID1,
      ReadingID2,
      ReadingCM,
      ReadingFLG,
      ReadingMMTime,
      ReadingXFL,
      ReadingOS,
      ReadingXLen1,
      ReadingXLen2,
      ReadingXLenData,
      ReadingFileName,
      ReadingComment,
      ReadingCRC16Part1,
      ReadingCRC16Part2,
      Done,
      ReadingCRC,
      ReadingFileSize,
    }

    [Flags]
    internal enum GZipOptionalHeaderFlags
    {
      CRCFlag = 2,
      ExtraFieldsFlag = 4,
      FileNameFlag = 8,
      CommentFlag = 16, // 0x00000010
    }
  }
}
