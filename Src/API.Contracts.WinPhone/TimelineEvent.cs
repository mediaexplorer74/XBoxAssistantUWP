// *********************************************************
// Type: Microsoft.XMedia.Service.TimelineEvent
// Assembly: API.Contracts.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F4DA62B-04F1-4176-9D4F-6166D697B436
// *********************************************************API.Contracts.WinPhone.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Microsoft.XMedia.Service
{
  [DataContract]
  public class TimelineEvent
  {
    [DataMember]
    public string Type { get; set; }

    [DataMember]
    public string Id { get; set; }

    [DataMember]
    public string Title { get; set; }

    [DataMember]
    public string Text { get; set; }

    [DataMember]
    public string ImageThumbnail { get; set; }

    [DataMember]
    public string Icon { get; set; }

    [DataMember]
    public TimeSpan StartTime { get; set; }

    [DataMember]
    public TimeSpan? EndTime { get; set; }

    [DataMember]
    public IDictionary<string, string> Data { get; set; }
  }
}
