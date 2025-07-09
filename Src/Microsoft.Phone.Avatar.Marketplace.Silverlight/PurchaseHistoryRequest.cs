// *********************************************************
// Type: Microsoft.Phone.Marketplace.PurchaseHistoryRequest
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using Microsoft.Phone.Marketplace.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Xml.Linq;


namespace Microsoft.Phone.Marketplace
{
  public class PurchaseHistoryRequest
  {
    protected uint _titleId;
    protected int _pageSize;
    protected int _pageNum;
    protected EventHandler<PurchaseReceiptEventArgs> _enumeratePurchaseHistoryCompleted;
    protected EventHandler<SignedDocumentEventArgs> _enumerateSignedPurchaseHistoryCompleted;
    private int _mediaType;
    private IEnumerable<Guid> _transactionIds;
    private DateTime? _startDateUtc;
    private SynchronizationContext _syncContext;
    private bool _cancelRequested;
    private SecureLIVEnRequest _receiptRequest;
    private Action _cancelGamerContext;
    private Action _cancelPartnerToken;
    private object _userState;
    private bool _signed;
    private object _syncObject = new object();

    public event EventHandler<PurchaseReceiptEventArgs> EnumeratePurchaseHistoryCompleted
    {
      add
      {
        this.DoMutableAction((Action) (() => this._enumeratePurchaseHistoryCompleted += value));
      }
      remove
      {
        this.DoMutableAction((Action) (() => this._enumeratePurchaseHistoryCompleted -= value));
      }
    }

    public event EventHandler<SignedDocumentEventArgs> EnumerateSignedPurchaseHistoryCompleted
    {
      add
      {
        this.DoMutableAction((Action) (() => this._enumerateSignedPurchaseHistoryCompleted += value));
      }
      remove
      {
        this.DoMutableAction((Action) (() => this._enumerateSignedPurchaseHistoryCompleted -= value));
      }
    }

    public void CancelAsync()
    {
      lock (this._syncObject)
      {
        if (!this.IsBusy)
          return;
        this._cancelRequested = true;
        if (this._receiptRequest != null && this._receiptRequest.IsBusy)
          this._receiptRequest.CancelAsync();
        else if (this._cancelGamerContext != null)
        {
          this._cancelGamerContext();
        }
        else
        {
          if (this._cancelPartnerToken == null)
            return;
          this._cancelPartnerToken();
        }
      }
    }

    public bool IsBusy { get; private set; }

    public void EnumeratePurchaseHistoryAsync(int titleId, int pageSize, int pageNum)
    {
      this.EnumeratePurchaseHistoryAsync(titleId, pageSize, pageNum, (IEnumerable<Guid>) null, MediaType.Durable, new DateTime?(), (object) null);
    }

    public void EnumeratePurchaseHistoryAsync(
      int titleId,
      int pageSize,
      int pageNum,
      IEnumerable<Guid> transactionIds,
      MediaType mediaType,
      DateTime? startDateUtc,
      object userData)
    {
      if (this._enumeratePurchaseHistoryCompleted == null)
        throw new InvalidOperationException(NonLocalizedResources.CompletedEventNotHooked);
      int offerMediaType = Enum.IsDefined(typeof (MediaType), (object) mediaType) ? this.GetMediaTypeId(mediaType) : throw new ArgumentOutOfRangeException(nameof (mediaType));
      this.EnumeratePurchaseHistoryAsyncCommon(titleId, pageSize, pageNum, transactionIds, offerMediaType, startDateUtc, userData, false);
    }

    public void EnumerateSignedPurchaseHistoryAsync(int titleId, int pageSize, int pageNum)
    {
      this.EnumerateSignedPurchaseHistoryAsync(titleId, pageSize, pageNum, (IEnumerable<Guid>) null, MediaType.Durable, new DateTime?(), (object) null);
    }

    public void EnumerateSignedPurchaseHistoryAsync(
      int titleId,
      int pageSize,
      int pageNum,
      IEnumerable<Guid> transactionIds,
      MediaType mediaType,
      DateTime? startDateUtc,
      object userData)
    {
      if (this._enumerateSignedPurchaseHistoryCompleted == null)
        throw new InvalidOperationException(NonLocalizedResources.CompletedEventNotHooked);
      int offerMediaType = Enum.IsDefined(typeof (MediaType), (object) mediaType) ? this.GetMediaTypeId(mediaType) : throw new ArgumentOutOfRangeException(nameof (mediaType));
      this.EnumeratePurchaseHistoryAsyncCommon(titleId, pageSize, pageNum, transactionIds, offerMediaType, startDateUtc, userData, true);
    }

    private void DoMutableAction(Action action)
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

