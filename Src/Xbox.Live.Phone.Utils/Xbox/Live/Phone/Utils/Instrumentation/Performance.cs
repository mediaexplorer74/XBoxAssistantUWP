// *********************************************************
// Type: Xbox.Live.Phone.Utils.Instrumentation.Performance
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using System.Collections.Generic;
using System.Diagnostics;


namespace Xbox.Live.Phone.Utils.Instrumentation
{
  public static class Performance
  {
    private const string ComponentName = "PERF";
    public const string Back = ":Back";
    public const string PageLoaded = "PageLoaded:";
    public const string Overall = "Overall:";
    public const string Parsing = ":Parsing";
    private static Dictionary<string, long> TrackingTable = new Dictionary<string, long>();
    public static Stopwatch Watch;

    [Conditional("PERF")]
    public static void Trace(Performance.MarkerType type, string marker)
    {
      long elapsedMilliseconds = Performance.Watch.ElapsedMilliseconds;
      switch (type)
      {
        case Performance.MarkerType.Begin:
          if (!Performance.TrackingTable.ContainsKey(marker))
          {
            Performance.TrackingTable.Add(marker, elapsedMilliseconds);
            break;
          }
          Performance.TrackingTable[marker] = elapsedMilliseconds;
          break;
        case Performance.MarkerType.End:
          if (!Performance.TrackingTable.ContainsKey(marker) && Performance.TrackingTable.ContainsKey(marker + ":Back"))
            marker += ":Back";
          if (!Performance.TrackingTable.ContainsKey(marker))
            break;
          long num = Performance.TrackingTable[marker];
          Performance.TrackingTable.Remove(marker);
          break;
      }
    }

    [Conditional("PERF")]
    public static void Start()
    {
      Performance.Watch = new Stopwatch();
      Performance.Watch.Start();
    }

    public enum MarkerType
    {
      Begin,
      End,
      Info,
    }
  }
}
