// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.DataRequest
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll


namespace Microsoft.XboxLive.Avatars.Internal
{
  public abstract class DataRequest
  {
    protected DataRequest(
      DataProvider dataSource,
      DownloadRequestEventHandler handler,
      object context)
    {
      this.Handler = handler;
      this.Context = context;
      this.DataSource = dataSource;
      this.CacheWriteDisabled = false;
    }

    protected DataRequest(
      DataProvider dataSource,
      DownloadRequestEventHandler handler,
      object context,
      bool disableCacheWrite)
    {
      this.Handler = handler;
      this.Context = context;
      this.DataSource = dataSource;
      this.CacheWriteDisabled = disableCacheWrite;
    }

    public DataProvider DataSource { get; private set; }

    public DownloadRequestEventHandler Handler { get; private set; }

    public object Context { get; private set; }

    public bool CacheWriteDisabled { get; private set; }

    public virtual void Clear()
    {
      this.Handler = (DownloadRequestEventHandler) null;
      this.Context = (object) null;
      this.DataSource = (DataProvider) null;
      this.CacheWriteDisabled = false;
    }
  }
}
