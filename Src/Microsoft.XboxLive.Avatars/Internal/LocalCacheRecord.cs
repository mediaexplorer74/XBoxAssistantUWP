// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.LocalCacheRecord
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll


namespace Microsoft.XboxLive.Avatars.Internal
{
  internal class LocalCacheRecord
  {
    public LocalCacheRecord(long fileSize, long lastAccessTime)
    {
      this.FileSize = fileSize;
      this.LastAccessTime = lastAccessTime;
    }

    public long FileSize { get; set; }

    public long LastAccessTime { get; set; }
  }
}
