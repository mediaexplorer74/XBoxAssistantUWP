// *********************************************************
// Type: Microsoft.XMedia.EventLink.Client.RetryPolicies
// Assembly: Microsoft.XMedia.EventLink.Client.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946994A4-3A3C-41D3-A520-1292D57CD5ED
// *********************************************************Microsoft.XMedia.EventLink.Client.WinPhone.dll

using System;


namespace Microsoft.XMedia.EventLink.Client
{
  internal static class RetryPolicies
  {
    private const double FastBackoffIntervalInSeconds = 3.0;
    private const double FastBackoffRandomizationInSeconds = 1.0;

    internal static TimeSpan DefaultClientBackoff => TimeSpan.FromSeconds(30.0);

    internal static int DefaultClientRetryCount => 3;

    internal static TimeSpan DefaultMaxBackoff => TimeSpan.FromSeconds(90.0);

    internal static TimeSpan DefaultMinBackoff => TimeSpan.FromSeconds(3.0);

    internal static ShouldRetry NoRetry
    {
      get
      {
        return (ShouldRetry) ((int retryCount, out TimeSpan retryInterval) =>
        {
          retryInterval = TimeSpan.Zero;
          return false;
        });
      }
    }

    internal static ShouldRetry Default
    {
      get
      {
        return RetryPolicies.Retry(RetryPolicies.DefaultClientRetryCount, RetryPolicies.DefaultClientBackoff);
      }
    }

    internal static ShouldRetry FastBackoff
    {
      get
      {
        return (ShouldRetry) ((int currentRetryCount, out TimeSpan retryInterval) =>
        {
          double num = 3.0 * (double) (currentRetryCount + 1) + 1.0 * (new Random().NextDouble() - 0.5);
          retryInterval = TimeSpan.FromSeconds(num);
          return currentRetryCount < RetryPolicies.DefaultClientRetryCount;
        });
      }
    }

    internal static ShouldRetry Retry(int retryCount, TimeSpan intervalBetweenRetries)
    {
      return (ShouldRetry) ((int currentRetryCount, out TimeSpan retryInterval) =>
      {
        retryInterval = intervalBetweenRetries;
        return currentRetryCount < retryCount;
      });
    }
  }
}
