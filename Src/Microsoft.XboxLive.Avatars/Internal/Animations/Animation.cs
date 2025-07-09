// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Animations.Animation
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll


namespace Microsoft.XboxLive.Avatars.Internal.Animations
{
  public class Animation
  {
    private float length;
    private int frameCount;
    private float fps;

    public AnimationPlayMode ClipPlayMode { get; set; }

    public float Length => this.length;

    public float Fps => this.fps;

    public int FrameCount => this.frameCount;

    internal Animation(int frameCount, float framerate)
    {
      this.frameCount = frameCount;
      this.fps = framerate;
      this.ClipPlayMode = AnimationPlayMode.Once;
      this.length = (float) frameCount / this.fps;
    }
  }
}
