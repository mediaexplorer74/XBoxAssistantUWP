// *********************************************************
// Type: Leet.Silverlight.RESTProxy.MobileLogging
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;


namespace Leet.Silverlight.RESTProxy
{
  public class MobileLogging : ILogging
  {
    public void Dump<T>(T o)
    {
      HttpWebRequest req = (object) o as HttpWebRequest;
      HttpWebResponse resp = (object) o as HttpWebResponse;
      if (req != null)
        MobileLogging.Dump(req);
      else if (resp != null)
        MobileLogging.Dump(resp);
      else
        MobileLogging.DumpInternal(MobileLogging.GetString((object) o), 2);
    }

    [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Don't want our logging to cause an unexpected failure.")]
    private static void Dump(HttpWebRequest req)
    {
      try
      {
        MobileLogging.DumpInternal("Dumping HttpWebRequest", 3);
        if (req == null)
          return;
        foreach (string allKey in req.Headers.AllKeys)
          ;
      }
      catch (Exception ex)
      {
        MobileLogging.DumpInternal(ex.ToString(), 1);
      }
    }

    [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Don't want our logging to cause an unexpected failure.")]
    private static void Dump(HttpWebResponse resp)
    {
      try
      {
        MobileLogging.DumpInternal("Dumping HttpWebResponse", 3);
        if (resp == null)
          return;
        foreach (string allKey in resp.Headers.AllKeys)
          ;
      }
      catch (Exception ex)
      {
        MobileLogging.DumpInternal(ex.ToString(), 1);
      }
    }

    private static void DumpInternal(string s, int framesToSkip)
    {
      new StackTrace().GetFrame(framesToSkip);
    }

    private static string GetString(object o) => o == null ? "<null>" : o.ToString();
  }
}
