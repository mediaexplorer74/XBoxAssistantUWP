// *********************************************************
// Type: Microsoft.Phone.Marketplace.GameEnumerationRequest
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;


namespace Microsoft.Phone.Marketplace
{
  public class GameEnumerationRequest
  {
    private int _pageSize;
    private int _pageNum;
    private OrderingOption _orderBy;
    private OrderingDirection _orderDirection;
    private SynchronizationContext _syncContext;
    private EventHandler<EnumerateGamesCompletedEventArgs> _enumerateGamesCompleted;
    private bool _cancelRequested;
    private HttpRequest _httpRequest;
    private Action _cancelGamerContext;
    private object _userState;
    private object _syncObject = new object();

    public event EventHandler<EnumerateGamesCompletedEventArgs> EnumerateGamesCompleted
    {
      add => this.DoMutableAction((Action) (() => this._enumerateGamesCompleted += value));
      remove => this.DoMutableAction((Action) (() => this._enumerateGamesCompleted -= value));
    }

    public void EnumerateGamesAsync(int pageSize, int pageNum)
    {
      this.EnumerateGamesAsync(pageSize, pageNum, OrderingOption.SortTitle, OrderingDirection.Ascending, (object) null);
    }

    public void EnumerateGamesAsync(
      int pageSize,
      int pageNum,
      OrderingOption orderBy,
      OrderingDirection orderDirection,
      object userState)
    {
      this.EnumerateGamesAsyncCommon(pageSize, pageNum, orderBy, orderDirection, userState);
    }

    public void CancelAsync()
    {
      lock (this._syncObject)
      {
        if (!this.IsBusy)
          return;
        this._cancelRequested = true;
        if (this._httpRequest != null && this._httpRequest.IsBusy)
        {
          this._httpRequest.CancelAsync();
        }
        else
        {
          if (this._cancelGamerContext == null)
            return;
          this._cancelGamerContext();
        }
      }
    }

    public bool IsBusy { get; private set; }

    private void DoMutableAction(Action action)
    {
      lock (this._syncObject)
      {
        if (this.IsBusy)
          throw new InvalidOperationException("Object cannot be modified while IsBusy is true.");
      }
      action();
    }

    private bool IsCancelRequested
    {
      get
      {
        lock (this._syncObject)
          return this._cancelRequested;
      }
    }

    private void RaiseEnumerateGamesCompleted(EnumerateGamesCompletedEventArgs e)
    {
      SynchronizationContext syncContext = this._syncContext;
      this.ResetRequestState();
      EventHandler<EnumerateGamesCompletedEventArgs> enumerateGamesCompleted = this._enumerateGamesCompleted;
      if (enumerateGamesCompleted == null)
        return;
      if (syncContext == null)
        enumerateGamesCompleted((object) this, e);
      else
        syncContext.Post((SendOrPostCallback) (state => enumerateGamesCompleted((object) this, e)), (object) null);
    }

    private void EnumerateGamesAsyncCommon(
      int pageSize,
      int pageNum,
      OrderingOption orderBy,
      OrderingDirection orderDirection,
      object userState)
    {
      if (pageSize <= 0 || pageSize > 100)
        throw new ArgumentOutOfRangeException(nameof (pageSize));
      if (pageNum <= 0)
        throw new ArgumentOutOfRangeException(nameof (pageNum));
      if (!Enum.IsDefined(typeof (OrderingOption), (object) orderBy))
        throw new ArgumentOutOfRangeException(nameof (orderBy));
      if (!Enum.IsDefined(typeof (OrderingDirection), (object) orderDirection))
        throw new ArgumentOutOfRangeException(nameof (orderDirection));
      this.DoMutableAction((Action) (() => this.IsBusy = true));
      this._syncContext = SynchronizationContext.Current;
      this._userState = userState;
      this._pageSize = pageSize;
      this._pageNum = pageNum;
      this._orderBy = orderBy;
      this._orderDirection = orderDirection;
      this._cancelGamerContext = GamerContext.Acquire((Action<GamerContext, Exception, bool>) ((gc, error, cancelled) =>
      {
        this._cancelGamerContext = (Action) null;
        if (this.HandleCancelAndError(cancelled, error))
          return;
        this._httpRequest = new HttpRequest(this.BuildFastUri(gc.XblMarketplaceCatalogUrl, gc.LegalLocale, gc.UserType));
        this._httpRequest.GetResponseCompleted += (EventHandler<GetResponseCompletedEventArgs>) ((s, e) => this.FastQueryComplete(gc.UserType, e));
        this._httpRequest.GetResponseAsync(userState);
      }));
    }

