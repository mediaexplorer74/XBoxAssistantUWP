// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.AvatarManifestCreator
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Version1;
using System;
using System.ComponentModel;
using System.Threading;


namespace Microsoft.XboxLive.Avatars.Internal
{
  public class AvatarManifestCreator
  {
    private void CreateRandomMnifestWorker(object state)
    {
      AvatarManifestCreator.AvatarManifestCreatorState manifestCreatorState = state as AvatarManifestCreator.AvatarManifestCreatorState;
      Exception e = (Exception) null;
      AvatarManifest[] randomManifests = (AvatarManifest[]) null;
      try
      {
        randomManifests = new RandomAvatar().CreateAvatars(manifestCreatorState.BodyMask, manifestCreatorState.AvatarsCount);
      }
      catch (Exception ex)
      {
        e = ex;
      }
      CreateRandomManifestCompletedEventArgs completedEventArgs = new CreateRandomManifestCompletedEventArgs(e, randomManifests);
      manifestCreatorState.AsyncOp.PostOperationCompleted(new SendOrPostCallback(this.OnCreateRandomCompleted), (object) completedEventArgs);
    }

    private void OnCreateRandomCompleted(object operationState)
    {
      CreateRandomManifestCompletedEventArgs e = operationState as CreateRandomManifestCompletedEventArgs;
      if (this.CreateRandomCompleted == null)
        return;
      this.CreateRandomCompleted((object) this, e);
    }

    public event CreateRandomManifestCompletedEventHandler CreateRandomCompleted;

    public void CreateRandomAsync(AvatarGender bodyMask, int avatarsCount)
    {
      ThreadPool.QueueUserWorkItem(new WaitCallback(this.CreateRandomMnifestWorker), (object) new AvatarManifestCreator.AvatarManifestCreatorState(AsyncOperationManager.CreateOperation((object) null))
      {
        AvatarsCount = avatarsCount,
        BodyMask = bodyMask
      });
    }

    private class AvatarManifestCreatorState
    {
      private AsyncOperation m_asyncOp;

      internal int AvatarsCount { get; set; }

      internal AvatarGender BodyMask { get; set; }

      internal AsyncOperation AsyncOp => this.m_asyncOp;

      internal AvatarManifestCreatorState(AsyncOperation asyncOp) => this.m_asyncOp = asyncOp;
    }
  }
}
