// *********************************************************
// Type: Microsoft.Phone.Marketplace.GamerContext
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using Microsoft.Phone.Marketplace.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using System;
using System.Collections.Generic;
using System.Threading;


namespace Microsoft.Phone.Marketplace
{
  internal class GamerContext
  {
    internal const string GamerContextService = "context";
    internal const string PointsBalanceService = "pointsbalance";
    internal const string PurchaseService = "purchase";
    internal const string VerifyTokenService = "verifytoken";
    internal const string ReceiptsService = "receipts";
    internal const string AssetsService = "assets";
    internal static int _pointsBalance;
    internal static TimeSpan _pointsBalanceCacheTimeout = TimeSpan.FromMinutes(10.0);
    internal static DateTime _pointsBalanceExpiresUtc = DateTime.UtcNow;
    internal static Uri MarketplaceUrl;
    internal static Uri PartnerTokenEndpoint = new Uri("http://xboxlive.com/pdlc");
    internal static Uri PartnerTokenSecureEndpoint = new Uri("http://xboxlive.com/pdlc/purchase");
    internal static string _partnerToken;
    internal static string _partnerTokenSecure;
    internal static DateTime _partnerTokenExpiresUtc = DateTime.UtcNow;
    internal static DateTime _partnerTokenSecureExpiresUtc = DateTime.UtcNow;
    internal static List<Action<GamerContext, Exception, bool>> _acquireCallbacks = new List<Action<GamerContext, Exception, bool>>();
    internal static SecureLIVEnRequest _gamerContextRequest;
    internal static SecureLIVEnRequest _pointsBalanceRequest;
    internal static GamerContext _instance;
    internal static object _syncObject = new object();

    internal static Action Acquire(
      Action<GamerContext, Exception, bool> acquireCallback)
    {
      lock (GamerContext._syncObject)
      {
        if (GamerContext._instance != null && GamerContext._instance.Expires > DateTime.UtcNow)
        {
          GamerContext.RunAsync((Action) (() => acquireCallback(GamerContext._instance, (Exception) null, false)));
        }
        else
        {
          bool flag = false;
          try
          {
            GamerContext._acquireCallbacks.Add(acquireCallback);
            flag = true;
            if (GamerContext._acquireCallbacks.Count == 1)
              GamerContext.RunAsync(new Action(GamerContext.FinishAcquire));
          }
          catch (Exception ex)
          {
            if (flag)
              GamerContext._acquireCallbacks.Remove(acquireCallback);
            GamerContext.RunAsync((Action) (() => acquireCallback((GamerContext) null, ex, false)));
          }
        }
      }
      return (Action) (() =>
      {
        lock (GamerContext._syncObject)
        {
          int index = GamerContext._acquireCallbacks.IndexOf(acquireCallback);
          if (index < 0)
            return;
          Action<GamerContext, Exception, bool> callback = GamerContext._acquireCallbacks[index];
          GamerContext._acquireCallbacks.RemoveAt(index);
          GamerContext.RunAsync((Action) (() => callback((GamerContext) null, (Exception) null, true)));
        }
      });
    }

    private static void RunAsync(Action action)
    {
      SynchronizationContext current = SynchronizationContext.Current;
      if (current != null)
        current.Post((SendOrPostCallback) (s => action()), (object) null);
      else
        ThreadPool.QueueUserWorkItem((WaitCallback) (s => action()));
    }

    private static void FinishAcquire()
    {
      if (Gamer.SignedInGamers[PlayerIndex.One] == null)
      {
        GamerContext.NotifyAcquireCallbacks((GamerContext) null, (Exception) new ServiceFailureException(NonLocalizedResources.NoSignedInGamerAvailable), false);
      }
      else
      {
        try
        {
          GamerContext.ReadPdlcEndpoints();
        }
        catch (Exception ex)
        {
          GamerContext.NotifyAcquireCallbacks((GamerContext) null, ex, false);
          return;
        }
        GamerContext.GetPartnerToken(false, (Action<string, Exception, bool>) ((partnerToken, e, canceled) =>
        {
          if (e != null)
          {
            GamerContext.NotifyAcquireCallbacks((GamerContext) null, e, false);
          }
          else
          {
            GamerContext._gamerContextRequest = new SecureLIVEnRequest(new Uri(HttpRequest.SafeCombine(GamerContext.MarketplaceUrl, "context").ToString()))
            {
              PartnerToken = partnerToken
            };
            GamerContext._gamerContextRequest.GetResponseCompleted += (EventHandler<GetResponseCompletedEventArgs>) ((s, eventArgs) =>
            {
              GamerContext._gamerContextRequest = (SecureLIVEnRequest) null;
              if (eventArgs.Error == null)
              {
                if (!eventArgs.Cancelled)
                {
                  try
                  {
                    GamerContextParser gamerContextParser = new GamerContextParser();
                    int points = 0;
                    GamerContext gamerContext = gamerContextParser.Parse(eventArgs.Response, out points);
                    GamerContext._instance = gamerContext;
                    GamerContext.SetPointsBalance(points);
                    GamerContext.NotifyAcquireCallbacks(gamerContext, (Exception) null, false);
                    return;
                  }
                  catch (Exception ex)
                  {
                    GamerContext.NotifyAcquireCallbacks((GamerContext) null, ex, false);
                    return;
                  }
                }
              }
              GamerContext.NotifyAcquireCallbacks((GamerContext) null, eventArgs.Error, eventArgs.Cancelled);
            });
            GamerContext._gamerContextRequest.GetResponseAsync();
          }
        }));
      }
    }

