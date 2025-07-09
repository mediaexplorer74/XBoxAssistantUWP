// *********************************************************
// Type: Microsoft.Xmedia.Client.WindowsPhone.IsolatedStorageClientTracer
// Assembly: Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2E3A1F77-365B-4EB2-85E1-D467924E2195
// *********************************************************Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy.dll

using Microsoft.XMedia.Client;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;


namespace Microsoft.Xmedia.Client.WindowsPhone
{
  public class IsolatedStorageClientTracer : IClientTracer
  {
    private const int MaxLogSize = 1048576;
    private const string LogPath = "XmediaClient.log";
    private static string[] TraceLevelNames = new string[5]
    {
      "VERBOSE",
      "INFO    ",
      "WARNING ",
      "ERROR   ",
      "CRITICAL"
    };
    private IsolatedStorageFile store;
    private IsolatedStorageFileStream fileStream;
    private object fileLock;

    public IsolatedStorageClientTracer()
    {
      this.fileLock = new object();
      this.store = (IsolatedStorageFile) null;
      this.fileStream = (IsolatedStorageFileStream) null;
    }

    public void WriteVerbose(string message, params object[] dataArgs)
    {
      this.Write(Microsoft.XMedia.Client.TraceLevel.Verbose, message, dataArgs);
    }

    public void WriteInfo(string message, params object[] dataArgs)
    {
      this.Write(Microsoft.XMedia.Client.TraceLevel.Informational, message, dataArgs);
    }

    public void WriteWarning(string message, params object[] dataArgs)
    {
      this.Write(Microsoft.XMedia.Client.TraceLevel.Warning, message, dataArgs);
    }

    public void WriteError(string message, params object[] dataArgs)
    {
      this.Write(Microsoft.XMedia.Client.TraceLevel.Error, message, dataArgs);
    }

    public void WriteCritical(string message, params object[] dataArgs)
    {
      this.Write(Microsoft.XMedia.Client.TraceLevel.Critical, message, dataArgs);
    }

    public void Write(Microsoft.XMedia.Client.TraceLevel traceLevel, string message, params object[] dataArgs)
    {
    }

    [Conditional("DEBUG")]
    private void WriteInternal(Microsoft.XMedia.Client.TraceLevel traceLevel, string message, params object[] dataArgs)
    {
      try
      {
        if (dataArgs.Length > 0)
          message = string.Format(message, dataArgs);
        byte[] bytes = Encoding.UTF8.GetBytes(string.Format("{0}: {1}: {2}", (object) DateTime.UtcNow, (object) IsolatedStorageClientTracer.TraceLevelNames[(int) traceLevel], (object) message) + "\r\n");
        lock (this.fileLock)
          this.fileStream.Write(bytes, 0, bytes.Length);
      }
      catch (Exception ex)
      {
      }
    }

    [Conditional("DEBUG")]
    private void TrimLogIfNeeded()
    {
      if (this.fileStream.Length <= 1048576L)
        return;
      this.fileStream.Close();
      string str = "XmediaClient.log.temp";
      if (this.store.FileExists(str))
        this.store.DeleteFile(str);
      this.store.MoveFile("XmediaClient.log", str);
      this.fileStream = this.store.OpenFile("XmediaClient.log", FileMode.Create);
      using (IsolatedStorageFileStream storageFileStream = this.store.OpenFile(str, FileMode.Open))
      {
        storageFileStream.Position = 524288L;
        storageFileStream.CopyTo((Stream) this.fileStream);
      }
      this.store.DeleteFile(str);
    }
  }
}
