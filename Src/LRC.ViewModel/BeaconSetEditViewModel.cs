// *********************************************************
// Type: LRC.ViewModel.BeaconSetEditViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Resources;
using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Xbox.Live.Phone.Services;
using Xbox.Live.Phone.Services.Beacons;
using Xbox.Live.Phone.Utils;


namespace LRC.ViewModel
{
  [XmlRoot(Namespace = "")]
  [DataContract(Namespace = "")]
  public class BeaconSetEditViewModel : ViewModelBase
  {
    private const int MaxMessageBodyCharacterCount = 40;
    private const string PerfGetBeaconForTitle = "BeaconSetEditViewModel:GetBeaconForTitle";
    private const string PerfSetBeaconForTitle = "BeaconSetEditViewModel:SetBeaconForTitle";
    private const string PerfDeleteBeaconForTitle = "BeaconSetEditViewModel:DeleteBeaconForTitle";
    private BeaconServiceCallType pendingServiceCall;
    private string commentBody;
    private bool isBeaconSet;
    private bool canUserSetBeaconText;
    private GameItem gameItemViewModel;

    public BeaconSetEditViewModel() => this.LifetimeInMinutes = int.MaxValue;

    public BeaconSetEditViewModel(GameItem gameItem)
      : this()
    {
      this.GameItemViewModel = gameItem != null ? gameItem : throw new ArgumentNullException(nameof (gameItem));
      this.CanUserSetBeaconText = true;
    }

    public event EventHandler<ServiceProxyEventArgs<string>> EventRemoveBeaconCompleted;

    public event EventHandler<ServiceProxyEventArgs<string>> EventSetBeaconCompleted;

    public static int MaxCommentBodyCharacterCountProperty => 40;

    [IgnoreDataMember]
    public string SetBeaconButtonText
    {
      get
      {
        return this.IsBeaconSet ? Resource.Beacon_EditBeaconButtonText : Resource.Beacon_SetBeaconButtonText;
      }
    }

    [DataMember]
    public GameItem GameItemViewModel
    {
      get => this.gameItemViewModel;
      set
      {
        this.SetPropertyValue<GameItem>(ref this.gameItemViewModel, value, nameof (GameItemViewModel));
      }
    }

    [DataMember]
    public string CommentBody
    {
      get => this.commentBody;
      set => this.SetPropertyValue<string>(ref this.commentBody, value, nameof (CommentBody));
    }

    [DataMember]
    public bool IsBeaconSet
    {
      get => this.isBeaconSet;
      set
      {
        this.SetPropertyValue<bool>(ref this.isBeaconSet, value, nameof (IsBeaconSet));
        this.NotifyPropertyChanged("BeaconHeaderText");
        this.NotifyPropertyChanged("SetBeaconButtonText");
      }
    }

    [DataMember]
    public bool CanUserSetBeaconText
    {
      get => this.canUserSetBeaconText;
      set
      {
        this.SetPropertyValue<bool>(ref this.canUserSetBeaconText, value, nameof (CanUserSetBeaconText));
      }
    }

    public string BeaconHeaderText
    {
      get => !this.IsBeaconSet ? Resource.Beacon_SetBeaconHeader : Resource.Beacon_EditBeaconHeader;
    }

    public override void Load()
    {
      if (!this.ShouldLoadData)
        return;
      this.CallBeaconService(BeaconServiceCallType.GetBeacon);
    }

    public void CallBeaconService(BeaconServiceCallType serviceCallType)
    {
      this.pendingServiceCall = serviceCallType;
      if (XboxLiveGamer.CurrentGamer.XboxUserId == 0UL)
      {
        this.IsBlocking = true;
        XboxLiveGamer.CurrentGamer.EventLoadedXboxUserId += new EventHandler<EventArgs>(this.LoadXboxUserIdCompleted);
        XboxLiveGamer.LoadXboxUserId();
      }
      else
        this.InternalCallBeaconService();
    }

    private void LoadXboxUserIdCompleted(object sender, EventArgs e)
    {
      this.IsBlocking = false;
      XboxLiveGamer.CurrentGamer.EventLoadedXboxUserId -= new EventHandler<EventArgs>(this.LoadXboxUserIdCompleted);
      if (XboxLiveGamer.CurrentGamer.XboxUserId == 0UL)
      {
        switch (this.pendingServiceCall)
        {
          case BeaconServiceCallType.SetBeacon:
            this.NotifyError(ErrorCodeEnum.FailedToSetBeacon);
            if (this.EventSetBeaconCompleted == null)
              break;
            this.EventSetBeaconCompleted((object) this, new ServiceProxyEventArgs<string>((object) null, (Exception) new XMobileException(0), false, (object) null));
            break;
          case BeaconServiceCallType.DeleteBeacon:
            this.NotifyError(ErrorCodeEnum.FailedToDeleteBeacon);
            if (this.EventRemoveBeaconCompleted == null)
              break;
            this.EventRemoveBeaconCompleted((object) this, new ServiceProxyEventArgs<string>((object) null, (Exception) new XMobileException(0), false, (object) null));
            break;
          default:
            this.IsBeaconSet = false;
            this.CommentBody = string.Empty;
            break;
        }
      }
      else
        this.InternalCallBeaconService();
    }

