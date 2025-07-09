// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Parsers.AssetMetadataParser
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;


namespace Microsoft.XboxLive.Avatars.Internal.Parsers
{
  internal class AssetMetadataParser
  {
    internal AvatarGender m_bodyTypeMask;
    internal BinaryAssetType m_AssetType;
    internal uint m_assetTypeDetails;
    internal AssetSubcategory m_assetSubCategory;
    internal Skeleton.SkeletonVersion m_skeletonVersion;

    public AvatarGender BodyTypeMask => this.m_bodyTypeMask;

    public BinaryAssetType AssetType => this.m_AssetType;

    public uint AssetTypeDetails => this.m_assetTypeDetails;

    public AssetSubcategory AssetSubcategory => this.m_assetSubCategory;

    public Skeleton.SkeletonVersion AssetSkeletonVersion => this.m_skeletonVersion;

    public void ParseLegacyV1(BlockIterator blockIterator)
    {
      bool littleEndian = blockIterator.LittleEndian;
      blockIterator.LittleEndian = true;
      this.m_bodyTypeMask = (AvatarGender) blockIterator.ReadByte();
      this.m_AssetType = (BinaryAssetType) blockIterator.ReadInt();
      this.m_assetTypeDetails = blockIterator.ReadUInt();
      this.m_assetSubCategory = (AssetSubcategory) blockIterator.ReadInt();
      blockIterator.LittleEndian = littleEndian;
      this.m_skeletonVersion = Skeleton.SkeletonVersion.Nxe;
    }

    public void Parse(BlockIterator blockIterator)
    {
      switch (blockIterator.ReadByte())
      {
        case 1:
          this.ParseLegacyV1(blockIterator);
          break;
        case 2:
          bool littleEndian = blockIterator.LittleEndian;
          blockIterator.LittleEndian = true;
          this.m_bodyTypeMask = (AvatarGender) blockIterator.ReadByte();
          this.m_AssetType = (BinaryAssetType) blockIterator.ReadInt();
          this.m_assetTypeDetails = blockIterator.ReadUInt();
          this.m_assetSubCategory = (AssetSubcategory) blockIterator.ReadInt();
          this.m_skeletonVersion = (Skeleton.SkeletonVersion) blockIterator.ReadByte();
          blockIterator.LittleEndian = littleEndian;
          break;
      }
    }

    public bool LoadFromStrb(BlockIterator strb)
    {
      if (strb.FindFirst(StructuredBinaryBlockId.AssetMetadata))
      {
        this.ParseLegacyV1(strb);
        return true;
      }
      if (!strb.FindFirst(StructuredBinaryBlockId.AssetMetadataVersioned))
        return false;
      this.Parse(strb);
      return true;
    }
  }
}
