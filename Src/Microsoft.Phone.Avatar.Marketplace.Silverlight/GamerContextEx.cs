// *********************************************************
// Type: Microsoft.Phone.Marketplace.GamerContextEx
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using Microsoft.Phone.Marketplace.Resources;
using System;
using System.ComponentModel;
using System.Threading;


namespace Microsoft.Phone.Marketplace
{
  public class GamerContextEx
  {
    private SynchronizationContext _syncContext;
    private object _syncObject = new object();
    private EventHandler<AsyncCompletedEventArgs> _getGamerContextCompleted;

    public event EventHandler<AsyncCompletedEventArgs> GetGamerContextCompleted
    {
      add => this.DoMutableAction((Action) (() => this._getGamerContextCompleted += value));
      remove => this.DoMutableAction((Action) (() => this._getGamerContextCompleted -= value));
    }

    public void GetGamerContextAsync()
    {
      this._syncContext = SynchronizationContext.Current;
      GamerContext.Acquire((Action<GamerContext, Exception, bool>) ((gc, error, cancelled) =>
      {
        if (this.HandleCancelAndError(cancelled, error))
          return;
        this.LegalLocale = gc.LegalLocale;
        this.HandleSuccess();
      }));
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

    private void RaiseEnumerateGamerContextCompleted(AsyncCompletedEventArgs e)
    {
      SynchronizationContext syncContext = this._syncContext;
      this.ResetRequestState();
      EventHandler<AsyncCompletedEventArgs> getGamerContextCompleted = this._getGamerContextCompleted;
      if (getGamerContextCompleted == null)
        return;
      if (syncContext == null)
        getGamerContextCompleted((object) this, e);
      else
        syncContext.Post((SendOrPostCallback) (state => getGamerContextCompleted((object) this, e)), (object) null);
    }

    private bool HandleCancelAndError(bool canceled, Exception error)
    {
      if (canceled)
      {
        this.HandleCancel();
        return true;
      }
      if (error == null)
        return false;
      this.HandleError(error);
      return true;
    }

    private void HandleSuccess()
    {
      this.RaiseEnumerateGamerContextCompleted(new AsyncCompletedEventArgs((Exception) null, false, (object) null));
    }

    private void HandleError(Exception e)
    {
      e = (Exception) ExceptionHelper.Transform(e);
      this.RaiseEnumerateGamerContextCompleted(new AsyncCompletedEventArgs(e, false, (object) null));
    }

    private void HandleCancel()
    {
      this.RaiseEnumerateGamerContextCompleted((AsyncCompletedEventArgs) new EnumerateCategoriesCompletedEventArgs((Exception) null, true, (object) null));
    }

    private void ResetRequestState()
    {
      lock (this._syncObject)
        this._syncContext = (SynchronizationContext) null;
    }

    public void InvalidateGamerContext() => GamerContext.InvalidateContext();

    public bool IsBusy { get; set; }

    public string LegalLocale { get; set; }
  }
}
