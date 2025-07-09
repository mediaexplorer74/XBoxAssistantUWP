// *********************************************************
// Type: Microsoft.Phone.Marketplace.RedeemRequest
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using Microsoft.Phone.Marketplace.Purchase.UI;
using Microsoft.Phone.Marketplace.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Controls;


namespace Microsoft.Phone.Marketplace
{
  public class RedeemRequest
  {
    protected OfferEnumerationRequest _offerRequest;
    private uint _titleId;
    private string _sessionId;
    private Offer _offer;
    private Guid _transactionId;
    private SynchronizationContext _syncContext;
    private EventHandler<PurchaseCompletedEventArgs> _showRedeemCompleted;
    private SecureLIVEnRequest _verifyRedeemRequest;
    private SecureLIVEnRequest _redeemRequest;
    private Action _cancelGamerContext;
    private Action _cancelPartnerToken;
    private Action _postCompleteAction;
    private bool _asyncCancel;
    private bool _cancelRequested;
    private bool _ownsUI;
    private IPurchaseRequestUI _ui;
    private object _userState;
    private object _syncObject = new object();
    private static Regex redeemFormat1;
    private static Regex redeemFormat2;

    public void ShowRedeemAsync(int titleId, Page page, out Guid transactionId)
    {
      this.ShowRedeemAsync(titleId, page, (object) null, out transactionId);
    }

    public void ShowRedeemAsync(int titleId, Page page, object userState, out Guid transactionId)
    {
      PurchaseRequestComponent ui = page != null ? new PurchaseRequestComponent(page) : throw new ArgumentNullException(nameof (page));
      this.ShowRedeemAsyncCommon(titleId, PurchaseRequest.GetSessionId(page), (IPurchaseRequestUI) ui, userState, out transactionId);
    }

    static RedeemRequest()
    {
      string str1 = "[A-Za-z0-9]";
      string str2 = string.Format("{0}{1}{2}{3}{4}", (object) str1, (object) str1, (object) str1, (object) str1, (object) str1);
      RedeemRequest.redeemFormat1 = new Regex(string.Format("^{0}-{1}-{2}-{3}-{4}$", (object) str2, (object) str2, (object) str2, (object) str2, (object) str2), RegexOptions.Compiled);
      RedeemRequest.redeemFormat2 = new Regex(string.Format("^({0})({1})({2})({3})({4})$", (object) str2, (object) str2, (object) str2, (object) str2, (object) str2), RegexOptions.Compiled);
    }

    public bool IsBusy { get; private set; }

    public event EventHandler<PurchaseCompletedEventArgs> ShowRedeemCompleted
    {
      add => this.DoMutableAction((Action) (() => this._showRedeemCompleted += value));
      remove => this.DoMutableAction((Action) (() => this._showRedeemCompleted -= value));
    }