    internal static Action GetPartnerToken(bool secure, Action<string, Exception, bool> callback)
    {
      bool requestCanceled = false;
      DateTime dateTime;
      string audienceUri;
      string token;
      if (secure)
      {
        dateTime = GamerContext._partnerTokenSecureExpiresUtc;
        audienceUri = GamerContext.PartnerTokenSecureEndpoint.ToString();
        token = GamerContext._partnerTokenSecure;
      }
      else
      {
        dateTime = GamerContext._partnerTokenExpiresUtc;
        audienceUri = GamerContext.PartnerTokenEndpoint.ToString();
        token = GamerContext._partnerToken;
      }
      if (!string.IsNullOrEmpty(token) && dateTime > DateTime.UtcNow)
        GamerContext.RunAsync((Action) (() => callback(token, (Exception) null, false)));
      else
        Gamer.BeginGetPartnerToken(audienceUri, (AsyncCallback) (result =>
        {
          lock (GamerContext._syncObject)
          {
            try
            {
              if (requestCanceled)
                return;
              string partnerToken = Gamer.EndGetPartnerToken(result);
              if (string.IsNullOrEmpty(partnerToken))
                throw new UnauthorizedRequestException(NonLocalizedResources.FailedToGetPartnerToken);
              GamerContext.ParseTokenExpiration(partnerToken, secure);
              if (secure)
                GamerContext._partnerTokenSecure = partnerToken;
              else
                GamerContext._partnerToken = partnerToken;
              GamerContext.RunAsync((Action) (() => callback(partnerToken, (Exception) null, false)));
            }
            catch (Exception ex)
            {
              GamerContext.RunAsync((Action) (() => callback((string) null, ex, false)));
            }
          }
        }), (object) null);
      return (Action) (() =>
      {
        lock (GamerContext._syncObject)
        {
          if (requestCanceled)
            return;
          requestCanceled = true;
          GamerContext.RunAsync((Action) (() => callback((string) null, (Exception) null, true)));
        }
      });
    }

    private static void ParseTokenExpiration(string partnerToken, bool secure)
    {
      PartnerTokenParser partnerTokenParser = new PartnerTokenParser();
      if (secure)
        GamerContext._partnerTokenSecureExpiresUtc = partnerTokenParser.ParseExpiration(partnerToken);
      else
        GamerContext._partnerTokenExpiresUtc = partnerTokenParser.ParseExpiration(partnerToken);
    }

    internal static void SetPointsBalance(int value)
    {
      GamerContext._pointsBalanceExpiresUtc = DateTime.UtcNow + GamerContext._pointsBalanceCacheTimeout;
      GamerContext._pointsBalance = value;
    }

