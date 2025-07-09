// *********************************************************
// Type: Xbox.Live.Phone.Services.IFriendServiceManager
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;


namespace Xbox.Live.Phone.Services
{
  public interface IFriendServiceManager
  {
    event EventHandler<ServiceProxyEventArgs<string>> EventAddFriendCompleted;

    event EventHandler<ServiceProxyEventArgs<string>> EventRemoveFriendCompleted;

    event EventHandler<ServiceProxyEventArgs<string>> EventAcceptFriendRequestCompleted;

    event EventHandler<ServiceProxyEventArgs<string>> EventDeclineFriendRequestCompleted;

    event EventHandler<ServiceProxyEventArgs<string>> EventCancelFriendRequestCompleted;

    void Initialize(ServiceCommon.Environment environmentName);

    void AddFriendAsync(string gamerTag);

    void RemoveFriendAsync(string gamerTag);

    void AcceptFriendRequestAsync(string gamerTag);

    void DeclineFriendRequestAsync(string gamerTag);

    void CancelFriendRequestAsync(string gamerTag);
  }
}
