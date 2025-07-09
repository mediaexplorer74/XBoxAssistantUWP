// *********************************************************
// Type: Xbox.Live.Phone.Utils.Performance.Framerate
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using Microsoft.Xna.Framework;
using System;


namespace Xbox.Live.Phone.Utils.Performance
{
  public class Framerate
  {
    public const int FilterFrames = 15;
    private const int ResetLowFpsMarker = 60;
    private static Framerate staticInstance;
    private CircularBuffer<int> fpsBuffer = new CircularBuffer<int>(15);

    public static Framerate Instance
    {
      get
      {
        if (Framerate.staticInstance == null)
          Framerate.staticInstance = new Framerate();
        return Framerate.staticInstance;
      }
    }

    public int InstantFps { get; private set; }

    public int LowFps { get; set; }

    public int AverageFps { get; private set; }

    public void ResetLowFps() => this.LowFps = 60;

    public void SetLast(GameTime time)
    {
      if (time == null)
        throw new ArgumentNullException(nameof (time));
      this.InstantFps = (int) (1.0 / time.ElapsedGameTime.TotalSeconds);
      if (this.InstantFps < this.LowFps)
        this.LowFps = this.InstantFps;
      this.fpsBuffer.Add(this.InstantFps);
      int num = 0;
      for (int i = 0; i < this.fpsBuffer.Count; ++i)
        num += this.fpsBuffer[i];
      this.AverageFps = num / this.fpsBuffer.Count;
    }
  }
}
