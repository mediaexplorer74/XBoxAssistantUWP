// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Version1.CachedBinaryAssetShapeOverride
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Parsers;
using System;
using System.Collections.Generic;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal.Version1
{
  internal class CachedBinaryAssetShapeOverride : CachedBinaryAsset
  {
    public List<AssetShapeOverrideParser> m_ShapeOverrides = new List<AssetShapeOverrideParser>();

    public CachedBinaryAssetShapeOverride(Guid assetId)
    {
      this.AssetId = assetId;
      this.m_AssetType = BinaryAssetParserType.ShapeOverride;
    }

    public override bool ParseAsset(
      StructuredBinary structuredBinary,
      BinaryAssetParseContext context)
    {
      BlockIterator iterator = structuredBinary.Iterator;
      if (iterator == null || !iterator.FirstBlock())
        return false;
      while (iterator.Find(StructuredBinaryBlockId.ShapeOverrides))
      {
        AssetShapeOverrideParser shapeOverrideParser = new AssetShapeOverrideParser(context.m_CoordinateSystem);
        this.m_ShapeOverrides.Add(shapeOverrideParser);
        shapeOverrideParser.Parse((Stream) iterator);
        iterator.NextBlock();
      }
      return true;
    }

    protected override int GetMemoryUsageInternal()
    {
      int memoryUsageInternal = base.GetMemoryUsageInternal();
      int count = this.m_ShapeOverrides.Count;
      for (int index = 0; index < count; ++index)
        memoryUsageInternal += this.m_ShapeOverrides[index].GetMemoryUsage();
      return memoryUsageInternal;
    }
  }
}
