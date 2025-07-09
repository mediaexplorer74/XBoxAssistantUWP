// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Version1.AvatarGetData
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;
using Microsoft.XboxLive.Avatars.Internal.Parsers;
using Microsoft.XboxLive.MathUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;


namespace Microsoft.XboxLive.Avatars.Internal.Version1
{
  internal class AvatarGetData
  {
    private static readonly Vector4 DefaultRimLight = new Vector4(0.549019635f, 0.5019608f, 0.403921574f, 1.6f);
    private static readonly AvatarGetData.ColorBlendPoint[] RimLightColours = new AvatarGetData.ColorBlendPoint[18]
    {
      new AvatarGetData.ColorBlendPoint(new Vector4(0.407843143f, 0.2509804f, 0.184313729f, 1.6f), new Colorb((byte) 101, (byte) 60, (byte) 35)),
      new AvatarGetData.ColorBlendPoint(new Vector4(0.549019635f, 0.470588237f, 0.4117647f, 1.6f), new Colorb((byte) 198, (byte) 157, (byte) 113)),
      new AvatarGetData.ColorBlendPoint(new Vector4(0.509803951f, 0.509803951f, 0.509803951f, 1.6f), new Colorb((byte) 232, (byte) 220, (byte) 221)),
      new AvatarGetData.ColorBlendPoint(new Vector4(0.466666669f, 0.2784314f, 0.2509804f, 1.6f), new Colorb((byte) 92, (byte) 60, (byte) 26)),
      new AvatarGetData.ColorBlendPoint(new Vector4(0.5372549f, 0.407843143f, 0.274509817f, 1.6f), new Colorb((byte) 175, (byte) 128, (byte) 73)),
      new AvatarGetData.ColorBlendPoint(new Vector4(0.509803951f, 0.4509804f, 0.419607848f, 1.6f), new Colorb((byte) 229, (byte) 202, (byte) 185)),
      new AvatarGetData.ColorBlendPoint(new Vector4(0.282352954f, 0.1764706f, 0.141176477f, 1.6f), new Colorb((byte) 59, (byte) 36, (byte) 13)),
      new AvatarGetData.ColorBlendPoint(new Vector4(0.427450985f, 0.2784314f, 0.180392161f, 1.6f), new Colorb((byte) 140, (byte) 93, (byte) 46)),
      new AvatarGetData.ColorBlendPoint(new Vector4(0.509803951f, 0.454901963f, 0.419607848f, 1.6f), new Colorb((byte) 212, (byte) 173, (byte) 142)),
      new AvatarGetData.ColorBlendPoint(new Vector4(0.5137255f, 0.470588237f, 0.4627451f, 1.6f), new Colorb((byte) 202, (byte) 183, (byte) 183)),
      new AvatarGetData.ColorBlendPoint(new Vector4(0.482352942f, 0.407843143f, 0.349019617f, 1.6f), new Colorb((byte) 209, (byte) 156, (byte) 118)),
      new AvatarGetData.ColorBlendPoint(new Vector4(0.5176471f, 0.443137258f, 0.349019617f, 1.6f), new Colorb((byte) 198, (byte) 171, (byte) 113)),
      new AvatarGetData.ColorBlendPoint(new Vector4(0.549019635f, 0.419607848f, 0.407843143f, 1.6f), new Colorb((byte) 221, (byte) 165, (byte) 147)),
      new AvatarGetData.ColorBlendPoint(new Vector4(0.5058824f, 0.3137255f, 0.211764708f, 1.6f), new Colorb((byte) 205, (byte) 134, (byte) 77)),
      new AvatarGetData.ColorBlendPoint(new Vector4(0.482352942f, 0.435294122f, 0.3019608f, 1.6f), new Colorb((byte) 184, (byte) 140, (byte) 76)),
      new AvatarGetData.ColorBlendPoint(new Vector4(0.4509804f, 0.3137255f, 0.321568638f, 1.6f), new Colorb((byte) 209, (byte) 136, (byte) 107)),
      new AvatarGetData.ColorBlendPoint(new Vector4(0.4627451f, 0.380392164f, 0.333333343f, 1.6f), new Colorb((byte) 195, (byte) 111, (byte) 60)),
      new AvatarGetData.ColorBlendPoint(new Vector4(0.458823532f, 0.34117648f, 0.243137255f, 1.6f), new Colorb((byte) 155, (byte) 115, (byte) 62))
    };
    private static readonly AvatarGetData.ParamsUsageMapItem[] ManifestColorToParamUsageMap = new AvatarGetData.ParamsUsageMapItem[9]
    {
      new AvatarGetData.ParamsUsageMapItem(ShaderParameterUsage.PixelConstantColorSkin, DynamicColorType.Skin),
      new AvatarGetData.ParamsUsageMapItem(ShaderParameterUsage.PixelConstantColorHair, DynamicColorType.Hair),
      new AvatarGetData.ParamsUsageMapItem(ShaderParameterUsage.PixelConstantColorMouth, DynamicColorType.Mouth),
      new AvatarGetData.ParamsUsageMapItem(ShaderParameterUsage.PixelConstantColorIris, DynamicColorType.Iris),
      new AvatarGetData.ParamsUsageMapItem(ShaderParameterUsage.PixelConstantColorEyebrow, DynamicColorType.Eyebrow),
      new AvatarGetData.ParamsUsageMapItem(ShaderParameterUsage.PixelConstantColorEyeShadow, DynamicColorType.EyeShadow),
      new AvatarGetData.ParamsUsageMapItem(ShaderParameterUsage.PixelConstantColorFacialHair, DynamicColorType.FacialHair),
      new AvatarGetData.ParamsUsageMapItem(ShaderParameterUsage.PixelConstantColorSkinFeature1, DynamicColorType.SkinFeatures1),
      new AvatarGetData.ParamsUsageMapItem(ShaderParameterUsage.PixelConstantColorSkinFeature2, DynamicColorType.SkinFeatures2)
    };
    private AvatarManifestV1 m_Manifest;
    private List<BinaryAsset> m_RequestedAssets;
    private AvatarAssetCacheManager m_AssetCache;
    private CoordinateSystem m_CoordinateSystem;
    private IResourceFactory m_ResourceFactory;
    private IDataManager m_dataManager;
    internal Avatar m_Target;

