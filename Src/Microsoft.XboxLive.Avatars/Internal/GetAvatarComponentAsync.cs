// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.GetAvatarComponentAsync
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.MathUtilities;
using System;


namespace Microsoft.XboxLive.Avatars.Internal
{
  internal class GetAvatarComponentAsync
  {
    private AssetLoader m_Api;
    private Guid m_ComponentId;
    private EventHandler<GetAvatarComponentEventArgs> m_EventHandler;
    private Colorb[] m_CustomColors;

    public GetAvatarComponentAsync(
      AssetLoader api,
      Guid componentId,
      Colorb[] customColors,
      EventHandler<GetAvatarComponentEventArgs> eventHandler)
    {
      this.m_Api = api;
      this.m_ComponentId = componentId;
      this.m_EventHandler = eventHandler;
      if (customColors == null)
        return;
      this.m_CustomColors = new Colorb[customColors.Length];
      customColors.CopyTo((Array) this.m_CustomColors, 0);
    }

    public void Process()
    {
      GetAvatarComponentEventArgs e = new GetAvatarComponentEventArgs();
      try
      {
        e.Component = this.m_Api.GetAvatarComponent(this.m_ComponentId, this.m_CustomColors);
      }
      catch (AvatarException ex)
      {
        e.Exception = ex;
      }
      this.m_EventHandler((object) this.m_ComponentId, e);
    }
  }
}
