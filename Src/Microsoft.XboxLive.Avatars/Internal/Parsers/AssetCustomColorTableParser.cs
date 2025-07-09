// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Parsers.AssetCustomColorTableParser
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal.Parsers
{
  internal class AssetCustomColorTableParser
  {
    public static ComponentColorTable Parse(BlockIterator blockIterator)
    {
      ComponentColorTable componentColorTable = new ComponentColorTable();
      new ByteStreamUnpacker<ComponentColors>((Stream) blockIterator, (DataUnpackerGeneric<ComponentColors>) new AssetCustomColorTableParser.ColorTableParser()).Unpack(out componentColorTable.Colors);
      return componentColorTable;
    }

    private class ColorTableParser : DataUnpackerGeneric<ComponentColors>
    {
      private IntegerDataUnpacker m_Red = new IntegerDataUnpacker();
      private IntegerDataUnpacker m_Green = new IntegerDataUnpacker();
      private IntegerDataUnpacker m_Blue = new IntegerDataUnpacker();

      public override void UnpackHeader(BitStream bitStream)
      {
        this.m_Red.UnpackHeader(bitStream);
        this.m_Green.UnpackHeader(bitStream);
        this.m_Blue.UnpackHeader(bitStream);
      }

      public override void UnpackData(BitStream bitStream, out ComponentColors data)
      {
        int data1;
        this.m_Red.UnpackData(bitStream, out data1);
        data.CustomColor0 = Utilities.Vector4FromInt(data1);
        this.m_Green.UnpackData(bitStream, out data1);
        data.CustomColor1 = Utilities.Vector4FromInt(data1);
        this.m_Blue.UnpackData(bitStream, out data1);
        data.CustomColor2 = Utilities.Vector4FromInt(data1);
      }
    }
  }
}
