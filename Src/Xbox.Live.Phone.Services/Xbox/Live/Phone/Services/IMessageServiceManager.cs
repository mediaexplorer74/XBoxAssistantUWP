// *********************************************************
// Type: Xbox.Live.Phone.Services.IMessageServiceManager
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Leet.MessageService.DataContracts;
using System;


namespace Xbox.Live.Phone.Services
{
  public interface IMessageServiceManager
  {
    event EventHandler<ServiceProxyEventArgs<MessageSummariesResponse>> EventGetMessageSummaryListCompleted;

    event EventHandler<ServiceProxyEventArgs<MessageDetails>> EventGetOneMessageCompleted;

    event EventHandler<ServiceProxyEventArgs<ResponseWithErrorCode>> EventDeleteMessageCompleted;

    event EventHandler<ServiceProxyEventArgs<ResponseWithErrorCode>> EventSendMessageCompleted;

    void Initialize(ServiceCommon.Environment environmentName);

    void GetMessageListAsync(string hashCode);

    void GetOneMessageAsync(uint messageId);

    void DeleteMessageAsync(uint messageId, bool blockSender);

    void SendMessageAsync(SendMessageRequest messageToSend);

    bool IsLoading();
  }
}
