// *********************************************************
// Type: Leet.MessageService.DataContracts.MessageSummary
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;
using System.Runtime.Serialization;
using Xbox.Live.Phone.Services.Resources;


namespace Leet.MessageService.DataContracts
{
  [DataContract(Name = "MessageSummary", Namespace = "http://schemas.datacontract.org/2004/07/GDS.Contracts")]
  public class MessageSummary
  {
    private string displayableSubject;

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "MessageId", Order = 0)]
    public uint MessageId { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "MessageType", Order = 1)]
    public uint MessageType { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "SenderGamertag", Order = 2)]
    public string SenderGamertag { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "SenderTitleId", Order = 3)]
    public uint SenderTitleId { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "SentTime", Order = 4)]
    public DateTime SentTime { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "Subject", Order = 5)]
    public string Subject { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "HasBeenRead", Order = 6)]
    public bool HasBeenRead { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "IsFromFriend", Order = 7)]
    public bool IsFromFriend { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "CanDelete", Order = 8)]
    public bool CanDelete { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "CanSetReadFlag", Order = 9)]
    public bool CanSetReadFlag { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "HasVoice", Order = 10)]
    public bool HasVoice { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "HasImage", Order = 11)]
    public bool HasImage { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "HasText", Order = 12)]
    public bool HasText { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "SenderGamerPic", Order = 13)]
    public string SenderGamerPicUrl { get; set; }

    public MessageSummary Summary => this;

    public bool HasNotBeenRead => !this.HasBeenRead;

    public bool CanDisplayOnDevice => this.MessageType != 2U;

    public string SentDate => this.SentTime.ToLocalTime().ToShortDateString();

    public string DisplayableSubject
    {
      get
      {
        if (!string.IsNullOrEmpty(this.displayableSubject))
          return this.displayableSubject;
        if (!string.IsNullOrEmpty(this.Subject))
        {
          this.displayableSubject = this.Subject;
        }
        else
        {
          switch (this.MessageType)
          {
            case 3:
              this.displayableSubject = Resource.Message_Summary_GameInvite;
              break;
            case 8:
              this.displayableSubject = !this.HasImage ? (!this.HasVoice ? Resource.Message_Summary_EmptySubject : Resource.Message_Subject_EmptySubjectWithVoice) : Resource.Message_Subject_EmptySubjectWithImage;
              break;
            case 9:
              this.displayableSubject = Resource.Message_Summary_VideoMessage;
              break;
            case 10:
              this.displayableSubject = Resource.Message_Summary_QuickChatInvite;
              break;
            case 11:
              this.displayableSubject = Resource.Message_Summary_VideoChatInvite;
              break;
            case 12:
              this.displayableSubject = Resource.Message_Summary_PartyInvite;
              break;
            default:
              this.displayableSubject = Resource.Message_Summary_EmptySubject;
              break;
          }
        }
        return this.displayableSubject;
      }
    }
  }
}
