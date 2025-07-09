// *********************************************************
// Type: Microsoft.Xmedia.Client.WindowsPhone.IXmediaSession
// Assembly: Microsoft.Xmedia.Client.WindowsPhone.Interface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B6D5468-129C-4117-AE93-587315C3ADB1
// *********************************************************Microsoft.Xmedia.Client.WindowsPhone.Interface.dll

using System;


namespace Microsoft.Xmedia.Client.WindowsPhone
{
  public interface IXmediaSession
  {
    event EventHandler<MediaStateUpdatedEventArgs> MediaStateUpdated;

    event EventHandler<TitleChangedEventArgs> TitleChanged;

    event EventHandler<MessageReceivedEventArgs> MessageReceived;

    CommunicationCapabilities CommunicationCapabilities { get; }

    IAsyncResult BeginJoinSession(AsyncCallback callback, object asyncState);

    void EndJoinSession(IAsyncResult ar);

    IAsyncResult BeginLeaveSession(AsyncCallback callback, object asyncState);

    void EndLeaveSession(IAsyncResult ar);

    IAsyncResult BeginGetActiveTitleId(AsyncCallback callback, object asyncState);

    uint EndGetActiveTitleId(IAsyncResult ar);

    IAsyncResult BeginGetMediaState(AsyncCallback callback, object asyncState);

    MediaState EndGetMediaState(IAsyncResult ar);

    IAsyncResult BeginSendControlCommand(ControlKey key, AsyncCallback callback, object asyncState);

    void EndSendControlCommand(IAsyncResult ar);

    IAsyncResult BeginSendSeekControlCommand(
      ulong seekPosition,
      AsyncCallback callback,
      object asyncState);

    void EndSendSeekControlCommand(IAsyncResult ar);

    IAsyncResult BeginLaunchTitle(
      uint titleId,
      string parameter,
      AsyncCallback callback,
      object asyncState);

    void EndLaunchTitle(IAsyncResult ar);

    IAsyncResult BeginSendMessage(string message, AsyncCallback callback, object asyncState);

    void EndSendMessage(IAsyncResult ar);
  }
}
