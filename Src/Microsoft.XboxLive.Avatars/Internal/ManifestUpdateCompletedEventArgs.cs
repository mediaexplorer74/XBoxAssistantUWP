// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.ManifestUpdateCompletedEventArgs
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;
using System.ComponentModel;


namespace Microsoft.XboxLive.Avatars.Internal
{
  public class ManifestUpdateCompletedEventArgs : AsyncCompletedEventArgs
  {
    private bool m_updateStatus;

    public ManifestUpdateCompletedEventArgs(Exception e, bool updateStatus)
      : base(e, false, (object) null)
    {
      this.m_updateStatus = updateStatus;
    }

    public bool UpdateStatus
    {
      get
      {
        this.RaiseExceptionIfNecessary();
        return this.m_updateStatus;
      }
    }
  }
}
