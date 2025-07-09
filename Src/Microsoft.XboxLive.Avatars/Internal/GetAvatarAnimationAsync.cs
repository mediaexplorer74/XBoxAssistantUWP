// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.GetAvatarAnimationAsync
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;


namespace Microsoft.XboxLive.Avatars.Internal
{
  internal class GetAvatarAnimationAsync
  {
    private AssetLoader m_Api;
    private Guid m_AnimationId;
    private EventHandler<GetAvatarAnimationEventArgs> m_EventHandler;

    public GetAvatarAnimationAsync(
      AssetLoader api,
      Guid animationId,
      EventHandler<GetAvatarAnimationEventArgs> eventHandler)
    {
      this.m_Api = api;
      this.m_AnimationId = animationId;
      this.m_EventHandler = eventHandler;
    }

    public void Process()
    {
      GetAvatarAnimationEventArgs e = new GetAvatarAnimationEventArgs();
      try
      {
        e.Animation = this.m_Api.GetAvatarAnimation(this.m_AnimationId);
      }
      catch (AvatarException ex)
      {
        e.Exception = ex;
      }
      this.m_EventHandler((object) this.m_AnimationId, e);
    }
  }
}
