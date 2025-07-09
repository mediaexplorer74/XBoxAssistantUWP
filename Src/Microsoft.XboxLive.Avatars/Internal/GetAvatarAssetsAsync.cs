// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.GetAvatarAssetsAsync
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;


namespace Microsoft.XboxLive.Avatars.Internal
{
  internal class GetAvatarAssetsAsync
  {
    private AssetLoader m_Api;
    private AvatarManifest m_Manifest;
    private AvatarComponentMasks m_ComponentMask;
    private EventHandler<GetAvatarAssetsEventArgs> m_EventHandler;

    public GetAvatarAssetsAsync(
      AssetLoader api,
      AvatarManifest manifest,
      AvatarComponentMasks componentMask,
      EventHandler<GetAvatarAssetsEventArgs> eventHandler)
    {
      this.m_Api = api;
      this.m_Manifest = manifest;
      this.m_ComponentMask = componentMask;
      this.m_EventHandler = eventHandler;
    }

    public void Process()
    {
      GetAvatarAssetsEventArgs e = new GetAvatarAssetsEventArgs();
      try
      {
        e.Avatar = this.m_Api.CreateAvatar(this.m_Manifest, this.m_ComponentMask);
      }
      catch (AvatarException ex)
      {
        e.Exception = ex;
      }
      this.m_EventHandler((object) this.m_Manifest, e);
    }
  }
}
