// *********************************************************
// Type: Microsoft.Phone.Marketplace.GetResponseCompletedEventArgs
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;
using System.ComponentModel;
using System.IO;


namespace Microsoft.Phone.Marketplace
{
  internal class GetResponseCompletedEventArgs : AsyncCompletedEventArgs
  {
    internal GetResponseCompletedEventArgs(Exception error, bool cancelled, object userState)
      : base(error, cancelled, userState)
    {
    }

    public Stream Response { get; internal set; }
  }
}
