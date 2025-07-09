// *********************************************************
// Type: Microsoft.Phone.Marketplace.PurchaseRequest
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using Microsoft.Phone.Controls;
using Microsoft.Phone.Marketplace.Purchase.UI;
using Microsoft.Phone.Marketplace.Resources;
using System;
using System.IO;
using System.Threading;
using System.Windows.Controls;


namespace Microsoft.Phone.Marketplace
{
  public class PurchaseRequest
  {
    protected Offer _offer;
    protected Guid _transactionId;
    protected RedeemRequest _redeemRequest;
    private uint _titleId;
    private string _sessionId;
    private SynchronizationContext _syncContext;
    private EventHandler<PurchaseCompletedEventArgs> _showPurchaseCompleted;
    private SecureLIVEnRequest _purchaseRequest;
    private Action _cancelGamerContext;
    private Action _cancelPartnerToken;
    private Action _cancelPointBalance;
    private Action _postCompleteAction;
    private bool _asyncCancel;
    private bool _cancelRequested;
    private IPurchaseRequestUI _ui;
    private object _userState;
    private object _syncObject = new object();
    protected MarketplaceAvatarBodyType _avatarBodyType;

    public void ShowPurchaseAsync(
      int titleId,
      MarketplaceAvatarBodyType avatarBodyType,
      Offer offer,
      Page page,
      out Guid transactionId)
    {
      this._avatarBodyType = avatarBodyType;
      this.ShowPurchaseAsync(titleId, offer, page, out transactionId);
    }

    private void ExecuteAddPointsWorkflow(object sender, EventArgs args)
    {
      this.BeginAsyncBusy_NonBlocking();
      PurchaseRedeemHelper.ShowMessageBox(this._ui, UIResources.AddingPointsTitle, UIResources.AddingPointsMessage, UIResources.Yes, UIResources.No, (Action<bool>) (yesPressed =>
      {
        if (!this.EndAsyncBusy(false) || !yesPressed)
          return;
        this.BeginAsyncBusy_NonBlocking();
        this._cancelGamerContext = GamerContext.Acquire((Action<GamerContext, Exception, bool>) ((gamerContext, e, gcCanceled) =>
        {
          this._cancelGamerContext = (Action) null;
          if (!this.EndAsyncBusy(gcCanceled))
            return;
          if (e != null)
            this.HandleErrorMessage(e, (Action) (() => this.ExecuteAddPointsWorkflow(sender, args)));
          else
            this.CompleteWithBrowserTask(gamerContext.MOPageAddPointsUrl, gamerContext.LegalLocale);
        }));
      }));
    }

    private void ExecuteLightweightAccountWorkflow()
    {
      this.BeginAsyncBusy_NonBlocking();
      PurchaseRedeemHelper.ShowMessageBox(this._ui, UIResources.LightweightAccountTitle, UIResources.LightweightAccountMessage, UIResources.Yes, UIResources.No, (Action<bool>) (yesPressed =>
      {
        if (!this.EndAsyncBusy(false))
          return;
        if (yesPressed)
        {
          this.BeginAsyncBusy_NonBlocking();
          this._cancelGamerContext = GamerContext.Acquire((Action<GamerContext, Exception, bool>) ((gamerContext, e, gcCanceled) =>
          {
            this._cancelGamerContext = (Action) null;
            if (!this.EndAsyncBusy(gcCanceled))
              return;
            if (e != null)
              this.HandleErrorMessage(e, new Action(this.ExecuteLightweightAccountWorkflow));
            else
              this.CompleteWithBrowserTask(gamerContext.MOPageSetupBillingUrl, gamerContext.LegalLocale);
          }));
        }
        else
          this.CancelAsyncInternal();
      }));
    }

    private void ExecuteTermsOfUseWorkflow(object sender, EventArgs args)
    {
      this.BeginAsyncBusy_NonBlocking();
      PurchaseRedeemHelper.ShowMessageBox(this._ui, UIResources.TermsOfUseTitle, UIResources.TermsOfUseMessage, UIResources.Yes, UIResources.No, (Action<bool>) (yesPressed =>
      {
        if (!this.EndAsyncBusy(false) || !yesPressed)
          return;
        this.BeginAsyncBusy_NonBlocking();
        this._cancelGamerContext = GamerContext.Acquire((Action<GamerContext, Exception, bool>) ((gamerContext, e, gcCanceled) =>
        {
          this._cancelGamerContext = (Action) null;
          if (!this.EndAsyncBusy(gcCanceled))
            return;
          if (e != null)
            this.HandleErrorMessage(e, new Action(this.RetryTermsOfUseWorkflow));
          else
            this.CompleteWithBrowserTask(gamerContext.MOPageTouUrl, gamerContext.LegalLocale);
        }));
      }));
    }

