// *********************************************************
// Type: Leet.Silverlight.RESTProxy.Logging
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System.Diagnostics.CodeAnalysis;


namespace Leet.Silverlight.RESTProxy
{
  public static class Logging
  {
    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Actually initialized if typedef is set.")]
    private static ILogging CurrentLogger { get; set; }

    public static void Dump<T>(T o)
    {
      if (Logging.CurrentLogger == null)
        return;
      Logging.CurrentLogger.Dump<T>(o);
    }
  }
}
