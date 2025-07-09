// *********************************************************
// Type: Microsoft.Phone.Marketplace.Util.FastEncoder
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;


namespace Microsoft.Phone.Marketplace.Util
{
  internal class FastEncoder
  {
    private FastEncoderWindow inputWindow;
    private Match currentMatch;
    private double lastCompressionRatio;

    public FastEncoder()
    {
      this.inputWindow = new FastEncoderWindow();
      this.currentMatch = new Match();
    }

    internal int BytesInHistory => this.inputWindow.BytesAvailable;

    internal DeflateInput UnprocessedInput => this.inputWindow.UnprocessedInput;

    internal void FlushInput() => this.inputWindow.FlushWindow();

    internal double LastCompressionRatio => this.lastCompressionRatio;

    internal void GetBlock(DeflateInput input, OutputBuffer output, int maxBytesToCopy)
    {
      FastEncoder.WriteDeflatePreamble(output);
      this.GetCompressedOutput(input, output, maxBytesToCopy);
      this.WriteEndOfBlock(output);
    }

    internal void GetCompressedData(DeflateInput input, OutputBuffer output)
    {
      this.GetCompressedOutput(input, output, -1);
    }

    internal void GetBlockHeader(OutputBuffer output) => FastEncoder.WriteDeflatePreamble(output);

    internal void GetBlockFooter(OutputBuffer output) => this.WriteEndOfBlock(output);

    private void GetCompressedOutput(DeflateInput input, OutputBuffer output, int maxBytesToCopy)
    {
      int bytesWritten = output.BytesWritten;
      int num1 = 0;
      int num2 = this.BytesInHistory + input.Count;
      do
      {
        int num3 = input.Count < this.inputWindow.FreeWindowSpace ? input.Count : this.inputWindow.FreeWindowSpace;
        if (maxBytesToCopy >= 1)
          num3 = Math.Min(num3, maxBytesToCopy - num1);
        if (num3 > 0)
        {
          this.inputWindow.CopyBytes(input.Buffer, input.StartIndex, num3);
          input.ConsumeBytes(num3);
          num1 += num3;
        }
        this.GetCompressedOutput(output);
      }
      while (this.SafeToWriteTo(output) && this.InputAvailable(input) && (maxBytesToCopy < 1 || num1 < maxBytesToCopy));
      int num4 = output.BytesWritten - bytesWritten;
      int num5 = this.BytesInHistory + input.Count;
      int num6 = num2 - num5;
      if (num4 == 0)
        return;
      this.lastCompressionRatio = (double) num4 / (double) num6;
    }

    private void GetCompressedOutput(OutputBuffer output)
    {
      while (this.inputWindow.BytesAvailable > 0 && this.SafeToWriteTo(output))
      {
        this.inputWindow.GetNextSymbolOrMatch(this.currentMatch);
        if (this.currentMatch.State == MatchState.HasSymbol)
          FastEncoder.WriteChar(this.currentMatch.Symbol, output);
        else if (this.currentMatch.State == MatchState.HasMatch)
        {
          FastEncoder.WriteMatch(this.currentMatch.Length, this.currentMatch.Position, output);
        }
        else
        {
          FastEncoder.WriteChar(this.currentMatch.Symbol, output);
          FastEncoder.WriteMatch(this.currentMatch.Length, this.currentMatch.Position, output);
        }
      }
    }

    private bool InputAvailable(DeflateInput input) => input.Count > 0 || this.BytesInHistory > 0;

    private bool SafeToWriteTo(OutputBuffer output) => output.FreeBytes > 16;

    private void WriteEndOfBlock(OutputBuffer output)
    {
      uint num = FastEncoderStatics.FastEncoderLiteralCodeInfo[256];
      int n = (int) num & 31;
      output.WriteBits(n, num >> 5);
    }

    internal static void WriteMatch(int matchLen, int matchPos, OutputBuffer output)
    {
      uint num1 = FastEncoderStatics.FastEncoderLiteralCodeInfo[254 + matchLen];
      int n1 = (int) num1 & 31;
      if (n1 <= 16)
      {
        output.WriteBits(n1, num1 >> 5);
      }
      else
      {
        output.WriteBits(16, num1 >> 5 & (uint) ushort.MaxValue);
        output.WriteBits(n1 - 16, num1 >> 21);
      }
      uint num2 = FastEncoderStatics.FastEncoderDistanceCodeInfo[FastEncoderStatics.GetSlot(matchPos)];
      output.WriteBits((int) num2 & 15, num2 >> 8);
      int n2 = (int) (num2 >> 4) & 15;
      if (n2 == 0)
        return;
      output.WriteBits(n2, (uint) matchPos & FastEncoderStatics.BitMask[n2]);
    }

    internal static void WriteChar(byte b, OutputBuffer output)
    {
      uint num = FastEncoderStatics.FastEncoderLiteralCodeInfo[(int) b];
      output.WriteBits((int) num & 31, num >> 5);
    }

    internal static void WriteDeflatePreamble(OutputBuffer output)
    {
      output.WriteBytes(FastEncoderStatics.FastEncoderTreeStructureData, 0, FastEncoderStatics.FastEncoderTreeStructureData.Length);
      output.WriteBits(9, 34U);
    }
  }
}