    private void RetryTermsOfUseWorkflow()
    {
      this.ExecuteTermsOfUseWorkflow((object) null, EventArgs.Empty);
    }

    protected virtual void CreateRedeemRequestObject()
    {
      this._redeemRequest = new RedeemRequest()
      {
        ExistingOffer = this._offer
      };
    }

    private void ExecuteRedeemWorkflow(object sender, EventArgs args)
    {
      this.BeginAsyncBusy_NonBlocking();
      this.CreateRedeemRequestObject();
      this._redeemRequest.ShowRedeemCompleted += new EventHandler<PurchaseCompletedEventArgs>(this.HandleRedeemResponse);
      this._redeemRequest.ShowRedeemAsyncWithinPurchase((int) this._titleId, this._sessionId, this._ui, this._userState, this._transactionId);
    }

    protected virtual void HandleRedeemResponse(object sender, PurchaseCompletedEventArgs e)
    {
      this._redeemRequest = (RedeemRequest) null;
      if (!this.EndAsyncBusy(e.Cancelled && !e.CanceledByUser))
        return;
      if (e.Error is ServiceFailureException)
        this.HandleError(e.Error);
      else if (e.Error != null)
        this.HandleErrorMessage(e.Error, (Action) null);
      else if (e.CanceledByUser)
      {
        this.ExecuteStartPurchaseWorkflow();
      }
      else
      {
        GamerContext.InvalidatePointsBalance();
        this.HandleSuccess(e.PurchaseReceipt);
      }
    }

    private void RetryRedeemWorkflow()
    {
      this.ExecuteRedeemWorkflow((object) null, EventArgs.Empty);
    }

    public bool IsBusy { get; private set; }

    public event EventHandler<PurchaseCompletedEventArgs> ShowPurchaseCompleted
    {
      add => this.DoMutableAction((Action) (() => this._showPurchaseCompleted += value));
      remove => this.DoMutableAction((Action) (() => this._showPurchaseCompleted -= value));
    }

    public event EventHandler<EventArgs> ScreenShown;

    public event EventHandler<EventArgs> ScreenHidden;

    private void RaiseShowPurchaseCompleted(PurchaseCompletedEventArgs e)
    {
      SynchronizationContext syncContext = this._syncContext;
      this.ResetRequestState();
      EventHandler<PurchaseCompletedEventArgs> showPurchaseCompleted = this._showPurchaseCompleted;
      try
      {
        if (showPurchaseCompleted == null)
          return;
        if (syncContext == null)
        {
          showPurchaseCompleted((object) this, e);
          this.FirePostCompleteAction();
        }
        else
          syncContext.Post((SendOrPostCallback) (state =>
          {
            showPurchaseCompleted((object) this, e);
            this.FirePostCompleteAction();
          }), (object) null);
      }
      catch
      {
      }
    }

    private void FirePostCompleteAction()
    {
      if (this._postCompleteAction == null)
        return;
      this._postCompleteAction();
      this._postCompleteAction = (Action) null;
    }

    protected virtual void CheckTitleId(Offer offer, int titleId)
    {
      if (offer.TitleId != titleId)
        throw new ArgumentException(NonLocalizedResources.OfferNotProvidedByTitle, nameof (offer));
    }

    private void ShowPurchaseAsyncCommon(
      int titleId,
      string sessionId,
      Offer offer,
      IPurchaseRequestUI ui,
      object userState,
      out Guid transactionId)
    {
      if (this._showPurchaseCompleted == null)
        throw new InvalidOperationException(NonLocalizedResources.CompletedEventNotHooked);
      if (ui == null)
        throw new ArgumentNullException(nameof (ui));
      if (offer == null)
        throw new ArgumentNullException(nameof (offer));
      this.CheckTitleId(offer, titleId);
      this.DoMutableAction((Action) (() => this.IsBusy = true));
      this._syncContext = SynchronizationContext.Current;
      this._titleId = (uint) titleId;
      this._sessionId = sessionId;
      this._offer = offer;
      this._transactionId = transactionId = Guid.NewGuid();
      this._userState = userState;
      this._ui = ui;
      this._ui.PurchaseOffer = this._offer;
      this.HookUIEvents();
      this.ExecuteStartPurchaseWorkflow();
    }

