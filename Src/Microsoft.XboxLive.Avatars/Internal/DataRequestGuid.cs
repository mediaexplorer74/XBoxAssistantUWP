// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.DataRequestGuid
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;


namespace Microsoft.XboxLive.Avatars.Internal
{
  public class DataRequestGuid : DataRequest
  {
    public DataRequestGuid(
      Guid dataId,
      DataProvider dataSource,
      DownloadRequestEventHandler handler,
      object context)
      : base(dataSource, handler, context)
    {
      this.DataId = dataId;
    }

    public DataRequestGuid(
      Guid dataId,
      DataProvider dataSource,
      DownloadRequestEventHandler handler,
      object context,
      bool disableCacheWrite)
      : base(dataSource, handler, context, disableCacheWrite)
    {
      this.DataId = dataId;
    }

    public override void Clear() => base.Clear();

    public Guid DataId { get; private set; }
  }
}
