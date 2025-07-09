// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.GetAvatarComponentEventArgs
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;
using System;


namespace Microsoft.XboxLive.Avatars.Internal
{
  public class GetAvatarComponentEventArgs : EventArgs
  {
    public AvatarComponent Component { get; set; }

    public AvatarException Exception { get; set; }
  }
}
