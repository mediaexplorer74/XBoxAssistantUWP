// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Version1.CachedBinaryAssetTexture
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Parsers;
using Microsoft.XboxLive.MathUtilities;
using System;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal.Version1
{
  internal class CachedBinaryAssetTexture : CachedBinaryAsset
  {
    private AssetTextureParser m_TextureParser = new AssetTextureParser();

    public CachedBinaryAssetTexture(Guid assetId)
    {
      this.AssetId = assetId;
      this.m_AssetType = BinaryAssetParserType.Texture;
    }

    internal IBaseTextureAnimated GetTexture() => this.m_TextureParser.Texture;

    public override bool ParseAsset(
      StructuredBinary structuredBinary,
      BinaryAssetParseContext context)
    {
      this.m_CoordinateSystem = CoordinateSystem.LeftHanded;
      BlockIterator iterator = structuredBinary.Iterator;
      if (iterator == null || !iterator.FindFirst(StructuredBinaryBlockId.Texture))
        return false;
      if ((int) iterator.Length <= 0)
      {
        Logger.Log((Log) new DebugLog((object) this, "block size <= 0; strb file is corrupted"));
        return false;
      }
      this.m_TextureParser.Parse((EndianStream) new DecompressStream((Stream) iterator, (int) iterator.Length), context.m_ResourceFactory);
      return true;
    }

    protected override int GetMemoryUsageInternal()
    {
      IBaseTexture texture = (IBaseTexture) this.m_TextureParser.Texture;
      return (texture == null ? 0 : texture.GetMemoryUsage()) + base.GetMemoryUsageInternal();
    }
  }
}
