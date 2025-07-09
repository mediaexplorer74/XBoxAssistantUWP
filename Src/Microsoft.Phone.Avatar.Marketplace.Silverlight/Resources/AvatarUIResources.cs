// *********************************************************
// Type: Microsoft.Phone.Marketplace.Resources.AvatarUIResources
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;


namespace Microsoft.Phone.Marketplace.Resources
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class AvatarUIResources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal AvatarUIResources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) AvatarUIResources.resourceMan, (object) null))
          AvatarUIResources.resourceMan = new ResourceManager("Microsoft.Phone.Avatar.Marketplace.Silverlight.Resources.AvatarUIResources", typeof (AvatarUIResources).Assembly);
        return AvatarUIResources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => AvatarUIResources.resourceCulture;
      set => AvatarUIResources.resourceCulture = value;
    }

    internal static string AvatarRedeemInputSubtitle
    {
      get
      {
        return AvatarUIResources.ResourceManager.GetString(nameof (AvatarRedeemInputSubtitle), AvatarUIResources.resourceCulture);
      }
    }

    internal static string BodyTypeFemale
    {
      get
      {
        return AvatarUIResources.ResourceManager.GetString(nameof (BodyTypeFemale), AvatarUIResources.resourceCulture);
      }
    }

    internal static string BodyTypeMale
    {
      get
      {
        return AvatarUIResources.ResourceManager.GetString(nameof (BodyTypeMale), AvatarUIResources.resourceCulture);
      }
    }

    internal static string ExceptionMessage_AvatarTokenNotRedeemable
    {
      get
      {
        return AvatarUIResources.ResourceManager.GetString(nameof (ExceptionMessage_AvatarTokenNotRedeemable), AvatarUIResources.resourceCulture);
      }
    }

    internal static string PurchaseLegalText
    {
      get
      {
        return AvatarUIResources.ResourceManager.GetString(nameof (PurchaseLegalText), AvatarUIResources.resourceCulture);
      }
    }

    internal static string RedeemCodeBodyTypeNotMatch
    {
      get
      {
        return AvatarUIResources.ResourceManager.GetString(nameof (RedeemCodeBodyTypeNotMatch), AvatarUIResources.resourceCulture);
      }
    }
  }
}
