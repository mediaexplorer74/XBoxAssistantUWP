// *********************************************************
// Type: Xbox.Live.Phone.Services.MessagesData
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Leet.MessageService.DataContracts;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Xbox.Live.Phone.Utils.Cache;


namespace Xbox.Live.Phone.Services
{
  [DataContract(Namespace = "")]
  [XmlRoot(Namespace = "")]
  public class MessagesData : CacheItem
  {
    private List<MessageSummary> summaryList;

    public MessagesData(string key)
      : base(key)
    {
      this.MessageSummaryList = new List<MessageSummary>();
      this.HashCode = string.Empty;
    }

    public int TotalMessagesCount { get; set; }

    public int UnreadMessagesCount { get; set; }

    [DataMember]
    public List<MessageSummary> MessageSummaryList
    {
      get => this.summaryList;
      set
      {
        if (value == null)
          return;
        this.summaryList = new List<MessageSummary>(value.Count);
        foreach (MessageSummary messageSummary in value)
        {
          if (messageSummary.CanDisplayOnDevice)
            this.summaryList.Add(messageSummary);
        }
        this.summaryList.Sort((Comparison<MessageSummary>) ((summary1, summary2) => -summary1.SentTime.CompareTo(summary2.SentTime)));
        this.UpdateTotalMessagesCount();
        this.UpdateUnReadMessagesCount();
      }
    }

    [DataMember]
    public string HashCode { get; set; }

    public void SetMessageReadLocally(MessageSummary summary)
    {
      if (summary == null)
        throw new ArgumentNullException(nameof (summary));
      int num = 0;
      foreach (MessageSummary messageSummary in this.MessageSummaryList)
      {
        if ((int) messageSummary.MessageId == (int) summary.MessageId)
          messageSummary.HasBeenRead = true;
        if (!messageSummary.HasBeenRead)
          ++num;
      }
      this.UnreadMessagesCount = num;
    }

    public void DeleteOneMessageLocally(MessageSummary summary)
    {
      if (this.MessageSummaryList.Remove(summary))
        --this.TotalMessagesCount;
      this.UpdateUnReadMessagesCount();
    }

    public void UpdateTotalMessagesCount()
    {
      if (this.summaryList != null)
        this.TotalMessagesCount = this.summaryList.Count;
      else
        this.TotalMessagesCount = 0;
    }

    public void UpdateUnReadMessagesCount()
    {
      int num = 0;
      foreach (MessageSummary messageSummary in this.MessageSummaryList)
      {
        if (!messageSummary.HasBeenRead)
          ++num;
      }
      this.UnreadMessagesCount = num;
    }
  }
}
