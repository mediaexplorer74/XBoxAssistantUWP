// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.LocalCacheIndex
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;


namespace Microsoft.XboxLive.Avatars.Internal
{
  internal class LocalCacheIndex
  {
    private const int SerializeVersion = 1;
    private const int MaxFileNameLen = 255;
    private const double SessionMinDiffTimeSpan = 3600.0;
    private Dictionary<string, LocalCacheRecord> m_dRecords = new Dictionary<string, LocalCacheRecord>();
    private static AutoResetEvent m_writeFinished = new AutoResetEvent(false);

    private static int ReadInt(StreamReader sr)
    {
      int num1 = sr.Read();
      int num2 = sr.Read();
      int num3 = sr.Read();
      int num4 = sr.Read();
      return num1 == -1 || num2 == -1 || num3 == -1 || num4 == -1 ? -1 : num4 << 24 | num3 << 16 | num2 << 8 | num1;
    }

    private static long ReadLong(StreamReader sr)
    {
      int num1 = LocalCacheIndex.ReadInt(sr);
      int num2 = LocalCacheIndex.ReadInt(sr);
      return num2 == -1 ? -1L : (long) num2 << 32 | (long) (uint) num1;
    }

    private static void WriteInt(StreamWriter sw, int value)
    {
      sw.Write((char) (value & (int) byte.MaxValue));
      sw.Write((char) (value >> 8 & (int) byte.MaxValue));
      sw.Write((char) (value >> 16 & (int) byte.MaxValue));
      sw.Write((char) (value >> 24 & (int) byte.MaxValue));
    }

    private static void WriteLong(StreamWriter sw, long value)
    {
      LocalCacheIndex.WriteInt(sw, (int) (uint) value);
      LocalCacheIndex.WriteInt(sw, (int) (value >>> 32));
    }

    private void WriteFinished(IAsyncResult ar) => LocalCacheIndex.m_writeFinished.Set();

    public bool Serialize(Stream stream)
    {
      if (stream == null)
        return false;
      StreamWriter sw = new StreamWriter((Stream) new MemoryStream());
      LocalCacheIndex.WriteInt(sw, 1);
      LocalCacheIndex.WriteInt(sw, this.m_dRecords.Count);
      foreach (KeyValuePair<string, LocalCacheRecord> dRecord in this.m_dRecords)
      {
        LocalCacheIndex.WriteLong(sw, dRecord.Value.FileSize);
        LocalCacheIndex.WriteLong(sw, dRecord.Value.LastAccessTime);
        LocalCacheIndex.WriteInt(sw, dRecord.Key.Length);
        sw.Write(dRecord.Key);
      }
      sw.Flush();
      long length = sw.BaseStream.Length;
      if (length == 0L)
        return true;
      sw.BaseStream.Seek(0L, SeekOrigin.Begin);
      byte[] buffer = new byte[length];
      sw.BaseStream.Read(buffer, 0, (int) length);
      IAsyncResult asyncResult = stream.BeginWrite(buffer, 0, (int) length, new AsyncCallback(this.WriteFinished), (object) null);
      LocalCacheIndex.m_writeFinished.WaitOne();
      stream.EndWrite(asyncResult);
      return true;
    }

    public bool Deserialize(Stream stream)
    {
      if (stream == null)
        return false;
      StreamReader sr = new StreamReader(stream);
      StringBuilder stringBuilder = new StringBuilder();
      if (LocalCacheIndex.ReadInt(sr) != 1)
        return false;
      this.m_dRecords.Clear();
      int num1 = LocalCacheIndex.ReadInt(sr);
      if (num1 < 0)
        return false;
      int num2 = 0;
      while (num2 < num1)
      {
        long fileSize = LocalCacheIndex.ReadLong(sr);
        if (fileSize != -1L)
        {
          long lastAccessTime = LocalCacheIndex.ReadLong(sr);
          if (lastAccessTime != -1L && num2 <= num1)
          {
            int length = LocalCacheIndex.ReadInt(sr);
            if (length >= 0 && length <= (int) byte.MaxValue)
            {
              char[] buffer = new char[length];
              if (sr.ReadBlock(buffer, 0, length) == length)
              {
                stringBuilder.Append(buffer, 0, length);
                this.m_dRecords.Add(stringBuilder.ToString(), new LocalCacheRecord(fileSize, lastAccessTime));
                ++num2;
                stringBuilder.Remove(0, length);
              }
              else
                break;
            }
            else
              break;
          }
          else
            break;
        }
        else
          break;
      }
      return num2 == num1;
    }

    public void Initialize() => this.m_dRecords.Clear();

    public void Initialize(string[] fileNames)
    {
      this.m_dRecords.Clear();
      int length = fileNames.GetLength(0);
      for (int index = 0; index < length; ++index)
        this.m_dRecords.Add(fileNames[index], new LocalCacheRecord(0L, 0L));
    }