    internal void CancelAsyncInternal()
    {
      lock (this._syncObject)
      {
        if (!this.IsBusy)
          return;
        this._cancelRequested = true;
        if (this._asyncCancel)
        {
          if (this._verifyRedeemRequest != null && this._verifyRedeemRequest.IsBusy)
            this._verifyRedeemRequest.CancelAsync();
          else if (this._redeemRequest != null && this._redeemRequest.IsBusy)
            this._redeemRequest.CancelAsync();
          else if (this._offerRequest != null && this._offerRequest.IsBusy)
            this._offerRequest.CancelAsync();
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
        else
          this.HandleCancel(false);
      }
    }

    internal Offer ExistingOffer
    {
      get => this._offer;
      set => this.DoMutableAction((Action) (() => this._offer = value));
    }

    private void RaiseShowRedeemCompleted(PurchaseCompletedEventArgs e)
    {
      SynchronizationContext syncContext = this._syncContext;
      this.ResetRequestState();
      EventHandler<PurchaseCompletedEventArgs> showRedeemCompleted = this._showRedeemCompleted;
      try
      {
        if (showRedeemCompleted == null)
          return;
        if (syncContext == null)
        {
          showRedeemCompleted((object) this, e);
          this.FirePostCompleteAction();
        }
        else
          syncContext.Post((SendOrPostCallback) (state =>
          {
            showRedeemCompleted((object) this, e);
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

    internal void ShowRedeemAsyncCommon(
      int titleId,
      string sessionId,
      IPurchaseRequestUI ui,
      object userState,
      out Guid transactionId)
    {
      this._ownsUI = true;
      this._transactionId = transactionId = Guid.NewGuid();
      this.ShowRedeemAsyncCore(titleId, sessionId, ui, userState);
      this.HookUIEvents();
      this.ExecuteRedeemWorkflow();
    }

    internal void ShowRedeemAsyncWithinPurchase(
      int titleId,
      string sessionId,
      IPurchaseRequestUI ui,
      object userState,
      Guid transactionId)
    {
      this._ownsUI = false;
      this._transactionId = transactionId;
      this.ShowRedeemAsyncCore(titleId, sessionId, ui, userState);
      this.PromptForRedeemCode(string.Empty, string.Empty);
    }

    internal void ShowRedeemAsyncCore(
      int titleId,
      string sessionId,
      IPurchaseRequestUI ui,
      object userState)
    {
      if (this._showRedeemCompleted == null)
        throw new InvalidOperationException(NonLocalizedResources.CompletedEventNotHooked);
      this.DoMutableAction((Action) (() => this.IsBusy = true));
      this._syncContext = SynchronizationContext.Current;
      this._userState = userState;
      this._titleId = (uint) titleId;
      this._sessionId = sessionId;
      this._ui = ui;
    }

    private void HookUIEvents()
    {
      this._ui.CancelPressed += new EventHandler<EventArgs>(this.CancelPressed);
      this._ui.TermsOfUsePressed += new EventHandler<EventArgs>(this.ExecuteTermsOfUseWorkflow);
    }

    private void UnHookUIEvents()
    {
      this._ui.CancelPressed -= new EventHandler<EventArgs>(this.CancelPressed);
      this._ui.TermsOfUsePressed -= new EventHandler<EventArgs>(this.ExecuteTermsOfUseWorkflow);
    }

    private void CancelPressed(object sender, EventArgs args)
    {
      if (this._ui.BusyMode == BusyMode.Blocking)
        return;
      this.CancelAsyncInternal();
    }

    private void ExecuteRedeemWorkflow()
    {
      this._ui.Show(PurchaseUIMode.Redeem);
      PurchaseRedeemHelper.SetLastKnownLegalLocale(this._ui);
      this.BeginAsyncBusy_NonBlocking();
      this._cancelGamerContext = GamerContext.Acquire((Action<GamerContext, Exception, bool>) ((context, e1, gcCanceled) =>
      {
        this._cancelGamerContext = (Action) null;
        if (!this.EndAsyncBusy(gcCanceled, false))
          return;
        if (e1 != null)
        {
          this.HandleErrorMessage(e1, new Action(this.ExecuteRedeemWorkflow));
        }
        else
        {
          PurchaseRedeemHelper.SetLastKnownLegalLocale(this._ui);
          this.PromptForRedeemCode(string.Empty, string.Empty);
        }
      }));
    }

    private void ExecuteTermsOfUseWorkflow(object sender, EventArgs args)
    {
      this.BeginAsyncBusy_NonBlocking();
      PurchaseRedeemHelper.ShowMessageBox(this._ui, UIResources.TermsOfUseTitle, UIResources.TermsOfUseMessage, UIResources.Yes, UIResources.No, (Action<bool>) (yesPressed =>
      {
        if (!this.EndAsyncBusy(false, false) || !yesPressed)
          return;
        this.BeginAsyncBusy_NonBlocking();
        this._cancelGamerContext = GamerContext.Acquire((Action<GamerContext, Exception, bool>) ((gamerContext, e, gcCanceled) =>
        {
          this._cancelGamerContext = (Action) null;
          if (!this.EndAsyncBusy(gcCanceled, false))
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

    private void CompleteWithBrowserTask(Uri uri, string locale)
    {
      this.BeginAsyncBusy_NonBlocking();
      this._ui.DelayOperation((Action) (() =>
      {
        if (!this.EndAsyncBusy(false, false))
          return;
        this._postCompleteAction = PurchaseRedeemHelper.CompleteWithBrowserTask(uri, locale, this._titleId, this._sessionId, (Action<Exception>) (e => this.HandleError(e as ServiceFailureException)));
      }), 15);
    }

    protected bool PromptForRedeemCodeCore(string redeemCode)
    {
      bool flag = false;
      if (redeemCode == null)
        flag = true;
      else
        redeemCode = redeemCode.Trim();
      if (this.EndAsyncBusy(flag, flag))
      {
        if (this.ValidateRedeemFormat(ref redeemCode))
        {
          this.VerifyRedeemCode(redeemCode);
        }
        else
        {
          this._ui.BusyMode = BusyMode.Blocking;
          this._ui.DelayOperation((Action) (() => this.PromptForRedeemCode(UIResources.RedeemFormatInvalid, redeemCode)), 5);
        }
      }
      return flag;
    }

    protected virtual string GetRedeemInputSubtitle() => UIResources.RedeemInputSubtitle;

    protected void PromptForRedeemCode(string errorMessage, string defaultText)
    {
      this.BeginAsyncBusy_NonBlocking();
      if (!Guide.IsVisible)
      {
        string description = this.GetRedeemInputSubtitle();
        if (!string.IsNullOrEmpty(errorMessage))
          description = description + Environment.NewLine + "*" + errorMessage;
        if (description.Length > (int) byte.MaxValue)
          description = description.Substring(0, (int) byte.MaxValue);
        Guide.BeginShowKeyboardInput(PlayerIndex.One, UIResources.RedeemInputTitle, description, defaultText, (AsyncCallback) (result =>
        {
          bool flag = false;
          try
          {
            this.PromptForRedeemCodeCore(Guide.EndShowKeyboardInput(result));
          }
          catch (Exception ex)
          {
            if (flag)
              return;
            this.HandleErrorMessage(ex, (Action) (() => this.PromptForRedeemCode(errorMessage, defaultText)));
          }
        }), (object) null);
      }
      else
        this._ui.DelayOperation((Action) (() => this.PromptForRedeemCode(errorMessage, defaultText)), 1);
    }

    private bool ValidateRedeemFormat(ref string redeemCode)
    {
      if (RedeemRequest.redeemFormat1.IsMatch(redeemCode))
        return true;
      Match match = RedeemRequest.redeemFormat2.Match(redeemCode);
      if (!match.Success)
        return false;
      redeemCode = match.Groups.Cast<Group>().Skip<Group>(1).Select<Group, string>((Func<Group, string>) (g => g.Value)).Aggregate<string>((Func<string, string, string>) ((a, b) => a + "-" + b));
      return true;
    }

    private void VerifyRedeemCode(string redeemCode)
    {
      this.BeginAsyncBusy_Blocking();
      this._cancelGamerContext = GamerContext.Acquire((Action<GamerContext, Exception, bool>) ((gamerContext, e1, gcCanceled) =>
      {
        this._cancelGamerContext = (Action) null;
        if (!this.EndAsyncBusy(gcCanceled, false))
          return;
        if (e1 != null)
        {
          this.HandleErrorMessage(e1, (Action) (() => this.VerifyRedeemCode(redeemCode)));
        }
        else
        {
          if (this._ownsUI)
            PurchaseRedeemHelper.SetLastKnownLegalLocale(this._ui);
          this.BeginAsyncBusy_Blocking();
          this._cancelPartnerToken = GamerContext.GetPartnerToken(false, (Action<string, Exception, bool>) ((token, e2, ptCanceled) =>
          {
            this._cancelPartnerToken = (Action) null;
            if (!this.EndAsyncBusy(ptCanceled, false))
              return;
            if (e2 != null)
            {
              this.HandleErrorMessage(e2, (Action) (() => this.VerifyRedeemCode(redeemCode)));
            }
            else
            {
              try
              {
                this.VerifyRedeemCodeCore(redeemCode, gamerContext, token);
              }
              catch (Exception ex)
              {
                this.HandleErrorMessage(ex, (Action) (() => this.VerifyRedeemCode(redeemCode)));
              }
            }
          }));
        }
      }));
    }

    private void VerifyRedeemCodeCore(string redeemCode, GamerContext gamerContext, string token)
    {
      this._verifyRedeemRequest = new SecureLIVEnRequest(this.GetVerifyRedeemUri(redeemCode))
      {
        PartnerToken = token,
        Locale = gamerContext.LegalLocale
      };
      this.BeginAsyncBusy_Blocking();
      this._verifyRedeemRequest.GetResponseCompleted += (EventHandler<GetResponseCompletedEventArgs>) ((s, e) =>
      {
        this._verifyRedeemRequest = (SecureLIVEnRequest) null;
        if (!this.EndAsyncBusy(e.Cancelled, false))
          return;
        if (e.Error != null)
        {
          this.HandleErrorMessage(e.Error, (Action) (() => this.HandleInvalidRedeem(redeemCode)));
        }
        else
        {
          try
          {
            this.GetOfferFromTokenOffer(new TokenOfferParser().Parse(e.Response), redeemCode);
          }
          catch (Exception ex)
          {
            this.HandleErrorMessage(ex, (Action) (() => this.VerifyRedeemCode(redeemCode)));
          }
        }
      });
      this._verifyRedeemRequest.GetResponseAsync(this._userState);
    }

    protected virtual string CreateUriFromRedeemCode(string redeemCode)
    {
      return string.Format("{0}?billingToken={1}&storeId={2}", (object) "verifytoken", (object) redeemCode, (object) 5);
    }

    private Uri GetVerifyRedeemUri(string redeemCode)
    {
      string uriFromRedeemCode = this.CreateUriFromRedeemCode(redeemCode);
      return HttpRequest.SafeCombine(GamerContext.MarketplaceUrl, uriFromRedeemCode);
    }

    protected virtual void ReturnExistingAssetIdAndPointsInReceipt()
    {
    }

    protected virtual void PresentOfferToUser(Offer offer, string redeemCode)
    {
      this.PresentRedeemOffer(offer, redeemCode);
    }

    protected virtual void EnumerateTokenOfferByOfferId(int titleId, Guid offerId)
    {
      this._offerRequest.EnumerateTokenOffersByOfferIdAsync(titleId, 1, 1, (IEnumerable<Guid>) new Guid[1]
      {
        offerId
      });
    }

    protected virtual OfferEnumerationRequest CreateOfferRequestObject()
    {
      return new OfferEnumerationRequest();
    }

    protected virtual bool CheckTitleId(Offer offer, uint titleId, string redeemCode)
    {
      if ((long) offer.TitleId == (long) titleId)
        return true;
      this.HandleTokenFromDifferentTitle(redeemCode);
      return false;
    }

    private void GetOfferFromTokenOffer(TokenOffer tokenOffer, string redeemCode)
    {
      if (this.ExistingOffer != null && tokenOffer.EmsOffer != null && tokenOffer.EmsOffer.OfferId == this.ExistingOffer.OfferId)
      {
        this.ReturnExistingAssetIdAndPointsInReceipt();
        this.PresentRedeemOffer(this.ExistingOffer, redeemCode);
      }
      else if (tokenOffer.EmsOffer != null)
      {
        this.BeginAsyncBusy_Blocking();
        this._offerRequest = this.CreateOfferRequestObject();
        this._offerRequest.EnumerateOffersCompleted += (EventHandler<EnumerateOffersCompletedEventArgs>) ((s, e) =>
        {
          this._offerRequest = (OfferEnumerationRequest) null;
          if (!this.EndAsyncBusy(e.Cancelled, false))
            return;
          if (e.Error != null)
            this.HandleErrorMessage(e.Error, (Action) (() => this.HandleOfferNotFound(redeemCode)));
          else if (e.Offers == null || e.Offers.Count<Offer>() < 1)
          {
            this.HandleOfferNotFound(redeemCode);
          }
          else
          {
            if (!this.CheckTitleId(e.Offers.First<Offer>(), this._titleId, redeemCode))
              return;
            this.PresentOfferToUser(e.Offers.First<Offer>(), redeemCode);
          }
        });
        this.EnumerateTokenOfferByOfferId(0, tokenOffer.EmsOffer.OfferId);
      }
      else
        this.HandleOfferNotFound(redeemCode);
    }

    protected void HandleInvalidRedeem(string redeemCode)
    {
      this.PromptForRedeemCode(UIResources.RedeemInvalidMessage, redeemCode);
    }

    private void HandleOfferNotFound(string redeemCode)
    {
      this.PromptForRedeemCode(UIResources.RedeemOfferNotFound, redeemCode);
    }

    private void HandleTokenFromDifferentTitle(string redeemCode)
    {
      this.PromptForRedeemCode(UIResources.RedeemTokenFromDifferentTitle, redeemCode);
    }

    protected virtual string GetOfferString(Offer offer)
    {
      string str = offer.DownloadSize == 0L ? string.Empty : Math.Max(1, (int) offer.DownloadSize / 1024).ToString() + "KB";
      return string.Format(UIResources.RedeemOfferMessage, (object) offer.OfferName, (object) str);
    }

    protected void PresentRedeemOffer(Offer offer, string redeemCode)
    {
      string offerString = this.GetOfferString(offer);
      this.BeginAsyncBusy_NonBlocking();
      PurchaseRedeemHelper.ShowMessageBox(this._ui, UIResources.RedeemOfferTitle, offerString, UIResources.Yes, UIResources.No, (Action<bool>) (pressed =>
      {
        if (!this.EndAsyncBusy(!pressed, !pressed) || !pressed)
          return;
        this.RedeemOffer(offer, redeemCode);
      }));
    }

    private void RedeemOffer(Offer offer, string redeemCode)
    {
      this.BeginAsyncBusy_Blocking();
      this._cancelGamerContext = GamerContext.Acquire((Action<GamerContext, Exception, bool>) ((gamerContext, e1, gcCanceled) =>
      {
        this._cancelGamerContext = (Action) null;
        if (!this.EndAsyncBusy(gcCanceled, false))
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
            if (!this.EndAsyncBusy(ptCanceled, false))
              return;
            if (e2 != null)
            {
              this.HandleErrorMessage(e2, (Action) null);
            }
            else
            {
              try
              {
                this.BeginAsyncBusy_Blocking();
                Stream purchaseRequestStream = PurchaseRedeemHelper.CreatePurchaseRequestStream(offer, this._transactionId, redeemCode);
                this._redeemRequest = new SecureLIVEnRequest(PurchaseRedeemHelper.GetPurchaseUri())
                {
                  PartnerToken = shortLivedToken,
                  Locale = gamerContext.LegalLocale,
                  Method = "POST",
                  RequestStream = purchaseRequestStream
                };
                this._redeemRequest.GetResponseCompleted += new EventHandler<GetResponseCompletedEventArgs>(this.HandleRedeemResponse);
                this._redeemRequest.GetResponseAsync((object) (Action) (() => this.RedeemOffer(offer, redeemCode)));
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

    protected virtual PurchaseReceipt ParseReceipt(Stream response)
    {
      return new PurchaseReceiptParser().Parse(response);
    }

    private void HandleRedeemResponse(object sender, GetResponseCompletedEventArgs e)
    {
      this._redeemRequest = (SecureLIVEnRequest) null;
      if (!this.EndAsyncBusy(e.Cancelled, false))
        return;
      if (e.Error != null)
      {
        this.HandleErrorMessage(e.Error, (Action) null);
      }
      else
      {
        PurchaseReceipt receipt;
        try
        {
          receipt = this.ParseReceipt(e.Response);
        }
        catch (Exception ex)
        {
          this.HandleErrorMessage(ex, (Action) null);
          return;
        }
        this.HandleSuccess(receipt);
      }
    }

    private void ResetRequestState()
    {
      lock (this._syncObject)
      {
        this.IsBusy = false;
        this._userState = (object) null;
        this._cancelRequested = false;
        this._asyncCancel = false;
        this._syncContext = (SynchronizationContext) null;
        this._verifyRedeemRequest = (SecureLIVEnRequest) null;
        this._redeemRequest = (SecureLIVEnRequest) null;
        this._offerRequest = (OfferEnumerationRequest) null;
        this._cancelGamerContext = (Action) null;
        this._cancelPartnerToken = (Action) null;
        this.RemoveUI();
      }
    }

    private void RemoveUI()
    {
      if (!this._ownsUI || this._ui == null)
        return;
      if (this._ui.IsActive)
      {
        this.UnHookUIEvents();
        this._ui.Remove();
      }
      this._ui = (IPurchaseRequestUI) null;
    }

    protected void HandleErrorMessage(Exception e, Action retryAction)
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
        if (!this.EndAsyncBusy(false, !retryPressed))
          return;
        if (retryAction != null && retryPressed)
          retryAction();
        else
          this.HandleError(sfe);
      }));
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

    private bool EndAsyncBusy(bool requestCanceled, bool canceledByUser)
    {
      if (this._ui != null)
        this._ui.BusyMode = BusyMode.None;
      this._asyncCancel = false;
      if (!this.IsCancelRequested && !requestCanceled)
        return true;
      this.HandleCancel(!this.IsCancelRequested || canceledByUser);
      return false;
    }

    private void HandleSuccess(PurchaseReceipt purchaseReceipt)
    {
      this.RaiseShowRedeemCompleted(new PurchaseCompletedEventArgs((Exception) null, false, this._userState)
      {
        PurchaseReceipt = purchaseReceipt,
        PurchaseOffer = this._offer,
        TransactionId = this._transactionId
      });
    }

    private void HandleError(ServiceFailureException e)
    {
      if (!this._ownsUI)
        this.EndAsyncBusy(false, false);
      this.RaiseShowRedeemCompleted(new PurchaseCompletedEventArgs((Exception) e, false, this._userState)
      {
        PurchaseOffer = this._offer,
        TransactionId = this._transactionId
      });
    }

    private void HandleCancel(bool canceledByUser)
    {
      this.RaiseShowRedeemCompleted(new PurchaseCompletedEventArgs((Exception) null, true, this._userState)
      {
        CanceledByUser = canceledByUser,
        PurchaseOffer = this._offer,
        TransactionId = this._transactionId
      });
    }

    private bool IsCancelRequested
    {
      get
      {
        lock (this._syncObject)
          return this._cancelRequested;
      }
    }

    protected void DoMutableAction(Action action)
    {
      lock (this._syncObject)
      {
        if (this.IsBusy)
          throw new InvalidOperationException(NonLocalizedResources.ObjectCannotBeModified);
      }
      action();
    }
  }
}
