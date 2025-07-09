// *********************************************************
// Type: Microsoft.XMedia.ServiceMessages
// Assembly: Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2E3A1F77-365B-4EB2-85E1-D467924E2195
// *********************************************************Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy.dll

using System;
using System.Globalization;


namespace Microsoft.XMedia
{
  public static class ServiceMessages
  {
    internal static ServiceMessage[] Messages = new ServiceMessage[36]
    {
      new ServiceMessage(true, ""),
      new ServiceMessage(true, "Unexpected error: {0}"),
      new ServiceMessage(true, "Exception: {0}"),
      new ServiceMessage(true, "Incorrect token version {0} found"),
      new ServiceMessage(true, "Expired date {0} found"),
      new ServiceMessage(true, "Error decoding from base64 encoded text: {0}"),
      new ServiceMessage(true, "Error while deserializing the security token: {0}"),
      new ServiceMessage(true, "Too big claim Id (value: {0}) to fit during Serialize"),
      new ServiceMessage(true, "Missing or empty authorization header"),
      new ServiceMessage(true, "Null or missing content in security token"),
      new ServiceMessage(true, "Missing content in SAML token: {0}"),
      new ServiceMessage(true, "Claim Id is less than 0"),
      new ServiceMessage(true, "Claim value is null or empty"),
      new ServiceMessage(true, "Input data length is too small"),
      new ServiceMessage(true, "Incorrect block size"),
      new ServiceMessage(true, "Hash verification failure"),
      new ServiceMessage(true, "Data too small"),
      new ServiceMessage(true, "Token's device Id does not match the device Id from client certificate"),
      new ServiceMessage(true, "Token's issuer does not match the expected value"),
      new ServiceMessage(true, "Found Machine Id or Device Id for Mobile type"),
      new ServiceMessage(true, "Incorrect SAML statement count"),
      new ServiceMessage(true, "Incorrect SAML statement type"),
      new ServiceMessage(true, "Incorrect SAML version"),
      new ServiceMessage(true, "There was a problem retrieving user info"),
      new ServiceMessage(true, "Too many {0} requests made by {1}"),
      new ServiceMessage(true, "Invalid or missing {0} header"),
      new ServiceMessage(true, "Incorrect service-to-service auth text"),
      new ServiceMessage(false, "The maximum number of clients are already connected to the session"),
      new ServiceMessage(false, "The maximum number of pending messages are in the queue"),
      new ServiceMessage(false, "The companion is not signed in"),
      new ServiceMessage(false, "The companion is in the process of signing in"),
      new ServiceMessage(true, "The format of IP address is not valid"),
      new ServiceMessage(true, "Refresh has been called too many times"),
      new ServiceMessage(true, "The device type does not match the signin type"),
      new ServiceMessage(true, "UpdateOrCreateSession failed"),
      new ServiceMessage(false, "No such session exists")
    };

    public static ServiceMessage GetMessage(ErrorCode errorCode)
    {
      return ServiceMessages.Messages[(int) errorCode];
    }

    public static string GetUserMessage(ErrorCode errorCode, params object[] args)
    {
      return ServiceMessages.GetMessageInternal(errorCode, false, args);
    }

    public static string GetServiceMessage(ErrorCode errorCode, params object[] args)
    {
      return ServiceMessages.GetMessageInternal(errorCode, true, args);
    }

    private static string GetMessageInternal(
      ErrorCode errorCode,
      bool includeInternalMessage,
      params object[] args)
    {
      string messageInternal = "";
      try
      {
        ServiceMessage message = ServiceMessages.GetMessage(errorCode);
        if (message.IsInternalMessage)
        {
          if (!includeInternalMessage)
            goto label_6;
        }
        messageInternal = string.Format((IFormatProvider) CultureInfo.InvariantCulture, message.ErrorMessageFormat, args);
      }
      catch (FormatException ex)
      {
        if (includeInternalMessage)
          messageInternal = ex.ToString();
      }
label_6:
      return messageInternal;
    }
  }
}
