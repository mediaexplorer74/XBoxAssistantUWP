// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.AssetLoader
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Animations;
using Microsoft.XboxLive.Avatars.Internal.Assets;
using Microsoft.XboxLive.Avatars.Internal.Parsers;
using Microsoft.XboxLive.Avatars.Internal.Version1;
using Microsoft.XboxLive.MathUtilities;
using System;
using System.IO;
using System.Threading;


namespace Microsoft.XboxLive.Avatars.Internal
{
  public class AssetLoader
  {
    private CoordinateSystem m_CoordinateSystem;
    private IResourceFactory m_ResourceFactory;
    private bool m_AllowAlternativeAssets = true;
    private IDataManager m_dataManager;
    private bool m_UseCache = true;
    private static AvatarAssetCacheManager s_AssetCache;

    public AssetLoader(
      IDataManager dataManager,
      IResourceFactory resourceFactory,
      CoordinateSystem coordinateSystem)
    {
      if (dataManager == null)
        throw new ArgumentNullException(nameof (dataManager));
      if (resourceFactory == null)
        throw new ArgumentNullException(nameof (resourceFactory));
      this.m_CoordinateSystem = coordinateSystem;
      this.m_ResourceFactory = resourceFactory;
      this.m_dataManager = dataManager;
      this.m_UseCache = true;
    }

    public CoordinateSystem CoordinateSystem => this.m_CoordinateSystem;

    public bool AllowAlternativeAvatarAssets
    {
      get => this.m_AllowAlternativeAssets;
      set => this.m_AllowAlternativeAssets = value;
    }

    public bool UseCache
    {
      get => this.m_UseCache;
      set => this.m_UseCache = value;
    }

    public static bool EnableAssetCaching() => AssetLoader.EnableAssetCaching(33554432);

    public static bool EnableAssetCaching(int cacheSize)
    {
      AvatarAssetCacheManager assetCache = AssetLoader.s_AssetCache;
      if (assetCache != null)
      {
        assetCache.SetCacheSize(cacheSize);
        return false;
      }
      AssetLoader.s_AssetCache = new AvatarAssetCacheManager(cacheSize);
      return true;
    }

    public static void DisableAssetCaching()
    {
      AssetLoader.s_AssetCache = (AvatarAssetCacheManager) null;
    }

    public static int GetAssetCacheEstimatedMemoryUsage()
    {
      AvatarAssetCacheManager assetCache = AssetLoader.s_AssetCache;
      return assetCache == null ? 0 : assetCache.GetEstimatedMemoryUsage();
    }

    public static void ClearCaches()
    {
      AvatarAssetsDependenciesResolver.InvalidateDependenciesTable();
      AssetLoader.s_AssetCache?.ReleaseCache();
    }

    internal AvatarAssetCacheManager GetAssetCacheManager()
    {
      return this.m_UseCache ? AssetLoader.s_AssetCache : (AvatarAssetCacheManager) null;
    }

    internal static AvatarAssetCacheManager GetCacheManager() => AssetLoader.s_AssetCache;

    public Avatar CreateAvatar(AvatarManifest manifest)
    {
      return this.CreateAvatar(manifest, AvatarComponentMasks.All);
    }

    public static AvatarGender GetAssetBodyType(Guid avatarAssetId)
    {
      switch ((int) avatarAssetId.ToByteArray()[6] & 15)
      {
        case 0:
          return AvatarGender.Unknown;
        case 1:
          return AvatarGender.Male;
        case 2:
          return AvatarGender.Female;
        case 3:
          return AvatarGender.Both;
        default:
          return AvatarGender.Unknown;
      }
    }

    public static ComponentCategories GetComponentTypeFromAssetId(Guid avatarAssetId)
    {
      byte[] byteArray = avatarAssetId.ToByteArray();
      return (ComponentCategories) BitConverter.ToInt32(new byte[4]
      {
        byteArray[0],
        byteArray[1],
        byteArray[2],
        byteArray[3]
      }, 0);
    }

    public static AssetGuidType GetAssetGuidType(Guid avatarAssetId)
    {
      byte[] byteArray = avatarAssetId.ToByteArray();
      if (((int) byteArray[8] & 240) != 192)
        return AssetGuidType.Custom;
      switch ((int) byteArray[7] & 15)
      {
        case 0:
          return AssetGuidType.TOC;
        case 1:
          return AssetGuidType.Awardable;
        case 2:
          return AssetGuidType.MarketPlace;
        case 15:
          return AssetGuidType.Custom;
        default:
          return AssetGuidType.Custom;
      }
    }

