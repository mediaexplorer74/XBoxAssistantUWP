// *********************************************************
// Type: Xbox.Live.Phone.GuideWrapper
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;


namespace Xbox.Live.Phone
{
  public class GuideWrapper
  {
    private const string ComponentName = "GuideWrapper";
    private const int MaxInputTextLength = 255;
    private static GuideWrapper instance = new GuideWrapper();
    private List<GuideWrapper.GuideQueueEntry> guideEntriesQueue = new List<GuideWrapper.GuideQueueEntry>();

    private GuideWrapper()
    {
    }

    public static int QueueLength
    {
      get
      {
        return GuideWrapper.instance.guideEntriesQueue != null ? GuideWrapper.instance.guideEntriesQueue.Count : 0;
      }
    }

    public static IAsyncResult BeginShowKeyboardInput(
      string title,
      string description,
      string defaultText,
      AsyncCallback callback,
      object state)
    {
      IAsyncResult asyncResult = (IAsyncResult) null;
      if (!string.IsNullOrEmpty(defaultText) && defaultText.Length > (int) byte.MaxValue)
        defaultText = defaultText.Substring(0, (int) byte.MaxValue);
      if (Guide.IsVisible)
        GuideWrapper.Enqueue((GuideWrapper.GuideQueueEntry) new GuideWrapper.InputQueueEntry(title, description, defaultText, callback, state));
      else
        asyncResult = Guide.BeginShowKeyboardInput(PlayerIndex.One, title, description, defaultText, callback, state);
      return asyncResult;
    }

    public static string EndShowKeyboardInput(IAsyncResult result)
    {
      return result != null ? Guide.EndShowKeyboardInput(result) : throw new ArgumentNullException(nameof (result));
    }

    [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Matching API call for the Guide.")]
    public static IAsyncResult BeginShowMessageBox(
      string title,
      string details,
      List<string> buttons,
      int focusButton,
      MessageBoxIcon icon,
      AsyncCallback callback,
      object state)
    {
      IAsyncResult asyncResult = (IAsyncResult) null;
      if (Guide.IsVisible)
        GuideWrapper.Enqueue((GuideWrapper.GuideQueueEntry) new GuideWrapper.MessageBoxQueueEntry(title, details, buttons, focusButton, icon, callback, state));
      else
        asyncResult = Guide.BeginShowMessageBox(title, details, (IEnumerable<string>) buttons, focusButton, icon, callback, state);
      return asyncResult;
    }

    public static int? EndShowMessageBox(IAsyncResult result)
    {
      return result != null ? Guide.EndShowMessageBox(result) : throw new ArgumentNullException(nameof (result));
    }

    [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Justification = "This style prevents unncessary initialize of objects.")]
    public static void Dequeue()
    {
      List<GuideWrapper.GuideQueueEntry> guideEntriesQueue = GuideWrapper.instance.guideEntriesQueue;
      if (guideEntriesQueue == null || guideEntriesQueue.Count <= 0)
        return;
      GuideWrapper.GuideQueueEntry guideQueueEntry = guideEntriesQueue[0];
      guideEntriesQueue.RemoveAt(0);
      switch (guideQueueEntry)
      {
        case GuideWrapper.InputQueueEntry _:
          GuideWrapper.InputQueueEntry inputQueueEntry = (GuideWrapper.InputQueueEntry) guideQueueEntry;
          GuideWrapper.BeginShowKeyboardInput(inputQueueEntry.Title, inputQueueEntry.Details, inputQueueEntry.DefaultText, inputQueueEntry.Callback, inputQueueEntry.StateData);
          break;
        case GuideWrapper.MessageBoxQueueEntry _:
          GuideWrapper.MessageBoxQueueEntry messageBoxQueueEntry = (GuideWrapper.MessageBoxQueueEntry) guideQueueEntry;
          GuideWrapper.BeginShowMessageBox(messageBoxQueueEntry.Title, messageBoxQueueEntry.Details, messageBoxQueueEntry.ButtonsText, messageBoxQueueEntry.FocusButton, messageBoxQueueEntry.Icon, messageBoxQueueEntry.Callback, messageBoxQueueEntry.StateData);
          break;
        default:
          throw new NotSupportedException("This entry type " + (object) guideQueueEntry.GetType() + " is not supported.");
      }
    }

    private static void Enqueue(GuideWrapper.GuideQueueEntry entry)
    {
      GuideWrapper.instance.guideEntriesQueue.Add(entry);
    }

    private class GuideQueueEntry
    {
      public GuideQueueEntry(
        string title,
        string details,
        AsyncCallback callback,
        object stateData)
      {
        this.Title = title;
        this.Details = details;
        this.Callback = callback;
        this.StateData = stateData;
      }

      public string Title { get; set; }

      public string Details { get; set; }

      public object StateData { get; set; }

      public AsyncCallback Callback { get; set; }
    }

    private class InputQueueEntry : GuideWrapper.GuideQueueEntry
    {
      public InputQueueEntry(
        string title,
        string details,
        string defaultText,
        AsyncCallback callback,
        object stateData)
        : base(title, details, callback, stateData)
      {
        this.DefaultText = defaultText;
        this.Callback = callback;
      }

      public string DefaultText { get; set; }
    }

    private class MessageBoxQueueEntry : GuideWrapper.GuideQueueEntry
    {
      public MessageBoxQueueEntry(
        string title,
        string details,
        List<string> buttonsText,
        int focusButton,
        MessageBoxIcon icon,
        AsyncCallback callback,
        object stateData)
        : base(title, details, callback, stateData)
      {
        this.ButtonsText = buttonsText;
        this.Callback = callback;
        this.FocusButton = focusButton;
        this.Icon = icon;
      }

      public List<string> ButtonsText { get; set; }

      public int FocusButton { get; set; }

      public MessageBoxIcon Icon { get; set; }
    }
  }
}
