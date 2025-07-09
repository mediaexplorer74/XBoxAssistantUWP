// *********************************************************
// Type: Microsoft.Phone.Marketplace.Tracer
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System.Diagnostics;


namespace Microsoft.Phone.Marketplace
{
  internal class Tracer
  {
    [Conditional("DEBUG")]
    public static void WriteLine(string message, params object[] args)
    {
      message = args.Length > 0 ? string.Format(message, args) : message;
    }
  }
}