    public static bool IsStockAsset(Guid avatarAssetId)
    {
      switch (AssetLoader.GetAssetGuidType(avatarAssetId))
      {
        case AssetGuidType.Custom:
        case AssetGuidType.TOC:
          return true;
        default:
          return false;
      }
    }

    public static string GetTitleIdFromAssetId(Guid avatarAssetId)
    {
      if (AssetLoader.IsStockAsset(avatarAssetId))
        return Resources.AvatarEditorTitleId;
      int num = 0;
      byte[] byteArray = avatarAssetId.ToByteArray();
      return Convert.ToString(num + (int) byteArray[15] + ((int) byteArray[14] << 8) + ((int) byteArray[13] << 16) + ((int) byteArray[12] << 24), 16);
    }

    public static DynamicColorType GetColorAssetType(Guid guid)
    {
      ComponentCategories componentTypeFromAssetId = AssetLoader.GetComponentTypeFromAssetId(guid);
      DynamicColorType colorAssetType = DynamicColorType.Count;
      switch (componentTypeFromAssetId)
      {
        case ComponentCategories.Hair:
          colorAssetType = DynamicColorType.Hair;
          break;
        case ComponentCategories.Eyes:
          colorAssetType = DynamicColorType.Iris;
          break;
        case ComponentCategories.Eyebrows:
          colorAssetType = DynamicColorType.Eyebrow;
          break;
        case ComponentCategories.Mouth:
          colorAssetType = DynamicColorType.Mouth;
          break;
        case ComponentCategories.FacialHair:
          colorAssetType = DynamicColorType.FacialHair;
          break;
        case ComponentCategories.FacialOther:
          colorAssetType = DynamicColorType.SkinFeatures1;
          break;
        case ComponentCategories.EyeShadow:
          colorAssetType = DynamicColorType.EyeShadow;
          break;
        case ComponentCategories.Valid:
          colorAssetType = DynamicColorType.Skin;
          break;
      }
      return colorAssetType;
    }

    public static DynamicTextureType GetTextureAssetType(Guid guid)
    {
      ComponentCategories componentTypeFromAssetId = AssetLoader.GetComponentTypeFromAssetId(guid);
      DynamicTextureType textureAssetType = DynamicTextureType.Count;
      switch (componentTypeFromAssetId)
      {
        case ComponentCategories.Eyes:
          textureAssetType = DynamicTextureType.Eye;
          break;
        case ComponentCategories.Eyebrows:
          textureAssetType = DynamicTextureType.Eyebrow;
          break;
        case ComponentCategories.Mouth:
          textureAssetType = DynamicTextureType.Mouth;
          break;
        case ComponentCategories.FacialHair:
          textureAssetType = DynamicTextureType.FacialHair;
          break;
        case ComponentCategories.FacialOther:
          textureAssetType = DynamicTextureType.SkinFeatures;
          break;
        case ComponentCategories.EyeShadow:
          textureAssetType = DynamicTextureType.EyeShadow;
          break;
      }
      return textureAssetType;
    }

    public static BlendShapeType GetBlendShapeAssetType(Guid guid)
    {
      ComponentCategories componentTypeFromAssetId = AssetLoader.GetComponentTypeFromAssetId(guid);
      BlendShapeType blendShapeAssetType = BlendShapeType.Count;
      switch (componentTypeFromAssetId)
      {
        case ComponentCategories.Nose:
          blendShapeAssetType = BlendShapeType.Nose;
          break;
        case ComponentCategories.Chin:
          blendShapeAssetType = BlendShapeType.Chin;
          break;
        case ComponentCategories.Ears:
          blendShapeAssetType = BlendShapeType.Ear;
          break;
      }
      return blendShapeAssetType;
    }

    private static int GetAssetVersion(Guid assetId) => 1;

    private static int GetAssetVersion(Stream assetStream) => 1;

    private bool CheckThreadAccess() => true;

