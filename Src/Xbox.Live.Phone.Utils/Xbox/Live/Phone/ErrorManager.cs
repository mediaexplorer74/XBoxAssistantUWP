// *********************************************************
// Type: Xbox.Live.Phone.ErrorManager
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using Microsoft.Xna.Framework.GamerServices;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Xbox.Live.Phone.Utils;
using Xbox.Live.Phone.Utils.Resources;


namespace Xbox.Live.Phone
{
  public class ErrorManager : IErrorManager
  {
    private const string ComponentName = "ErrorManager";
    private const int MaxErrorMessageLength = 255;
    private static ErrorManager staticInstance;

    public static ErrorManager Instance
    {
      get
      {
        if (ErrorManager.staticInstance == null)
          ErrorManager.staticInstance = new ErrorManager();
        return ErrorManager.staticInstance;
      }
    }

    [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This method declaration should be consistent with similar methods.")]
    public void Fatal(string message) => throw new NotImplementedException();

    [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This method declaration should be consistent with similar methods.")]
    public void Info(string message)
    {
    }

    public void Nonfatal(string message) => this.Nonfatal(Resource.ErrorNonfatalError, message);

    public void Nonfatal(string title, string message)
    {
      this.Nonfatal(title, message, (Action) null);
    }

    public void Nonfatal(string message, Action callback)
    {
      this.Nonfatal(Resource.ErrorNonfatalError, message, callback);
    }

    public void Nonfatal(string title, string body, Action callback)
    {
      if (title == null)
        throw new ArgumentNullException(title);
      if (body == null)
        throw new ArgumentNullException(body);
      string errorMessage = body.Length > (int) byte.MaxValue ? body.Substring(0, (int) byte.MaxValue) : body;
      this.ShowErrorMessage(title, errorMessage, callback);
    }

    private void NonfatalCallback(IAsyncResult result)
    {
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        GuideWrapper.EndShowMessageBox(result);
        if (!(result.AsyncState is Action asyncState2))
          return;
        asyncState2();
      }, (object) this);
    }

    private void ShowErrorMessage(string title, string errorMessage, Action callback)
    {
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        GuideWrapper.BeginShowMessageBox(title, errorMessage, new List<string>()
        {
          Resource.OkButton_Text
        }, 0, MessageBoxIcon.Warning, new AsyncCallback(this.NonfatalCallback), (object) callback);
      }, (object) this);
    }
  }
}
