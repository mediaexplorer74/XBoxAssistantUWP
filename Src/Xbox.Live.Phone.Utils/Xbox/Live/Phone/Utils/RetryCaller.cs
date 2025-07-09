// *********************************************************
// Type: Xbox.Live.Phone.Utils.RetryCaller
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using System;
using System.Net;
using System.Threading;


namespace Xbox.Live.Phone.Utils
{
  public class RetryCaller
  {
    private const string ComponentName = "RetryCaller";
    private const int MaxRetryCount = 3;

    public static void Retry(SendOrPostCallback operation, object state)
    {
      int num = 0;
      if (operation == null)
        throw new ArgumentNullException(nameof (operation));
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      while (true)
      {
        try
        {
          operation(state);
          break;
        }
        catch (Exception ex)
        {
          WebException webException = ex as WebException;
          XMobileException xmobileException = ex as XMobileException;
          if ((webException != null && webException.Status == WebExceptionStatus.RequestCanceled || xmobileException != null) && num < 3)
          {
            Thread.Sleep((num <= 0 ? 1 : num * 2) * 1000);
            ++num;
          }
          else
            throw;
        }
      }
    }
  }
}