    private void HookUIEvents()
    {
      this._ui.AddPointsPressed += new EventHandler<EventArgs>(this.ExecuteAddPointsWorkflow);
      this._ui.BuyPressed += new EventHandler<EventArgs>(this.ExecuteBuyWorkflow);
      this._ui.CancelPressed += new EventHandler<EventArgs>(this.CancelPressed);
      this._ui.TermsOfUsePressed += new EventHandler<EventArgs>(this.ExecuteTermsOfUseWorkflow);
      this._ui.RedeemPressed += new EventHandler<EventArgs>(this.ExecuteRedeemWorkflow);
      this._ui.ScreenShown += new EventHandler<EventArgs>(this.OnScreenShown);
      this._ui.ScreenHidden += new EventHandler<EventArgs>(this.OnScreenHidden);
    }

    private void UnHookUIEvents()
    {
      this._ui.AddPointsPressed -= new EventHandler<EventArgs>(this.ExecuteAddPointsWorkflow);
      this._ui.BuyPressed -= new EventHandler<EventArgs>(this.ExecuteBuyWorkflow);
      this._ui.CancelPressed -= new EventHandler<EventArgs>(this.CancelPressed);
      this._ui.TermsOfUsePressed -= new EventHandler<EventArgs>(this.ExecuteTermsOfUseWorkflow);
      this._ui.RedeemPressed -= new EventHandler<EventArgs>(this.ExecuteRedeemWorkflow);
      this._ui.ScreenShown -= new EventHandler<EventArgs>(this.OnScreenShown);
      this._ui.ScreenHidden -= new EventHandler<EventArgs>(this.OnScreenHidden);
    }

    private void OnScreenShown(object sender, EventArgs e)
    {
      EventHandler<EventArgs> screenShown = this.ScreenShown;
      if (screenShown == null)
        return;
      screenShown((object) this, EventArgs.Empty);
    }

    private void OnScreenHidden(object sender, EventArgs e)
    {
      EventHandler<EventArgs> screenHidden = this.ScreenHidden;
      if (screenHidden != null)
        screenHidden((object) this, EventArgs.Empty);
      if (this._ui == null)
        return;
      this.UnHookUIEvents();
      this._ui = (IPurchaseRequestUI) null;
    }

    private void CancelPressed(object sender, EventArgs args)
    {
      if (this._ui.BusyMode == BusyMode.Blocking)
        return;
      this.CancelAsyncInternal();
    }

    private void CancelAsyncInternal()
    {
      lock (this._syncObject)
      {
        if (!this.IsBusy)
          return;
        this._cancelRequested = true;
        if (this._asyncCancel)
        {
          if (this._purchaseRequest != null && this._purchaseRequest.IsBusy)
            this._purchaseRequest.CancelAsync();
          else if (this._redeemRequest != null && this._redeemRequest.IsBusy)
            this._redeemRequest.CancelAsyncInternal();
          else if (this._cancelGamerContext != null)
            this._cancelGamerContext();
          else if (this._cancelPartnerToken != null)
          {
            this._cancelPartnerToken();
          }
          else
          {
            if (this._cancelPointBalance == null)
              return;
            this._cancelPointBalance();
          }
        }
        else
          this.HandleCancel();
      }
    }

    private void ResetRequestState()
    {
      lock (this._syncObject)
      {
        this.IsBusy = false;
        this._userState = (object) null;
        this._syncContext = (SynchronizationContext) null;
        this._purchaseRequest = (SecureLIVEnRequest) null;
        this._redeemRequest = (RedeemRequest) null;
        this._asyncCancel = false;
        this._cancelRequested = false;
        this._cancelGamerContext = (Action) null;
        this._cancelPartnerToken = (Action) null;
        this._cancelPointBalance = (Action) null;
        this.RemoveUI();
      }
    }

    private void RemoveUI()
    {
      if (this._ui == null || !this._ui.IsActive)
        return;
      this._ui.Remove();
    }

    private void HandleSuccess(PurchaseReceipt purchaseReceipt)
    {
      this.RaiseShowPurchaseCompleted(new PurchaseCompletedEventArgs((Exception) null, false, this._userState)
      {
        PurchaseReceipt = purchaseReceipt,
        PurchaseOffer = this._offer,
        TransactionId = this._transactionId
      });
    }

