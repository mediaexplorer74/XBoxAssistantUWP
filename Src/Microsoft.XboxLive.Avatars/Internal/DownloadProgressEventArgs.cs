// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.DownloadProgressEventArgs
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;


namespace Microsoft.XboxLive.Avatars.Internal
{
  public class DownloadProgressEventArgs : EventArgs
  {
    public int TotalNumberOfFilesToDownload { get; set; }

    public int NumberOfDownloadingFiles { get; set; }

    public int ProgressPercentage { get; set; }

    public long BytesReceived { get; set; }

    public long TotalBytesToReceive { get; set; }
  }
}
