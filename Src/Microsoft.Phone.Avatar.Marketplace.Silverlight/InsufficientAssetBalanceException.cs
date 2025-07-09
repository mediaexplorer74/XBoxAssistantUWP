// *********************************************************
// Type: Microsoft.Phone.Marketplace.InsufficientAssetBalanceException
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;


namespace Microsoft.Phone.Marketplace
{
  public class InsufficientAssetBalanceException : ServiceFailureException
  {
    public InsufficientAssetBalanceException()
    {
    }

    public InsufficientAssetBalanceException(string message)
      : base(message)
    {
    }

    public InsufficientAssetBalanceException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
