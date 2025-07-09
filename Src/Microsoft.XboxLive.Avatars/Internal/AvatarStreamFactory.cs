// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.AvatarStreamFactory
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal
{
  internal abstract class AvatarStreamFactory
  {
    public abstract Stream CreateStream(Guid avatarAsset);
  }
}