    internal List<BinaryAsset> RequestedAssets => this.m_RequestedAssets;

    internal AvatarGetData(
      IDataManager dataManager,
      AvatarAssetCacheManager assetCache,
      IResourceFactory resourceFactory,
      CoordinateSystem coordinateSystem)
    {
      this.m_RequestedAssets = new List<BinaryAsset>();
      this.m_CoordinateSystem = coordinateSystem;
      this.m_AssetCache = assetCache;
      this.m_ResourceFactory = resourceFactory;
      this.m_dataManager = dataManager;
    }

    internal Avatar Load(
      AvatarManifest manifest,
      AvatarComponentMasks componentMask,
      bool allowAlternativeAssets)
    {
      this.m_Manifest = manifest as AvatarManifestV1;
      this.m_Target = new Avatar((AvatarManifest) this.m_Manifest);
      if (this.m_Manifest.DressDefaultClothes)
      {
        this.m_Manifest.RemoveComponents(~componentMask);
        this.DressDefaultClothes(componentMask);
      }
      else
        this.m_Manifest.ClearComponents(~componentMask);
      this.CreateAssetsList(componentMask);
      if (!this.ValidateComponentMasks())
        throw new AvatarException(Resources.ManifestValidationFailed);
      BinaryAssetParseContext assetParseContext = new BinaryAssetParseContext();
      assetParseContext.m_CombinedComponentMask = this.m_Manifest.GetUserCombinedComponentMask(componentMask);
      assetParseContext.m_CoordinateSystem = this.m_CoordinateSystem;
      assetParseContext.m_Target = this.m_Target;
      assetParseContext.m_BodyType = this.m_Manifest.BodyType;
      assetParseContext.m_ResourceFactory = this.m_ResourceFactory;
      assetParseContext.m_AssetCache = this.m_AssetCache;
      if (assetParseContext.m_AssetCache != null)
      {
        AvatarAssetCacheV1 assetCache = assetParseContext.m_AssetCache.GetAssetCache(1) as AvatarAssetCacheV1;
        while (!assetCache.LoadAssets(this.m_RequestedAssets, assetParseContext, this.m_dataManager))
        {
          if (!this.m_Manifest.DressDefaultClothes || !allowAlternativeAssets)
            throw new AvatarException(Resources.FailedToLoadAssetText);
          foreach (BinaryAsset requestedAsset in this.m_RequestedAssets)
          {
            CachedBinaryAsset.AssetState assetState = CachedBinaryAsset.AssetState.Invalid;
            if (requestedAsset.m_Cache != null)
              assetState = requestedAsset.m_Cache.m_AssetState;
            if (assetState != CachedBinaryAsset.AssetState.Parsed)
            {
              if (this.m_Manifest.IsCoreAsset(requestedAsset.AssetId))
                throw new AvatarException(Resources.FailedToLoadCoreAssetText);
              Logger.Log((Log) new DebugLog((object) this, string.Format("Failed to load non-stock assets {0}, loading previous component instead.", (object) requestedAsset.AssetId.ToString())));
              this.m_Manifest.RemoveAsset(requestedAsset.AssetId);
            }
          }
          this.DressDefaultClothes(componentMask);
          this.CreateAssetsList(componentMask);
        }
        if (this.CreateAvatarFromCache(assetParseContext) != AvatarGetData.Result.Ok)
          throw new AvatarException(Resources.IncompatibleAssetsText);
      }
      else
      {
        while (true)
        {
          int num = (int) this.LoadAssets();
          AvatarGetData.Result result = AvatarGetData.Result.Ok;
          foreach (BinaryAsset requestedAsset in this.m_RequestedAssets)
          {
            if (assetParseContext.m_skeletonVersion == Skeleton.SkeletonVersion.Invalid)
            {
              AssetMetadataParser metadata = requestedAsset.GetMetadata();
              if (metadata != null)
                assetParseContext.m_skeletonVersion = metadata.AssetSkeletonVersion;
            }
            if (requestedAsset.m_Stream == null || !requestedAsset.Validate(assetParseContext))
            {
              if (this.m_Manifest.IsCoreAsset(requestedAsset.AssetId))
                throw new AvatarException(Resources.FailedToLoadCoreAssetText);
              if (!this.m_Manifest.DressDefaultClothes || !allowAlternativeAssets)
                throw new AvatarException(Resources.FailedToLoadAssetText);
              Logger.Log((Log) new DebugLog((object) this, string.Format("Failed to load non-stock assets {0}, loading previous component instead.", (object) requestedAsset.AssetId.ToString())));
              this.m_Manifest.RemoveAsset(requestedAsset.AssetId);
              result = AvatarGetData.Result.Failed;
            }
          }
          if (result != AvatarGetData.Result.Ok)
          {
            this.DressDefaultClothes(componentMask);
            this.CreateAssetsList(componentMask);
          }
          else
            break;
        }
        if (this.CreateAvatarFromStreams(assetParseContext) != AvatarGetData.Result.Ok)
          throw new AvatarException(Resources.IncompatibleAssetsText);
      }
      this.GenerateSkeleton(assetParseContext.m_skeletonVersion);
      this.FinalizeAssets();
      return this.m_Target;
    }

