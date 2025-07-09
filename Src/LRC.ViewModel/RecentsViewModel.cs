// *********************************************************
// Type: LRC.ViewModel.RecentsViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Service.Omniture;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Threading;
using System.Xml.Serialization;
using Xbox.Live.Phone.Services;
using Xbox.Live.Phone.Services.TitleHistory;
using Xbox.Live.Phone.Utils;


namespace LRC.ViewModel
{
  [DataContract(Namespace = "")]
  [XmlRoot(Namespace = "")]
  public class RecentsViewModel : ViewModelBase
  {
    private const int RecentGameCount = 15;
    private const string ComponentName = "RecentsViewModel";
    private const string PerfLoadXboxUserId = "RecentsViewModel:LoadXboxUserId";
    private const string PerfGetTitleHistory = "RecentsViewModel:GetTitleHistory";
    private const string TitleArtUrlBase = "http://tiles.xbox.com/consoleAssets/{0}/{1}/smallboxart.jpg";
    private ObservableCollection<RecentItemViewModel> recentItems;

    public RecentsViewModel()
    {
      this.LifetimeInMinutes = 5;
      this.RecentItems = new ObservableCollection<RecentItemViewModel>();
    }

    [DataMember]
    public ObservableCollection<RecentItemViewModel> RecentItems
    {
      get => this.recentItems;
      set
      {
        this.SetPropertyValue<ObservableCollection<RecentItemViewModel>>(ref this.recentItems, value, nameof (RecentItems));
      }
    }

    public override void Load()
    {
      if (!this.ShouldLoadData || XboxLiveGamer.CurrentGamer == null)
        return;
      this.IsBusy = true;
      this.CurrentState = this.RecentItems.Count == 0 ? 3 : 0;
      this.CallTitleHistoryService();
    }

    public void CallTitleHistoryService()
    {
      if (XboxLiveGamer.CurrentGamer.XboxUserId == 0UL)
      {
        this.IsBlocking = true;
        XboxLiveGamer.CurrentGamer.EventLoadedXboxUserId += new EventHandler<EventArgs>(this.LoadXboxUserIdCompleted);
        XboxLiveGamer.LoadXboxUserId();
      }
      else
      {
        this.InternalCallTitleHistoryService();
        OmnitureAppMeasurement.Instance.XboxUserId = XboxLiveGamer.CurrentGamer.XboxUserId;
      }
    }

    internal static string GetTitleIdBoxArtUrl(uint titleId)
    {
      if (titleId == 0U)
        return (string) null;
      return XboxLiveGamer.CurrentGamer != null && !string.IsNullOrWhiteSpace(XboxLiveGamer.CurrentGamer.LegalLocale) ? string.Format((IFormatProvider) CultureInfo.InvariantCulture, "http://tiles.xbox.com/consoleAssets/{0}/{1}/smallboxart.jpg", new object[2]
      {
        (object) titleId.ToString("x", (IFormatProvider) CultureInfo.InvariantCulture),
        (object) XboxLiveGamer.CurrentGamer.LegalLocale
      }) : string.Format((IFormatProvider) CultureInfo.InvariantCulture, "http://tiles.xbox.com/consoleAssets/{0}/{1}/smallboxart.jpg", new object[2]
      {
        (object) titleId.ToString("x", (IFormatProvider) CultureInfo.InvariantCulture),
        (object) CultureInfo.CurrentUICulture.Name
      });
    }

    private void InternalCallTitleHistoryService()
    {
      XboxLiveGamer.CurrentGamer.EventGetTitleHistoryCompleted += new EventHandler<ServiceProxyEventArgs<List<TitleHistoryInfo>>>(this.OnGetTitleHistoryCompleted);
      XboxLiveGamer.CurrentGamer.GetTitleHistory();
    }

    private void LoadXboxUserIdCompleted(object sender, EventArgs e)
    {
      this.IsBlocking = false;
      XboxLiveGamer.CurrentGamer.EventLoadedXboxUserId -= new EventHandler<EventArgs>(this.LoadXboxUserIdCompleted);
      if (XboxLiveGamer.CurrentGamer.XboxUserId == 0UL)
      {
        this.CurrentState = this.RecentItems.Count == 0 ? 1 : 0;
      }
      else
      {
        this.InternalCallTitleHistoryService();
        OmnitureAppMeasurement.Instance.XboxUserId = XboxLiveGamer.CurrentGamer.XboxUserId;
      }
    }

    private void OnGetTitleHistoryCompleted(
      object sender,
      ServiceProxyEventArgs<List<TitleHistoryInfo>> e)
    {
      this.IsBusy = false;
      XboxLiveGamer.CurrentGamer.EventGetTitleHistoryCompleted -= new EventHandler<ServiceProxyEventArgs<List<TitleHistoryInfo>>>(this.OnGetTitleHistoryCompleted);
      if (e == null || e.Error != null)
        this.CurrentState = this.RecentItems.Count == 0 ? 1 : 0;
      else
        ThreadPool.QueueUserWorkItem((WaitCallback) delegate
        {
          ObservableCollection<RecentItemViewModel> recentList = new ObservableCollection<RecentItemViewModel>();
          for (int index = 0; index < e.Result.Count && recentList.Count <= 15; ++index)
          {
            TitleHistoryInfo titleHistoryInfo = e.Result[index];
            RecentItemViewModel recentItemViewModel = new RecentItemViewModel()
            {
              ImageUrl = RecentsViewModel.GetTitleIdBoxArtUrl(titleHistoryInfo.TitleId),
              Title = titleHistoryInfo.Name,
              TitleId = titleHistoryInfo.TitleId,
              Id = GetMediaInfoUtil.GetMediaIdFromTitleId(titleHistoryInfo.TitleId),
              TitleType = titleHistoryInfo.TitleType
            };
            recentList.Add(recentItemViewModel);
          }
          ThreadManager.UIThreadPost((SendOrPostCallback) delegate
          {
            this.RecentItems = recentList;
            this.CurrentState = 0;
            this.LastRefreshTime = DateTime.UtcNow;
          }, (object) this);
        });
    }
  }
}
