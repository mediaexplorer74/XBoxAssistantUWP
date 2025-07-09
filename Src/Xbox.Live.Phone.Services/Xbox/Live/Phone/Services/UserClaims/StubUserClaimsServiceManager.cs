// *********************************************************
// Type: Xbox.Live.Phone.Services.UserClaims.StubUserClaimsServiceManager
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Xbox.Live.Phone.Utils;


namespace Xbox.Live.Phone.Services.UserClaims
{
  public sealed class StubUserClaimsServiceManager : IUserClaimsServiceManager
  {
    public event EventHandler<ServiceProxyEventArgs<UserInfo>> EventGetUserInfoCompleted;

    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "environmentName", Justification = "This is stub")]
    public void Initialize(ServiceCommon.Environment environmentName)
    {
    }

    public void GetUserInfo()
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        UserInfo userInfo = UserClaimsResponseParser.ParseGetUserInfoResponse(ServiceCommon.ReadResource("Xbox.Live.Phone.Services.StubData.UserInfo.txt"));
        Thread.Sleep(2000);
        ThreadManager.UIThreadPost((SendOrPostCallback) delegate
        {
          EventHandler<ServiceProxyEventArgs<UserInfo>> userInfoCompleted = this.EventGetUserInfoCompleted;
          if (userInfoCompleted == null)
            return;
          userInfoCompleted((object) this, new ServiceProxyEventArgs<UserInfo>((object) userInfo, (Exception) null, false, (object) null));
        }, (object) this);
      });
    }
  }
}
