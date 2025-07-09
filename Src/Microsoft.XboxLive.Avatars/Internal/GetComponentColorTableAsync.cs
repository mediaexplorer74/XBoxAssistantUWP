// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.GetComponentColorTableAsync
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;


namespace Microsoft.XboxLive.Avatars.Internal
{
  internal class GetComponentColorTableAsync
  {
    private AssetLoader m_Api;
    private Guid m_ComponentId;
    private EventHandler<GetComponentColorTableEventArgs> m_EventHandler;

    public GetComponentColorTableAsync(
      AssetLoader api,
      Guid componentId,
      EventHandler<GetComponentColorTableEventArgs> eventHandler)
    {
      this.m_Api = api;
      this.m_ComponentId = componentId;
      this.m_EventHandler = eventHandler;
    }

    public void Process()
    {
      GetComponentColorTableEventArgs e = new GetComponentColorTableEventArgs();
      try
      {
        e.AssetId = this.m_ComponentId;
        e.m_CustomColors = this.m_Api.GetComponentColorTable(this.m_ComponentId);
      }
      catch (AvatarException ex)
      {
        e.Exception = ex;
      }
      this.m_EventHandler((object) this.m_ComponentId, e);
    }
  }
}