    internal void LoadAvatarComponent(
      BinaryAssetModel assetLoader,
      BinaryAssetParseContext parseContext)
    {
      ComponentInfo componentDescription = assetLoader.m_ComponentDescription;
      ShaderConstantOverride[] constantOverrides = assetLoader.ShaderConstantOverrides;
      constantOverrides[0].m_Constant = ShaderParameterUsage.PixelConstantColorCustom0;
      constantOverrides[0].m_Value = Utilities.ColorbToVector4(componentDescription.m_CustomColors0);
      constantOverrides[1].m_Constant = ShaderParameterUsage.PixelConstantColorCustom1;
      constantOverrides[1].m_Value = Utilities.ColorbToVector4(componentDescription.m_CustomColors1);
      constantOverrides[2].m_Constant = ShaderParameterUsage.PixelConstantColorCustom2;
      constantOverrides[2].m_Value = Utilities.ColorbToVector4(componentDescription.m_CustomColors2);
      constantOverrides[3].m_Constant = ShaderParameterUsage.PixelConstantColorRimLight;
      constantOverrides[3].m_Value = AvatarGetData.DefaultRimLight;
      this.m_RequestedAssets = new List<BinaryAsset>();
      this.m_RequestedAssets.Add((BinaryAsset) assetLoader);
      if (assetLoader.m_Stream != null)
      {
        int num1 = this.Validate(parseContext) == AvatarGetData.Result.Ok ? (int) this.CreateAvatarFromStreams(parseContext) : throw new AvatarException(Resources.InvalidComponentIdText);
      }
      else if (parseContext.m_AssetCache != null)
      {
        if (!(parseContext.m_AssetCache.GetAssetCache(1) as AvatarAssetCacheV1).LoadAssets(this.m_RequestedAssets, parseContext, this.m_dataManager))
        {
          if (assetLoader.m_Cache.m_AssetState == CachedBinaryAsset.AssetState.Downloaded)
            throw new AvatarException(Resources.InvalidComponentIdText);
          throw new AvatarException(Resources.FailedToLoadAssetText);
        }
        int avatarFromCache = (int) this.CreateAvatarFromCache(parseContext);
      }
      else
      {
        if (this.LoadAssets() != AvatarGetData.Result.Ok)
          throw new AvatarException(Resources.FailedToLoadAssetText);
        int num2 = this.Validate(parseContext) == AvatarGetData.Result.Ok ? (int) this.CreateAvatarFromStreams(parseContext) : throw new AvatarException(Resources.InvalidComponentIdText);
      }
    }

    internal AvatarComponent LoadAvatarComponent(Guid componentId, Colorb[] customColors)
    {
      this.m_Target = new Avatar((AvatarManifest) null);
      BinaryAssetParseContext parseContext = new BinaryAssetParseContext();
      parseContext.m_CombinedComponentMask = AvatarComponentMasks.All;
      parseContext.m_CoordinateSystem = this.m_CoordinateSystem;
      parseContext.m_Target = this.m_Target;
      parseContext.m_BodyType = AvatarGender.Both;
      parseContext.m_ResourceFactory = this.m_ResourceFactory;
      parseContext.m_AssetCache = this.m_AssetCache;
      ComponentInfo description;
      if (customColors == null)
      {
        Colorb colorb = new Colorb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        description = new ComponentInfo(componentId, AvatarComponentMasks.All, colorb, colorb, colorb);
      }
      else
        description = customColors.Length == 3 ? new ComponentInfo(componentId, AvatarComponentMasks.All, customColors[0], customColors[1], customColors[2]) : throw new ArgumentException(Resources.InvalidCustomColorsText);
      this.LoadAvatarComponent(new BinaryAssetModel(description, 4, AvatarComponentMasks.None), parseContext);
      if (this.m_Target.m_Models.Count != 1)
        throw new AvatarException(Resources.InvalidComponentIdText);
      return this.m_Target.m_Models[0];
    }

    internal AvatarComponent LoadAvatarComponent(Stream componentStream, Colorb[] customColors)
    {
      this.m_Target = new Avatar((AvatarManifest) null);
      BinaryAssetParseContext parseContext = new BinaryAssetParseContext();
      parseContext.m_CombinedComponentMask = AvatarComponentMasks.All;
      parseContext.m_CoordinateSystem = this.m_CoordinateSystem;
      parseContext.m_Target = this.m_Target;
      parseContext.m_BodyType = AvatarGender.Both;
      parseContext.m_ResourceFactory = this.m_ResourceFactory;
      parseContext.m_AssetCache = this.m_AssetCache;
      ComponentInfo description;
      if (customColors == null)
      {
        Colorb colorb = new Colorb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        description = new ComponentInfo(Guid.Empty, AvatarComponentMasks.All, colorb, colorb, colorb);
      }
      else
        description = customColors.Length == 3 ? new ComponentInfo(Guid.Empty, AvatarComponentMasks.All, customColors[0], customColors[1], customColors[2]) : throw new ArgumentException(Resources.InvalidCustomColorsText);
      BinaryAssetModel assetLoader = new BinaryAssetModel(description, 4, AvatarComponentMasks.None);
      assetLoader.Stream = componentStream;
      this.LoadAvatarComponent(assetLoader, parseContext);
      if (this.m_Target.m_Models.Count != 1)
        throw new AvatarException(Resources.InvalidComponentIdText);
      return this.m_Target.m_Models[0];
    }

