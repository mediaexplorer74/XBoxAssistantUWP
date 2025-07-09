// *********************************************************
// Type: Microsoft.Phone.Marketplace.AccountStatusException
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using Microsoft.Phone.Marketplace.Resources;
using System;


namespace Microsoft.Phone.Marketplace
{
  public class AccountStatusException : ServiceFailureException
  {
    public AccountStatusException()
    {
    }

    public AccountStatusException(string message)
      : base(message)
    {
    }

    public AccountStatusException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    internal override string GetErrorMessageDescription()
    {
      return string.Format(UIResources.ExceptionMessage_AccountStatus, (object) this.ErrorCode);
    }
  }
}
