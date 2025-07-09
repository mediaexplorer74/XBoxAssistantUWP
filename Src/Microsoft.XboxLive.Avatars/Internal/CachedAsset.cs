// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.CachedAsset
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;


namespace Microsoft.XboxLive.Avatars.Internal
{
  internal abstract class CachedAsset
  {
    internal CoordinateSystem m_CoordinateSystem;
    internal Guid AssetId;
    internal int m_MemoryUsage;

    protected virtual int GetMemoryUsageInternal() => 64;

    public int MemoryUsage => this.m_MemoryUsage;

    public abstract AssetCacheKey AssetKey { get; }
  }
}
