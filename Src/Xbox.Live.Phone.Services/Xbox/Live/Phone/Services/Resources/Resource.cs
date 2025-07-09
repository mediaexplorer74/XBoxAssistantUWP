// *********************************************************
// Type: Xbox.Live.Phone.Services.Resources.Resource
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;


namespace Xbox.Live.Phone.Services.Resources
{
  [CompilerGenerated]
  [DebuggerNonUserCode]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
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
          Resource.resourceMan = new ResourceManager("Xbox.Live.Phone.Services.Resources.Resource", typeof (Resource).Assembly);
        return Resource.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => Resource.resourceCulture;
      set => Resource.resourceCulture = value;
    }

    internal static string ClosetReadColor
    {
      get => Resource.ResourceManager.GetString(nameof (ClosetReadColor), Resource.resourceCulture);
    }

    internal static string LastSeen_Date
    {
      get => Resource.ResourceManager.GetString(nameof (LastSeen_Date), Resource.resourceCulture);
    }

    internal static string LastSeen_Hour
    {
      get => Resource.ResourceManager.GetString(nameof (LastSeen_Hour), Resource.resourceCulture);
    }

    internal static string LastSeen_Hour_Singular
    {
      get
      {
        return Resource.ResourceManager.GetString(nameof (LastSeen_Hour_Singular), Resource.resourceCulture);
      }
    }

    internal static string LastSeen_LessThanOneMinute
    {
      get
      {
        return Resource.ResourceManager.GetString(nameof (LastSeen_LessThanOneMinute), Resource.resourceCulture);
      }
    }

    internal static string LastSeen_Minute
    {
      get => Resource.ResourceManager.GetString(nameof (LastSeen_Minute), Resource.resourceCulture);
    }

    internal static string LastSeen_Minute_Singular
    {
      get
      {
        return Resource.ResourceManager.GetString(nameof (LastSeen_Minute_Singular), Resource.resourceCulture);
      }
    }

    internal static string LastSeenPlaying_Date
    {
      get
      {
        return Resource.ResourceManager.GetString(nameof (LastSeenPlaying_Date), Resource.resourceCulture);
      }
    }

    internal static string LastSeenPlaying_Hour
    {
      get
      {
        return Resource.ResourceManager.GetString(nameof (LastSeenPlaying_Hour), Resource.resourceCulture);
      }
    }

    internal static string LastSeenPlaying_Hour_Singular
    {
      get
      {
        return Resource.ResourceManager.GetString(nameof (LastSeenPlaying_Hour_Singular), Resource.resourceCulture);
      }
    }

    internal static string LastSeenPlaying_LessThanOneMinute
    {
      get
      {
        return Resource.ResourceManager.GetString(nameof (LastSeenPlaying_LessThanOneMinute), Resource.resourceCulture);
      }
    }

    internal static string LastSeenPlaying_Minute
    {
      get
      {
        return Resource.ResourceManager.GetString(nameof (LastSeenPlaying_Minute), Resource.resourceCulture);
      }
    }

    internal static string LastSeenPlaying_Minute_Singular
    {
      get
      {
        return Resource.ResourceManager.GetString(nameof (LastSeenPlaying_Minute_Singular), Resource.resourceCulture);
      }
    }

    internal static string Message_Subject_EmptySubjectWithImage
    {
      get
      {
        return Resource.ResourceManager.GetString(nameof (Message_Subject_EmptySubjectWithImage), Resource.resourceCulture);
      }
    }

    internal static string Message_Subject_EmptySubjectWithVoice
    {
      get
      {
        return Resource.ResourceManager.GetString(nameof (Message_Subject_EmptySubjectWithVoice), Resource.resourceCulture);
      }
    }

    internal static string Message_Summary_EmptySubject
    {
      get
      {
        return Resource.ResourceManager.GetString(nameof (Message_Summary_EmptySubject), Resource.resourceCulture);
      }
    }

    internal static string Message_Summary_GameInvite
    {
      get
      {
        return Resource.ResourceManager.GetString(nameof (Message_Summary_GameInvite), Resource.resourceCulture);
      }
    }

    internal static string Message_Summary_PartyInvite
    {
      get
      {
        return Resource.ResourceManager.GetString(nameof (Message_Summary_PartyInvite), Resource.resourceCulture);
      }
    }

    internal static string Message_Summary_QuickChatInvite
    {
      get
      {
        return Resource.ResourceManager.GetString(nameof (Message_Summary_QuickChatInvite), Resource.resourceCulture);
      }
    }

    internal static string Message_Summary_VideoChatInvite
    {
      get
      {
        return Resource.ResourceManager.GetString(nameof (Message_Summary_VideoChatInvite), Resource.resourceCulture);
      }
    }

    internal static string Message_Summary_VideoMessage
    {
      get
      {
        return Resource.ResourceManager.GetString(nameof (Message_Summary_VideoMessage), Resource.resourceCulture);
      }
    }

    internal static string Playing
    {
      get => Resource.ResourceManager.GetString(nameof (Playing), Resource.resourceCulture);
    }
  }
}
