// *********************************************************
// Type: Xbox.Live.Phone.Utils.OkCancelDialog
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using Microsoft.Xna.Framework.GamerServices;
using System;
using System.Collections.Generic;
using System.Threading;
using Xbox.Live.Phone.Utils.Resources;


namespace Xbox.Live.Phone.Utils
{
  public class OkCancelDialog
  {
    private string title;
    private string message;
    private SendOrPostCallback delegateOk;
    private SendOrPostCallback delegateCancel;

    public OkCancelDialog(
      string title,
      string message,
      SendOrPostCallback ok,
      SendOrPostCallback cancel)
      : this(title, message, Resource.OkButton_Text, Resource.CancelButton_Text, ok, cancel)
    {
    }

    public OkCancelDialog(
      string title,
      string message,
      string okayButtonText,
      string cancelButtonText,
      SendOrPostCallback ok,
      SendOrPostCallback cancel)
    {
      this.OkButtonText = okayButtonText;
      this.CancelButtonText = cancelButtonText;
      this.title = title;
      this.message = message;
      this.delegateOk = ok;
      this.delegateCancel = cancel;
    }

    public string OkButtonText { get; set; }

    public string CancelButtonText { get; set; }

    public void Start()
    {
      List<string> buttons = new List<string>();
      buttons.Add(this.OkButtonText);
      if (!string.IsNullOrEmpty(this.CancelButtonText))
        buttons.Add(this.CancelButtonText);
      GuideWrapper.BeginShowMessageBox(this.title, this.message, buttons, 0, MessageBoxIcon.Alert, new AsyncCallback(this.DialogCallback), (object) this);
    }

    private void DialogCallback(IAsyncResult result)
    {
      int? nullable1 = GuideWrapper.EndShowMessageBox(result);
      int num;
      if (nullable1.HasValue)
      {
        int? nullable2 = nullable1;
        num = nullable2.GetValueOrDefault() != 0 ? 0 : (nullable2.HasValue ? 1 : 0);
      }
      else
        num = 0;
      SendOrPostCallback callback = num != 0 ? this.delegateOk : this.delegateCancel;
      if (callback == null)
        return;
      ThreadManager.UIThreadPost(callback, (object) this);
    }

    private enum OkCancelTypes
    {
      Ok,
      Cancel,
    }
  }
}
