// *********************************************************
// Type: Xonline.server.stats.fd.Avatar
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using Microsoft.XboxLive.Avatars.Internal;
using Microsoft.XboxLive.Avatars.Internal.Assets;
using Microsoft.XboxLive.Avatars.Internal.Version1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;


namespace Xonline.server.stats.fd
{
  public static class Avatar
  {
    private static Guid guidMaleBody = new Guid(2, (short) 0, (short) 1, (byte) 193, (byte) 200, (byte) 241, (byte) 9, (byte) 161, (byte) 156, (byte) 178, (byte) 224);
    private static Guid guidFemaleBody = new Guid(2, (short) 1, (short) 2, (byte) 193, (byte) 200, (byte) 241, (byte) 9, (byte) 161, (byte) 156, (byte) 178, (byte) 224);
    private static Guid guidHead = new Guid(1, (short) 2, (short) 3, (byte) 193, (byte) 200, (byte) 241, (byte) 9, (byte) 161, (byte) 156, (byte) 178, (byte) 224);
    private static Dictionary<Guid, int> guestGuids = new Dictionary<Guid, int>()
    {
      {
        new Guid(8, (short) 92, (short) 1, (byte) 193, (byte) 200, (byte) 241, (byte) 9, (byte) 161, (byte) 156, (byte) 178, (byte) 224),
        0
      },
      {
        new Guid(8, (short) 93, (short) 1, (byte) 193, (byte) 200, (byte) 241, (byte) 9, (byte) 161, (byte) 156, (byte) 178, (byte) 224),
        0
      },
      {
        new Guid(8, (short) 94, (short) 1, (byte) 193, (byte) 200, (byte) 241, (byte) 9, (byte) 161, (byte) 156, (byte) 178, (byte) 224),
        0
      },
      {
        new Guid(8, (short) 301, (short) 2, (byte) 193, (byte) 200, (byte) 241, (byte) 9, (byte) 161, (byte) 156, (byte) 178, (byte) 224),
        0
      },
      {
        new Guid(8, (short) 302, (short) 2, (byte) 193, (byte) 200, (byte) 241, (byte) 9, (byte) 161, (byte) 156, (byte) 178, (byte) 224),
        0
      },
      {
        new Guid(8, (short) 303, (short) 2, (byte) 193, (byte) 200, (byte) 241, (byte) 9, (byte) 161, (byte) 156, (byte) 178, (byte) 224),
        0
      }
    };
    private static AvatarComponentMasks[] requiredComponents = new AvatarComponentMasks[4]
    {
      AvatarComponentMasks.Shoes,
      AvatarComponentMasks.Trousers,
      AvatarComponentMasks.Shirt,
      AvatarComponentMasks.Hair
    };

    public static bool CompareByteArrays(byte[] b1, byte[] b2)
    {
      if (b1 == null)
        throw new ArgumentNullException(nameof (b1));
      if (b2 == null)
        throw new ArgumentNullException(nameof (b2));
      if (b1.Length != b2.Length)
        return false;
      for (int index = 0; index < b1.Length; ++index)
      {
        if ((int) b1[index] != (int) b2[index])
          return false;
      }
      return true;
    }

