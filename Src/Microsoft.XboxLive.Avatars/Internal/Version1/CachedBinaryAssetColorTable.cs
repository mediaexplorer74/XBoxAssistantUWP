// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Version1.CachedBinaryAssetColorTable
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;
using Microsoft.XboxLive.Avatars.Internal.Parsers;
using System;


namespace Microsoft.XboxLive.Avatars.Internal.Version1
{
  internal class CachedBinaryAssetColorTable : CachedBinaryAsset
  {
    private ComponentColorTable m_ColorTable;

    public CachedBinaryAssetColorTable(Guid assetId)
    {
      this.AssetId = assetId;
      this.m_AssetType = BinaryAssetParserType.ColorTable;
    }

    internal ComponentColorTable ColorTable => this.m_ColorTable;

    public override bool ParseAsset(
      StructuredBinary structuredBinary,
      BinaryAssetParseContext context)
    {
      this.m_CoordinateSystem = CoordinateSystem.LeftHanded;
      BlockIterator iterator = structuredBinary.Iterator;
      if (iterator == null || !iterator.FindFirst(StructuredBinaryBlockId.Model))
        return false;
      if (iterator.FindFirst(StructuredBinaryBlockId.CustomColorTable))
        this.m_ColorTable = AssetCustomColorTableParser.Parse(iterator);
      return true;
    }

    protected override int GetMemoryUsageInternal()
    {
      return (this.m_ColorTable == null ? 0 : this.m_ColorTable.GetMemoryUsage()) + base.GetMemoryUsageInternal();
    }
  }
}
