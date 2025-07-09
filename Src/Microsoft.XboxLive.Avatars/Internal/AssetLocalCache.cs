// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.AssetLocalCache
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;
using System.IO;
using System.IO.IsolatedStorage;


namespace Microsoft.XboxLive.Avatars.Internal
{
  internal class AssetLocalCache
  {
    private const string LocalCacheDirectory = "AvatarCache";
    private const string LocalCacheFileIndex = "_CacheIndex.bin";
    private const long DefaultLocalCacheQuota = 1048576;
    private const long StandardLocalCacheQuota = 12582912;
    private const long ReserveForLocalCacheQuota = 32768;
    private static object m_fileAccesLock = new object();
    private static long m_currentQuotaSize = -1;
    private static bool m_cacheInitialized;
    private long m_askedQuotaSize = 1048576;
    private long m_reserveForLocalCacheQuota = 32768;

    internal AssetLocalCache()
    {
    }

    internal event EventHandler<IncreaseQuotaEventArgs> IncreaseQuota;

    internal long QuotaSize => AssetLocalCache.m_currentQuotaSize;

    private static string TranslateAddressToLocalCachePath(string address)
    {
      return Path.Combine("AvatarCache", Path.GetFileName(address));
    }

    private void StoreIsolatedFile(IsolatedStorageFile store, string path, Stream stream)
    {
      try
      {
        if (!AssetLocalCache.LocalCacheInitialize(store))
          return;
        using (IsolatedStorageFileStream storageFileStream = IsolatedStorageAccess.CreateIsolatedStorageFileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None, store))
        {
          if (storageFileStream == null)
            return;
          long position = stream.Position;
          byte[] buffer = new byte[32768];
          while (true)
          {
            int count = stream.Read(buffer, 0, buffer.Length);
            if (count > 0)
              storageFileStream.Write(buffer, 0, count);
            else
              break;
          }
          storageFileStream.Flush();
          stream.Position = position;
        }
      }
      catch (IsolatedStorageException ex)
      {
      }
    }

