// *********************************************************
// Type: LRC.Service.IZuneCatalogServiceManager
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll

using System;


namespace LRC.Service
{
  public interface IZuneCatalogServiceManager
  {
    event EventHandler<ServiceProxyEventArgs<string>> EventGetFeedCompleted;

    event EventHandler<ServiceProxyEventArgs<string>> EventGetVideoItemCompleted;

    event EventHandler<ServiceProxyEventArgs<string>> EventGetMediaItemTypeCompleted;

    void GetFeedList(string path, object userState);

    void GetMediaDetail(string mediaId, string mediaType, object userState);

    void GetMediaItemType(string mediaId, object userState);
  }
}
