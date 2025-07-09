// *********************************************************
// Type: Xbox.Live.Phone.Utils.Cache.CacheManager
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;
using Xbox.Live.Phone.Utils.Instrumentation;


namespace Xbox.Live.Phone.Utils.Cache
{
  public sealed class CacheManager : IDisposable
  {
    private const string ComponentName = "CacheManager";
    private const int SaveDelayMilliseconds = 2000;
    private static CacheManager instance;
    private Thread saveThread;
    private ManualResetEvent exit = new ManualResetEvent(false);
    private bool isStarted;
    private ManualResetEvent haveWork = new ManualResetEvent(false);
    private List<CacheItem> saveQueue = new List<CacheItem>();

    private CacheManager()
    {
    }

    public static CacheManager Instance
    {
      get
      {
        if (CacheManager.instance == null)
          CacheManager.instance = new CacheManager();
        return CacheManager.instance;
      }
    }

    public static void Delete(string path)
    {
      FileHelper fileHelper = new FileHelper();
      try
      {
        fileHelper.DeleteFile(path);
      }
      catch (Exception ex)
      {
      }
    }

    public static DataType Load<DataType>(string path)
    {
      DataType dataType = default (DataType);
      FileHelper fileHelper = new FileHelper();
      MemoryStream memoryStream = (MemoryStream) null;
      try
      {
        memoryStream = (MemoryStream) fileHelper.LoadFile(path, true);
        if (memoryStream != null)
          return (DataType) new DataContractSerializer(typeof (DataType)).ReadObject((Stream) memoryStream);
      }
      catch (Exception ex)
      {
        LiveMobileTrace.LogException(nameof (CacheManager), ex);
      }
      finally
      {
        memoryStream?.Dispose();
      }
      return dataType;
    }

    public static void Save(string path, object data)
    {
      FileHelper fileHelper = new FileHelper();
      MemoryStream data1 = new MemoryStream();
      try
      {
        new DataContractSerializer(data.GetType()).WriteObject((Stream) data1, data);
        data1.Seek(0L, SeekOrigin.Begin);
        fileHelper.SaveFile(path, (object) data1);
      }
      catch (Exception ex)
      {
        LiveMobileTrace.LogException(nameof (CacheManager), ex);
      }
      finally
      {
        data1.Dispose();
      }
    }

    public static void OnExit(object sender, EventArgs e)
    {
      CacheManager.Instance.exit.Set();
      CacheManager.Instance.haveWork.Set();
    }

    public void Dispose()
    {
      if (this.haveWork != null)
        this.haveWork.Close();
      if (this.exit == null)
        return;
      this.exit.Close();
    }

    public void Start()
    {
      if (this.isStarted)
        return;
      this.saveThread = new Thread(new ThreadStart(this.SaveThreadProc));
      this.saveThread.IsBackground = true;
      this.saveThread.Start();
      this.isStarted = true;
    }

    public bool ExistInCache(string key) => new FileHelper().FileExists(key);

    public void SaveAsync(CacheItem item)
    {
      lock (this.saveQueue)
      {
        foreach (CacheItem save in this.saveQueue)
        {
          if (string.Compare(save.StorageKey, item.StorageKey, CultureInfo.InvariantCulture, CompareOptions.IgnoreCase) == 0)
          {
            this.saveQueue.Remove(save);
            break;
          }
        }
        this.saveQueue.Add(item);
        this.haveWork.Set();
      }
    }

    private void SaveThreadProc()
    {
      while (true)
      {
        this.haveWork.WaitOne();
        if (!this.exit.WaitOne(0))
        {
          this.DoSave();
          Thread.Sleep(2000);
        }
        else
          break;
      }
    }

    private void DoSave()
    {
      CacheItem data = (CacheItem) null;
      lock (this.saveQueue)
      {
        if (this.saveQueue.Count == 0)
          return;
        data = this.saveQueue[0];
        this.saveQueue.RemoveAt(0);
        if (this.saveQueue.Count == 0)
          this.haveWork.Reset();
      }
      try
      {
        if (data == null)
          return;
        lock (data.StorageLock)
          CacheManager.Save(data.StorageKey, (object) data);
      }
      catch (Exception ex)
      {
      }
    }
  }
}