    private void HandleCancel()
    {
      this.RaiseShowPurchaseCompleted(new PurchaseCompletedEventArgs((Exception) null, true, this._userState)
      {
        PurchaseOffer = this._offer,
        TransactionId = this._transactionId
      });
    }

    private void HandleError(Exception e)
    {
      this.RaiseShowPurchaseCompleted(new PurchaseCompletedEventArgs(e, false, this._userState)
      {
        PurchaseOffer = this._offer,
        TransactionId = this._transactionId
      });
    }

    private void HandleErrorMessage(Exception e, Action retryAction)
    {
      ServiceFailureException sfe = ExceptionHelper.Transform(e);
      string button1;
      string button2;
      if (retryAction == null)
      {
        button1 = (string) null;
        button2 = UIResources.Ok;
      }
      else
      {
        button1 = UIResources.Ok;
        button2 = UIResources.Cancel;
      }
      this.BeginAsyncBusy_NonBlocking();
      PurchaseRedeemHelper.ShowMessageBox(this._ui, sfe.GetErrorMessageTitle(), sfe.GetErrorMessageDescription(), button1, button2, (Action<bool>) (retryPressed =>
      {
        if (!this.EndAsyncBusy(false))
          return;
        if (retryAction != null && retryPressed)
          retryAction();
        else
          this.HandleError((Exception) sfe);
      }));
    }

    private bool IsCancelRequested
    {
      get
      {
        lock (this._syncObject)
          return this._cancelRequested;
      }
    }

    private void BeginAsyncBusy_Blocking()
    {
      if (this._ui != null)
        this._ui.BusyMode = BusyMode.Blocking;
      this._asyncCancel = true;
    }

    private void BeginAsyncBusy_NonBlocking()
    {
      if (this._ui != null)
        this._ui.BusyMode = BusyMode.NonBlocking;
      this._asyncCancel = true;
    }