    private void RaiseEnumeratePurchaseHistoryCompleted(PurchaseReceiptEventArgs e)
    {
      SynchronizationContext syncContext = this._syncContext;
      this.ResetRequestState();
      try
      {
        EventHandler<PurchaseReceiptEventArgs> enumeratePurchaseHistoryCompleted = this._enumeratePurchaseHistoryCompleted;
        if (enumeratePurchaseHistoryCompleted == null)
          return;
        if (syncContext == null)
          enumeratePurchaseHistoryCompleted((object) this, e);
        else
          syncContext.Post((SendOrPostCallback) (state => enumeratePurchaseHistoryCompleted((object) this, e)), (object) null);
      }
      catch
      {
      }
    }

    private void RaiseEnumerateSignedPurchaseHistoryCompleted(SignedDocumentEventArgs e)
    {
      SynchronizationContext syncContext = this._syncContext;
      this.ResetRequestState();
      try
      {
        EventHandler<SignedDocumentEventArgs> enumerateSignedPurchaseHistoryCompleted = this._enumerateSignedPurchaseHistoryCompleted;
        if (enumerateSignedPurchaseHistoryCompleted == null)
          return;
        if (syncContext == null)
          enumerateSignedPurchaseHistoryCompleted((object) this, e);
        else
          syncContext.Post((SendOrPostCallback) (state => enumerateSignedPurchaseHistoryCompleted((object) this, e)), (object) null);
      }
      catch
      {
      }
    }

    protected void EnumeratePurchaseHistoryAsyncCommon(
      int titleId,
      int pageSize,
      int pageNum,
      IEnumerable<Guid> transactionIds,
      int offerMediaType,
      DateTime? startDateUtc,
      object userState,
      bool signed)
    {
      if (this._enumeratePurchaseHistoryCompleted == null && this._enumerateSignedPurchaseHistoryCompleted == null)
        throw new InvalidOperationException(NonLocalizedResources.CompletedEventNotHooked);
      if (pageNum <= 0)
        throw new ArgumentOutOfRangeException(nameof (pageNum));
      if (pageSize <= 0 || pageSize > 100)
        throw new ArgumentOutOfRangeException(nameof (pageSize));
      if (startDateUtc.HasValue && startDateUtc.Value.Kind != DateTimeKind.Utc)
        throw new ArgumentOutOfRangeException(nameof (startDateUtc), NonLocalizedResources.UtcTimeRequired);
      this.DoMutableAction((Action) (() => this.IsBusy = true));
      this._syncContext = SynchronizationContext.Current;
      this._titleId = (uint) titleId;
      this._pageSize = pageSize;
      this._pageNum = pageNum;
      this._transactionIds = transactionIds;
      this._startDateUtc = startDateUtc;
      this._mediaType = offerMediaType;
      this._userState = userState;
      this._signed = signed;
      this.EnumeratePurchaseHistoryAsyncCore();
    }

    private void EnumeratePurchaseHistoryAsyncCore()
    {
      this._cancelGamerContext = GamerContext.Acquire((Action<GamerContext, Exception, bool>) ((gamerContext, e1, gcCanceled) =>
      {
        this._cancelGamerContext = (Action) null;
        if (this.HandleCancelAndError(gcCanceled, e1))
          return;
        this._cancelPartnerToken = GamerContext.GetPartnerToken(false, (Action<string, Exception, bool>) ((token, e2, ptCanceled) =>
        {
          this._cancelPartnerToken = (Action) null;
          if (this.HandleCancelAndError(ptCanceled, e2))
            return;
          try
          {
            this.DoReceiptRequest(gamerContext, token);
          }
          catch (Exception ex)
          {
            this.HandleCancelAndError(false, ex);
          }
        }));
      }));
    }

    private void DoReceiptRequest(GamerContext gamerContext, string token)
    {
      Stream receiptRequestStream = this.CreateReceiptRequestStream();
      SecureLIVEnRequest secureLivEnRequest = new SecureLIVEnRequest(this.GetReceiptUri());
      secureLivEnRequest.PartnerToken = token;
      secureLivEnRequest.Locale = gamerContext.LegalLocale;
      secureLivEnRequest.Method = "POST";
      secureLivEnRequest.RequestStream = receiptRequestStream;
      this._receiptRequest = secureLivEnRequest;
      this._receiptRequest.GetResponseCompleted += new EventHandler<GetResponseCompletedEventArgs>(this.HandleReceiptResponse);
      this._receiptRequest.GetResponseAsync();
    }

