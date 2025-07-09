// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Parsers.AssetTextureParser
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.MathUtilities;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal.Parsers
{
  internal class AssetTextureParser
  {
    private IBaseTextureAnimated m_texture;
    private AssetTextureParser.D3DFORMAT m_format;
    private int m_width;
    private int m_height;
    private int m_layerCount;

    public IBaseTextureAnimated Texture => this.m_texture;

    private static TextureDataFormat RemapFormat(AssetTextureParser.D3DFORMAT format)
    {
      switch (format)
      {
        case AssetTextureParser.D3DFORMAT.D3DFMT_LIN_DXT1:
        case AssetTextureParser.D3DFORMAT.D3DFMT_DXT1:
          return TextureDataFormat.Dxt1;
        case AssetTextureParser.D3DFORMAT.D3DFMT_LIN_DXT2:
        case AssetTextureParser.D3DFORMAT.D3DFMT_DXT2:
          return TextureDataFormat.Dxt2;
        case AssetTextureParser.D3DFORMAT.D3DFMT_LIN_DXT4:
        case AssetTextureParser.D3DFORMAT.D3DFMT_DXT4:
          return TextureDataFormat.Dxt4;
        default:
          return TextureDataFormat.Dxt5;
      }
    }

    private static void SwapEndians(byte[] image)
    {
      for (int index = 0; index < image.Length; index += 2)
      {
        byte num = image[index];
        image[index] = image[index + 1];
        image[index + 1] = num;
      }
    }

    public void Parse(EndianStream stream, IResourceFactory resourceFactory)
    {
      BitStream bitStream = new BitStream((Stream) stream);
      this.m_format = (AssetTextureParser.D3DFORMAT) bitStream.ReadUint(32);
      this.m_width = bitStream.ReadInt(32);
      this.m_height = bitStream.ReadInt(32);
      bitStream.ReadInt(32);
      bitStream.ReadInt(32);
      this.m_layerCount = bitStream.ReadInt(32);
      bool flag = bitStream.ReadBool(1);
      bitStream.ReadBool(1);
      int num1 = bitStream.ReadInt(32);
      int num2 = bitStream.ReadInt(32);
      if ((uint) this.m_width > 1024U)
      {
        Logger.Log((Log) new DebugLog((object) this, Resources.TextureParserError1));
        throw new AvatarException(Resources.TextureParserError1);
      }
      if ((uint) this.m_height > 1024U)
      {
        Logger.Log((Log) new DebugLog((object) this, Resources.TextureParserError2));
        throw new AvatarException(Resources.TextureParserError2);
      }
      if ((uint) this.m_layerCount > 14U)
      {
        Logger.Log((Log) new DebugLog((object) this, Resources.TextureParserError3));
        throw new AvatarException(Resources.TextureParserError3);
      }
      if (this.m_layerCount == 0)
      {
        Logger.Log((Log) new DebugLog((object) this, Resources.TextureParserError3));
        throw new AvatarException(Resources.TextureParserError3);
      }
      TextureDataFormat dataFormat = AssetTextureParser.RemapFormat(this.m_format);
      if (!flag)
      {
        this.m_texture = resourceFactory.CreateAnimatedTexture(this.m_width, this.m_height, this.m_layerCount, dataFormat);
        int num3 = dataFormat == TextureDataFormat.Dxt1 ? 8 : 16;
        int num4 = this.m_width + 3 >> 2;
        int num5 = this.m_height + 3 >> 2;
        if (num4 * num3 != num1)
        {
          Logger.Log((Log) new DebugLog((object) this, Resources.TextureParserError4));
          throw new AvatarException(Resources.TextureParserError4);
        }
        if (num5 != num2)
        {
          Logger.Log((Log) new DebugLog((object) this, Resources.TextureParserError2));
          throw new AvatarException(Resources.TextureParserError2);
        }
        int count = num2 * num1;
        for (int layerIndex = 0; layerIndex < this.m_layerCount; ++layerIndex)
        {
          byte[] numArray = new byte[count];
          stream.Read(numArray, 0, count);
          AssetTextureParser.SwapEndians(numArray);
          this.m_texture.SetTextureLayer(layerIndex, numArray, this.m_width, this.m_height);
        }
      }
      else
        this.m_texture = resourceFactory.CreateAnimatedTexture(4, 4, this.m_layerCount, dataFormat | TextureDataFormat.Empty);
    }

    private enum D3DFORMAT
    {
      D3DFMT_LIN_DXT1 = 438304850, // 0x1A200052
      D3DFMT_LIN_DXT2 = 438304851, // 0x1A200053
      D3DFMT_LIN_DXT3 = 438304851, // 0x1A200053
      D3DFMT_LIN_DXT4 = 438304852, // 0x1A200054
      D3DFMT_LIN_DXT5 = 438304852, // 0x1A200054
      D3DFMT_LIN_DXT3A = 438304890, // 0x1A20007A
      D3DFMT_LIN_DXT3A_1111 = 438304893, // 0x1A20007D
      D3DFMT_DXT1 = 438305106, // 0x1A200152
      D3DFMT_DXT2 = 438305107, // 0x1A200153
      D3DFMT_DXT3 = 438305107, // 0x1A200153
      D3DFMT_DXT4 = 438305108, // 0x1A200154
      D3DFMT_DXT5 = 438305108, // 0x1A200154
      D3DFMT_DXT3A = 438305146, // 0x1A20017A
      D3DFMT_DXT3A_1111 = 438305149, // 0x1A20017D
    }
  }
}
