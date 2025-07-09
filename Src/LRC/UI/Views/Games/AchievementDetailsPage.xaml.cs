// *********************************************************
// Type: LRC.AchievementDetailsPage
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.ViewModel;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace LRC
{
  public class AchievementDetailsPage : LrcPage
  {
Grid LayoutRoot;
Grid AchievementEntry;
Image AchievementTile;
TextBlock AchievementTitle;
StackPanel Points;
Image GamerscoreG;
TextBlock GamerscoreText;
TextBlock AchievementDateEarned;
TextBlock AchievementDescription;
TextBlock PageTitle;
MediaBar MediaControls;
    

    public AchievementDetailsPage()
    {
      this.InitializeComponent();
      this.NeedToSaveViewModel = false;
      this.AllowMultipleInstances = false;
      this.Persistent = false;
      this.DataContext = (object) App.GlobalData.SelectedAchievement;
      App.GlobalData.SelectedAchievement = (AchievementViewModel) null;
      this.OmniturePageName = "wp:lrc:gamedetail:achievementdetail";
      this.OmnitureChannelName = "wp:lrc:game";
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      this.OmnitureTrackPageVisit();
    }

    // [Удалено для UWP портирования]
// public void InitializeComponent()
// {
      
        return;
      
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Views/Games/AchievementDetailsPage.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.AchievementEntry = (Grid) ((FrameworkElement) this).FindName("AchievementEntry");
      this.AchievementTile = (Image) ((FrameworkElement) this).FindName("AchievementTile");
      this.AchievementTitle = (TextBlock) ((FrameworkElement) this).FindName("AchievementTitle");
      this.Points = (StackPanel) ((FrameworkElement) this).FindName("Points");
      this.GamerscoreG = (Image) ((FrameworkElement) this).FindName("GamerscoreG");
      this.GamerscoreText = (TextBlock) ((FrameworkElement) this).FindName("GamerscoreText");
      this.AchievementDateEarned = (TextBlock) ((FrameworkElement) this).FindName("AchievementDateEarned");
      this.AchievementDescription = (TextBlock) ((FrameworkElement) this).FindName("AchievementDescription");
      this.PageTitle = (TextBlock) ((FrameworkElement) this).FindName("PageTitle");
      this.MediaControls = (MediaBar) ((FrameworkElement) this).FindName("MediaControls");
    }
  }
}
