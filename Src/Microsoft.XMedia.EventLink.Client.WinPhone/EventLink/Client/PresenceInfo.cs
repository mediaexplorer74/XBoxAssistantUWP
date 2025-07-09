// *********************************************************
// Type: Microsoft.XMedia.EventLink.Client.PresenceInfo
// Assembly: Microsoft.XMedia.EventLink.Client.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946994A4-3A3C-41D3-A520-1292D57CD5ED
// *********************************************************Microsoft.XMedia.EventLink.Client.WinPhone.dll

using System.Collections.Generic;


namespace Microsoft.XMedia.EventLink.Client
{
  public class PresenceInfo
  {
    public uint ClientId { get; internal set; }

    public DeviceType DeviceType { get; internal set; }

    public IEnumerable<string> UserDisplayNames { get; internal set; }
  }
}
