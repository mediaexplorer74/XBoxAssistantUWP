// *********************************************************
// Type: Microsoft.Phone.Marketplace.ServiceFailureException
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using Microsoft.Phone.Marketplace.Resources;
using System;


namespace Microsoft.Phone.Marketplace
{
  public class ServiceFailureException : Exception
  {
    public ServiceFailureException() => this.ErrorCode = -2147467259;

    public ServiceFailureException(string message)
      : base(message)
    {
      this.ErrorCode = -2147467259;
    }

    public ServiceFailureException(string message, Exception innerException)
      : base(message, innerException)
    {
      this.ErrorCode = -2147467259;
    }

    public int ErrorCode { get; set; }

    internal ServiceErrorBucket Bucket { get; set; }

    internal virtual string GetErrorMessageTitle() => UIResources.Sorry;

    internal virtual string GetErrorMessageDescription()
    {
      return string.Format(UIResources.ExceptionMessage_TryAgainLater, (object) this.ErrorCode);
    }

    internal static ServiceFailureException GetServiceFailure(
      ServiceErrorBucket bucket,
      int errorCode,
      string message,
      Exception innerException)
    {
      ServiceFailureException serviceFailure;
      switch (bucket)
      {
        case ServiceErrorBucket.TryAgainLater:
          serviceFailure = (ServiceFailureException) new TryAgainLaterException(message, innerException);
          break;
        case ServiceErrorBucket.TryAgainRefresh:
          serviceFailure = (ServiceFailureException) new TryAgainLaterException(message, innerException);
          break;
        case ServiceErrorBucket.AccountSetting:
          serviceFailure = (ServiceFailureException) new AccountStatusException(message, innerException);
          break;
        case ServiceErrorBucket.NotEnoughPoints:
          serviceFailure = (ServiceFailureException) new InsufficientPointsBalanceException(message, innerException);
          break;
        case ServiceErrorBucket.OfferNotPurchasable:
          serviceFailure = (ServiceFailureException) new OfferNotPurchasableException(message, innerException);
          break;
        case ServiceErrorBucket.InputArguments:
          serviceFailure = (ServiceFailureException) new UnauthorizedRequestException(message, innerException);
          break;
        case ServiceErrorBucket.AssetBalance:
          serviceFailure = (ServiceFailureException) new InsufficientAssetBalanceException(message, innerException);
          break;
        case ServiceErrorBucket.TokenNotRedeemable:
          serviceFailure = (ServiceFailureException) new TokenNotRedeemableException(message, innerException);
          break;
        default:
          serviceFailure = new ServiceFailureException(message, innerException);
          break;
      }
      serviceFailure.Bucket = bucket;
      serviceFailure.ErrorCode = errorCode;
      return serviceFailure;
    }
  }
}
