// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.GetAvatarAnimationEventArgs
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Animations;
using System;


namespace Microsoft.XboxLive.Avatars.Internal
{
  public class GetAvatarAnimationEventArgs : EventArgs
  {
    public AvatarAnimation Animation { get; set; }

    public AvatarException Exception { get; set; }
  }
}
