// *********************************************************
// Type: LRC.GameDetailsPage
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Service.Omniture;
using LRC.ViewModel;
using Microsoft.Phone.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Navigation;
using XLToolKit;


namespace LRC
{
  public class GameDetailsPage : LrcPivotPage
  {
    private const string GameOverviewPivotName = "overview";
    private const string FriendPivotName = "friends";
    private const string AchievementPivotName = "achievements";
    private const string ImagesPivotName = "images";
    public static readonly DependencyProperty IsRelatedAvailableProperty = DependencyProperty.Register("IsRelatedAvailable", typeof (bool), typeof (GameDetailsPage), new PropertyMetadata((object) false, (PropertyChangedCallback) ((d, e) => ((GameDetailsPage) d).AddRelatedPivotItem(e))));
BusyIndicator BusyIndicator;
VisualStateGroup VisualStateGroup;
VisualState Collapsed;
VisualState Expanded;
Grid LayoutRoot;
PivotItem related;
RelatedItems GameRelatedItems;
XLPivot GameDetailsPivot;
PivotItem overview;
ScrollViewer GameOverview;
Grid GameRootPanel;
Grid GameGrid;
Image GameImage;
TextBlock GameDetailsName;
TextBlock PublisherName;
StarRatingControl StarRating;
TextBlock GameDetailsRelease;
Button BeaconButton;
StackPanel GameDescriptionPanel;
TextBlock DescriptionTitle;
TextBlock Description;
Button MoreButton;
Button LessButton;
TextBlock PlayOnXboxTitle;
Button LaunchGame;
TextBlock GameRating;
Image GameRatingIcon;
ItemsControl GameRatingDescriptorControl;
TextBlock OverviewErrorText;
BusyIndicator OverviewLoadingIndicator;
PivotItem friends;
Grid FriendPivotGrid;
Button FriendsRefreshButton;
ScrollViewer FriendsPlayingScrollViewer;
TextBlock FriendActivity;
ListBox FriendsPlayingListBox;
TextBlock FriendsErrorText;
TextBlock NoFriendsText;
BusyIndicator FriendsLoadingIndicator;
PivotItem achievements;
Grid AchievementPivotGrid;
Grid AchievementsTotals;
TextBlock GamerscoreText;
Image GamerscoreG;
TextBlock AchievementProgressText;
Image AchievementsIcon;
Button AchievementsRefreshButton;
BusyIndicator AchievementsBusyIndicator;
ListBoxWithCompression AchievementsListBox;
TextBlock AchievementsErrorText;
TextBlock NoAchievementsText;
BusyIndicator AchievementLoadingIndicator;
PivotItem images;
GameImages GameImages;
TextBlock ImageErrorText;
BusyIndicator ImageLoadingIndicator;
MediaBar MediaControls;
    

    public GameDetailsPage()
    {
      this.InitializeComponent();
      this.NeedToSaveViewModel = true;
      this.AllowMultipleInstances = false;
      this.Persistent = false;
      this.OmniturePageName = "wp:lrc:gamedetail";
      this.OmnitureChannelName = "wp:lrc:game";
    }

    protected override Pivot Pivot => (Pivot) this.GameDetailsPivot;

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      if (!(this.DataContext is GameItem gameItem))
      {
        gameItem = App.GlobalData.SelectedMediaDetails as GameItem;
        App.GlobalData.SelectedMediaDetails = (ViewModelBase) null;
        this.DataContext = (object) gameItem;
      }
      if (gameItem == null)
        return;
      if (gameItem.SearchItemViewModel != null)
        gameItem.SearchItemViewModel.Title = gameItem.Title;
      Binding binding = new Binding("IsRelatedAvailable")
      {
        Source = (object) gameItem.RelatedItemsViewModel
      };
      BindingOperations.SetBinding((DependencyObject) this, GameDetailsPage.IsRelatedAvailableProperty, (BindingBase) binding);
      gameItem.Load();
    }

    protected override void OnError(object sender, LRCAsyncCompletedEventArgs eventArgs)
    {
    }