    [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "This is necessary for completely validating the manifest.")]
    public static bool ValidateAvatarManifest(ulong userId, byte[] manifestBytes)
    {
      if (manifestBytes == null)
        throw new ArgumentNullException(nameof (manifestBytes));
      if (manifestBytes.Length != 1000)
      {
        Debug.Assert(false, string.Empty, "Manifest validation failed for user {0}: incorrect manifest length {1}", (object) userId, (object) manifestBytes.Length);
        return false;
      }
      AvatarManifestV1 manifest = AvatarManifest.Create(manifestBytes) as AvatarManifestV1;
      if ((object) manifest == null)
        return false;
      if (manifest.BodyType != AvatarGender.Male && manifest.BodyType != AvatarGender.Female)
      {
        Debug.Assert(false, string.Empty, "Manifest validation failed for user {0}: unknown body type {1}", (object) userId, (object) manifest.BodyType);
        return false;
      }
      ComponentInfo componentInfo = manifest.HeadComponentInfo.m_ComponentInfo;
      if (manifest.HeadComponentInfo.m_ComponentInfo.AssetId != Avatar.guidHead)
      {
        Debug.Assert(false, string.Empty, "Manifest validation failed for user {0}: unknown head type {1}", (object) userId);
        return false;
      }
      for (int index = 0; index < manifest.m_PreviousRequiredComponentInfo.Length; ++index)
      {
        if (manifest.m_PreviousRequiredComponentInfo[index].ComponentMask != Avatar.requiredComponents[index])
        {
          Debug.Assert(false, string.Empty, "Manifest validation failed for user {0}: PreviousRequiredComponentInfo entry {1} is incorrect type {2}", (object) userId, (object) index, (object) manifest.m_PreviousRequiredComponentInfo[index].ComponentMask);
          return false;
        }
      }
      bool flag = false;
      uint num = 0;
      foreach (ComponentInfo component in manifest.GetComponents(AvatarComponentMasks.All))
      {
        if (flag && component.AssetId != Guid.Empty)
        {
          Debug.Assert(false, string.Empty, "Manifest validation failed for user {0}: ComponentInfo array is not packed properly", (object) userId);
          return false;
        }
        if (!flag)
        {
          if (component.AssetId == Guid.Empty)
          {
            flag = true;
          }
          else
          {
            if (((AvatarComponentMasks) num & component.ComponentMask) != AvatarComponentMasks.None)
            {
              Debug.Assert(false, string.Empty, "Manifest validation failed for user {0}: Multiple components present with type {1}", (object) userId, (object) ((AvatarComponentMasks) num & component.ComponentMask));
              return false;
            }
            num = (uint) ((AvatarComponentMasks) num | component.ComponentMask);
          }
        }
      }
      int index1 = -1;
      for (int index2 = 0; index2 < Avatar.requiredComponents.Length; ++index2)
      {
        uint requiredComponent = (uint) Avatar.requiredComponents[index2];
        if (((int) num & (int) requiredComponent) != (int) requiredComponent)
        {
          index1 = index2;
          break;
        }
      }
      if (index1 != -1)
      {
        Debug.Assert(false, string.Empty, "Manifest validation failed for user {0}: Required component of type {1} is missing", (object) userId, (object) Avatar.requiredComponents[index1]);
        return false;
      }
      foreach (ComponentInfo component in manifest.GetComponents(AvatarComponentMasks.All))
      {
        if (!Avatar.ValidateAvatarComponent(userId, manifest.BodyComponentInfo.m_ComponentInfo, component))
          return false;
      }
      foreach (ComponentInfo component in manifest.m_PreviousRequiredComponentInfo)
      {
        if (!Avatar.ValidateAvatarComponent(userId, manifest.BodyComponentInfo.m_ComponentInfo, component))
          return false;
      }
      if ((double) manifest.HeightFactor < -1.0 || (double) manifest.HeightFactor > 1.0)
      {
        Debug.Assert(false, string.Empty, "Manifest validation failed for user {0}: Height factor {1} is out of bounds", (object) userId, (object) manifest.HeightFactor);
        return false;
      }
      if ((double) manifest.WidthFactor < -1.0 || (double) manifest.WidthFactor > 1.0)
      {
        Debug.Assert(false, string.Empty, "Manifest validation failed for user {0}: Weight factor {1} is out of bounds", (object) userId, (object) manifest.WidthFactor);
        return false;
      }
      return Avatar.ValidateBlendShape(userId, BlendShapeType.Chin, Avatar.XAVATAR_CATEGORY_MASK.Chin, manifest.GetBlendShape(BlendShapeType.Chin)) && Avatar.ValidateBlendShape(userId, BlendShapeType.Nose, Avatar.XAVATAR_CATEGORY_MASK.Nose, manifest.GetBlendShape(BlendShapeType.Nose)) && Avatar.ValidateBlendShape(userId, BlendShapeType.Ear, Avatar.XAVATAR_CATEGORY_MASK.Ears, manifest.GetBlendShape(BlendShapeType.Ear)) && Avatar.ValidateReplacementTexture(userId, DynamicTextureType.Mouth, Avatar.XAVATAR_CATEGORY_MASK.Mouth, manifest.GetReplacementTexture(DynamicTextureType.Mouth).m_TextureAssetId, true) && Avatar.ValidateReplacementTexture(userId, DynamicTextureType.Eye, Avatar.XAVATAR_CATEGORY_MASK.Eye, manifest.GetReplacementTexture(DynamicTextureType.Eye).m_TextureAssetId, true) && Avatar.ValidateReplacementTexture(userId, DynamicTextureType.Eyebrow, Avatar.XAVATAR_CATEGORY_MASK.EyeBrow, manifest.GetReplacementTexture(DynamicTextureType.Eyebrow).m_TextureAssetId, true) && Avatar.ValidateReplacementTexture(userId, DynamicTextureType.FacialHair, Avatar.XAVATAR_CATEGORY_MASK.FacialHair, manifest.GetReplacementTexture(DynamicTextureType.FacialHair).m_TextureAssetId, false) && Avatar.ValidateReplacementTexture(userId, DynamicTextureType.EyeShadow, Avatar.XAVATAR_CATEGORY_MASK.EyeShadow, manifest.GetReplacementTexture(DynamicTextureType.EyeShadow).m_TextureAssetId, false) && Avatar.ValidateReplacementTexture(userId, DynamicTextureType.SkinFeatures, Avatar.XAVATAR_CATEGORY_MASK.FacialOther, manifest.GetReplacementTexture(DynamicTextureType.SkinFeatures).m_TextureAssetId, false) && Avatar.ValidateAssetOwnership(userId, (AvatarManifest) manifest);
    }