    internal static Action GetPointsBalance(Action<int, Exception, bool> callback)
    {
      bool requestCanceled = false;
      Action partnerTokenCancelation = (Action) null;
      if (GamerContext._pointsBalanceExpiresUtc > DateTime.UtcNow)
        GamerContext.RunAsync((Action) (() => callback(GamerContext._pointsBalance, (Exception) null, false)));
      else
        partnerTokenCancelation = GamerContext.GetPartnerToken(false, (Action<string, Exception, bool>) ((partnerToken, e2, ptCanceled) =>
        {
          lock (GamerContext._syncObject)
          {
            partnerTokenCancelation = (Action) null;
            if (ptCanceled)
              return;
            if (e2 != null && !requestCanceled)
            {
              requestCanceled = true;
              GamerContext.RunAsync((Action) (() => callback(-1, e2, false)));
            }
            else
            {
              GamerContext._pointsBalanceRequest = new SecureLIVEnRequest(HttpRequest.SafeCombine(GamerContext.MarketplaceUrl, "pointsbalance"))
              {
                PartnerToken = partnerToken
              };
              GamerContext._pointsBalanceRequest.GetResponseCompleted += (EventHandler<GetResponseCompletedEventArgs>) ((s, eventArgs) =>
              {
                lock (GamerContext._syncObject)
                {
                  GamerContext._pointsBalanceRequest = (SecureLIVEnRequest) null;
                  if (eventArgs.Error == null)
                  {
                    if (!eventArgs.Cancelled)
                      goto label_5;
                  }
                  if (!requestCanceled)
                  {
                    requestCanceled = true;
                    GamerContext.RunAsync((Action) (() => callback(-1, eventArgs.Error, eventArgs.Cancelled)));
                    return;
                  }
label_5:
                  try
                  {
                    int points = new PointsBalanceParser().Parse(eventArgs.Response);
                    GamerContext.SetPointsBalance(points);
                    if (requestCanceled)
                      return;
                    requestCanceled = true;
                    GamerContext.RunAsync((Action) (() => callback(points, (Exception) null, false)));
                  }
                  catch (Exception ex)
                  {
                    if (requestCanceled)
                      return;
                    requestCanceled = true;
                    GamerContext.RunAsync((Action) (() => callback(-1, ex, false)));
                  }
                }
              });
              GamerContext._pointsBalanceRequest.GetResponseAsync();
            }
          }
        }));
      return (Action) (() =>
      {
        lock (GamerContext._syncObject)
        {
          if (requestCanceled)
            return;
          if (partnerTokenCancelation != null)
            partnerTokenCancelation();
          else if (GamerContext._pointsBalanceRequest != null && GamerContext._pointsBalanceRequest.IsBusy)
            GamerContext._pointsBalanceRequest.CancelAsync();
          requestCanceled = true;
          GamerContext.RunAsync((Action) (() => callback(-1, (Exception) null, true)));
        }
      });
    }

    private static void ReadPdlcEndpoints()
    {
      GamerContext.MarketplaceUrl = new XboxLIVESettingsParser().ParseMarketplaceURL();
    }

    private static void NotifyAcquireCallbacks(
      GamerContext gamerContext,
      Exception e,
      bool canceled)
    {
      bool lockTaken = false;
      object syncObject;
      try
      {
        Monitor.Enter(syncObject = GamerContext._syncObject, ref lockTaken);
        Action<GamerContext, Exception, bool>[] callbacksCopy = GamerContext._acquireCallbacks.ToArray();
        GamerContext.RunAsync((Action) (() => GamerContext.InvokeAcquireCallbacks(callbacksCopy, gamerContext, e, canceled)));
        GamerContext._acquireCallbacks.Clear();
      }
      finally
      {
        if (lockTaken)
          Monitor.Exit(syncObject);
      }
    }

    private static void InvokeAcquireCallbacks(
      Action<GamerContext, Exception, bool>[] callbacksCopy,
      GamerContext gamerContext,
      Exception e,
      bool canceled)
    {
      foreach (Action<GamerContext, Exception, bool> action in callbacksCopy)
      {
        try
        {
          action(gamerContext, e, canceled);
        }
        catch
        {
        }
      }
    }

    internal static void InvalidateContext() => GamerContext._instance = (GamerContext) null;

    internal static void InvalidatePartnerTokens()
    {
      GamerContext._partnerTokenExpiresUtc = GamerContext._partnerTokenSecureExpiresUtc = DateTime.UtcNow;
    }

    internal static void InvalidatePointsBalance()
    {
      GamerContext._pointsBalanceExpiresUtc = DateTime.UtcNow;
    }

    internal DateTime Expires { get; set; }

    internal Uri XblMarketplaceCatalogUrl { get; set; }

    internal Uri MOPageAddPointsUrl { get; set; }

    internal Uri MOPageSetupBillingUrl { get; set; }

    internal Uri MOPageTouUrl { get; set; }

    internal string LegalLocale { get; set; }

    internal int UserType { get; set; }

    internal bool CanPurchasePDLC { get; set; }

    internal bool IsLightweightAccount { get; set; }

    internal static string LastKnownLegalLanguage { get; set; }

    internal string AvatarMarketplaceUrl { get; set; }
  }
}
