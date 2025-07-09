// *********************************************************
// Type: Xbox.Live.Phone.Utils.PerfTracer
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using System.Diagnostics;


namespace Xbox.Live.Phone.Utils
{
  public class PerfTracer
  {
    private const string ComponentName = "PerfTracer";
    private Stopwatch watch;
    private long startMilliseconds;

    public PerfTracer() => this.watch = new Stopwatch();

    [Conditional("DEBUG")]
    public void Start()
    {
      this.watch.Reset();
      this.watch.Start();
      this.startMilliseconds = this.watch.ElapsedMilliseconds;
    }

    [Conditional("DEBUG")]
    public void Stop() => this.watch.Stop();

    [Conditional("DEBUG")]
    public void TracePerfCheckpoint(string message)
    {
      this.startMilliseconds = this.watch.ElapsedMilliseconds;
    }
  }
}
