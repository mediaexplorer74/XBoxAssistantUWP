// *********************************************************
// Type: Microsoft.Phone.Marketplace.SignedDocumentEventArgs
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;
using System.ComponentModel;
using System.IO;


namespace Microsoft.Phone.Marketplace
{
  public class SignedDocumentEventArgs : AsyncCompletedEventArgs
  {
    internal SignedDocumentEventArgs(Exception error, bool cancelled, object userState)
      : base(error, cancelled, userState)
    {
    }

    public Stream SignedDocument { get; internal set; }
  }
}
