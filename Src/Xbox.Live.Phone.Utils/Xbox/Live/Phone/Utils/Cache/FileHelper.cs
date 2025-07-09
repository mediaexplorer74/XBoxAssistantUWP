// *********************************************************
// Type: Xbox.Live.Phone.Utils.Cache.FileHelper
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;
using System.Xml.Serialization;


namespace Xbox.Live.Phone.Utils.Cache
{
  public class FileHelper
  {
    private static readonly object IsolatedStorageLock = new object();
    private static IsolatedStorageFile userStore = (IsolatedStorageFile) null;

    public long AvailableFreeSpace
    {
      get
      {
        lock (FileHelper.IsolatedStorageLock)
          return FileHelper.UserStore.AvailableFreeSpace;
      }
    }

    private static IsolatedStorageFile UserStore
    {
      get
      {
        if (FileHelper.userStore == null)
        {
          lock (FileHelper.IsolatedStorageLock)
          {
            if (FileHelper.userStore == null)
              FileHelper.userStore = IsolatedStorageFile.GetUserStoreForApplication();
          }
        }
        return FileHelper.userStore;
      }
      set => FileHelper.userStore = value;
    }

    public static void SetProperty(string id, object data)
    {
      lock (FileHelper.IsolatedStorageLock)
      {
        try
        {
          IsolatedStorageSettings.ApplicationSettings[id] = data;
        }
        catch (IsolatedStorageException ex)
        {
        }
      }
    }

    public static object GetProperty(string id)
    {
      lock (FileHelper.IsolatedStorageLock)
      {
        IsolatedStorageSettings isolatedStorageSettings = (IsolatedStorageSettings) null;
        try
        {
          isolatedStorageSettings = IsolatedStorageSettings.ApplicationSettings;
        }
        catch (IsolatedStorageException ex)
        {
        }
        return isolatedStorageSettings == null || !isolatedStorageSettings.Contains(id) ? (object) null : isolatedStorageSettings[id];
      }
    }

    public void AppendFile(string fileName, string data)
    {
      lock (FileHelper.IsolatedStorageLock)
      {
        this.EnsureDirectory(fileName);
        byte[] bytes = Encoding.Unicode.GetBytes(data);
        using (IsolatedStorageFileStream storageFileStream = FileHelper.UserStore.OpenFile(fileName, FileMode.Append))
          storageFileStream.Write(bytes, 0, bytes.Length);
      }
    }

    public bool FileExists(string fileName)
    {
      lock (FileHelper.IsolatedStorageLock)
        return FileHelper.UserStore.FileExists(fileName);
    }

    public void DeleteFile(string fileName)
    {
      lock (FileHelper.IsolatedStorageLock)
      {
        if (!FileHelper.UserStore.FileExists(fileName))
          return;
        FileHelper.UserStore.DeleteFile(fileName);
      }
    }

    public long FileSize(string fileName)
    {
      lock (FileHelper.IsolatedStorageLock)
      {
        if (!FileHelper.UserStore.FileExists(fileName))
          return 0;
        using (IsolatedStorageFileStream storageFileStream = FileHelper.UserStore.OpenFile(fileName, FileMode.Open))
          return storageFileStream.Length;
      }
    }

    public object LoadFile(string fileName, bool isBinary)
    {
      return this.LoadFile(fileName, FileAccess.Read, isBinary);
    }

    [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Shouldn't dispose return value")]
    public object LoadFile(string fileName, FileAccess access, bool isBinary)
    {
      lock (FileHelper.IsolatedStorageLock)
      {
        if (!FileHelper.UserStore.FileExists(fileName))
          return (object) null;
        int length;
        byte[] numArray;
        using (IsolatedStorageFileStream storageFileStream = FileHelper.UserStore.OpenFile(fileName, FileMode.Open, access))
        {
          length = (int) storageFileStream.Length;
          numArray = new byte[length];
          storageFileStream.Read(numArray, 0, length);
        }
        return !isBinary ? (object) Encoding.Unicode.GetString(numArray, 0, length) : (object) new MemoryStream(numArray, 0, length, false, true);
      }
    }

    public void SaveFile(string path, object data)
    {
      lock (FileHelper.IsolatedStorageLock)
      {
        string str = path.Replace('/', '\\');
        this.EnsureDirectory(path);
        byte[] buffer;
        if (data is string)
        {
          buffer = Encoding.Unicode.GetBytes((string) data);
        }
        else
        {
          Stream stream = (Stream) data;
          buffer = new byte[stream.Length];
          stream.Read(buffer, 0, buffer.Length);
        }
        try
        {
          this.WriteBytes(str, buffer);
        }
        catch (IsolatedStorageException ex)
        {
          if (!this.FileExists(str))
            return;
          FileHelper.UserStore.DeleteFile(str);
        }
      }
    }

    public void SaveSerializedXML(string fileName, object instance)
    {
      lock (FileHelper.IsolatedStorageLock)
      {
        try
        {
          if (FileHelper.UserStore.FileExists(fileName))
            FileHelper.UserStore.DeleteFile(fileName);
          IsolatedStorageFileStream file = FileHelper.UserStore.CreateFile(fileName);
          new XmlSerializer(instance.GetType()).Serialize((Stream) file, instance);
          file.Close();
        }
        catch (Exception ex)
        {
        }
      }
    }

    public bool TryLoadSerializedXML(string fileName, ref object instance)
    {
      lock (FileHelper.IsolatedStorageLock)
      {
        bool flag = false;
        try
        {
          if (FileHelper.UserStore.FileExists(fileName))
          {
            IsolatedStorageFileStream storageFileStream = FileHelper.UserStore.OpenFile(fileName, FileMode.Open, FileAccess.Read);
            XmlSerializer xmlSerializer = new XmlSerializer(instance.GetType());
            instance = xmlSerializer.Deserialize((Stream) storageFileStream);
            storageFileStream.Close();
            flag = true;
          }
        }
        catch (Exception ex)
        {
        }
        return flag;
      }
    }

    public void Truncate(string fileName, long size)
    {
      lock (FileHelper.IsolatedStorageLock)
      {
        long num = this.FileSize(fileName);
        if (num < size)
          return;
        string str = this.LoadFile(fileName, false) as string;
        this.SaveFile(fileName, (object) str.Substring((int) (num - size)));
      }
    }

    private void EnsureDirectory(string fileName)
    {
      string[] strArray = fileName.Split(new char[1]{ '/' }, StringSplitOptions.RemoveEmptyEntries);
      if (strArray.Length < 2)
        return;
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < strArray.Length - 1; ++index)
      {
        stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "{0}\\", new object[1]
        {
          (object) strArray[index]
        });
        if (!FileHelper.UserStore.DirectoryExists(stringBuilder.ToString()))
          FileHelper.UserStore.CreateDirectory(stringBuilder.ToString());
      }
    }

    private void WriteBytes(string file, byte[] buffer)
    {
      using (IsolatedStorageFileStream file1 = FileHelper.UserStore.CreateFile(file))
        file1.Write(buffer, 0, buffer.Length);
    }
  }
}
