// *********************************************************
// Type: Microsoft.Phone.Marketplace.AssetRequest
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using Microsoft.Phone.Marketplace.Resources;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;


namespace Microsoft.Phone.Marketplace
{
  public class AssetRequest
  {
    private SynchronizationContext _syncContext;
    private EventHandler<EnumerateAssetsEventArgs> _enumerateAssetsCompleted;
    private EventHandler<SignedDocumentEventArgs> _enumerateSignedAssetsCompleted;
    private uint _titleId;
    private bool _signed;
    private bool _cancelRequested;
    private SecureLIVEnRequest _assetCountRequest;
    private Action _cancelGamerContext;
    private Action _cancelPartnerToken;
    private object _userState;
    private object _syncObject = new object();

    public event EventHandler<EnumerateAssetsEventArgs> EnumerateAssetsCompleted
    {
      add => this.DoMutableAction((Action) (() => this._enumerateAssetsCompleted += value));
      remove => this.DoMutableAction((Action) (() => this._enumerateAssetsCompleted -= value));
    }

    public event EventHandler<SignedDocumentEventArgs> EnumerateSignedAssetsCompleted
    {
      add => this.DoMutableAction((Action) (() => this._enumerateSignedAssetsCompleted += value));
      remove
      {
        this.DoMutableAction((Action) (() => this._enumerateSignedAssetsCompleted -= value));
      }
    }

    public void CancelAsync()
    {
      lock (this._syncObject)
      {
        if (!this.IsBusy)
          return;
        this._cancelRequested = true;
        if (this._assetCountRequest != null && this._assetCountRequest.IsBusy)
          this._assetCountRequest.CancelAsync();
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

    public void EnumerateAssetsAsync(int titleId)
    {
      this.EnumerateAssetsAsync(titleId, (object) null);
    }

    public void EnumerateAssetsAsync(int titleId, object userState)
    {
      if (this._enumerateAssetsCompleted == null)
        throw new InvalidOperationException(NonLocalizedResources.CompletedEventNotHooked);
      this.EnumerateAssetsAsyncCommon(titleId, userState, false);
    }

    public void EnumerateSignedAssetsAsync(int titleId)
    {
      this.EnumerateSignedAssetsAsync(titleId, (object) null);
    }

    public void EnumerateSignedAssetsAsync(int titleId, object userState)
    {
      if (this._enumerateSignedAssetsCompleted == null)
        throw new InvalidOperationException(NonLocalizedResources.CompletedEventNotHooked);
      this.EnumerateAssetsAsyncCommon(titleId, userState, true);
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

    private void RaiseEnumerateAssetsCompleted(EnumerateAssetsEventArgs e)
    {
      SynchronizationContext syncContext = this._syncContext;
      this.ResetRequestState();
      try
      {
        EventHandler<EnumerateAssetsEventArgs> enumerateAssetsCompleted = this._enumerateAssetsCompleted;
        if (enumerateAssetsCompleted == null)
          return;
        if (syncContext == null)
          enumerateAssetsCompleted((object) this, e);
        else
          syncContext.Post((SendOrPostCallback) (state => enumerateAssetsCompleted((object) this, e)), (object) null);
      }
      catch
      {
      }
    }

    private void RaiseEnumerateSignedAssetsCompleted(SignedDocumentEventArgs e)
    {
      SynchronizationContext syncContext = this._syncContext;
      this.ResetRequestState();
      try
      {
        EventHandler<SignedDocumentEventArgs> enumerateSignedAssetsCompleted = this._enumerateSignedAssetsCompleted;
        if (enumerateSignedAssetsCompleted == null)
          return;
        if (syncContext == null)
          enumerateSignedAssetsCompleted((object) this, e);
        else
          syncContext.Post((SendOrPostCallback) (state => enumerateSignedAssetsCompleted((object) this, e)), (object) null);
      }
      catch
      {
      }
    }

    private void EnumerateAssetsAsyncCommon(int titleId, object userState, bool signed)
    {
      if (this._enumerateAssetsCompleted == null && this._enumerateSignedAssetsCompleted == null)
        throw new InvalidOperationException(NonLocalizedResources.CompletedEventNotHooked);
      this.DoMutableAction((Action) (() => this.IsBusy = true));
      this._syncContext = SynchronizationContext.Current;
      this._userState = userState;
      this._signed = signed;
      this._titleId = (uint) titleId;
      this.EnumerateAssetsAsyncCore();
    }

    private void EnumerateAssetsAsyncCore()
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
            this.DoAssetRequest(gamerContext, token);
          }
          catch (Exception ex)
          {
            this.HandleCancelAndError(false, ex);
          }
        }));
      }));
    }

    private void DoAssetRequest(GamerContext gamerContext, string partnerToken)
    {
      this._assetCountRequest = new SecureLIVEnRequest(this.GetAssetCountUri())
      {
        PartnerToken = partnerToken,
        Locale = gamerContext.LegalLocale
      };
      this._assetCountRequest.GetResponseCompleted += new EventHandler<GetResponseCompletedEventArgs>(this.HandleAssetCountResponse);
      this._assetCountRequest.GetResponseAsync();
    }

    private void HandleAssetCountResponse(object sender, GetResponseCompletedEventArgs e)
    {
      this._assetCountRequest = (SecureLIVEnRequest) null;
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
        ReadOnlyCollection<Asset> assets;
        try
        {
          assets = new AssetCountParser().Parse(e.Response);
        }
        catch (Exception ex)
        {
          this.HandleCancelAndError(false, ex);
          return;
        }
        this.HandleSuccess(assets);
      }
    }

    private Uri GetAssetCountUri()
    {
      string relative = string.Format("{0}?titleId={1}&signAssets={2}", (object) "assets", (object) this._titleId, (object) this._signed);
      return HttpRequest.SafeCombine(GamerContext.MarketplaceUrl, relative);
    }

    private void ResetRequestState()
    {
      lock (this._syncObject)
      {
        this.IsBusy = false;
        this._assetCountRequest = (SecureLIVEnRequest) null;
        this._cancelRequested = false;
        this._userState = (object) null;
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

    private void HandleSuccess(ReadOnlyCollection<Asset> assets)
    {
      this.RaiseEnumerateAssetsCompleted(new EnumerateAssetsEventArgs((Exception) null, false, this._userState)
      {
        Assets = assets
      });
    }

    private void SignedHandleSuccess(Stream signedReceipts)
    {
      this.RaiseEnumerateSignedAssetsCompleted(new SignedDocumentEventArgs((Exception) null, false, this._userState)
      {
        SignedDocument = signedReceipts
      });
    }

    private void HandleError(Exception e)
    {
      e = (Exception) ExceptionHelper.Transform(e);
      if (this._signed)
        this.RaiseEnumerateSignedAssetsCompleted(new SignedDocumentEventArgs(e, false, this._userState));
      else
        this.RaiseEnumerateAssetsCompleted(new EnumerateAssetsEventArgs(e, false, this._userState));
    }

    private void HandleCancel()
    {
      if (this._signed)
        this.RaiseEnumerateSignedAssetsCompleted(new SignedDocumentEventArgs((Exception) null, true, this._userState));
      else
        this.RaiseEnumerateAssetsCompleted(new EnumerateAssetsEventArgs((Exception) null, true, this._userState));
    }
  }
}