    public bool TryGetFileInfo(string fileName, out LocalCacheRecord fileInfo)
    {
      return this.m_dRecords.TryGetValue(fileName, out fileInfo);
    }

    public void UpdateFileInfo(string fileName, LocalCacheRecord fileInfo)
    {
      LocalCacheRecord localCacheRecord;
      if (this.m_dRecords.TryGetValue(fileName, out localCacheRecord))
      {
        localCacheRecord.LastAccessTime = fileInfo.LastAccessTime;
        localCacheRecord.FileSize = fileInfo.FileSize;
      }
      else
        this.m_dRecords.Add(fileName, fileInfo);
    }

    public bool Remove(string fileName) => this.m_dRecords.Remove(fileName);

    public void Merge(LocalCacheIndex localCacheIndex)
    {
      foreach (KeyValuePair<string, LocalCacheRecord> dRecord in this.m_dRecords)
      {
        if (!localCacheIndex.TryGetFileInfo(dRecord.Key, out LocalCacheRecord _))
          localCacheIndex.UpdateFileInfo(dRecord.Key, dRecord.Value);
      }
    }

    public void UpdateBy(LocalCacheIndex localCacheIndex)
    {
      List<string> stringList = new List<string>();
      foreach (KeyValuePair<string, LocalCacheRecord> dRecord in this.m_dRecords)
      {
        LocalCacheRecord fileInfo;
        if (localCacheIndex.TryGetFileInfo(dRecord.Key, out fileInfo))
        {
          if (fileInfo.LastAccessTime != 0L)
          {
            dRecord.Value.LastAccessTime = fileInfo.LastAccessTime;
            dRecord.Value.FileSize = fileInfo.FileSize;
          }
        }
        else
          stringList.Add(dRecord.Key);
      }
      foreach (string key in stringList)
        this.m_dRecords.Remove(key);
      localCacheIndex.Merge(this);
    }

    private static void InsertCandidate(
      LinkedList<LocalCacheSortRecord> sortedCandidates,
      string candidateName,
      LocalCacheRecord candidate)
    {
      if (candidate.LastAccessTime == 0L)
      {
        sortedCandidates.AddFirst(new LocalCacheSortRecord(candidateName, candidate));
      }
      else
      {
        for (LinkedListNode<LocalCacheSortRecord> node = sortedCandidates.First; node != null; node = node.Next)
        {
          if (node.Value.LastAccessTime != 0L && node.Value.LastAccessTime >= candidate.LastAccessTime)
          {
            sortedCandidates.AddBefore(node, new LocalCacheSortRecord(candidateName, candidate));
            return;
          }
        }
        sortedCandidates.AddLast(new LocalCacheSortRecord(candidateName, candidate));
      }
    }

    private static int CompareByFileSizeDescending(LocalCacheSortRecord a, LocalCacheSortRecord b)
    {
      if (a == null)
        return b == null ? 0 : -1;
      if (b == null)
        return 1;
      if (a.FileSize == b.FileSize)
        return 0;
      return a.FileSize > b.FileSize ? -1 : 1;
    }

    public List<string> GetListOfReleaseCandidates(long requiredSize)
    {
      List<string> releaseCandidates = new List<string>();
      LinkedList<LocalCacheSortRecord> sortedCandidates = new LinkedList<LocalCacheSortRecord>();
      foreach (KeyValuePair<string, LocalCacheRecord> dRecord in this.m_dRecords)
        LocalCacheIndex.InsertCandidate(sortedCandidates, dRecord.Key, dRecord.Value);
      List<LocalCacheSortRecord> localCacheSortRecordList = new List<LocalCacheSortRecord>();
      LinkedListNode<LocalCacheSortRecord> linkedListNode = sortedCandidates.First;
      long lastAccessTime = linkedListNode != null ? linkedListNode.Value.LastAccessTime : 0L;
      long num = 0;
      while (true)
      {
        if (linkedListNode == null || linkedListNode.Value.LastAccessTime != 0L && new TimeSpan(linkedListNode.Value.LastAccessTime - lastAccessTime).TotalSeconds > 3600.0)
        {
          localCacheSortRecordList.Sort(new Comparison<LocalCacheSortRecord>(LocalCacheIndex.CompareByFileSizeDescending));
          foreach (LocalCacheSortRecord localCacheSortRecord in localCacheSortRecordList)
            releaseCandidates.Add(localCacheSortRecord.FileName);
          if (num < requiredSize && linkedListNode != null)
          {
            lastAccessTime = linkedListNode.Value.LastAccessTime;
            localCacheSortRecordList.Clear();
          }
          else
            break;
        }
        localCacheSortRecordList.Add(linkedListNode.Value);
        if (linkedListNode.Value.LastAccessTime != 0L)
          num += linkedListNode.Value.FileSize;
        linkedListNode = linkedListNode.Next;
      }
      return releaseCandidates;
    }
  }
}