    private void InternalCallBeaconService()
    {
      this.IsBlocking = true;
      switch (this.pendingServiceCall)
      {
        case BeaconServiceCallType.GetBeacon:
          XboxLiveGamer.CurrentGamer.EventGetBeaconForTitleCompleted += new EventHandler<ServiceProxyEventArgs<BeaconInfo>>(this.OnGetBeaconForTitleCompleted);
          XboxLiveGamer.CurrentGamer.GetBeaconForTitle(this.GameItemViewModel.TitleId);
          break;
        case BeaconServiceCallType.SetBeacon:
          XboxLiveGamer.CurrentGamer.EventSetBeaconForTitleCompleted += new EventHandler<ServiceProxyEventArgs<string>>(this.OnSetBeaconForTitleCompleted);
          XboxLiveGamer.CurrentGamer.SetBeaconForTitle(this.GameItemViewModel.TitleId, this.CommentBody);
          break;
        case BeaconServiceCallType.DeleteBeacon:
          XboxLiveGamer.CurrentGamer.EventDeleteBeaconForTitleCompleted += new EventHandler<ServiceProxyEventArgs<string>>(this.OnDeleteBeaconForTitleCompleted);
          XboxLiveGamer.CurrentGamer.DeleteBeaconForTitle(this.GameItemViewModel.TitleId);
          break;
      }
    }

    private void OnGetBeaconForTitleCompleted(object sender, ServiceProxyEventArgs<BeaconInfo> e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      XboxLiveGamer.CurrentGamer.EventGetBeaconForTitleCompleted -= new EventHandler<ServiceProxyEventArgs<BeaconInfo>>(this.OnGetBeaconForTitleCompleted);
      this.IsBlocking = false;
      if (e.Error != null)
      {
        this.IsBeaconSet = false;
        this.IsBlocking = true;
        XboxLiveGamer.CurrentGamer.EventGetBeaconDefaultTextCompleted += new EventHandler<ServiceProxyEventArgs<BeaconDefaultTextData>>(this.OnGetBeaconDefaultTextCompleted);
        XboxLiveGamer.CurrentGamer.GetBeaconDefaultText(this.GameItemViewModel.TitleId);
      }
      else
      {
        this.LastRefreshTime = DateTime.UtcNow;
        this.IsBeaconSet = true;
        if (e.Result == null)
          return;
        this.CommentBody = e.Result.BeaconText;
      }
    }

    private void OnGetBeaconDefaultTextCompleted(
      object sender,
      ServiceProxyEventArgs<BeaconDefaultTextData> e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      XboxLiveGamer.CurrentGamer.EventGetBeaconDefaultTextCompleted -= new EventHandler<ServiceProxyEventArgs<BeaconDefaultTextData>>(this.OnGetBeaconDefaultTextCompleted);
      this.IsBlocking = false;
      if (e.Error != null)
      {
        if (e.Error is XLiveMobileException error && error.ErrorCode == 2)
        {
          this.CommentBody = Resource.Beacon_UserBannedCommentBoxText;
          this.CanUserSetBeaconText = false;
        }
        else
          this.CommentBody = string.Empty;
      }
      else
      {
        this.LastRefreshTime = DateTime.UtcNow;
        this.CanUserSetBeaconText = true;
        if (e.Result == null)
          return;
        this.CommentBody = e.Result.BeaconDefaultText;
      }
    }

    private void OnDeleteBeaconForTitleCompleted(object sender, ServiceProxyEventArgs<string> e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      XboxLiveGamer.CurrentGamer.EventDeleteBeaconForTitleCompleted -= new EventHandler<ServiceProxyEventArgs<string>>(this.OnDeleteBeaconForTitleCompleted);
      this.IsBlocking = false;
      if (e.Error != null && (!(e.Error is XLiveMobileException error) || error.ErrorCode != 1))
        this.NotifyError(ErrorCodeEnum.FailedToDeleteBeacon);
      if (this.EventRemoveBeaconCompleted == null)
        return;
      this.EventRemoveBeaconCompleted((object) this, e);
    }

    private void OnSetBeaconForTitleCompleted(object sender, ServiceProxyEventArgs<string> e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      XboxLiveGamer.CurrentGamer.EventSetBeaconForTitleCompleted -= new EventHandler<ServiceProxyEventArgs<string>>(this.OnSetBeaconForTitleCompleted);
      this.IsBlocking = false;
      if (e.Error != null)
      {
        ErrorCodeEnum errorCodeEnum = ErrorCodeEnum.FailedToSetBeacon;
        if (e.Error is XMobileException error)
        {
          switch (error.ErrorCode)
          {
            case 3:
              errorCodeEnum = ErrorCodeEnum.FailedToSetBeacon_BeaconTextTooLong;
              break;
            case 4:
              errorCodeEnum = ErrorCodeEnum.FailedToSetBeacon_BeaconLimitReached;
              break;
            default:
              errorCodeEnum = ErrorCodeEnum.FailedToSetBeacon;
              break;
          }
        }
        this.NotifyError(errorCodeEnum);
      }
      if (this.EventSetBeaconCompleted == null)
        return;
      this.EventSetBeaconCompleted((object) this, e);
    }
  }
}
