// *********************************************************
// Type: Xbox.Live.Phone.Utils.ThreadManager
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Windows;


namespace Xbox.Live.Phone.Utils
{
  public static class ThreadManager
  {
    private static object uiThreadPostLock = new object();

    public static ThreadManager.ThreadData UIThread { get; set; }

    public static void UIThreadPost(SendOrPostCallback callback, object state)
    {
      if (ThreadManager.UIThread.IsCurrentThread())
      {
        callback(state);
      }
      else
      {
        lock (ThreadManager.uiThreadPostLock)
          ((DependencyObject) Deployment.Current).Dispatcher.BeginInvoke((Delegate) callback, new object[1]
          {
            state
          });
      }
    }

    public static void UIThreadSend(SendOrPostCallback callback, object state)
    {
      if (ThreadManager.UIThread.IsCurrentThread())
      {
        callback(state);
      }
      else
      {
        ManualResetEvent stateReady = new ManualResetEvent(false);
        ThreadManager.UIThreadPost((SendOrPostCallback) (stateWrapper =>
        {
          try
          {
            callback(stateWrapper);
          }
          finally
          {
            stateReady.Set();
          }
        }), state);
        stateReady.WaitOne();
      }
    }

    [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Simpler for clarity when used elsewhere that this is nested.")]
    public class ThreadData
    {
      private int id;

      [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "context", Justification = "Used in another assembly.")]
      public ThreadData(SynchronizationContext context, int id) => this.id = id;

      public bool IsCurrentThread() => Thread.CurrentThread.ManagedThreadId == this.id;

      public void AssertIsCurrentThread()
      {
      }

      public void AssertIsNotCurrentThread()
      {
      }
    }
  }
}
