// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.DataRequestCompletedEventArgs
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;
using System.ComponentModel;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal
{
  public class DataRequestCompletedEventArgs(Exception error, bool canceled, object userState) : 
    AsyncCompletedEventArgs(error, canceled, userState)
  {
    public Stream Result { get; internal set; }

    public string SourceAddress { get; internal set; }
  }
}
