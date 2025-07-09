// *********************************************************
// Type: Microsoft.Xmedia.Client.WindowsPhone.DeviceChangedEventArgs
// Assembly: Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2E3A1F77-365B-4EB2-85E1-D467924E2195
// *********************************************************Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy.dll

using Microsoft.XMedia;
using System;
using System.Collections.Generic;


namespace Microsoft.Xmedia.Client.WindowsPhone
{
  public class DeviceChangedEventArgs : EventArgs
  {
    internal DeviceChangedEventArgs(
      uint clientId,
      DeviceType deviceType,
      IEnumerable<string> userDisplayNames)
    {
      this.ClientId = clientId;
      this.DeviceType = deviceType;
      this.UserDisplayNames = (IEnumerable<string>) new List<string>(userDisplayNames);
    }

    public uint ClientId { get; internal set; }

    public DeviceType DeviceType { get; internal set; }

    public IEnumerable<string> UserDisplayNames { get; internal set; }
  }
}