    internal AvatarCarryable LoadAvatarCarryable(
      Guid componentId,
      Skeleton avatarSkeleton,
      Colorb[] customColors)
    {
      this.m_Target = new Avatar((AvatarManifest) null);
      BinaryAssetParseContext parseContext = new BinaryAssetParseContext();
      parseContext.m_CombinedComponentMask = AvatarComponentMasks.All;
      parseContext.m_CoordinateSystem = this.m_CoordinateSystem;
      parseContext.m_Target = this.m_Target;
      parseContext.m_BodyType = AvatarGender.Both;
      parseContext.m_ResourceFactory = this.m_ResourceFactory;
      parseContext.m_AssetCache = this.m_AssetCache;
      ComponentInfo description;
      if (customColors == null)
      {
        Colorb colorb = new Colorb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        description = new ComponentInfo(componentId, AvatarComponentMasks.All, colorb, colorb, colorb);
      }
      else
        description = customColors.Length == 3 ? new ComponentInfo(componentId, AvatarComponentMasks.All, customColors[0], customColors[1], customColors[2]) : throw new ArgumentException(Resources.InvalidCustomColorsText);
      this.LoadAvatarComponent((BinaryAssetModel) new BinaryAssetCarryable(description, 4, AvatarComponentMasks.None), parseContext);
      if (this.m_Target.m_Carryable == null)
        throw new AvatarException(Resources.InvalidComponentIdText);
      if (avatarSkeleton != null)
      {
        Vector3 scale = avatarSkeleton.Joints[0].Local.scale;
        this.m_Target.m_Carryable.m_Skeleton.Joints[0].Local.scale = scale;
        this.m_Target.m_Carryable.m_Skeleton.Joints[0].Local.position.X *= scale.X;
        this.m_Target.m_Carryable.m_Skeleton.Joints[0].Local.position.Y *= scale.Y;
        this.m_Target.m_Carryable.m_Skeleton.Joints[0].Local.position.Z *= scale.Z;
      }
      return this.m_Target.m_Carryable;
    }

    internal AvatarCarryable LoadAvatarCarryable(
      Stream componentStream,
      Skeleton avatarSkeleton,
      Colorb[] customColors)
    {
      this.m_Target = new Avatar((AvatarManifest) null);
      BinaryAssetParseContext parseContext = new BinaryAssetParseContext();
      parseContext.m_CombinedComponentMask = AvatarComponentMasks.All;
      parseContext.m_CoordinateSystem = this.m_CoordinateSystem;
      parseContext.m_Target = this.m_Target;
      parseContext.m_BodyType = AvatarGender.Both;
      parseContext.m_ResourceFactory = this.m_ResourceFactory;
      parseContext.m_AssetCache = this.m_AssetCache;
      ComponentInfo description;
      if (customColors == null)
      {
        Colorb colorb = new Colorb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        description = new ComponentInfo(Guid.Empty, AvatarComponentMasks.All, colorb, colorb, colorb);
      }
      else
        description = customColors.Length == 3 ? new ComponentInfo(Guid.Empty, AvatarComponentMasks.All, customColors[0], customColors[1], customColors[2]) : throw new ArgumentException(Resources.InvalidCustomColorsText);
      BinaryAssetCarryable assetLoader = new BinaryAssetCarryable(description, 4, AvatarComponentMasks.None);
      assetLoader.Stream = componentStream;
      this.LoadAvatarComponent((BinaryAssetModel) assetLoader, parseContext);
      if (this.m_Target.m_Carryable == null)
        throw new AvatarException(Resources.InvalidComponentIdText);
      if (avatarSkeleton != null)
      {
        Vector3 scale = avatarSkeleton.Joints[0].Local.scale;
        this.m_Target.m_Carryable.m_Skeleton.Joints[0].Local.scale = scale;
        this.m_Target.m_Carryable.m_Skeleton.Joints[0].Local.position.X *= scale.X;
        this.m_Target.m_Carryable.m_Skeleton.Joints[0].Local.position.Y *= scale.Y;
        this.m_Target.m_Carryable.m_Skeleton.Joints[0].Local.position.Z *= scale.Z;
      }
      return this.m_Target.m_Carryable;
    }

