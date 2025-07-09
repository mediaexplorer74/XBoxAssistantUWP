// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Version1.CachedBinaryAssetModel
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;
using Microsoft.XboxLive.Avatars.Internal.Parsers;
using System;
using System.Collections.Generic;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal.Version1
{
  internal class CachedBinaryAssetModel : CachedBinaryAssetShapeOverride
  {
    public ComponentColorTable ColorTable;
    public List<AvatarComponent> Models = new List<AvatarComponent>();

    public CachedBinaryAssetModel(Guid assetId)
      : base(assetId)
    {
      this.m_AssetType = BinaryAssetParserType.Model;
    }

    public override bool ParseAsset(
      StructuredBinary structuredBinary,
      BinaryAssetParseContext context)
    {
      BlockIterator iterator = structuredBinary.Iterator;
      if (!iterator.FindFirst(StructuredBinaryBlockId.Model))
        return false;
      do
      {
        AvatarComponent model = new AvatarComponent();
        this.Models.Add(model);
        if (!CachedBinaryAssetModel.ParseModel(iterator, model, context.m_ResourceFactory, context.m_CoordinateSystem))
          return false;
      }
      while (iterator.NextBlock() && iterator.Find(StructuredBinaryBlockId.Model));
      return this.ParseColorTable(iterator) && base.ParseAsset(structuredBinary, context);
    }

    protected static bool ParseModel(
      BlockIterator iterator,
      AvatarComponent model,
      IResourceFactory resourceFactory,
      CoordinateSystem coordinateSystem)
    {
      AssetModelParser assetModelParser = new AssetModelParser(model, coordinateSystem);
      int length = (int) iterator.Length;
      if (length <= 0)
      {
        Logger.Log((Log) new DebugLog(new object(), "block size <= 0; strb file is corrupted"));
        return false;
      }
      DecompressStream stream = new DecompressStream((Stream) iterator, length);
      assetModelParser.Parse((EndianStream) stream, resourceFactory);
      return true;
    }

    protected bool ParseColorTable(BlockIterator iterator)
    {
      if (iterator.FindFirst(StructuredBinaryBlockId.CustomColorTable))
        this.ColorTable = AssetCustomColorTableParser.Parse(iterator);
      return true;
    }

    protected override int GetMemoryUsageInternal()
    {
      int memoryUsageInternal = base.GetMemoryUsageInternal();
      int count = this.Models.Count;
      for (int index = 0; index < count; ++index)
        memoryUsageInternal += this.Models[index].GetMemoryUsage();
      return memoryUsageInternal;
    }
  }
}
