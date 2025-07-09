// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.IncreaseQuotaEventArgs
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;


namespace Microsoft.XboxLive.Avatars.Internal
{
  public class IncreaseQuotaEventArgs : EventArgs
  {
    public IncreaseQuotaEventArgs(long availableFreeSpace, long quota, long recommendedQuota)
    {
      this.AvailableFreeSpace = availableFreeSpace;
      this.Quota = quota;
      this.RecommendedQuota = recommendedQuota;
    }

    public long AvailableFreeSpace { private set; get; }

    public long Quota { private set; get; }

    public long RecommendedQuota { private set; get; }
  }
}
