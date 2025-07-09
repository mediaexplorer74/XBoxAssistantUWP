// *********************************************************
// Type: Xbox.Live.Phone.Services.FriendsListData
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Xbox.Live.Phone.Utils.Cache;
using Xbox.Live.Phone.Utils.Serialization;


namespace Xbox.Live.Phone.Services
{
  [XmlRoot(Namespace = "")]
  [DataContract(Namespace = "")]
  public class FriendsListData : CacheItem
  {
    private const int ExpireTime = 2;
    private XmlSerializableList<FriendEntryData> friendsList;
    private Dictionary<string, string> friendsListDictionary;
    private XmlSerializableList<FriendEntryData> pendingFriendsList;
    private XmlSerializableList<FriendEntryData> requestingFriendsList;
    private Dictionary<string, string> pendingFriendsListDictionary;
    private List<FriendEntryData> friendsWhiteList;
    private Dictionary<string, DateTime> friendsWhiteListDictionary;
    private Dictionary<string, DateTime> requestingFriendsWhiteList;
    private Dictionary<string, DateTime> friendsBlackList;
    private Dictionary<string, DateTime> pendingFriendsBlackList;
    private Dictionary<string, DateTime> requestingFriendsBlackList;
    private Dictionary<string, bool> requestingFriendsListDictionary;

    public FriendsListData(string key)
      : base(key)
    {
      this.friendsList = new XmlSerializableList<FriendEntryData>();
      this.pendingFriendsList = new XmlSerializableList<FriendEntryData>();
      this.requestingFriendsList = new XmlSerializableList<FriendEntryData>();
      this.friendsListDictionary = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      this.pendingFriendsListDictionary = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      this.requestingFriendsListDictionary = new Dictionary<string, bool>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      this.OfflineFriends = (List<FriendEntryData>) new XmlSerializableList<FriendEntryData>();
      this.OnlineFriends = (List<FriendEntryData>) new XmlSerializableList<FriendEntryData>();
      this.FriendsOnlineCount = 0;
      this.friendsWhiteList = new List<FriendEntryData>();
      this.friendsWhiteListDictionary = new Dictionary<string, DateTime>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      this.requestingFriendsWhiteList = new Dictionary<string, DateTime>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      this.friendsBlackList = new Dictionary<string, DateTime>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      this.pendingFriendsBlackList = new Dictionary<string, DateTime>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      this.requestingFriendsBlackList = new Dictionary<string, DateTime>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    }

    [DataMember]
    public int FriendsOnlineCount { get; set; }

    [DataMember]
    public XmlSerializableList<FriendEntryData> FriendsList
    {
      get => this.friendsList;
      set
      {
        this.friendsList = value;
        this.CreateOnlineOfflineFriends();
        this.ResetFriendsListDictionary();
      }
    }

    [DataMember]
    public XmlSerializableList<FriendEntryData> PendingFriendsList
    {
      get => this.pendingFriendsList;
      set
      {
        this.pendingFriendsList = value;
        this.ResetPendingFriendsListDictionary();
      }
    }

    [DataMember]
    public XmlSerializableList<FriendEntryData> RequestedFriendsList
    {
      get => this.requestingFriendsList;
      set
      {
        this.requestingFriendsList = value;
        this.ResetRequestedFriendsListDictionary();
      }
    }

    public List<FriendEntryData> OnlineFriends { get; private set; }

    public List<FriendEntryData> OfflineFriends { get; private set; }

    public bool IsPendingFriendsVisible
    {
      get => this.pendingFriendsList != null && this.pendingFriendsList.Count > 0;
    }

    public bool IsRequestedFriendsVisible
    {
      get => this.requestingFriendsList != null && this.requestingFriendsList.Count > 0;
    }

    public bool IsLoadedFromWebService { get; set; }

    public void CreateOnlineOfflineFriends()
    {
      if (this.friendsList == null)
        return;
      this.friendsList.Sort();
      if (this.OfflineFriends != null)
        this.OfflineFriends.Clear();
      else
        this.OfflineFriends = (List<FriendEntryData>) new XmlSerializableList<FriendEntryData>();
      if (this.OnlineFriends != null)
        this.OnlineFriends.Clear();
      else
        this.OnlineFriends = (List<FriendEntryData>) new XmlSerializableList<FriendEntryData>();
      foreach (FriendEntryData friends in (List<FriendEntryData>) this.friendsList)
      {
        if (friends.IsOnline)
          this.OnlineFriends.Add(friends);
        else
          this.OfflineFriends.Add(friends);
      }
      this.FriendsOnlineCount = this.OnlineFriends.Count;
    }

