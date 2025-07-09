// *********************************************************
// Type: com.omniture.AppMeasurement_SetInterval
// Assembly: AppMeasurement_Silverlight_WP, Version=1.3.7.0, Culture=neutral, PublicKeyToken=null
// MVID: B2938048-604D-4B3E-B432-7854B0CBA8DA
// *********************************************************AppMeasurement_Silverlight_WP.dll

using System;
using System.Collections.Generic;
using System.Windows.Threading;


namespace com.omniture
{
  public class AppMeasurement_SetInterval
  {
    private static List<object> intervalList = new List<object>();
    private static List<object> intervalHandlerList = new List<object>();

    public static int setInterval(EventHandler handler, int intervalTime)
    {
      DispatcherTimer dispatcherTimer = new DispatcherTimer();
      dispatcherTimer.Interval = TimeSpan.FromMilliseconds((double) intervalTime);
      dispatcherTimer.Tick += handler;
      dispatcherTimer.Start();
      int index;
      for (index = 0; index < AppMeasurement_SetInterval.intervalList.Count; ++index)
      {
        if (AppMeasurement_SetInterval.intervalList[index] == null)
        {
          AppMeasurement_SetInterval.intervalList[index] = (object) dispatcherTimer;
          AppMeasurement_SetInterval.intervalHandlerList[index] = (object) handler;
          return index + 1;
        }
      }
      AppMeasurement_SetInterval.intervalList.Add((object) dispatcherTimer);
      AppMeasurement_SetInterval.intervalHandlerList.Add((object) handler);
      return index + 1;
    }

    public static void clearInterval(int intervalNum)
    {
      --intervalNum;
      if (AppMeasurement_SetInterval.intervalList[intervalNum] == null)
        return;
      DispatcherTimer interval = (DispatcherTimer) AppMeasurement_SetInterval.intervalList[intervalNum];
      EventHandler intervalHandler = (EventHandler) AppMeasurement_SetInterval.intervalHandlerList[intervalNum];
      AppMeasurement_SetInterval.intervalHandlerList[intervalNum] = (object) null;
      AppMeasurement_SetInterval.intervalList[intervalNum] = (object) null;
      interval.Stop();
      interval.Tick -= intervalHandler;
    }
  }
}