    public Avatar CreateAvatar(AvatarManifest manifest, AvatarComponentMasks componentMask)
    {
      if (!this.CheckThreadAccess())
        throw new NotSupportedException(Resources.IvalidCallingThread);
      if (manifest == (AvatarManifest) null)
        throw new ArgumentException(Resources.ManifestCannotBeNullText);
      return manifest.Version == 1 ? new AvatarGetData(this.m_dataManager, this.GetAssetCacheManager(), this.m_ResourceFactory, this.m_CoordinateSystem).Load((AvatarManifest) (manifest.Clone() as AvatarManifestV1), componentMask, this.m_AllowAlternativeAssets) : (Avatar) null;
    }

    public AvatarComponent GetAvatarComponent(Guid componentId, Colorb[] customColors)
    {
      if (!this.CheckThreadAccess())
        throw new NotSupportedException(Resources.IvalidCallingThread);
      if (AssetLoader.GetAssetVersion(componentId) == 1)
        return new AvatarGetData(this.m_dataManager, this.GetAssetCacheManager(), this.m_ResourceFactory, this.m_CoordinateSystem).LoadAvatarComponent(componentId, customColors);
      throw new InvalidOperationException(Resources.UnknownAssetVersionText);
    }

    public AvatarComponent GetAvatarComponent(Stream componentStream, Colorb[] customColors)
    {
      if (!this.CheckThreadAccess())
        throw new NotSupportedException(Resources.IvalidCallingThread);
      if (AssetLoader.GetAssetVersion(componentStream) == 1)
        return new AvatarGetData(this.m_dataManager, this.GetAssetCacheManager(), this.m_ResourceFactory, this.m_CoordinateSystem).LoadAvatarComponent(componentStream, customColors);
      throw new InvalidOperationException(Resources.UnknownAssetVersionText);
    }

    public AvatarCarryable GetAvatarCarryable(
      Guid carryableId,
      Skeleton avatarSkeleton,
      Colorb[] customColors)
    {
      if (!this.CheckThreadAccess())
        throw new NotSupportedException(Resources.IvalidCallingThread);
      if (AssetLoader.GetAssetVersion(carryableId) == 1)
        return new AvatarGetData(this.m_dataManager, this.GetAssetCacheManager(), this.m_ResourceFactory, this.m_CoordinateSystem).LoadAvatarCarryable(carryableId, avatarSkeleton, customColors);
      throw new InvalidOperationException(Resources.UnknownAssetVersionText);
    }

    public AvatarCarryable GetAvatarCarryable(
      Stream carryableStream,
      Skeleton avatarSkeleton,
      Colorb[] customColors)
    {
      if (!this.CheckThreadAccess())
        throw new NotSupportedException(Resources.IvalidCallingThread);
      if (AssetLoader.GetAssetVersion(carryableStream) == 1)
        return new AvatarGetData(this.m_dataManager, this.GetAssetCacheManager(), this.m_ResourceFactory, this.m_CoordinateSystem).LoadAvatarCarryable(carryableStream, avatarSkeleton, customColors);
      throw new InvalidOperationException(Resources.UnknownAssetVersionText);
    }

    public ComponentColors[] GetComponentColorTable(Guid componentId)
    {
      if (!this.CheckThreadAccess())
        throw new NotSupportedException(Resources.IvalidCallingThread);
      if (AssetLoader.GetAssetVersion(componentId) == 1)
        return new AvatarGetData(this.m_dataManager, this.GetAssetCacheManager(), this.m_ResourceFactory, this.m_CoordinateSystem).LoadColorTable(componentId);
      throw new InvalidOperationException(Resources.UnknownAssetVersionText);
    }

    public ComponentColors[] GetComponentColorTable(Stream componentStream)
    {
      if (!this.CheckThreadAccess())
        throw new NotSupportedException(Resources.IvalidCallingThread);
      if (AssetLoader.GetAssetVersion(componentStream) == 1)
        return new AvatarGetData(this.m_dataManager, this.GetAssetCacheManager(), this.m_ResourceFactory, this.m_CoordinateSystem).LoadColorTable(componentStream);
      throw new InvalidOperationException(Resources.UnknownAssetVersionText);
    }

    public AvatarAnimation GetAvatarAnimation(Guid animationId)
    {
      if (!this.CheckThreadAccess())
        throw new NotSupportedException(Resources.IvalidCallingThread);
      if (AssetLoader.GetAssetVersion(animationId) == 1)
        return new AvatarGetAnimation(this.m_CoordinateSystem).Load(animationId, this, this.m_dataManager);
      throw new InvalidOperationException(Resources.UnknownAssetVersionText);
    }