    internal ComponentColors[] LoadColorTable(Guid assetId)
    {
      BinaryAssetParseContext assetParseContext = new BinaryAssetParseContext();
      assetParseContext.m_CombinedComponentMask = AvatarComponentMasks.All;
      assetParseContext.m_CoordinateSystem = this.m_CoordinateSystem;
      assetParseContext.m_Target = (Avatar) null;
      assetParseContext.m_BodyType = AvatarGender.Both;
      assetParseContext.m_ResourceFactory = this.m_ResourceFactory;
      assetParseContext.m_AssetCache = this.m_AssetCache;
      Colorb colorb = new Colorb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
      ComponentInfo componentInfo = new ComponentInfo(assetId, AvatarComponentMasks.All, colorb, colorb, colorb);
      BinaryAssetColorTable binaryAssetColorTable = new BinaryAssetColorTable(assetId, AvatarComponentMasks.None);
      this.m_RequestedAssets = new List<BinaryAsset>();
      this.m_RequestedAssets.Add((BinaryAsset) binaryAssetColorTable);
      ComponentColorTable componentColorTable;
      if (assetParseContext.m_AssetCache != null)
      {
        bool flag = (assetParseContext.m_AssetCache.GetAssetCache(1) as AvatarAssetCacheV1).LoadAssets(this.m_RequestedAssets, assetParseContext, this.m_dataManager);
        if (!(binaryAssetColorTable.m_Cache is CachedBinaryAssetColorTable cache))
          throw new AvatarException(Resources.InvalidComponentIdText);
        if (!flag)
        {
          if (cache.m_AssetState == CachedBinaryAsset.AssetState.Downloaded)
            throw new AvatarException(Resources.InvalidComponentIdText);
          throw new AvatarException(Resources.FailedToLoadAssetText);
        }
        componentColorTable = cache.ColorTable;
      }
      else
      {
        if (this.LoadAssets() != AvatarGetData.Result.Ok)
          throw new AvatarException(Resources.FailedToLoadAssetText);
        if (this.Validate(assetParseContext) != AvatarGetData.Result.Ok)
          throw new AvatarException(Resources.InvalidComponentIdText);
        componentColorTable = binaryAssetColorTable.GetPrimaryColorTable();
      }
      return componentColorTable?.Colors;
    }

    internal ComponentColors[] LoadColorTable(Stream assetStream)
    {
      BinaryAssetParseContext ctx = new BinaryAssetParseContext();
      ctx.m_CombinedComponentMask = AvatarComponentMasks.All;
      ctx.m_CoordinateSystem = this.m_CoordinateSystem;
      ctx.m_Target = (Avatar) null;
      ctx.m_BodyType = AvatarGender.Both;
      ctx.m_ResourceFactory = this.m_ResourceFactory;
      Colorb colorb = new Colorb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
      ComponentInfo componentInfo = new ComponentInfo(Guid.Empty, AvatarComponentMasks.All, colorb, colorb, colorb);
      BinaryAssetColorTable binaryAssetColorTable = new BinaryAssetColorTable(Guid.Empty, AvatarComponentMasks.None);
      this.m_RequestedAssets = new List<BinaryAsset>();
      this.m_RequestedAssets.Add((BinaryAsset) binaryAssetColorTable);
      binaryAssetColorTable.Stream = assetStream;
      if (this.Validate(ctx) != AvatarGetData.Result.Ok)
        throw new AvatarException(Resources.InvalidComponentIdText);
      return binaryAssetColorTable.GetPrimaryColorTable()?.Colors;
    }

    internal AssetMetadataParser LoadMetadata(Guid assetId)
    {
      AvatarGetData.LoadMetaDataContext context = new AvatarGetData.LoadMetaDataContext();
      AssetsAsyncLoadContextCommon common = context.common;
      using (common.syncEvent = new AutoResetEvent(false))
      {
        this.m_dataManager.GetAssetAsync(assetId, new DownloadRequestEventHandler(this.LoadMetadataCompleted), (object) context);
        common.syncEvent.WaitOne();
      }
      return context.parser;
    }

    private void LoadMetadataCompleted(object sender, DataRequestCompletedEventArgs e)
    {
      AvatarGetData.LoadMetaDataContext userState = e.UserState as AvatarGetData.LoadMetaDataContext;
      lock (userState.common.syncRequestLock)
      {
        userState.parser = e.Error == null ? (!e.Cancelled ? this.LoadMetadata(e.Result) : (AssetMetadataParser) null) : (AssetMetadataParser) null;
        userState.common.syncEvent.Set();
      }
    }

    internal AssetMetadataParser LoadMetadata(Stream assetStream)
    {
      StructuredBinary structuredBinary = new StructuredBinary();
      if (!structuredBinary.Open(assetStream))
        return (AssetMetadataParser) null;
      if (!(structuredBinary.Namespace == BinaryAsset.AvatarAssetGuid))
        return (AssetMetadataParser) null;
      AssetMetadataParser assetMetadataParser = new AssetMetadataParser();
      return !assetMetadataParser.LoadFromStrb(structuredBinary.Iterator) ? (AssetMetadataParser) null : assetMetadataParser;
    }

    private void DownloadAssetCompleted(object sender, DataRequestCompletedEventArgs e)
    {
      AssetsAsyncLoadContext userState = e.UserState as AssetsAsyncLoadContext;
      lock (userState.common.syncRequestLock)
      {
        --userState.common.numRequests;
        if (e.Error != null)
          userState.common.failed = true;
        else if (e.Cancelled)
          userState.common.failed = true;
        else
          this.m_RequestedAssets[userState.index].Stream = e.Result;
        if (userState.common.numRequests != 0)
          return;
        userState.common.syncEvent.Set();
      }
    }