    private void Pivot_Loaded(object sender, RoutedEventArgs e)
    {
      if (!(sender is Pivot pivot) || !(pivot.SelectedItem is PivotItem selectedItem))
        return;
      this.UpdateBusyIndicators(selectedItem);
    }

    private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (sender is Pivot pivot && pivot.SelectedItem is PivotItem selectedItem)
        this.UpdateBusyIndicators(selectedItem);
      this.OmnitureTrackPageVisit();
    }

    private void UpdateBusyIndicators(PivotItem pivotItem)
    {
      if (pivotItem == null)
        return;
      this.OverviewLoadingIndicator.IsOffscreen = true;
      this.FriendsLoadingIndicator.IsOffscreen = true;
      this.AchievementsBusyIndicator.IsOffscreen = true;
      this.AchievementLoadingIndicator.IsOffscreen = true;
      this.ImageLoadingIndicator.IsOffscreen = true;
      switch (((FrameworkElement) pivotItem).Name)
      {
        case "overview":
          this.OverviewLoadingIndicator.IsOffscreen = false;
          break;
        case "friends":
          this.FriendsLoadingIndicator.IsOffscreen = false;
          break;
        case "achievements":
          this.AchievementsBusyIndicator.IsOffscreen = false;
          this.AchievementLoadingIndicator.IsOffscreen = false;
          break;
        case "images":
          this.ImageLoadingIndicator.IsOffscreen = false;
          break;
      }
    }

    private void AchievementsListBox_Loaded(object sender, RoutedEventArgs e)
    {
      if (!(sender is ListBoxWithCompression boxWithCompression))
        return;
      boxWithCompression.EventListboxBottomReached += new EventHandler<VisualStateChangedEventArgs>(this.AchievementsListBox_BottomReached);
    }

    private void AchievementsListBox_BottomReached(object sender, VisualStateChangedEventArgs e)
    {
      ((GameItem) this.DataContext)?.AchievementListViewModel.ProcessPendingAchievements(true);
    }

    private void AchievementsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      ListBox listBox = sender as ListBox;
      if (((Selector) listBox).SelectedIndex == -1)
        return;
      App.GlobalData.SelectedAchievement = ((Selector) listBox).SelectedItem as AchievementViewModel;
      if (App.GlobalData.SelectedAchievement != null)
        NavHelper.SafeNavigate((Page) this, NavHelper.AchievementDetailsPage);
      ((Selector) listBox).SelectedIndex = -1;
    }

    private void FriendsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      ListBox listBox = sender as ListBox;
      if (((Selector) listBox).SelectedIndex == -1)
        return;
      if (((Selector) listBox).SelectedItem is FriendViewModel selectedItem)
        ((GameItem) this.DataContext).LaunchGamercard(selectedItem.GamerTag);
      ((Selector) listBox).SelectedIndex = -1;
    }

    private void BeaconButton_Click(object sender, RoutedEventArgs e)
    {
      if (this.DataContext == null)
        return;
      App.GlobalData.SelectedMediaDetails = (ViewModelBase) this.DataContext;
      NavHelper.SafeNavigate((Page) this, NavHelper.BeaconSetEditPageUri);
    }

    private void AchievementsRefreshButton_Click(object sender, RoutedEventArgs e)
    {
      if (this.DataContext == null)
        return;
      this.AchievementsListBox.ResetPosition();
      ((GameItem) this.DataContext).AchievementListViewModel.RefreshData();
    }

    private void FriendsRefreshButton_Click(object sender, RoutedEventArgs e)
    {
      if (this.DataContext == null)
        return;
      this.FriendsPlayingScrollViewer.ScrollToVerticalOffset(0.0);
      ((GameItem) this.DataContext).FriendsPlaying.RefreshData();
    }

    private void LaunchGame_Click(object sender, RoutedEventArgs e)
    {
      if (!(this.DataContext is GameItem dataContext) || dataContext.SearchItemViewModel == null)
        return;
      dataContext.LaunchGame(dataContext.SearchItemViewModel.TitleId, dataContext.SearchItemViewModel.Id, (object) null);
      OmnitureAppMeasurement.Instance.TrackPlayOnConsoleEvent("played on Xbox", this.OmnitureEntryPointName, new OmnitureMediaContentTrackInfo()
      {
        Title = dataContext.Title,
        MediaType = "game",
        MediaId = dataContext.Id,
        Rank = -1,
        Genre = dataContext.SearchItemViewModel.Genres == null || dataContext.SearchItemViewModel.Genres.Count <= 0 ? "na" : dataContext.SearchItemViewModel.Genres[0],
        Studio = "na",
        Network = "na"
      }, new OmnitureProviderTrackInfo()
      {
        Title = "na",
        TitleId = 0U,
        ProductId = "na",
        LaunchType = "plain"
      });
    }

    private void MoreButton_Click(object sender, RoutedEventArgs e)
    {
      VisualStateManager.GoToState((Control) this, "Expanded", true);
    }

    private void LessButton_Click(object sender, RoutedEventArgs e)
    {
      VisualStateManager.GoToState((Control) this, "Collapsed", true);
    }

    private void Description_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (((FrameworkElement) this.Description).ActualHeight <= ((FrameworkElement) this.Description).MaxHeight)
        ((UIElement) this.MoreButton).Visibility = (Visibility) 1;
      else
        ((UIElement) this.MoreButton).Visibility = (Visibility) 0;
    }

    private void AddRelatedPivotItem(DependencyPropertyChangedEventArgs e)
    {
      if (!(bool) e.NewValue)
        return;
      ((PresentationFrameworkCollection<UIElement>) ((Panel) this.LayoutRoot).Children).Remove((UIElement) this.related);
      ((UIElement) this.related).Visibility = (Visibility) 0;
      ((PresentationFrameworkCollection<object>) this.Pivot.Items).Add((object) this.related);
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      
        return;
      
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Views/Games/GameDetailsPage.xaml", UriKind.Relative));
      this.BusyIndicator = (BusyIndicator) ((FrameworkElement) this).FindName("BusyIndicator");
      this.VisualStateGroup = (VisualStateGroup) ((FrameworkElement) this).FindName("VisualStateGroup");
      this.Collapsed = (VisualState) ((FrameworkElement) this).FindName("Collapsed");
      this.Expanded = (VisualState) ((FrameworkElement) this).FindName("Expanded");
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.related = (PivotItem) ((FrameworkElement) this).FindName("related");
      this.GameRelatedItems = (RelatedItems) ((FrameworkElement) this).FindName("GameRelatedItems");
      this.GameDetailsPivot = (XLPivot) ((FrameworkElement) this).FindName("GameDetailsPivot");
      this.overview = (PivotItem) ((FrameworkElement) this).FindName("overview");
      this.GameOverview = (ScrollViewer) ((FrameworkElement) this).FindName("GameOverview");
      this.GameRootPanel = (Grid) ((FrameworkElement) this).FindName("GameRootPanel");
      this.GameGrid = (Grid) ((FrameworkElement) this).FindName("GameGrid");
      this.GameImage = (Image) ((FrameworkElement) this).FindName("GameImage");
      this.GameDetailsName = (TextBlock) ((FrameworkElement) this).FindName("GameDetailsName");
      this.PublisherName = (TextBlock) ((FrameworkElement) this).FindName("PublisherName");
      this.StarRating = (StarRatingControl) ((FrameworkElement) this).FindName("StarRating");
      this.GameDetailsRelease = (TextBlock) ((FrameworkElement) this).FindName("GameDetailsRelease");
      this.BeaconButton = (Button) ((FrameworkElement) this).FindName("BeaconButton");
      this.GameDescriptionPanel = (StackPanel) ((FrameworkElement) this).FindName("GameDescriptionPanel");
      this.DescriptionTitle = (TextBlock) ((FrameworkElement) this).FindName("DescriptionTitle");
      this.Description = (TextBlock) ((FrameworkElement) this).FindName("Description");
      this.MoreButton = (Button) ((FrameworkElement) this).FindName("MoreButton");
      this.LessButton = (Button) ((FrameworkElement) this).FindName("LessButton");
      this.PlayOnXboxTitle = (TextBlock) ((FrameworkElement) this).FindName("PlayOnXboxTitle");
      this.LaunchGame = (Button) ((FrameworkElement) this).FindName("LaunchGame");
      this.GameRating = (TextBlock) ((FrameworkElement) this).FindName("GameRating");
      this.GameRatingIcon = (Image) ((FrameworkElement) this).FindName("GameRatingIcon");
      this.GameRatingDescriptorControl = (ItemsControl) ((FrameworkElement) this).FindName("GameRatingDescriptorControl");
      this.OverviewErrorText = (TextBlock) ((FrameworkElement) this).FindName("OverviewErrorText");
      this.OverviewLoadingIndicator = (BusyIndicator) ((FrameworkElement) this).FindName("OverviewLoadingIndicator");
      this.friends = (PivotItem) ((FrameworkElement) this).FindName("friends");
      this.FriendPivotGrid = (Grid) ((FrameworkElement) this).FindName("FriendPivotGrid");
      this.FriendsRefreshButton = (Button) ((FrameworkElement) this).FindName("FriendsRefreshButton");
      this.FriendsPlayingScrollViewer = (ScrollViewer) ((FrameworkElement) this).FindName("FriendsPlayingScrollViewer");
      this.FriendActivity = (TextBlock) ((FrameworkElement) this).FindName("FriendActivity");
      this.FriendsPlayingListBox = (ListBox) ((FrameworkElement) this).FindName("FriendsPlayingListBox");
      this.FriendsErrorText = (TextBlock) ((FrameworkElement) this).FindName("FriendsErrorText");
      this.NoFriendsText = (TextBlock) ((FrameworkElement) this).FindName("NoFriendsText");
      this.FriendsLoadingIndicator = (BusyIndicator) ((FrameworkElement) this).FindName("FriendsLoadingIndicator");
      this.achievements = (PivotItem) ((FrameworkElement) this).FindName("achievements");
      this.AchievementPivotGrid = (Grid) ((FrameworkElement) this).FindName("AchievementPivotGrid");
      this.AchievementsTotals = (Grid) ((FrameworkElement) this).FindName("AchievementsTotals");
      this.GamerscoreText = (TextBlock) ((FrameworkElement) this).FindName("GamerscoreText");
      this.GamerscoreG = (Image) ((FrameworkElement) this).FindName("GamerscoreG");
      this.AchievementProgressText = (TextBlock) ((FrameworkElement) this).FindName("AchievementProgressText");
      this.AchievementsIcon = (Image) ((FrameworkElement) this).FindName("AchievementsIcon");
      this.AchievementsRefreshButton = (Button) ((FrameworkElement) this).FindName("AchievementsRefreshButton");
      this.AchievementsBusyIndicator = (BusyIndicator) ((FrameworkElement) this).FindName("AchievementsBusyIndicator");
      this.AchievementsListBox = (ListBoxWithCompression) ((FrameworkElement) this).FindName("AchievementsListBox");
      this.AchievementsErrorText = (TextBlock) ((FrameworkElement) this).FindName("AchievementsErrorText");
      this.NoAchievementsText = (TextBlock) ((FrameworkElement) this).FindName("NoAchievementsText");
      this.AchievementLoadingIndicator = (BusyIndicator) ((FrameworkElement) this).FindName("AchievementLoadingIndicator");
      this.images = (PivotItem) ((FrameworkElement) this).FindName("images");
      this.GameImages = (GameImages) ((FrameworkElement) this).FindName("GameImages");
      this.ImageErrorText = (TextBlock) ((FrameworkElement) this).FindName("ImageErrorText");
      this.ImageLoadingIndicator = (BusyIndicator) ((FrameworkElement) this).FindName("ImageLoadingIndicator");
      this.MediaControls = (MediaBar) ((FrameworkElement) this).FindName("MediaControls");
    }
  }
}
