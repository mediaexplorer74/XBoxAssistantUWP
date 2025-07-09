// *********************************************************
// Type: Xbox.Live.Phone.Utils.Instrumentation.LiveMobileTrace
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;


namespace Xbox.Live.Phone.Utils.Instrumentation
{
  public static class LiveMobileTrace
  {
    private const string LogFileName = "XboxLiveApp.log";
    private const int TraceBufferSize = 5120;
    private static TraceLevel currentTraceLevel = TraceLevel.Error;
    private static StringBuilder traceBuffer = new StringBuilder(5320);

    public static event LiveMobileTrace.TraceMessageHandler EventTraceMessageWritten;

    public static TraceLevel CurrentTraceLevel
    {
      get => LiveMobileTrace.currentTraceLevel;
      set => LiveMobileTrace.currentTraceLevel = value;
    }

    [Conditional("DEBUG")]
    public static void WriteLine(TraceLevel traceLevel, string componentName, string traceMessage)
    {
      if (!LiveMobileTrace.ShouldWriteTrace(traceLevel))
        return;
      LiveMobileTrace.OutputMessage(componentName, traceMessage, false);
    }

    [Conditional("DEBUG")]
    public static void WriteLine(
      TraceLevel traceLevel,
      string componentName,
      params string[] valueList)
    {
      if (!LiveMobileTrace.ShouldWriteTrace(traceLevel))
        return;
      LiveMobileTrace.WriteLineHelper(componentName, valueList);
    }

    [Conditional("DEBUG")]
    public static void WriteLine(
      TraceLevel traceLevel,
      string componentName,
      params object[] valueList)
    {
      if (!LiveMobileTrace.ShouldWriteTrace(traceLevel))
        return;
      LiveMobileTrace.WriteLineHelper(componentName, valueList);
    }

    [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "These are debug strings.")]
    public static void LogException(string componentName, Exception ex)
    {
    }

    public static void FlushBuffer()
    {
      LiveMobileTrace.OutputMessage(nameof (LiveMobileTrace), "Forcing trace buffer to commit.", true);
    }

    private static void WriteLineHelper(string componentName, params string[] stringList)
    {
      string message = (string) null;
      if (stringList != null)
        message = string.Concat(stringList);
      LiveMobileTrace.OutputMessage(componentName, message, false);
    }

    private static void WriteLineHelper(string componentName, params object[] objectList)
    {
      string message = (string) null;
      if (objectList != null)
        message = string.Concat(objectList);
      LiveMobileTrace.OutputMessage(componentName, message, false);
    }

    private static bool ShouldWriteTrace(TraceLevel traceLevel)
    {
      return traceLevel <= LiveMobileTrace.currentTraceLevel;
    }

    private static void OutputMessage(string componentName, string message, bool forceFlush)
    {
      string message1 = DateTime.Now.ToString("G") + " " + componentName + "> " + message;
      LiveMobileTrace.TraceMessageHandler traceMessageWritten = LiveMobileTrace.EventTraceMessageWritten;
      if (traceMessageWritten == null)
        return;
      traceMessageWritten((object) null, new TraceMessageEventArgs(message1));
    }

    public delegate void TraceMessageHandler(object sender, TraceMessageEventArgs e);
  }
}