    public FriendEntryData GetFriendData(string gamerTag)
    {
      if (string.IsNullOrEmpty(gamerTag))
        return (FriendEntryData) null;
      FriendEntryData friendData = (FriendEntryData) null;
      if (this.IsGamerTagFriend(gamerTag))
        friendData = FriendsListData.GetFriendEntryData(gamerTag, this.friendsList);
      else if (this.IsGamerTagPendingFriend(gamerTag))
        friendData = FriendsListData.GetFriendEntryData(gamerTag, this.pendingFriendsList);
      else if (this.IsGamerTagRequestingFriend(gamerTag))
        friendData = FriendsListData.GetFriendEntryData(gamerTag, this.requestingFriendsList);
      return friendData;
    }

    public string GetFriendGamerPic(string gamerTag)
    {
      return this.IsGamerTagFriend(gamerTag) ? this.friendsListDictionary[gamerTag] : string.Empty;
    }

    public string GetPendingFriendGamerPic(string gamerTag)
    {
      return this.IsGamerTagPendingFriend(gamerTag) ? this.pendingFriendsListDictionary[gamerTag] : string.Empty;
    }

    public bool IsGamerTagFriend(string gamerTag)
    {
      return !string.IsNullOrEmpty(gamerTag) && this.friendsListDictionary != null && this.friendsListDictionary.ContainsKey(gamerTag);
    }

    public bool IsGamerTagPendingFriend(string gamerTag)
    {
      return !string.IsNullOrEmpty(gamerTag) && this.pendingFriendsListDictionary != null && this.pendingFriendsListDictionary.ContainsKey(gamerTag);
    }

    public bool IsGamerTagRequestingFriend(string gamerTag)
    {
      return !string.IsNullOrEmpty(gamerTag) && this.requestingFriendsListDictionary != null && this.requestingFriendsListDictionary.ContainsKey(gamerTag);
    }

    public void AddToFriendsList(FriendEntryData friend)
    {
      if (friend == null)
        throw new ArgumentNullException(nameof (friend));
      if (friend == null || this.friendsList == null || this.IsGamerTagFriend(friend.GamerTag))
        return;
      this.friendsList.Add(friend);
      this.friendsListDictionary.Add(friend.GamerTag, friend.GamerPicUrl);
    }

    public void AddToPendingFriendsList(FriendEntryData friend)
    {
      if (friend == null)
        throw new ArgumentNullException(nameof (friend));
      if (friend == null || this.pendingFriendsList == null || this.IsGamerTagPendingFriend(friend.GamerTag))
        return;
      this.pendingFriendsList.Add(friend);
      this.pendingFriendsListDictionary.Add(friend.GamerTag, friend.GamerPicUrl);
    }

    public void AddToRequestingFriendsList(FriendEntryData friend)
    {
      if (friend == null || this.requestingFriendsList == null || this.IsGamerTagRequestingFriend(friend.GamerTag))
        return;
      this.requestingFriendsList.Add(friend);
      this.requestingFriendsListDictionary.Add(friend.GamerTag, true);
    }

    public void AddToRequestingFriendsList(string gamerTag)
    {
      if (string.IsNullOrEmpty(gamerTag) || this.IsGamerTagRequestingFriend(gamerTag))
        return;
      FriendEntryData friendEntryData = FriendsListData.GetFriendEntryData(gamerTag, this.requestingFriendsList);
      if (friendEntryData == null)
        return;
      this.AddToRequestingFriendsList(friendEntryData);
    }

    public void RemoveFromFriendsList(string gamerTag)
    {
      if (string.IsNullOrEmpty(gamerTag) || !this.IsGamerTagFriend(gamerTag))
        return;
      FriendEntryData friendEntryData = FriendsListData.GetFriendEntryData(gamerTag, this.friendsList);
      if (friendEntryData == null)
        return;
      this.friendsList.Remove(friendEntryData);
      this.friendsListDictionary.Remove(gamerTag);
    }

    public void RemoveFromPendingFriendsList(string gamerTag)
    {
      if (string.IsNullOrEmpty(gamerTag) || !this.IsGamerTagPendingFriend(gamerTag))
        return;
      FriendEntryData friendEntryData = FriendsListData.GetFriendEntryData(gamerTag, this.pendingFriendsList);
      if (friendEntryData == null)
        return;
      this.pendingFriendsList.Remove(friendEntryData);
      this.pendingFriendsListDictionary.Remove(gamerTag);
    }