    private bool EndAsyncBusy(bool requestCanceled)
    {
      if (this._ui != null)
        this._ui.BusyMode = BusyMode.None;
      this._asyncCancel = false;
      if (this.IsCancelRequested)
      {
        this.HandleCancel();
        return false;
      }
      return !requestCanceled;
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

    private void CompleteWithBrowserTask(Uri uri, string locale)
    {
      this.BeginAsyncBusy_NonBlocking();
      this._ui.DelayOperation((Action) (() =>
      {
        if (!this.EndAsyncBusy(false))
          return;
        this._postCompleteAction = PurchaseRedeemHelper.CompleteWithBrowserTask(uri, locale, this._titleId, this._sessionId, (Action<Exception>) (e => this.HandleError(e)));
      }), 15);
    }

    private void ExecuteCantPurchasePDLCWorkflow()
    {
      this.BeginAsyncBusy_NonBlocking();
      PurchaseRedeemHelper.ShowMessageBox(this._ui, UIResources.Sorry, UIResources.CantPurchasePDLCMessage, (string) null, UIResources.Cancel, (Action<bool>) (pressed =>
      {
        GamerContext.InvalidateContext();
        if (!this.EndAsyncBusy(false))
          return;
        this.CancelAsyncInternal();
      }));
    }

    public void ShowPurchaseAsync(int titleId, Offer offer, Page page, out Guid transactionId)
    {
      this.ShowPurchaseAsync(titleId, offer, page, (object) null, out transactionId);
    }

    public void ShowPurchaseAsync(
      int titleId,
      Offer offer,
      Page page,
      object userState,
      out Guid transactionId)
    {
      PurchaseRequestComponent ui = page != null ? new PurchaseRequestComponent(page) : throw new ArgumentNullException(nameof (page));
      this.ShowPurchaseAsyncCommon(titleId, PurchaseRequest.GetSessionId(page), offer, (IPurchaseRequestUI) ui, userState, out transactionId);
    }

    public bool HandleBackButton()
    {
      return this._ui is PurchaseRequestComponent ui && ui.HandleBackButton();
    }

    internal static string GetSessionId(Page page)
    {
      string sessionId = (string) null;
      ((Page) (page as PhoneApplicationPage)).NavigationContext.QueryString.TryGetValue("SessionID", out sessionId);
      return sessionId;
    }

    private void ExecuteStartPurchaseWorkflow()
    {
      this._ui.CurrentPointsBalance = new int?();
      this._ui.Show(PurchaseUIMode.Buy);
      PurchaseRedeemHelper.SetLastKnownLegalLocale(this._ui);
      this.BeginAsyncBusy_NonBlocking();
      this._cancelGamerContext = GamerContext.Acquire((Action<GamerContext, Exception, bool>) ((context, e1, gcCanceled) =>
      {
        this._cancelGamerContext = (Action) null;
        if (!this.EndAsyncBusy(gcCanceled))
          return;
        if (e1 != null)
        {
          this.HandleErrorMessage(e1, (Action) null);
        }
        else
        {
          PurchaseRedeemHelper.SetLastKnownLegalLocale(this._ui);
          if (this._offer.PointsPrice > 0 && context.IsLightweightAccount)
            this.ExecuteLightweightAccountWorkflow();
          else if (this._offer.PointsPrice > 0 && !context.CanPurchasePDLC)
          {
            this.ExecuteCantPurchasePDLCWorkflow();
          }
          else
          {
            this.BeginAsyncBusy_NonBlocking();
            this._cancelPointBalance = GamerContext.GetPointsBalance((Action<int, Exception, bool>) ((points, e2, pbCanceled) =>
            {
              this._cancelPointBalance = (Action) null;
              if (!this.EndAsyncBusy(pbCanceled))
                return;
              if (e2 == null)
              {
                this._ui.CurrentPointsBalance = new int?(points);
                if (points >= this._offer.PointsPrice)
                  return;
                this._ui.Show(PurchaseUIMode.AddPoints);
              }
              else
                this._ui.Show(PurchaseUIMode.BuyUnknownPointsBalance);
            }));
          }
        }
      }));
    }

    private void ExecuteBuyWorkflow(object sender, EventArgs args)
    {
      this.BeginAsyncBusy_Blocking();
      this._cancelGamerContext = GamerContext.Acquire((Action<GamerContext, Exception, bool>) ((gamerContext, e1, gcCanceled) =>
      {
        this._cancelGamerContext = (Action) null;
        if (!this.EndAsyncBusy(gcCanceled))
          return;
        if (e1 != null)
        {
          this.HandleErrorMessage(e1, (Action) null);
        }
        else
        {
          this.BeginAsyncBusy_Blocking();
          this._cancelPartnerToken = GamerContext.GetPartnerToken(true, (Action<string, Exception, bool>) ((shortLivedToken, e2, ptCanceled) =>
          {
            this._cancelPartnerToken = (Action) null;
            if (!this.EndAsyncBusy(ptCanceled))
              return;
            if (e2 != null)
            {
              this.HandleErrorMessage(e2, (Action) null);
            }
            else
            {
              try
              {
                this.DoPurchaseRequest(gamerContext, shortLivedToken);
              }
              catch (Exception ex)
              {
                this.HandleErrorMessage(ex, (Action) null);
              }
            }
          }));
        }
      }));
    }

    private void RetryBuyWorkflow() => this.ExecuteBuyWorkflow((object) null, EventArgs.Empty);

    private void DoPurchaseRequest(GamerContext gamerContext, string shortLivedToken)
    {
      Stream purchaseRequestStream = PurchaseRedeemHelper.CreatePurchaseRequestStream(this._offer, this._transactionId, (string) null);
      SecureLIVEnRequest secureLivEnRequest = new SecureLIVEnRequest(PurchaseRedeemHelper.GetPurchaseUri());
      secureLivEnRequest.PartnerToken = shortLivedToken;
      secureLivEnRequest.Locale = gamerContext.LegalLocale;
      secureLivEnRequest.Method = "POST";
      secureLivEnRequest.RequestStream = purchaseRequestStream;
      this._purchaseRequest = secureLivEnRequest;
      this.BeginAsyncBusy_Blocking();
      this._purchaseRequest.GetResponseCompleted += new EventHandler<GetResponseCompletedEventArgs>(this.HandlePurchaseResponse);
      this._purchaseRequest.GetResponseAsync();
    }

    private void HandlePurchaseResponse(object sender, GetResponseCompletedEventArgs e)
    {
      this._purchaseRequest = (SecureLIVEnRequest) null;
      if (!this.EndAsyncBusy(e.Cancelled))
        return;
      if (e.Error != null)
      {
        this.HandleErrorMessage(e.Error, (Action) null);
      }
      else
      {
        PurchaseReceipt purchaseReceipt;
        try
        {
          purchaseReceipt = new PurchaseReceiptParser().Parse(e.Response);
        }
        catch (Exception ex)
        {
          this.HandleErrorMessage(ex, (Action) null);
          return;
        }
        GamerContext.InvalidatePointsBalance();
        this.HandleSuccess(purchaseReceipt);
      }
    }
  }
}
