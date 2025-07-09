// *********************************************************
// Type: Microsoft.XMedia.EventLink.Client.DispatcherExtensions
// Assembly: Microsoft.XMedia.EventLink.Client.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946994A4-3A3C-41D3-A520-1292D57CD5ED
// *********************************************************Microsoft.XMedia.EventLink.Client.WinPhone.dll

using System;
using System.Windows.Threading;


namespace Microsoft.XMedia.EventLink.Client
{
  internal static class DispatcherExtensions
  {
    public static void BeginInvoke(this Dispatcher dispatcher, Action action)
    {
      if (dispatcher.CheckAccess())
        action();
      else
        dispatcher.BeginInvoke((Delegate) action, new object[0]);
    }

    public static void VerifyAccess(this Dispatcher dispatcher)
    {
      if (!dispatcher.CheckAccess())
        throw new InvalidOperationException();
    }
  }
}