    public void RemoveFromRequestingFriendsList(string gamerTag)
    {
      if (string.IsNullOrEmpty(gamerTag) || !this.IsGamerTagRequestingFriend(gamerTag))
        return;
      FriendEntryData friendEntryData = FriendsListData.GetFriendEntryData(gamerTag, this.requestingFriendsList);
      if (friendEntryData == null)
        return;
      this.requestingFriendsList.Remove(friendEntryData);
      this.requestingFriendsListDictionary.Remove(gamerTag);
    }

    public void AddToFriendsWhiteList(FriendEntryData friend)
    {
      if (friend == null)
        throw new ArgumentNullException(nameof (friend));
      if (this.friendsWhiteList == null || this.friendsWhiteListDictionary == null || this.friendsWhiteListDictionary.ContainsKey(friend.GamerTag))
        return;
      this.friendsWhiteList.Add(friend);
      this.friendsWhiteListDictionary.Add(friend.GamerTag, DateTime.Now);
    }

    public void AddToRequestingFriendsWhiteList(string gamerTag)
    {
      if (this.requestingFriendsWhiteList == null || this.requestingFriendsWhiteList.ContainsKey(gamerTag))
        return;
      this.requestingFriendsWhiteList.Add(gamerTag, DateTime.Now);
    }

    public void AddToFriendsBlackList(string gamerTag)
    {
      if (this.friendsBlackList == null || this.friendsBlackList.ContainsKey(gamerTag))
        return;
      this.friendsBlackList.Add(gamerTag, DateTime.Now);
    }

    public void AddToPendingFriendsBlackList(string gamerTag)
    {
      if (this.pendingFriendsBlackList == null || this.pendingFriendsBlackList.ContainsKey(gamerTag))
        return;
      this.pendingFriendsBlackList.Add(gamerTag, DateTime.Now);
    }

    public void AddToRequestingFriendsBlackList(string gamerTag)
    {
      if (this.requestingFriendsBlackList == null || this.requestingFriendsBlackList.ContainsKey(gamerTag))
        return;
      this.requestingFriendsBlackList.Add(gamerTag, DateTime.Now);
    }

    public void SyncWithLocalData(FriendsListData localFriendListData)
    {
      if (localFriendListData == null)
        return;
      if (localFriendListData.friendsWhiteList != null)
        this.friendsWhiteList = localFriendListData.friendsWhiteList;
      if (localFriendListData.friendsWhiteListDictionary != null)
        this.friendsWhiteListDictionary = localFriendListData.friendsWhiteListDictionary;
      if (localFriendListData.requestingFriendsWhiteList != null)
        this.requestingFriendsWhiteList = localFriendListData.requestingFriendsWhiteList;
      if (localFriendListData.friendsBlackList != null)
        this.friendsBlackList = localFriendListData.friendsBlackList;
      if (localFriendListData.pendingFriendsBlackList != null)
        this.pendingFriendsBlackList = localFriendListData.pendingFriendsBlackList;
      if (localFriendListData.requestingFriendsBlackList != null)
        this.requestingFriendsBlackList = localFriendListData.requestingFriendsBlackList;
      this.SyncFriendsList();
      this.SyncPendingFriendsList();
      this.SyncRequestingFriendsList();
    }

    private static bool IsNotExpired(DateTime date)
    {
      return DateTime.Now.CompareTo(date.AddMinutes(2.0)) < 0;
    }

    private static FriendEntryData GetFriendEntryData(
      string gamerTag,
      XmlSerializableList<FriendEntryData> list)
    {
      FriendEntryData friendEntryData1 = (FriendEntryData) null;
      foreach (FriendEntryData friendEntryData2 in (List<FriendEntryData>) list)
      {
        if (string.Equals(gamerTag, friendEntryData2.GamerTag, StringComparison.OrdinalIgnoreCase))
        {
          friendEntryData1 = friendEntryData2;
          break;
        }
      }
      return friendEntryData1;
    }

