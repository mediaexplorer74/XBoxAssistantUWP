// *********************************************************
// Type: Microsoft.Phone.Marketplace.TokenNotRedeemableException
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using Microsoft.Phone.Marketplace.Resources;
using System;


namespace Microsoft.Phone.Marketplace
{
  public class TokenNotRedeemableException : ServiceFailureException
  {
    public TokenNotRedeemableException()
    {
    }

    public TokenNotRedeemableException(string message)
      : base(message)
    {
    }

    public TokenNotRedeemableException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    internal override string GetErrorMessageDescription()
    {
      return string.Format(AvatarUIResources.ExceptionMessage_AvatarTokenNotRedeemable, (object) this.ErrorCode);
    }
  }
}
