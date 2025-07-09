// *********************************************************
// Type: Microsoft.Phone.Marketplace.ExceptionHelper
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using Microsoft.Phone.Marketplace.Resources;
using System;


namespace Microsoft.Phone.Marketplace
{
  internal static class ExceptionHelper
  {
    public static ServiceFailureException Transform(Exception e)
    {
      return e is ServiceFailureException failureException ? failureException : new ServiceFailureException(NonLocalizedResources.UnexpectedFailure, e);
    }

    internal static bool HandleServiceFailure(ServiceFailureException serviceFailure)
    {
      switch (serviceFailure.Bucket)
      {
        case ServiceErrorBucket.TryAgainRefresh:
          GamerContext.InvalidatePartnerTokens();
          GamerContext.InvalidateContext();
          GamerContext.InvalidatePointsBalance();
          break;
        case ServiceErrorBucket.AccountSetting:
          GamerContext.InvalidateContext();
          GamerContext.InvalidatePointsBalance();
          break;
        case ServiceErrorBucket.NotEnoughPoints:
          GamerContext.InvalidatePointsBalance();
          break;
        case ServiceErrorBucket.OfferNotPurchasable:
          GamerContext.InvalidateContext();
          break;
        case ServiceErrorBucket.InputArguments:
          GamerContext.InvalidatePartnerTokens();
          break;
      }
      return false;
    }
  }
}
