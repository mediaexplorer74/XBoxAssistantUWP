// *********************************************************
// Type: Microsoft.XMedia.ServiceMessage
// Assembly: Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2E3A1F77-365B-4EB2-85E1-D467924E2195
// *********************************************************Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy.dll


namespace Microsoft.XMedia
{
  public class ServiceMessage
  {
    public bool IsInternalMessage { get; private set; }

    public string ErrorMessageFormat { get; private set; }

    public ServiceMessage(bool isInternalMessage, string errorMessageFormat)
    {
      this.IsInternalMessage = isInternalMessage;
      this.ErrorMessageFormat = errorMessageFormat;
    }
  }
}