    private void HandleReceiptResponse(object sender, GetResponseCompletedEventArgs e)
    {
      this._receiptRequest = (SecureLIVEnRequest) null;
      if (this.HandleCancelAndError(e.Cancelled, e.Error))
        return;
      if (this._signed)
      {
        try
        {
          MemoryStream memoryStream = new MemoryStream();
          StreamHelper.CopyStream(e.Response, (Stream) memoryStream);
          memoryStream.Seek(0L, SeekOrigin.Begin);
          this.SignedHandleSuccess((Stream) memoryStream);
        }
        catch (Exception ex)
        {
          this.HandleCancelAndError(false, ex);
        }
      }
      else
      {
        int totalAvailable = 0;
        ReadOnlyCollection<PurchaseReceipt> purchases;
        try
        {
          purchases = new PurchaseHistoryParser().Parse(e.Response, out totalAvailable);
        }
        catch (Exception ex)
        {
          this.HandleCancelAndError(false, ex);
          return;
        }
        this.HandleSuccess(totalAvailable, purchases);
      }
    }

    protected virtual XElement CreateReceiptRequestXml(XNamespace ns)
    {
      return new XElement(ns + "ReceiptRequest", new object[4]
      {
        (object) new XElement(ns + "PageNumber", (object) this._pageNum),
        (object) new XElement(ns + "PageSize", (object) this._pageSize),
        (object) new XElement(ns + "TitleId", (object) this._titleId),
        (object) new XElement(ns + "StoreId", (object) 5)
      });
    }

    private Stream CreateReceiptRequestStream()
    {
      XNamespace pdlcNs = XmlNamespaces.PdlcNs;
      XElement receiptRequestXml = this.CreateReceiptRequestXml(pdlcNs);
      if (this._transactionIds != null)
      {
        XElement content = new XElement(pdlcNs + "TransactionIds");
        foreach (Guid transactionId in this._transactionIds)
          content.Add((object) new XElement(XmlNamespaces.ArraysNs + "guid", (object) transactionId));
        receiptRequestXml.Add((object) content);
      }
      receiptRequestXml.Add((object) new XElement(pdlcNs + "MediaTypeId", (object) this._mediaType));
      if (this._startDateUtc.HasValue)
        receiptRequestXml.Add((object) new XElement(pdlcNs + "StartDate", (object) this._startDateUtc.Value));
      receiptRequestXml.Add((object) new XElement(pdlcNs + "SignReceipts", (object) this._signed));
      MemoryStream receiptRequestStream = new MemoryStream();
      receiptRequestXml.Save((Stream) receiptRequestStream, SaveOptions.DisableFormatting | SaveOptions.OmitDuplicateNamespaces);
      receiptRequestStream.Position = 0L;
      return (Stream) receiptRequestStream;
    }

    private int GetMediaTypeId(MediaType mediaType)
    {
      switch (mediaType)
      {
        case MediaType.Durable:
          return 59;
        case MediaType.Consumable:
          return 60;
        default:
          throw new ArgumentOutOfRangeException(nameof (mediaType));
      }
    }

    private Uri GetReceiptUri()
    {
      string relative = "receipts";
      return HttpRequest.SafeCombine(GamerContext.MarketplaceUrl, relative);
    }

    private void ResetRequestState()
    {
      lock (this._syncObject)
      {
        this.IsBusy = false;
        this._receiptRequest = (SecureLIVEnRequest) null;
        this._cancelRequested = false;
        this._userState = (object) null;
        this._signed = false;
        this._syncContext = (SynchronizationContext) null;
        this._cancelGamerContext = (Action) null;
        this._cancelPartnerToken = (Action) null;
      }
    }

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

    private void HandleSuccess(int totalAvailable, ReadOnlyCollection<PurchaseReceipt> purchases)
    {
      this.RaiseEnumeratePurchaseHistoryCompleted(new PurchaseReceiptEventArgs((Exception) null, false, this._userState)
      {
        TotalAvailable = totalAvailable,
        Purchases = purchases
      });
    }

    private void SignedHandleSuccess(Stream signedReceipts)
    {
      this.RaiseEnumerateSignedPurchaseHistoryCompleted(new SignedDocumentEventArgs((Exception) null, false, this._userState)
      {
        SignedDocument = signedReceipts
      });
    }

    private void HandleError(Exception e)
    {
      e = (Exception) ExceptionHelper.Transform(e);
      if (this._signed)
        this.RaiseEnumerateSignedPurchaseHistoryCompleted(new SignedDocumentEventArgs(e, false, this._userState));
      else
        this.RaiseEnumeratePurchaseHistoryCompleted(new PurchaseReceiptEventArgs(e, false, this._userState));
    }

    private void HandleCancel()
    {
      if (this._signed)
        this.RaiseEnumerateSignedPurchaseHistoryCompleted(new SignedDocumentEventArgs((Exception) null, true, this._userState));
      else
        this.RaiseEnumeratePurchaseHistoryCompleted(new PurchaseReceiptEventArgs((Exception) null, true, this._userState));
    }
  }
}
