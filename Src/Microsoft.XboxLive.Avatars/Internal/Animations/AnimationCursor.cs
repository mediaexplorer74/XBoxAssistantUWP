// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Animations.AnimationCursor
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll


namespace Microsoft.XboxLive.Avatars.Internal.Animations
{
  public class AnimationCursor
  {
    private AnimationPlayMode playMode;
    private float replaySpeed;
    private float localTime;

    public AnimationPlayMode PlayMode => this.playMode;

    public float Speed => this.replaySpeed;

    public float Time
    {
      get => this.localTime;
      set => this.localTime = value;
    }

    public AnimationCursor(float speed, AnimationPlayMode mode)
    {
      this.playMode = mode;
      this.replaySpeed = speed;
    }
  }
}