    internal AvatarGetData.Result LoadAssets()
    {
      int count = this.m_RequestedAssets.Count;
      if (count == 0)
        return AvatarGetData.Result.Ok;
      AssetsAsyncLoadContextCommon loadContextCommon = new AssetsAsyncLoadContextCommon();
      loadContextCommon.syncRequestLock = new object();
      using (loadContextCommon.syncEvent = new AutoResetEvent(false))
      {
        loadContextCommon.numRequests = count;
        for (int index = 0; index < count; ++index)
          this.m_dataManager.GetAssetAsync(this.m_RequestedAssets[index].AssetId, new DownloadRequestEventHandler(this.DownloadAssetCompleted), (object) new AssetsAsyncLoadContext()
          {
            index = index,
            common = loadContextCommon
          });
        loadContextCommon.syncEvent.WaitOne();
      }
      return !loadContextCommon.failed ? AvatarGetData.Result.Ok : AvatarGetData.Result.Failed;
    }

    private bool ValidateComponentMasks()
    {
      int count = this.m_RequestedAssets.Count;
      AvatarComponentMasks avatarComponentMasks = AvatarComponentMasks.None;
      for (int index = 0; index < count; ++index)
      {
        AvatarComponentMasks componentMask = this.m_RequestedAssets[index].m_ComponentMask;
        if ((componentMask & avatarComponentMasks) != AvatarComponentMasks.None)
          return false;
        avatarComponentMasks |= componentMask;
      }
      return true;
    }

    internal AvatarGetData.Result Validate(BinaryAssetParseContext ctx)
    {
      int count = this.m_RequestedAssets.Count;
      if (ctx.m_skeletonVersion == Skeleton.SkeletonVersion.Invalid && count > 0)
      {
        ctx.m_skeletonVersion = this.m_RequestedAssets[0].m_SkeletonVersion;
        if (ctx.m_skeletonVersion == Skeleton.SkeletonVersion.Invalid)
        {
          AssetMetadataParser metadata = this.m_RequestedAssets[0].GetMetadata();
          if (metadata != null)
            ctx.m_skeletonVersion = metadata.AssetSkeletonVersion;
        }
      }
      for (int index = 0; index < count; ++index)
      {
        if (!this.m_RequestedAssets[index].Validate(ctx))
          return AvatarGetData.Result.InvalidManifest;
      }
      return AvatarGetData.Result.Ok;
    }

    internal AvatarGetData.Result CreateAvatarFromCache(BinaryAssetParseContext ctx)
    {
      if (this.m_RequestedAssets.Count > 0)
      {
        if (ctx.m_skeletonVersion == Skeleton.SkeletonVersion.Invalid)
        {
          for (int index = 0; index < this.m_RequestedAssets.Count; ++index)
          {
            CachedBinaryAsset cache = this.m_RequestedAssets[0].m_Cache;
            if (cache != null)
            {
              ctx.m_skeletonVersion = cache.GetAssetSkeletonVersion();
              break;
            }
          }
        }
        for (int index = 0; index < this.m_RequestedAssets.Count; ++index)
        {
          if (!this.m_RequestedAssets[index].ValidateFromCache(ctx))
          {
            Logger.Log((Log) new DebugLog((object) this, string.Format("Cached asset {0} cannot be used in current loading context.", (object) this.m_RequestedAssets[index].AssetId.ToString())));
            return AvatarGetData.Result.Failed;
          }
        }
      }
      for (int index = 0; index < this.m_RequestedAssets.Count; ++index)
      {
        if (!this.m_RequestedAssets[index].ProcessComponentsFromCache(ctx))
          return AvatarGetData.Result.Failed;
      }
      for (int index = 0; index < this.m_RequestedAssets.Count; ++index)
      {
        if (!this.m_RequestedAssets[index].ProcessAssetsFromCache(ctx))
          return AvatarGetData.Result.Failed;
      }
      for (int index = 0; index < this.m_RequestedAssets.Count; ++index)
      {
        if (!this.m_RequestedAssets[index].ProcessOverridesFromCache(ctx))
          return AvatarGetData.Result.Failed;
      }
      return AvatarGetData.Result.Ok;
    }

    internal AvatarGetData.Result CreateAvatarFromStreams(BinaryAssetParseContext ctx)
    {
      for (int index = 0; index < this.m_RequestedAssets.Count; ++index)
      {
        if (!this.m_RequestedAssets[index].ProcessComponentsFromStream(ctx))
          return AvatarGetData.Result.Failed;
      }
      for (int index = 0; index < this.m_RequestedAssets.Count; ++index)
      {
        if (!this.m_RequestedAssets[index].ProcessAssetsFromStream(ctx))
          return AvatarGetData.Result.Failed;
      }
      for (int index = 0; index < this.m_RequestedAssets.Count; ++index)
      {
        if (!this.m_RequestedAssets[index].ProcessOverridesFromStream(ctx))
          return AvatarGetData.Result.Failed;
      }
      return AvatarGetData.Result.Ok;
    }

