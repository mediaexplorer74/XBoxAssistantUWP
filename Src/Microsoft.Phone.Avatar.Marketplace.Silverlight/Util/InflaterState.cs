// *********************************************************
// Type: Microsoft.Phone.Marketplace.Util.InflaterState
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll


namespace Microsoft.Phone.Marketplace.Util
{
  internal enum InflaterState
  {
    ReadingHeader = 0,
    ReadingBFinal = 2,
    ReadingBType = 3,
    ReadingNumLitCodes = 4,
    ReadingNumDistCodes = 5,
    ReadingNumCodeLengthCodes = 6,
    ReadingCodeLengthCodes = 7,
    ReadingTreeCodesBefore = 8,
    ReadingTreeCodesAfter = 9,
    DecodeTop = 10, // 0x0000000A
    HaveInitialLength = 11, // 0x0000000B
    HaveFullLength = 12, // 0x0000000C
    HaveDistCode = 13, // 0x0000000D
    UncompressedAligning = 15, // 0x0000000F
    UncompressedByte1 = 16, // 0x00000010
    UncompressedByte2 = 17, // 0x00000011
    UncompressedByte3 = 18, // 0x00000012
    UncompressedByte4 = 19, // 0x00000013
    DecodingUncompressed = 20, // 0x00000014
    StartReadingFooter = 21, // 0x00000015
    ReadingFooter = 22, // 0x00000016
    VerifyingFooter = 23, // 0x00000017
    Done = 24, // 0x00000018
  }
}
