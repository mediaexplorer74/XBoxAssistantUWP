// *********************************************************
// Type: Microsoft.Phone.Marketplace.OfferEnumerationRequest
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using Microsoft.Phone.Marketplace.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;


namespace Microsoft.Phone.Marketplace
{
  public class OfferEnumerationRequest
  {
    protected int _pageSize;
    protected int _pageNum;
    protected OrderingOption _orderBy;
    protected OrderingDirection _orderDirection;
    protected object _userState;
    protected int _paymentType;
    private uint _titleId;
    private int? _contentCategoryMask;
    private SynchronizationContext _syncContext;
    private EventHandler<EnumerateOffersCompletedEventArgs> _enumerateOffersCompleted;
    private bool _cancelRequested;
    private HttpRequest _httpRequest;
    private Action _cancelGamerContext;
    private object _syncObject = new object();

    public event EventHandler<EnumerateOffersCompletedEventArgs> EnumerateOffersCompleted
    {
      add => this.DoMutableAction((Action) (() => this._enumerateOffersCompleted += value));
      remove => this.DoMutableAction((Action) (() => this._enumerateOffersCompleted -= value));
    }

    public void EnumerateOffersAsync(int titleId, int pageSize, int pageNum)
    {
      this.EnumerateOffersAsync(titleId, pageSize, pageNum, new int?(), OrderingOption.SortTitle, OrderingDirection.Ascending, (object) null);
    }

    public void EnumerateOffersAsync(
      int titleId,
      int pageSize,
      int pageNum,
      int? contentCategoryMask,
      OrderingOption orderBy,
      OrderingDirection orderDirection,
      object userState)
    {
      this.EnumerateOffersAsyncCommon(titleId, pageSize, pageNum, (IEnumerable<Guid>) null, contentCategoryMask, orderBy, orderDirection, userState, 1);
    }

    public void EnumerateOffersByOfferIdAsync(
      int titleId,
      int pageSize,
      int pageNum,
      IEnumerable<Guid> offerIds)
    {
      this.EnumerateOffersByOfferIdAsync(titleId, pageSize, pageNum, offerIds, new int?(), OrderingOption.SortTitle, OrderingDirection.Ascending, (object) null);
    }

    public void EnumerateOffersByOfferIdAsync(
      int titleId,
      int pageSize,
      int pageNum,
      IEnumerable<Guid> offerIds,
      int? contentCategoryMask,
      OrderingOption orderBy,
      OrderingDirection orderDirection,
      object userState)
    {
      this.EnumerateOffersAsyncCommon(titleId, pageSize, pageNum, offerIds, contentCategoryMask, orderBy, orderDirection, userState, 1);
    }

