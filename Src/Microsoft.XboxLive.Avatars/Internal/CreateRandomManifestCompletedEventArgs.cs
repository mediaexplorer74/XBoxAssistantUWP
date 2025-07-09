// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.CreateRandomManifestCompletedEventArgs
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;
using System.ComponentModel;


namespace Microsoft.XboxLive.Avatars.Internal
{
  public class CreateRandomManifestCompletedEventArgs : AsyncCompletedEventArgs
  {
    private AvatarManifest[] m_randomManifests;

    public CreateRandomManifestCompletedEventArgs(Exception e, AvatarManifest[] randomManifests)
      : base(e, false, (object) null)
    {
      this.m_randomManifests = randomManifests;
    }

    public AvatarManifest[] RandomManifests
    {
      get
      {
        this.RaiseExceptionIfNecessary();
        return this.m_randomManifests;
      }
    }
  }
}