    private bool CreateAssetsList(AvatarComponentMasks componentMask)
    {
      if (this.m_Manifest.m_Dirty)
        this.m_Manifest.UpdateDependencies(this.m_dataManager);
      this.m_RequestedAssets.Clear();
      bool flag1 = AvatarComponentMasks.None != (componentMask & this.m_Manifest.BodyComponentInfo.m_ComponentInfo.m_ComponentMask);
      bool flag2 = AvatarComponentMasks.None != (componentMask & this.m_Manifest.HeadComponentInfo.m_ComponentInfo.m_ComponentMask);
      if (flag1)
      {
        BinaryAssetModel binaryAssetModel = new BinaryAssetModel(this.m_Manifest.BodyComponentInfo.m_ComponentInfo, 2, this.m_Manifest.BodyComponentInfo.m_ComponentInfo.m_ComponentMask);
        ShaderConstantOverride[] constantOverrides = binaryAssetModel.ShaderConstantOverrides;
        constantOverrides[0].m_Constant = ShaderParameterUsage.PixelConstantColorCustom0;
        constantOverrides[0].m_Value = this.m_Manifest.GetDynamicColor(DynamicColorType.Skin);
        constantOverrides[1].m_Constant = ShaderParameterUsage.PixelConstantColorRimLight;
        constantOverrides[1].m_Value = AvatarGetData.GetRimLight(this.m_Manifest.GetDynamicColor(DynamicColorType.Skin));
        this.m_RequestedAssets.Add((BinaryAsset) binaryAssetModel);
      }
      if (flag2)
      {
        BinaryAssetModel binaryAssetModel = new BinaryAssetModel(this.m_Manifest.HeadComponentInfo.m_ComponentInfo, 10, this.m_Manifest.HeadComponentInfo.m_ComponentInfo.m_ComponentMask);
        ShaderConstantOverride[] constantOverrides = binaryAssetModel.ShaderConstantOverrides;
        constantOverrides[0].m_Constant = ShaderParameterUsage.PixelConstantColorRimLight;
        constantOverrides[0].m_Value = AvatarGetData.GetRimLight(this.m_Manifest.GetDynamicColor(DynamicColorType.Skin));
        int index1 = 9;
        while (--index1 >= 0)
        {
          constantOverrides[index1 + 1].m_Constant = AvatarGetData.ManifestColorToParamUsageMap[index1].use;
          constantOverrides[index1 + 1].m_Value = this.m_Manifest.GetDynamicColor(AvatarGetData.ManifestColorToParamUsageMap[index1].color);
        }
        this.m_RequestedAssets.Add((BinaryAsset) binaryAssetModel);
        for (int index2 = 0; index2 < 6; ++index2)
        {
          DynamicTextureType dynamicTextureType = (DynamicTextureType) index2;
          AvatarManifestV1.ReplacementTexture replacementTexture = this.m_Manifest.GetReplacementTexture(dynamicTextureType);
          if (replacementTexture.m_TextureAssetId != Guid.Empty)
            this.m_RequestedAssets.Add((BinaryAsset) new BinaryAssetTexture(dynamicTextureType, replacementTexture.m_TextureAssetId));
        }
        for (int index3 = 0; index3 < 3; ++index3)
        {
          BlendShapeType blendShapeType = (BlendShapeType) index3;
          if (!(Guid.Empty == this.m_Manifest.GetBlendShape(blendShapeType)))
            this.m_RequestedAssets.Add((BinaryAsset) new BinaryAssetBlendShape(blendShapeType, this.m_Manifest.GetBlendShape(blendShapeType)));
        }
        if (this.m_Manifest.HeadComponentInfo.m_OverrideAsset != Guid.Empty)
          this.AddOverride(this.m_Manifest.HeadComponentInfo.m_OverrideAsset);
      }
      Vector4 dynamicColor = this.m_Manifest.GetDynamicColor(DynamicColorType.Hair);
      for (int index = 0; index < this.m_Manifest.m_ComponentInfo.Count; ++index)
      {
        if ((this.m_Manifest.m_ComponentInfo[index].m_ComponentInfo.m_ComponentMask & componentMask) != AvatarComponentMasks.None)
          this.AddModel(this.m_Manifest.m_ComponentInfo[index], dynamicColor);
      }
      return true;
    }

    internal void AddOverride(Guid guid)
    {
      this.m_RequestedAssets.Add((BinaryAsset) new BinaryAssetShapeOverride(guid, AvatarComponentMasks.None));
    }

    internal void AddModel(ComponentDescription info, Vector4 colorHair)
    {
      BinaryAssetModel binaryAssetModel = info.m_ComponentInfo.m_ComponentMask != AvatarComponentMasks.Carryable ? new BinaryAssetModel(info.m_ComponentInfo, 4, info.m_ComponentInfo.m_ComponentMask) : (BinaryAssetModel) new BinaryAssetCarryable(info.m_ComponentInfo, 4, AvatarComponentMasks.Carryable);
      ShaderConstantOverride[] constantOverrides = binaryAssetModel.ShaderConstantOverrides;
      if (info.m_ComponentInfo.m_ComponentMask == AvatarComponentMasks.Hair)
      {
        constantOverrides[0].m_Constant = ShaderParameterUsage.PixelConstantColorCustom0;
        constantOverrides[0].m_Value = colorHair;
        if (info.m_OverrideAsset != Guid.Empty && (this.m_Manifest.GetCombinedComponentMask() & AvatarComponentMasks.Hat) != AvatarComponentMasks.None)
          binaryAssetModel.SetAssetId(info.m_OverrideAsset);
      }
      else
      {
        if (info.m_OverrideAsset != Guid.Empty)
          this.AddOverride(info.m_OverrideAsset);
        constantOverrides[0].m_Constant = ShaderParameterUsage.PixelConstantColorCustom0;
        constantOverrides[0].m_Value = Utilities.ColorbToVector4(info.m_ComponentInfo.m_CustomColors0);
      }
      constantOverrides[1].m_Constant = ShaderParameterUsage.PixelConstantColorCustom1;
      constantOverrides[1].m_Value = Utilities.ColorbToVector4(info.m_ComponentInfo.m_CustomColors1);
      constantOverrides[2].m_Constant = ShaderParameterUsage.PixelConstantColorCustom2;
      constantOverrides[2].m_Value = Utilities.ColorbToVector4(info.m_ComponentInfo.m_CustomColors2);
      constantOverrides[3].m_Constant = ShaderParameterUsage.PixelConstantColorRimLight;
      constantOverrides[3].m_Value = AvatarGetData.DefaultRimLight;
      this.m_RequestedAssets.Add((BinaryAsset) binaryAssetModel);
    }