    private static bool ValidateAssetOwnership(ulong userId, AvatarManifest manifest)
    {
      List<object> objectList1 = new List<object>();
      List<object> objectList2 = new List<object>();
      foreach (ComponentInfo component in manifest.GetComponents(AvatarComponentMasks.All))
      {
        switch (AssetLoader.GetAssetGuidType(component.AssetId))
        {
          case AssetGuidType.TOC:
            continue;
          case AssetGuidType.Awardable:
            objectList1.Add((object) component.AssetId);
            continue;
          case AssetGuidType.MarketPlace:
            objectList2.Add((object) component.AssetId);
            continue;
          default:
            Debug.Assert(false, string.Empty, "Manifest validation failed for user {0}: invalid avatar asset id {1}. Unknown version {2}", (object) userId, (object) component.AssetId);
            return false;
        }
      }
      foreach (ComponentInfo componentInfo in ((AvatarManifestV1) manifest).m_PreviousRequiredComponentInfo)
      {
        switch (AssetLoader.GetAssetGuidType(componentInfo.AssetId))
        {
          case AssetGuidType.TOC:
            continue;
          case AssetGuidType.Awardable:
            objectList1.Add((object) componentInfo.AssetId);
            continue;
          case AssetGuidType.MarketPlace:
            objectList2.Add((object) componentInfo.AssetId);
            continue;
          default:
            Debug.Assert(false, string.Empty, "Manifest validation failed for user {0}: invalid avatar asset id {1}. Unknown version {2}", (object) userId, (object) componentInfo.AssetId);
            return false;
        }
      }
      return true;
    }

    private static bool ValidateAvatarComponent(
      ulong userId,
      ComponentInfo body,
      ComponentInfo component)
    {
      if (body.AssetId == Avatar.guidMaleBody && AssetLoader.GetAssetBodyType(component.AssetId) == AvatarGender.Female || body.AssetId == Avatar.guidFemaleBody && AssetLoader.GetAssetBodyType(component.AssetId) == AvatarGender.Male)
      {
        Debug.Assert(false, string.Empty, "Manifest validation failed for user {0}: avatar is wearing component {1} for the wrong body type", (object) userId, (object) component.AssetId);
        return false;
      }
      if ((component.ComponentMask & AvatarComponentMasks.Body) != AvatarComponentMasks.None)
      {
        Debug.Assert(false, string.Empty, "Manifest validation failed for user {0}: avatar has a body component equipped in a clothing slot", (object) userId);
        return false;
      }
      if ((component.ComponentMask & AvatarComponentMasks.Head) != AvatarComponentMasks.None)
      {
        Debug.Assert(false, string.Empty, "Manifest validation failed for user {0}: avatar has a head component equipped in a clothing slot", (object) userId);
        return false;
      }
      if (!Avatar.guestGuids.ContainsKey(component.AssetId))
        return true;
      Debug.Assert(false, string.Empty, "Manifest validation failed for user {0}: avatar is wearing guest shirt", (object) userId);
      return false;
    }

    private static bool ValidateBlendShape(
      ulong userId,
      BlendShapeType shape,
      Avatar.XAVATAR_CATEGORY_MASK expectedMask,
      Guid asset)
    {
      if (AssetLoader.GetComponentTypeFromAssetId(asset) == (ComponentCategories) expectedMask)
        return true;
      Debug.Assert(false, string.Empty, "Manifest validation failed for user {0}: blend shape {1} in {2} slot is incorrect type.", (object) userId, (object) asset, (object) shape);
      return false;
    }

    private static bool ValidateReplacementTexture(
      ulong userId,
      DynamicTextureType texture,
      Avatar.XAVATAR_CATEGORY_MASK expectedMask,
      Guid asset,
      bool required)
    {
      if (AssetLoader.GetComponentTypeFromAssetId(asset) == (ComponentCategories) expectedMask || !required && !(asset != Guid.Empty))
        return true;
      Debug.Assert(false, string.Empty, "Manifest validation failed for user {0}: replacement texture {1} in {2} slot is incorrect type.", (object) userId, (object) asset, (object) texture);
      return false;
    }

    [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Datatype loses meaning outside of static data class")]
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1602:EnumerationItemsMustBeDocumented", Justification = "Self-documenting")]
    private enum XAVATAR_CATEGORY_MASK
    {
      Eye = 8192, // 0x00002000
      EyeBrow = 16384, // 0x00004000
      Mouth = 32768, // 0x00008000
      FacialHair = 65536, // 0x00010000
      FacialOther = 131072, // 0x00020000
      EyeShadow = 262144, // 0x00040000
      Nose = 524288, // 0x00080000
      Chin = 1048576, // 0x00100000
      Ears = 2097152, // 0x00200000
      Shape = 16777216, // 0x01000000
    }
  }
}
