// *********************************************************
// Type: Xbox.Live.Phone.Services.FriendEntryData
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace Xbox.Live.Phone.Services
{
  [DataContract(Namespace = "")]
  [SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Justification = "Not used in a public API, and sticking with the one method keeps the code simpler.")]
  [XmlRoot(Namespace = "")]
  public class FriendEntryData : IComparable
  {
    [DataMember]
    public string GamerTag { get; set; }

    [DataMember]
    public string GamerPicUrl { get; set; }

    [DataMember]
    public bool IsOnline { get; set; }

    [DataMember]
    public string DetailedPresence { get; set; }

    [DataMember]
    public string PresenceString { get; set; }

    [DataMember]
    public uint LastSeenTitleId { get; set; }

    [DataMember]
    public FriendState FriendState { get; set; }

    public static int Compare(FriendEntryData friendEntryData1, FriendEntryData friendEntryData2)
    {
      return friendEntryData1 == null ? (friendEntryData2 != null ? -1 : 0) : (friendEntryData2 == null ? 1 : string.CompareOrdinal(friendEntryData1.GamerTag.ToUpperInvariant(), friendEntryData2.GamerTag.ToUpperInvariant()));
    }

    public int CompareTo(object obj) => FriendEntryData.Compare(this, obj as FriendEntryData);
  }
}
