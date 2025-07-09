// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.SyncDownloadContext
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;
using System.IO;
using System.Threading;


namespace Microsoft.XboxLive.Avatars.Internal
{
  internal class SyncDownloadContext : IDisposable
  {
    public AutoResetEvent syncEvent = new AutoResetEvent(false);
    public Stream stream;
    public Exception error;

    internal Stream DetachStream()
    {
      Stream stream = this.stream;
      this.stream = (Stream) null;
      return stream;
    }

    private void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      this.syncEvent.Close();
      if (this.stream == null)
        return;
      this.stream.Dispose();
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }
  }
}
