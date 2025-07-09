// *********************************************************
// Type: Xbox.Live.Phone.Services.Facebook.FacebookServiceManager
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Leet.MessageService.DataContracts;
using System;
using System.Diagnostics.CodeAnalysis;


namespace Xbox.Live.Phone.Services.Facebook
{
  public sealed class FacebookServiceManager : IFacebookServiceManager
  {
    public event EventHandler<ServiceProxyEventArgs<ResponseWithErrorCode>> EventPostLinkCompleted;

    public event EventHandler<ServiceProxyEventArgs<ResponseWithErrorCode>> EventPostCredentialsCompleted;

    public event EventHandler<ServiceProxyEventArgs<ResponseWithErrorCode>> EventPostPermissionsCompleted;

    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "TODO: This is not implemented yet")]
    public void Initialize(ServiceCommon.Environment environmentName)
    {
    }

    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "TODO: This is not implemented yet")]
    public void PostLink(
      uint phoneAppTitleId,
      string message,
      string caption,
      string linkDescription,
      string linkName,
      string linkImage,
      string linkUrl,
      string actionName,
      string actionUrl)
    {
      EventHandler<ServiceProxyEventArgs<ResponseWithErrorCode>> postLinkCompleted = this.EventPostLinkCompleted;
      if (postLinkCompleted == null)
        return;
      postLinkCompleted((object) this, new ServiceProxyEventArgs<ResponseWithErrorCode>((object) null, (Exception) new NotImplementedException(), false, (object) null));
    }

    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "TODO: This is not implemented yet")]
    public void PostCredentials(string username, string password, bool allowFacebookInfoOnXboxLive)
    {
      EventHandler<ServiceProxyEventArgs<ResponseWithErrorCode>> credentialsCompleted = this.EventPostCredentialsCompleted;
      if (credentialsCompleted == null)
        return;
      credentialsCompleted((object) this, new ServiceProxyEventArgs<ResponseWithErrorCode>((object) null, (Exception) new NotImplementedException(), false, (object) null));
    }

    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "TODO: This is not implemented yet")]
    public void PostPermissions(uint phoneAppTitleId, bool allow)
    {
      EventHandler<ServiceProxyEventArgs<ResponseWithErrorCode>> permissionsCompleted = this.EventPostPermissionsCompleted;
      if (permissionsCompleted == null)
        return;
      permissionsCompleted((object) this, new ServiceProxyEventArgs<ResponseWithErrorCode>((object) null, (Exception) new NotImplementedException(), false, (object) null));
    }
  }
}
