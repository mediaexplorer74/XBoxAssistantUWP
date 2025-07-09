// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.AssetsAsyncLoadContextCommon
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System.Threading;


namespace Microsoft.XboxLive.Avatars.Internal
{
  internal class AssetsAsyncLoadContextCommon
  {
    public int numRequests;
    public bool failed;
    public AutoResetEvent syncEvent;
    public object syncRequestLock = new object();
  }
}
