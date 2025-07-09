// *********************************************************
// Type: Xbox.Live.Phone.Services.Facebook.IFacebookServiceManager
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Leet.MessageService.DataContracts;
using System;


namespace Xbox.Live.Phone.Services.Facebook
{
  public interface IFacebookServiceManager
  {
    event EventHandler<ServiceProxyEventArgs<ResponseWithErrorCode>> EventPostLinkCompleted;

    event EventHandler<ServiceProxyEventArgs<ResponseWithErrorCode>> EventPostCredentialsCompleted;

    event EventHandler<ServiceProxyEventArgs<ResponseWithErrorCode>> EventPostPermissionsCompleted;

    void Initialize(ServiceCommon.Environment environmentName);

    void PostLink(
      uint phoneAppTitleId,
      string message,
      string caption,
      string linkDescription,
      string linkName,
      string linkImage,
      string linkUrl,
      string actionName,
      string actionUrl);

    void PostCredentials(string username, string password, bool allowFacebookInfoOnXboxLive);

    void PostPermissions(uint phoneAppTitleId, bool allow);
  }
}