    internal void EnumerateTokenOffersByOfferIdAsync(
      int titleId,
      int pageSize,
      int pageNum,
      IEnumerable<Guid> offerIds)
    {
      this.EnumerateOffersAsyncCommon(titleId, pageSize, pageNum, offerIds, new int?(), OrderingOption.SortTitle, OrderingDirection.Ascending, (object) null, 2);
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

    public bool IsBusy { get; protected set; }

    protected void DoMutableAction(Action action)
    {
      lock (this._syncObject)
      {
        if (this.IsBusy)
          throw new InvalidOperationException(NonLocalizedResources.ObjectCannotBeModified);
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

    private void RaiseEnumerateOffersCompleted(EnumerateOffersCompletedEventArgs e)
    {
      SynchronizationContext syncContext = this._syncContext;
      this.ResetRequestState();
      try
      {
        EventHandler<EnumerateOffersCompletedEventArgs> enumerateOffersCompleted = this._enumerateOffersCompleted;
        if (enumerateOffersCompleted == null)
          return;
        if (syncContext == null)
          enumerateOffersCompleted((object) this, e);
        else
          syncContext.Post((SendOrPostCallback) (state => enumerateOffersCompleted((object) this, e)), (object) null);
      }
      catch
      {
      }
    }

    protected void EnumerateOffersAsyncCommon(
      int titleId,
      int pageSize,
      int pageNum,
      IEnumerable<Guid> offerIds,
      int? contentCategoryMask,
      OrderingOption orderBy,
      OrderingDirection orderDirection,
      object userState,
      int paymentType)
    {
      if (this._enumerateOffersCompleted == null)
        throw new InvalidOperationException(NonLocalizedResources.CompletedEventNotHooked);
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
      this._titleId = (uint) titleId;
      this._pageSize = pageSize;
      this._pageNum = pageNum;
      this._contentCategoryMask = contentCategoryMask;
      this._orderBy = orderBy;
      this._orderDirection = orderDirection;
      this._paymentType = paymentType;
      this._cancelGamerContext = GamerContext.Acquire((Action<GamerContext, Exception, bool>) ((gc, e1, gtCanceled) =>
      {
        this._cancelGamerContext = (Action) null;
        if (this.HandleCancelAndError(gtCanceled, e1))
          return;
        this._httpRequest = new HttpRequest(this.BuildFastUri(gc.XblMarketplaceCatalogUrl, gc.LegalLocale, gc.UserType, offerIds));
        if (this._httpRequest.HeaderCollection != null)
          this._httpRequest.HeaderCollection["Accept-Encoding"] = "gzip";
        this._httpRequest.GetResponseCompleted += (EventHandler<GetResponseCompletedEventArgs>) ((s, e) => this.FastQueryComplete(gc.UserType, e));
        this._httpRequest.GetResponseAsync(userState);
      }));
    }

    protected virtual ReadOnlyCollection<Offer> ParseOffer(
      Stream response,
      int tier,
      out int totalAvailable)
    {
      return new FastMarketplaceResponseParser().Parse(response, tier, this._paymentType, out totalAvailable);
    }

    private void FastQueryComplete(int tier, GetResponseCompletedEventArgs e)
    {
      if (this.HandleCancelAndError(e.Cancelled, e.Error))
        return;
      int totalAvailable = 0;
      ReadOnlyCollection<Offer> offer;
      try
      {
        offer = this.ParseOffer(e.Response, tier, out totalAvailable);
      }
      catch (Exception ex)
      {
        this.HandleCancelAndError(false, ex);
        return;
      }
      this.HandleSuccess(totalAvailable, offer);
    }

    protected virtual StringBuilder BuildRelativeUri(
      string locale,
      int tier,
      IEnumerable<Guid> offerIds)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(locale);
      stringBuilder.Append("?tiers=");
      stringBuilder.Append(tier);
      stringBuilder.Append("&detailview=detailmobile");
      stringBuilder.Append("&producttypes=");
      stringBuilder.Append(59);
      stringBuilder.Append(".");
      stringBuilder.Append(60);
      stringBuilder.Append("&stores=");
      stringBuilder.Append(5);
      if (this._titleId > 0U)
      {
        stringBuilder.Append("&hextitles=");
        stringBuilder.Append(string.Format("0x{0:x}", (object) this._titleId));
      }
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
      if (this._pageSize != 100)
      {
        stringBuilder.Append("&pagesize=");
        stringBuilder.Append(this._pageSize);
      }
      if (this._contentCategoryMask.HasValue)
      {
        uint mask = (uint) this._contentCategoryMask.Value;
        string str = Enumerable.Range(0, 32).Where<int>((Func<int, bool>) (bitShift => ((int) (mask >> bitShift) & 1) != 0)).Select<int, string>((Func<int, string>) (bitShift => (12001 + bitShift).ToString())).Aggregate<string, string>(string.Empty, new Func<string, string, string>(this.CombineWithOr));
        if (str != string.Empty)
        {
          stringBuilder.Append("&categories=");
          stringBuilder.Append(str);
        }
      }
      if (offerIds != null)
      {
        string str = offerIds.Select<Guid, string>((Func<Guid, string>) (id => id.ToString())).Aggregate<string, string>(string.Empty, new Func<string, string, string>(this.CombineWithOr));
        if (str != string.Empty)
        {
          stringBuilder.Append("&offers=");
          stringBuilder.Append(str);
        }
      }
      return stringBuilder;
    }

    private Uri BuildFastUri(
      Uri browseEndpoint,
      string locale,
      int tier,
      IEnumerable<Guid> offerIds)
    {
      StringBuilder stringBuilder = this.BuildRelativeUri(locale, tier, offerIds);
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

    internal static int GetFASTOrderingOption(OrderingOption orderBy)
    {
      switch (orderBy)
      {
        case OrderingOption.SortTitle:
          return 1;
        case OrderingOption.ReleaseDate:
          return 10;
        case OrderingOption.DownloadCount:
          return 4;
        case OrderingOption.DownloadCountToday:
          return 5;
        case OrderingOption.PurchaseCountPaid:
          return 8;
        case OrderingOption.PurchaseCountPaidToday:
          return 9;
        default:
          throw new ArgumentOutOfRangeException(nameof (orderBy));
      }
    }

    internal static int GetFASTOrderingDirection(OrderingDirection orderDirection)
    {
      switch (orderDirection)
      {
        case OrderingDirection.Ascending:
          return 1;
        case OrderingDirection.Descending:
          return 2;
        default:
          throw new ArgumentOutOfRangeException(nameof (orderDirection));
      }
    }

    protected string CombineWithOr(string a, string b) => a != string.Empty ? a + "." + b : b;

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

    protected bool HandleCancelAndError(bool canceled, Exception error)
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

    protected void HandleSuccess(int totalAvailable, ReadOnlyCollection<Offer> offers)
    {
      this.RaiseEnumerateOffersCompleted(new EnumerateOffersCompletedEventArgs((Exception) null, false, this._userState)
      {
        TotalAvailable = totalAvailable,
        Offers = offers
      });
    }

    protected void HandleError(Exception e)
    {
      e = (Exception) ExceptionHelper.Transform(e);
      this.RaiseEnumerateOffersCompleted(new EnumerateOffersCompletedEventArgs(e, false, this._userState));
    }

    protected void HandleCancel()
    {
      this.RaiseEnumerateOffersCompleted(new EnumerateOffersCompletedEventArgs((Exception) null, true, this._userState));
    }
  }
}