    private Stream ReadIsolatedFile(IsolatedStorageFile isf, string path)
    {
      try
      {
        if (!AssetLocalCache.LocalCacheInitialize(isf))
          return (Stream) null;
        using (IsolatedStorageFileStream storageFileStream = IsolatedStorageAccess.CreateIsolatedStorageFileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, isf))
        {
          if (storageFileStream != null)
          {
            Stream stream = (Stream) new MemoryStream();
            byte[] buffer = new byte[32768];
            while (true)
            {
              int count = storageFileStream.Read(buffer, 0, buffer.Length);
              if (count > 0)
                stream.Write(buffer, 0, count);
              else
                break;
            }
            stream.Seek(0L, SeekOrigin.Begin);
            return stream;
          }
        }
      }
      catch (IsolatedStorageException ex)
      {
      }
      return (Stream) null;
    }

    private static bool LocalCacheInitialize(IsolatedStorageFile store)
    {
      bool flag = AssetLocalCache.m_cacheInitialized;
      if (!AssetLocalCache.m_cacheInitialized)
      {
        try
        {
          if (!store.DirectoryExists("AvatarCache"))
            store.CreateDirectory("AvatarCache");
          AssetLocalCache.m_currentQuotaSize = store.Quota;
          flag = AssetLocalCache.InitializeIndexFile(store);
        }
        catch (IsolatedStorageException ex)
        {
        }
        catch (ObjectDisposedException ex)
        {
        }
        AssetLocalCache.m_cacheInitialized = flag;
      }
      return flag;
    }

    private void UpdateAccessTime(IsolatedStorageFile store, string fileName, long fileAccessTime)
    {
      if (!AssetLocalCache.LocalCacheInitialize(store))
        return;
      string path = Path.Combine("AvatarCache", "_CacheIndex.bin");
      if (!store.FileExists(path))
        return;
      LocalCacheIndex localCacheIndex = new LocalCacheIndex();
      using (IsolatedStorageFileStream storageFileStream = IsolatedStorageAccess.CreateIsolatedStorageFileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.Read, store))
      {
        LocalCacheRecord fileInfo;
        if (!localCacheIndex.Deserialize((Stream) storageFileStream) || !localCacheIndex.TryGetFileInfo(fileName, out fileInfo))
          return;
        localCacheIndex.UpdateFileInfo(fileName, new LocalCacheRecord(fileInfo.FileSize, fileAccessTime));
        storageFileStream.Seek(0L, SeekOrigin.Begin);
        localCacheIndex.Serialize((Stream) storageFileStream);
      }
    }

    private Stream ReadFileFromStorage(string readFilePath)
    {
      Stream stream = (Stream) null;
      try
      {
        using (IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication())
        {
          if (storeForApplication.FileExists(readFilePath))
          {
            stream = this.ReadIsolatedFile(storeForApplication, readFilePath);
            if (stream != null)
              this.UpdateAccessTime(storeForApplication, Path.GetFileName(readFilePath), DateTime.Now.Ticks);
          }
        }
      }
      catch (IsolatedStorageException ex)
      {
      }
      catch (IOException ex)
      {
      }
      catch (ObjectDisposedException ex)
      {
      }
      catch (InvalidOperationException ex)
      {
      }
      return stream;
    }

    private void StoreEvent(string filePath, Stream writeStream)
    {
      try
      {
        using (IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication())
        {
          if (storeForApplication.FileExists(filePath))
            storeForApplication.DeleteFile(filePath);
          if (!this.IsAvailableSpace(storeForApplication, writeStream))
            return;
          this.StoreIsolatedFile(storeForApplication, filePath, writeStream);
          this.UpdateIndexFile(storeForApplication, Path.GetFileName(filePath), writeStream.Length, DateTime.Now.Ticks);
        }
      }
      catch (IsolatedStorageException ex)
      {
      }
      catch (IOException ex)
      {
      }
      catch (ObjectDisposedException ex)
      {
      }
      catch (InvalidOperationException ex)
      {
      }
    }

    private void UpdateIndexFile(
      IsolatedStorageFile isf,
      string fileName,
      long fileLength,
      long fileAccessTime)
    {
      if (!AssetLocalCache.LocalCacheInitialize(isf))
        return;
      string path = Path.Combine("AvatarCache", "_CacheIndex.bin");
      if (!isf.FileExists(path))
        return;
      LocalCacheIndex localCacheIndex = new LocalCacheIndex();
      bool flag = false;
      using (IsolatedStorageFileStream storageFileStream = IsolatedStorageAccess.CreateIsolatedStorageFileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, isf))
      {
        flag = localCacheIndex.Deserialize((Stream) storageFileStream);
        if (flag)
          localCacheIndex.UpdateFileInfo(fileName, new LocalCacheRecord(fileLength, fileAccessTime));
      }
      if (!flag)
        return;
      using (IsolatedStorageFileStream storageFileStream = IsolatedStorageAccess.CreateIsolatedStorageFileStream(path, FileMode.Open, FileAccess.Write, FileShare.None, isf))
      {
        if (storageFileStream == null)
          return;
        localCacheIndex.Serialize((Stream) storageFileStream);
        storageFileStream.Flush();
      }
    }

    private static bool InitializeIndexFile(IsolatedStorageFile isf)
    {
      bool flag1 = false;
      if (!isf.DirectoryExists("AvatarCache"))
        return false;
      string[] fileNames = isf.GetFileNames(Path.Combine("AvatarCache", "*.*"));
      string path = Path.Combine("AvatarCache", "_CacheIndex.bin");
      LocalCacheIndex localCacheIndex1 = new LocalCacheIndex();
      localCacheIndex1.Initialize(fileNames);
      localCacheIndex1.Remove("_CacheIndex.bin");
      if (!isf.FileExists(path))
      {
        using (IsolatedStorageFileStream storageFileStream = IsolatedStorageAccess.CreateIsolatedStorageFileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None, isf))
        {
          if (storageFileStream != null)
          {
            flag1 = localCacheIndex1.Serialize((Stream) storageFileStream);
            storageFileStream.Flush();
          }
        }
      }
      else
      {
        LocalCacheIndex localCacheIndex2 = new LocalCacheIndex();
        bool flag2 = false;
        using (IsolatedStorageFileStream storageFileStream = IsolatedStorageAccess.CreateIsolatedStorageFileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, isf))
        {
          if (storageFileStream != null)
            flag2 = localCacheIndex2.Deserialize((Stream) storageFileStream);
        }
        using (IsolatedStorageFileStream storageFileStream = IsolatedStorageAccess.CreateIsolatedStorageFileStream(path, FileMode.Truncate, FileAccess.Write, FileShare.None, isf))
        {
          if (storageFileStream != null)
          {
            if (flag2)
            {
              localCacheIndex2.UpdateBy(localCacheIndex1);
              flag1 = localCacheIndex2.Serialize((Stream) storageFileStream);
            }
            else
              flag1 = localCacheIndex1.Serialize((Stream) storageFileStream);
            storageFileStream.Flush();
          }
        }
      }
      return flag1;
    }

    private bool IsAvailableSpace(IsolatedStorageFile store, Stream writeStream)
    {
      long num1 = writeStream.Length + this.m_reserveForLocalCacheQuota;
      if (store.AvailableFreeSpace < num1)
      {
        long num2 = AssetLocalCache.m_currentQuotaSize == -1L || AssetLocalCache.m_currentQuotaSize > store.Quota ? store.Quota : AssetLocalCache.m_currentQuotaSize;
        if (this.IncreaseQuota != null && this.m_askedQuotaSize <= num2)
        {
          long num3 = num2;
          long recommendedQuota = num3 > 1048576L ? num3 + num3 / 2L : 12582912L;
          this.m_askedQuotaSize = recommendedQuota;
          this.IncreaseQuota((object) this, new IncreaseQuotaEventArgs(store.AvailableFreeSpace, store.Quota, recommendedQuota));
        }
        AssetLocalCache.TryReleaseCacheSpace(store, num1 - store.AvailableFreeSpace);
        if (store.AvailableFreeSpace < num1)
          return false;
      }
      return true;
    }

    private static long TryReleaseCacheSpace(IsolatedStorageFile isf, long requiredSize)
    {
      if (!AssetLocalCache.LocalCacheInitialize(isf))
        return 0;
      string path = Path.Combine("AvatarCache", "_CacheIndex.bin");
      if (!isf.FileExists(path))
        return 0;
      long num = 0;
      bool flag = true;
      LocalCacheIndex localCacheIndex = new LocalCacheIndex();
      using (IsolatedStorageFileStream storageFileStream = IsolatedStorageAccess.CreateIsolatedStorageFileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, isf))
      {
        if (storageFileStream == null || !localCacheIndex.Deserialize((Stream) storageFileStream))
          return 0;
        foreach (string releaseCandidate in localCacheIndex.GetListOfReleaseCandidates(requiredSize))
        {
          string str = Path.Combine("AvatarCache", releaseCandidate);
          if (isf.FileExists(str))
            isf.DeleteFile(str);
          LocalCacheRecord fileInfo;
          if (localCacheIndex.TryGetFileInfo(releaseCandidate, out fileInfo))
          {
            if (fileInfo.LastAccessTime != 0L)
              num += fileInfo.FileSize;
            else
              flag = false;
          }
          localCacheIndex.Remove(releaseCandidate);
          if (flag)
          {
            if (requiredSize <= num)
              break;
          }
          else if (requiredSize <= isf.AvailableFreeSpace)
            break;
        }
      }
      if (num != 0L || !flag)
      {
        using (IsolatedStorageFileStream storageFileStream = IsolatedStorageAccess.CreateIsolatedStorageFileStream(path, FileMode.Truncate, FileAccess.Write, FileShare.None, isf))
        {
          if (storageFileStream != null)
          {
            localCacheIndex.Serialize((Stream) storageFileStream);
            storageFileStream.Flush();
            storageFileStream.Close();
          }
        }
      }
      return !flag ? -num : num;
    }

    internal Stream TryOpen(string address)
    {
      Stream stream = (Stream) null;
      if (address != null)
      {
        string localCachePath = AssetLocalCache.TranslateAddressToLocalCachePath(address);
        lock (AssetLocalCache.m_fileAccesLock)
          stream = this.ReadFileFromStorage(localCachePath);
      }
      return stream;
    }

    internal void Store(string address, Stream stream)
    {
      if (address == null)
        return;
      string localCachePath = AssetLocalCache.TranslateAddressToLocalCachePath(address);
      lock (AssetLocalCache.m_fileAccesLock)
        this.StoreEvent(localCachePath, stream);
    }

    internal bool IncreaseQuotaTo(long size)
    {
      if (size == -1L)
        return false;
      try
      {
        using (IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication())
        {
          if (size > storeForApplication.Quota)
          {
            storeForApplication.IncreaseQuotaTo(size);
            AssetLocalCache.m_currentQuotaSize = storeForApplication.Quota;
          }
        }
      }
      catch (IsolatedStorageException ex)
      {
      }
      catch (IOException ex)
      {
      }
      catch (ObjectDisposedException ex)
      {
      }
      catch (InvalidOperationException ex)
      {
      }
      return AssetLocalCache.m_currentQuotaSize == size;
    }

    internal static void Clean()
    {
      lock (AssetLocalCache.m_fileAccesLock)
      {
        try
        {
          using (IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication())
            AssetLocalCache.TryReleaseCacheSpace(storeForApplication, long.MaxValue);
        }
        catch (IsolatedStorageException ex)
        {
        }
        catch (IOException ex)
        {
        }
        catch (ObjectDisposedException ex)
        {
        }
        catch (InvalidOperationException ex)
        {
        }
      }
    }
  }
}
