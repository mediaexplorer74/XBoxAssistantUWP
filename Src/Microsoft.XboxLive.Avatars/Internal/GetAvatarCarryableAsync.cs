// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.GetAvatarCarryableAsync
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;
using Microsoft.XboxLive.MathUtilities;
using System;


namespace Microsoft.XboxLive.Avatars.Internal
{
  internal class GetAvatarCarryableAsync
  {
    private AssetLoader m_Api;
    private Guid m_CarryableId;
    private EventHandler<GetAvatarCarryableEventArgs> m_EventHandler;
    private Colorb[] m_CustomColors;
    private Skeleton m_AvatarSkeleton;

    public GetAvatarCarryableAsync(
      AssetLoader api,
      Guid carryableId,
      Skeleton avatarSkeleton,
      Colorb[] customColors,
      EventHandler<GetAvatarCarryableEventArgs> eventHandler)
    {
      this.m_Api = api;
      this.m_CarryableId = carryableId;
      this.m_EventHandler = eventHandler;
      this.m_AvatarSkeleton = avatarSkeleton;
      if (customColors == null)
        return;
      this.m_CustomColors = new Colorb[customColors.Length];
      customColors.CopyTo((Array) this.m_CustomColors, 0);
    }

    public void Process()
    {
      GetAvatarCarryableEventArgs e = new GetAvatarCarryableEventArgs();
      try
      {
        e.Carryable = this.m_Api.GetAvatarCarryable(this.m_CarryableId, this.m_AvatarSkeleton, this.m_CustomColors);
      }
      catch (AvatarException ex)
      {
        e.Exception = ex;
      }
      this.m_EventHandler((object) this.m_CarryableId, e);
    }
  }
}
