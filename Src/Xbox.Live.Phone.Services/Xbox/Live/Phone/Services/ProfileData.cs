// *********************************************************
// Type: Xbox.Live.Phone.Services.ProfileData
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Leet.MessageService.DataContracts;
using Leet.UserGameData.DataContracts;
using System;


namespace Xbox.Live.Phone.Services
{
  public class ProfileData
  {
    private XboxLiveGamer gamer;

    public ProfileData(XboxLiveGamer gamer) => this.gamer = gamer;

    public string Bio
    {
      get
      {
        return this.gamer.ProfileProperties != null ? (string) this.gamer.ProfileProperties[ProfileProperty.Bio] : string.Empty;
      }
      set
      {
        if (this.gamer.ProfileProperties == null)
          throw new InvalidOperationException("Cannot set value when gamer.ProfileProperties is null.");
        this.gamer.ProfileProperties[ProfileProperty.Bio] = (object) value;
      }
    }

    public string Motto
    {
      get
      {
        return this.gamer.ProfileProperties != null ? (string) this.gamer.ProfileProperties[ProfileProperty.Motto] : string.Empty;
      }
      set
      {
        if (this.gamer.ProfileProperties == null)
          throw new InvalidOperationException("Cannot set value when gamer.ProfileProperties is null.");
        this.gamer.ProfileProperties[ProfileProperty.Motto] = (object) value;
      }
    }

    public int GamerScore
    {
      get
      {
        return this.gamer.ProfileProperties != null ? (int) this.gamer.ProfileProperties[ProfileProperty.GamerScore] : 0;
      }
    }

    public string Location
    {
      get
      {
        return this.gamer.ProfileProperties != null ? (string) this.gamer.ProfileProperties[ProfileProperty.Location] : string.Empty;
      }
      set
      {
        if (this.gamer.ProfileProperties == null)
          throw new InvalidOperationException("Cannot set value when gamer.ProfileProperties is null.");
        this.gamer.ProfileProperties[ProfileProperty.Location] = (object) value;
      }
    }

    public string Name
    {
      get
      {
        return this.gamer.ProfileProperties != null ? (string) this.gamer.ProfileProperties[ProfileProperty.Name] : string.Empty;
      }
      set
      {
        if (this.gamer.ProfileProperties == null)
          throw new InvalidOperationException("Cannot set value when gamer.ProfileProperties is null.");
        this.gamer.ProfileProperties[ProfileProperty.Name] = (object) value;
      }
    }

    public int FriendsOnlineCount
    {
      get => this.gamer.FriendsListData != null ? this.gamer.FriendsListData.FriendsOnlineCount : 0;
    }

    public int UnreadSentMessageCount
    {
      get
      {
        int sentMessageCount = 0;
        MessagesData messagesData = XboxLiveGamer.CurrentGamer.MessagesData;
        if (messagesData != null)
        {
          foreach (MessageSummary messageSummary in messagesData.MessageSummaryList)
          {
            if (!messageSummary.HasBeenRead && string.Equals(messageSummary.SenderGamertag, this.gamer.GamerTag, StringComparison.OrdinalIgnoreCase))
              ++sentMessageCount;
          }
        }
        return sentMessageCount;
      }
    }

    public bool MottoBubbleVisibility
    {
      get
      {
        return this.gamer.ProfileProperties != null && this.gamer.ProfileProperties[ProfileProperty.Motto] != null && this.gamer.ProfileProperties[ProfileProperty.Motto].ToString().Trim().Length > 0;
      }
    }
  }
}
