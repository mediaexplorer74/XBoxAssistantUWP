// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Version1.BinaryAssetColorTable
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;
using Microsoft.XboxLive.Avatars.Internal.Parsers;
using System;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal.Version1
{
  internal class BinaryAssetColorTable : BinaryAsset
  {
    public BinaryAssetColorTable(Guid id, AvatarComponentMasks mask)
      : base(mask)
    {
      this.m_AssetId = id;
      this.m_AssetType = BinaryAssetParserType.ColorTable;
    }

    public ComponentColorTable GetPrimaryColorTable()
    {
      StructuredBinary structuredBinary = new StructuredBinary();
      this.m_Stream.Seek(0L, SeekOrigin.Begin);
      if (!structuredBinary.Open(this.m_Stream))
        return (ComponentColorTable) null;
      if (!(structuredBinary.Namespace == BinaryAsset.AvatarAssetGuid))
        return (ComponentColorTable) null;
      BlockIterator iterator = structuredBinary.Iterator;
      if (!iterator.FindFirst(StructuredBinaryBlockId.Model))
        return (ComponentColorTable) null;
      return !iterator.FindFirst(StructuredBinaryBlockId.CustomColorTable) ? (ComponentColorTable) null : AssetCustomColorTableParser.Parse(iterator);
    }

    public override bool Validate(BinaryAssetParseContext context) => true;

    public override bool ValidateFromCache(BinaryAssetParseContext context) => true;

    public override CachedBinaryAsset CreateCacheItem()
    {
      return (CachedBinaryAsset) new CachedBinaryAssetColorTable(this.m_AssetId);
    }

    public override bool IsCoordinateSystemIndependent => true;
  }
}
