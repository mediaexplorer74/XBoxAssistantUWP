// *********************************************************
// Type: Microsoft.Phone.Marketplace.UnauthorizedRequestException
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using Microsoft.Phone.Marketplace.Resources;
using System;


namespace Microsoft.Phone.Marketplace
{
  public class UnauthorizedRequestException : ServiceFailureException
  {
    public UnauthorizedRequestException() => this.Bucket = ServiceErrorBucket.InputArguments;

    public UnauthorizedRequestException(string message)
      : base(message)
    {
      this.Bucket = ServiceErrorBucket.InputArguments;
    }

    public UnauthorizedRequestException(string message, Exception innerException)
      : base(message, innerException)
    {
      this.Bucket = ServiceErrorBucket.InputArguments;
    }

    internal override string GetErrorMessageDescription()
    {
      return string.Format(UIResources.ExceptionMessage_UnauthorizedRequest, (object) this.ErrorCode);
    }
  }
}