    internal void DressDefaultClothes(AvatarComponentMasks componentMask)
    {
      if (!this.m_Manifest.ReplaceMissingComponents(true))
        throw new AvatarException(Resources.InvalidManifest1);
      if (!this.m_Manifest.GetRequiredComponentsPresent())
        throw new AvatarException(Resources.InvalidManifest2);
      if ((componentMask & AvatarComponentMasks.Body) != AvatarComponentMasks.None && this.m_Manifest.BodyComponentInfo.m_ComponentInfo.m_ComponentMask != AvatarComponentMasks.Body)
        throw new AvatarException(Resources.InvalidManifest3);
      if ((componentMask & AvatarComponentMasks.Head) == AvatarComponentMasks.None)
        return;
      if (this.m_Manifest.HeadComponentInfo.m_ComponentInfo.m_ComponentMask != AvatarComponentMasks.Head)
        throw new AvatarException(Resources.InvalidManifest4);
      if (!this.m_Manifest.GetRequiredBlendShapesPresent())
        throw new AvatarException(Resources.InvalidManifest5);
      if (!this.m_Manifest.GetRequiredReplacementTexturesPresent())
        throw new AvatarException(Resources.InvalidManifest6);
    }

    private static Vector4 GetRimLight(Vector4 skinColour4)
    {
      Colorb colorb = Utilities.ColorbFromVector4(skinColour4);
      int num1 = 268435455;
      uint index1 = 0;
      for (uint index2 = 0; (long) index2 < (long) AvatarGetData.RimLightColours.Length; ++index2)
      {
        AvatarGetData.ColorBlendPoint rimLightColour = AvatarGetData.RimLightColours[(IntPtr) index2];
        int num2 = ((int) rimLightColour.Skin.red - (int) colorb.red) * ((int) rimLightColour.Skin.red - (int) colorb.red) + ((int) rimLightColour.Skin.green - (int) colorb.green) * ((int) rimLightColour.Skin.green - (int) colorb.green) + ((int) rimLightColour.Skin.blue - (int) colorb.blue) * ((int) rimLightColour.Skin.blue - (int) colorb.blue);
        if (num2 < num1)
        {
          index1 = index2;
          num1 = num2;
        }
      }
      return AvatarGetData.RimLightColours[(IntPtr) index1].Rim;
    }

    private void GenerateSkeleton(Skeleton.SkeletonVersion skeletonVersion)
    {
      this.m_Target.m_Skeleton = EmbeddedSkeleton.GetEmbeddedSkeleton(this.m_CoordinateSystem, skeletonVersion);
      switch (skeletonVersion)
      {
        case Skeleton.SkeletonVersion.Nxe:
          AvatarSkeletonScaling.ApplyTo(this.m_Manifest.BodyType, this.m_Manifest.WidthFactor, this.m_Manifest.HeightFactor, this.m_Target.m_Skeleton);
          break;
        case Skeleton.SkeletonVersion.Natal:
          AvatarSkeletonScalingV2.ApplyTo(this.m_Manifest.BodyType, this.m_Manifest.WidthFactor, this.m_Manifest.HeightFactor, this.m_Target.m_Skeleton);
          break;
        default:
          throw new AvatarException(Resources.InvalidSkeletonVersion);
      }
      if (this.m_Target.m_Carryable == null)
        return;
      Vector3 scale = this.m_Target.m_Skeleton.Joints[0].Local.scale;
      this.m_Target.m_Carryable.m_Skeleton.Joints[0].Local.scale = scale;
      this.m_Target.m_Carryable.m_Skeleton.Joints[0].Local.position.X *= scale.X;
      this.m_Target.m_Carryable.m_Skeleton.Joints[0].Local.position.Y *= scale.Y;
      this.m_Target.m_Carryable.m_Skeleton.Joints[0].Local.position.Z *= scale.Z;
    }

    internal void FinalizeAssets()
    {
      int index1 = 0;
      while (index1 < this.m_Target.m_Models.Count)
      {
        int length = this.m_Target.m_Models[index1].m_Batches.Length;
        int num = 0;
        for (int index2 = 0; index2 < length; ++index2)
          num += this.m_Target.m_Models[index1].m_Batches[index2].Triangles.Length;
        if (num > 0)
          ++index1;
        else
          this.m_Target.m_Models.RemoveAt(index1);
      }
    }

    internal enum Result
    {
      Ok,
      Pending,
      InvalidManifest,
      Failed,
    }

    internal struct ColorBlendPoint
    {
      internal Vector4 Rim;
      internal Colorb Skin;

      internal ColorBlendPoint(Vector4 r, Colorb s)
      {
        this.Rim = r;
        this.Skin = s;
      }
    }

    internal struct ParamsUsageMapItem
    {
      internal ShaderParameterUsage use;
      internal DynamicColorType color;

      internal ParamsUsageMapItem(ShaderParameterUsage u, DynamicColorType c)
      {
        this.use = u;
        this.color = c;
      }
    }

    private class LoadMetaDataContext
    {
      public AssetMetadataParser parser;
      public AssetsAsyncLoadContextCommon common = new AssetsAsyncLoadContextCommon();
    }
  }
}