    public AvatarAnimation GetAvatarAnimation(Stream animationStream)
    {
      if (!this.CheckThreadAccess())
        throw new NotSupportedException(Resources.IvalidCallingThread);
      return AssetLoader.GetAssetVersion(animationStream) == 1 ? new AvatarGetAnimation(this.m_CoordinateSystem).Load(animationStream) : throw new InvalidOperationException(Resources.UnknownAssetVersionText);
    }

    public AssetSubcategory GetAssetSubcategory(Guid assetId)
    {
      if (!this.CheckThreadAccess())
        throw new NotSupportedException(Resources.IvalidCallingThread);
      if (AssetLoader.GetAssetVersion(assetId) != 1)
        throw new InvalidOperationException(Resources.UnknownAssetVersionText);
      AssetMetadataParser assetMetadataParser = new AvatarGetData(this.m_dataManager, this.GetAssetCacheManager(), this.m_ResourceFactory, this.m_CoordinateSystem).LoadMetadata(assetId);
      return assetMetadataParser != null ? assetMetadataParser.AssetSubcategory : AssetSubcategory.None;
    }

    public AssetSubcategory GetAssetSubcategory(Stream assetStream)
    {
      if (!this.CheckThreadAccess())
        throw new NotSupportedException(Resources.IvalidCallingThread);
      if (assetStream == null)
        throw new ArgumentNullException(nameof (assetStream));
      if (AssetLoader.GetAssetVersion(assetStream) != 1)
        throw new InvalidOperationException(Resources.UnknownAssetVersionText);
      AssetMetadataParser assetMetadataParser = new AvatarGetData(this.m_dataManager, this.GetAssetCacheManager(), this.m_ResourceFactory, this.m_CoordinateSystem).LoadMetadata(assetStream);
      return assetMetadataParser != null ? assetMetadataParser.AssetSubcategory : AssetSubcategory.None;
    }

    public void CreateAvatarAsync(
      AvatarManifest manifest,
      EventHandler<GetAvatarAssetsEventArgs> eventHandler)
    {
      this.CreateAvatarAsync(manifest, AvatarComponentMasks.All, eventHandler);
    }

    public void CreateAvatarAsync(
      AvatarManifest manifest,
      AvatarComponentMasks componentMask,
      EventHandler<GetAvatarAssetsEventArgs> eventHandler)
    {
      if (manifest == (AvatarManifest) null)
        throw new ArgumentException(Resources.ManifestCannotBeNullText);
      new Thread(new ThreadStart(new GetAvatarAssetsAsync(this, manifest.Clone(), componentMask, eventHandler).Process)).Start();
    }

    public void GetAvatarAnimationAsync(
      Guid animationId,
      EventHandler<GetAvatarAnimationEventArgs> eventHandler)
    {
      new Thread(new ThreadStart(new Microsoft.XboxLive.Avatars.Internal.GetAvatarAnimationAsync(this, animationId, eventHandler).Process)).Start();
    }

    public void GetAvatarComponentAsync(
      Guid componentId,
      Colorb[] customColors,
      EventHandler<GetAvatarComponentEventArgs> eventHandler)
    {
      new Thread(new ThreadStart(new Microsoft.XboxLive.Avatars.Internal.GetAvatarComponentAsync(this, componentId, customColors, eventHandler).Process)).Start();
    }

    public void GetComponentColorTableAsync(
      Guid componentId,
      EventHandler<GetComponentColorTableEventArgs> eventHandler)
    {
      new Thread(new ThreadStart(new Microsoft.XboxLive.Avatars.Internal.GetComponentColorTableAsync(this, componentId, eventHandler).Process)).Start();
    }

    public void GetAvatarCarryableAsync(
      Guid carryableId,
      Skeleton avatarSkeleton,
      Colorb[] customColors,
      EventHandler<GetAvatarCarryableEventArgs> eventHandler)
    {
      new Thread(new ThreadStart(new Microsoft.XboxLive.Avatars.Internal.GetAvatarCarryableAsync(this, carryableId, avatarSkeleton, customColors, eventHandler).Process)).Start();
    }
  }
}