    private void SyncFriendsList()
    {
      List<FriendEntryData> friendEntryDataList = new List<FriendEntryData>();
      List<string> stringList = new List<string>();
      if (this.friendsWhiteList != null)
      {
        foreach (FriendEntryData friendsWhite in this.friendsWhiteList)
        {
          if (!this.friendsListDictionary.ContainsKey(friendsWhite.GamerTag) && FriendsListData.IsNotExpired(this.friendsWhiteListDictionary[friendsWhite.GamerTag]))
            this.AddToFriendsList(friendsWhite);
          else
            friendEntryDataList.Add(friendsWhite);
        }
      }
      if (this.friendsBlackList != null)
      {
        foreach (KeyValuePair<string, DateTime> friendsBlack in this.friendsBlackList)
        {
          if (this.friendsListDictionary.ContainsKey(friendsBlack.Key) && FriendsListData.IsNotExpired(friendsBlack.Value))
            this.RemoveFromFriendsList(friendsBlack.Key);
          else
            stringList.Add(friendsBlack.Key);
        }
      }
      this.CreateOnlineOfflineFriends();
      foreach (FriendEntryData friendEntryData in friendEntryDataList)
      {
        this.friendsWhiteList.Remove(friendEntryData);
        this.friendsWhiteListDictionary.Remove(friendEntryData.GamerTag);
      }
      foreach (string key in stringList)
        this.friendsBlackList.Remove(key);
    }

    private void SyncPendingFriendsList()
    {
      List<string> stringList = new List<string>();
      if (this.pendingFriendsBlackList != null)
      {
        foreach (KeyValuePair<string, DateTime> pendingFriendsBlack in this.pendingFriendsBlackList)
        {
          if (this.pendingFriendsListDictionary.ContainsKey(pendingFriendsBlack.Key) && FriendsListData.IsNotExpired(pendingFriendsBlack.Value))
            this.RemoveFromPendingFriendsList(pendingFriendsBlack.Key);
          else
            stringList.Add(pendingFriendsBlack.Key);
        }
      }
      foreach (string key in stringList)
        this.pendingFriendsBlackList.Remove(key);
    }

    private void SyncRequestingFriendsList()
    {
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      if (this.requestingFriendsWhiteList != null)
      {
        foreach (KeyValuePair<string, DateTime> requestingFriendsWhite in this.requestingFriendsWhiteList)
        {
          if (!this.requestingFriendsListDictionary.ContainsKey(requestingFriendsWhite.Key) && FriendsListData.IsNotExpired(requestingFriendsWhite.Value))
            this.AddToRequestingFriendsList(requestingFriendsWhite.Key);
          else
            stringList1.Add(requestingFriendsWhite.Key);
        }
      }
      if (this.requestingFriendsBlackList != null)
      {
        foreach (KeyValuePair<string, DateTime> requestingFriendsBlack in this.requestingFriendsBlackList)
        {
          if (this.requestingFriendsListDictionary.ContainsKey(requestingFriendsBlack.Key) && FriendsListData.IsNotExpired(requestingFriendsBlack.Value))
            this.RemoveFromRequestingFriendsList(requestingFriendsBlack.Key);
          else
            stringList2.Add(requestingFriendsBlack.Key);
        }
      }
      foreach (string key in stringList1)
        this.requestingFriendsWhiteList.Remove(key);
      foreach (string key in stringList2)
        this.requestingFriendsBlackList.Remove(key);
    }

    private void ResetFriendsListDictionary()
    {
      if (this.friendsListDictionary != null)
        this.friendsListDictionary.Clear();
      else
        this.friendsListDictionary = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      if (this.friendsList == null)
        return;
      foreach (FriendEntryData friends in (List<FriendEntryData>) this.friendsList)
        this.friendsListDictionary.Add(friends.GamerTag, friends.GamerPicUrl);
    }

    private void ResetPendingFriendsListDictionary()
    {
      if (this.pendingFriendsListDictionary != null)
        this.pendingFriendsListDictionary.Clear();
      else
        this.pendingFriendsListDictionary = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      if (this.pendingFriendsList == null)
        return;
      foreach (FriendEntryData pendingFriends in (List<FriendEntryData>) this.pendingFriendsList)
        this.pendingFriendsListDictionary.Add(pendingFriends.GamerTag, pendingFriends.GamerPicUrl);
    }

    private void ResetRequestedFriendsListDictionary()
    {
      if (this.requestingFriendsListDictionary != null)
        this.requestingFriendsListDictionary.Clear();
      else
        this.requestingFriendsListDictionary = new Dictionary<string, bool>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      if (this.requestingFriendsList == null)
        return;
      foreach (FriendEntryData requestingFriends in (List<FriendEntryData>) this.requestingFriendsList)
        this.requestingFriendsListDictionary.Add(requestingFriends.GamerTag, true);
    }
  }
}