    private void FastQueryComplete(int tier, GetResponseCompletedEventArgs e)
    {
      if (this.HandleCancelAndError(e.Cancelled, e.Error))
        return;
      int totalAvailable = 0;
      ReadOnlyCollection<Product> games;
      try
      {
        games = new AvatarFastMarketplaceResponseParser().ParseGames(e.Response, out totalAvailable);
      }
      catch (Exception ex)
      {
        this.HandleCancelAndError(false, ex);
        return;
      }
      this.HandleSuccess(totalAvailable, games);
    }

    private void ResetRequestState()
    {
      lock (this._syncObject)
      {
        this.IsBusy = false;
        this._httpRequest = (HttpRequest) null;
        this._cancelRequested = false;
        this._userState = (object) null;
        this._syncContext = (SynchronizationContext) null;
        this._cancelGamerContext = (Action) null;
      }
    }

    private Uri BuildFastUri(Uri browseEndpoint, string locale, int tier)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(locale);
      stringBuilder.Append("?categories=3027");
      stringBuilder.Append("&detailview=detailmobile");
      stringBuilder.Append("&offerfilter=3");
      stringBuilder.Append("&offertargettypes=");
      stringBuilder.Append(47);
      if (this._orderBy != OrderingOption.SortTitle)
      {
        stringBuilder.Append("&orderby=");
        stringBuilder.Append(OfferEnumerationRequest.GetFASTOrderingOption(this._orderBy));
      }
      if (this._orderDirection != OrderingDirection.Ascending)
      {
        stringBuilder.Append("&orderdirection=");
        stringBuilder.Append(OfferEnumerationRequest.GetFASTOrderingDirection(this._orderDirection));
      }
      if (this._pageNum != 1)
      {
        stringBuilder.Append("&pagenum=");
        stringBuilder.Append(this._pageNum);
      }
      stringBuilder.Append("&pagesize=");
      stringBuilder.Append(this._pageSize);
      stringBuilder.Append("&platformtypes=1");
      stringBuilder.Append("&producttypes=1.21.23");
      stringBuilder.Append("&stores=");
      stringBuilder.Append(1);
      stringBuilder.Append("&tiers=");
      stringBuilder.Append(tier);
      try
      {
        return HttpRequest.SafeCombine(browseEndpoint, stringBuilder.ToString());
      }
      catch (Exception ex)
      {
        this.HandleError(ex);
        return (Uri) null;
      }
    }

    private string CombineWithOr(string a, string b) => a + "." + b;

    private bool HandleCancelAndError(bool canceled, Exception error)
    {
      if (this.IsCancelRequested || canceled)
      {
        this.HandleCancel();
        return true;
      }
      if (error == null)
        return false;
      this.HandleError(error);
      return true;
    }

    private void HandleSuccess(int totalAvailable, ReadOnlyCollection<Product> games)
    {
      this.RaiseEnumerateGamesCompleted(new EnumerateGamesCompletedEventArgs((Exception) null, false, this._userState)
      {
        TotalAvailable = totalAvailable,
        Games = games
      });
    }

    private void HandleError(Exception e)
    {
      e = (Exception) ExceptionHelper.Transform(e);
      this.RaiseEnumerateGamesCompleted(new EnumerateGamesCompletedEventArgs(e, false, this._userState));
    }

    private void HandleCancel()
    {
      this.RaiseEnumerateGamesCompleted(new EnumerateGamesCompletedEventArgs((Exception) null, true, this._userState));
    }
  }
}
