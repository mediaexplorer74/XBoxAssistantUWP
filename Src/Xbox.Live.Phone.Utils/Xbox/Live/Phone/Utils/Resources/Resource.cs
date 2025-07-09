// *********************************************************
// Type: Xbox.Live.Phone.Utils.Resources.Resource
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;


namespace Xbox.Live.Phone.Utils.Resources
{
  [CompilerGenerated]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  internal class Resource
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    internal Resource()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) Resource.resourceMan, (object) null))
          Resource.resourceMan = new ResourceManager("Xbox.Live.Phone.Utils.Resources.Resource", typeof (Resource).Assembly);
        return Resource.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => Resource.resourceCulture;
      set => Resource.resourceCulture = value;
    }

    internal static string AvatarFailedToDownloadAssets
    {
      get
      {
        return Resource.ResourceManager.GetString(nameof (AvatarFailedToDownloadAssets), Resource.resourceCulture);
      }
    }

    internal static string AvatarFailedToDownloadPropAnimation
    {
      get
      {
        return Resource.ResourceManager.GetString(nameof (AvatarFailedToDownloadPropAnimation), Resource.resourceCulture);
      }
    }

    internal static string AvatarInvalidManifest
    {
      get
      {
        return Resource.ResourceManager.GetString(nameof (AvatarInvalidManifest), Resource.resourceCulture);
      }
    }

    internal static string CancelButton_Text
    {
      get
      {
        return Resource.ResourceManager.GetString(nameof (CancelButton_Text), Resource.resourceCulture);
      }
    }

    internal static string Error_ErrorMessage_Title
    {
      get
      {
        return Resource.ResourceManager.GetString(nameof (Error_ErrorMessage_Title), Resource.resourceCulture);
      }
    }

    internal static string Error_StandardException_Title
    {
      get
      {
        return Resource.ResourceManager.GetString(nameof (Error_StandardException_Title), Resource.resourceCulture);
      }
    }

    internal static string ErrorNonfatalError
    {
      get
      {
        return Resource.ResourceManager.GetString(nameof (ErrorNonfatalError), Resource.resourceCulture);
      }
    }

    internal static string OkButton_Text
    {
      get => Resource.ResourceManager.GetString(nameof (OkButton_Text), Resource.resourceCulture);
    }

    internal static string ProgrammingError_InvalidTypeOrAction
    {
      get
      {
        return Resource.ResourceManager.GetString(nameof (ProgrammingError_InvalidTypeOrAction), Resource.resourceCulture);
      }
    }

    internal static string ProgrammingError_NoErrorStringInResource
    {
      get
      {
        return Resource.ResourceManager.GetString(nameof (ProgrammingError_NoErrorStringInResource), Resource.resourceCulture);
      }
    }
  }
}
