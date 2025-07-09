// *********************************************************
// Type: Microsoft.Xmedia.Client.WindowsPhone.IXmediaService
// Assembly: Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2E3A1F77-365B-4EB2-85E1-D467924E2195
// *********************************************************Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy.dll

using System;
using System.Collections.Generic;


namespace Microsoft.Xmedia.Client.WindowsPhone
{
  public interface IXmediaService
  {
    IAsyncResult BeginCompanionLogin(AsyncCallback callback, object asyncState);

    void EndCompanionLogin(IAsyncResult ar);

    IAsyncResult BeginCompanionLogout(AsyncCallback callback, object asyncState);

    void EndCompanionLogout(IAsyncResult ar);

    IAsyncResult BeginGetPairedConsoles(AsyncCallback callback, object asyncState);

    IEnumerable<DeviceInfo> EndGetPairedConsoles(IAsyncResult ar);

    IAsyncResult BeginGetUserSessions(AsyncCallback callback, object asyncState);

    IEnumerable<SessionInfo> EndGetUserSessions(IAsyncResult ar);

    IAsyncResult BeginGetUserInfo(string userId, AsyncCallback callback, object asyncState);

    UserInfo EndGetUserInfo(IAsyncResult ar);

    IAsyncResult BeginGetDeviceInfo(string deviceId, AsyncCallback callback, object asyncState);

    DeviceInfo EndGetDeviceInfo(IAsyncResult ar);

    IAsyncResult BeginConnectToSession(
      SessionInfo sessionInfo,
      AsyncCallback callback,
      object asyncState);

    XmediaSession EndConnectToSession(IAsyncResult ar);
  }
}
