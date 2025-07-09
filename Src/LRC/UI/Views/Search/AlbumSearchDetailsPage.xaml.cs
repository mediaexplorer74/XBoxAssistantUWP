// *********************************************************
// Type: LRC.AlbumSearchDetailsPage
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Service.Omniture;
using LRC.ViewModel;
using Microsoft.Phone.Controls;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Navigation;
using XLToolKit;


namespace LRC
{
  public class AlbumSearchDetailsPage : SearchDetailsPageBase
  {
    public static readonly DependencyProperty IsReviewAvailableProperty = DependencyProperty.Register("IsReviewAvailable", typeof (bool), typeof (AlbumSearchDetailsPage), new PropertyMetadata((object) false, (PropertyChangedCallback) ((d, e) => ((AlbumSearchDetailsPage) d).AddReviewPivotItem(e))));
    public static readonly DependencyProperty IsRelatedAvailableProperty = DependencyProperty.Register("IsRelatedAvailable", typeof (bool), typeof (AlbumSearchDetailsPage), new PropertyMetadata((object) false, (PropertyChangedCallback) ((d, e) => ((AlbumSearchDetailsPage) d).AddRelatedPivotItem(e))));
    public static readonly DependencyProperty IsImageAvailableProperty = DependencyProperty.Register("IsImageAvailable", typeof (bool), typeof (AlbumSearchDetailsPage), new PropertyMetadata((object) false, (PropertyChangedCallback) ((d, e) => ((AlbumSearchDetailsPage) d).AddImagePivotItem(e))));
    private AlbumItem viewModel;
    private double? albumListOffset;
BusyIndicator BusyIndicator;
VisualStateGroup VisualStateGroup;
VisualState HideAlbums;
VisualState ShowAlbums;
VisualState HideSongs;
VisualState ShowSongs;
Grid LayoutRoot;
PivotItem reviews;
PivotItem images;
ListBox ImagesListBox;
PivotItem related;
ListBox RelatedItemsListBox;
XLPivot AlbumSearchDetailsControl;
PivotItem overview;
ScrollViewer OverviewScroll;
Image albumArtImage;
TextBlock titleTextBlock;
TextBlock artistTextBlock;
TextBlock explicitTextBlock;
StackPanel songsGroup;
TextBlock SongsHeader;
Button ShowSongsButton;
Button HideSongsButton;
ListBox SongListBox;
TextBlock ProvidersHeader;
ItemsControl ProvidersList;
TextBlock OverviewErrorText;
BusyIndicator OverviewLoadingIndicator;
PivotItem artist;
ScrollViewer ArtistScroll;
TextBlock AlbumsTitle;
ListBoxWithCompression AlbumsOfSameArtistListBox;
Button ShowAlbumsButton;
Button HideAlbumsButton;
TextBlock BioTitle;
TextBlock DiscographyNotFound;
MediaBar MediaControls;
    

    public AlbumSearchDetailsPage()
    {
      this.InitializeComponent();
      this.AllowMultipleInstances = true;
      this.Persistent = false;
      this.NeedToSaveViewModel = true;
      Binding binding1 = new Binding("IsReviewAvailable")
      {
        Source = this.DataContext
      };
      BindingOperations.SetBinding((DependencyObject) this, AlbumSearchDetailsPage.IsReviewAvailableProperty, (BindingBase) binding1);
      Binding binding2 = new Binding("IsPhotoAvailable")
      {
        Source = this.DataContext
      };
      BindingOperations.SetBinding((DependencyObject) this, AlbumSearchDetailsPage.IsImageAvailableProperty, (BindingBase) binding2);
      Binding binding3 = new Binding("IsRelatedAvailable")
      {
        Source = this.DataContext
      };
      BindingOperations.SetBinding((DependencyObject) this, AlbumSearchDetailsPage.IsRelatedAvailableProperty, (BindingBase) binding3);
      this.OmniturePageName = "wp:lrc:search:albumdetail";
      this.OmnitureChannelName = "wp:lrc:search";
      ((ItemsControl) this.AlbumsOfSameArtistListBox).ItemContainerGenerator.ItemsChanged += new ItemsChangedEventHandler(this.AlbumsOfSameArtistListBox_ItemsChanged);
    }

    protected override Pivot Pivot => (Pivot) this.AlbumSearchDetailsControl;

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      this.viewModel = this.DataContext as AlbumItem;
      if (this.viewModel != null)
      {
        this.viewModel.Load();
      }
      else
      {
        if (!((Page) this).NavigationContext.QueryString.ContainsKey("item"))
          return;
        this.viewModel = new AlbumItem(((Page) this).NavigationContext.QueryString["item"]);
        if (App.GlobalData.SelectedMediaDetails is AlbumItem selectedMediaDetails)
        {
          this.viewModel.Title = selectedMediaDetails.Title;
          App.GlobalData.SelectedMediaDetails = (ViewModelBase) null;
        }
        this.DataContext = (object) this.viewModel;
        this.viewModel.Load();
        if (!this.viewModel.IsNowPlaying)
          return;
        VisualStateManager.GoToState((Control) this, "ShowSongs", true);
      }
    }

    private void AddReviewPivotItem(DependencyPropertyChangedEventArgs e)
    {
      if (!(bool) e.NewValue)
        return;
      ((PresentationFrameworkCollection<UIElement>) ((Panel) this.LayoutRoot).Children).Remove((UIElement) this.reviews);
      ((UIElement) this.reviews).Visibility = (Visibility) 0;
      ((PresentationFrameworkCollection<object>) this.Pivot.Items).Add((object) this.reviews);
    }

    private void AddRelatedPivotItem(DependencyPropertyChangedEventArgs e)
    {
      if (!(bool) e.NewValue)
        return;
      ((PresentationFrameworkCollection<UIElement>) ((Panel) this.LayoutRoot).Children).Remove((UIElement) this.related);
      ((UIElement) this.related).Visibility = (Visibility) 0;
      ((PresentationFrameworkCollection<object>) this.Pivot.Items).Add((object) this.related);
    }

    private void AddImagePivotItem(DependencyPropertyChangedEventArgs e)
    {
      if (!(bool) e.NewValue)
        return;
      ((PresentationFrameworkCollection<UIElement>) ((Panel) this.LayoutRoot).Children).Remove((UIElement) this.images);
      ((UIElement) this.images).Visibility = (Visibility) 0;
      ((PresentationFrameworkCollection<object>) this.Pivot.Items).Add((object) this.images);
    }

    private void SongListBoxSizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (this.SongListBox == null)
        return;
      for (int index = 0; index < ((PresentationFrameworkCollection<object>) ((ItemsControl) this.SongListBox).Items).Count; ++index)
      {
        if (((ItemsControl) this.SongListBox).ItemContainerGenerator.ContainerFromIndex(index) is ListBoxItem listBoxItem)
          ((DependencyObject) listBoxItem).SetValue(TiltEffect.SuppressTiltProperty, (object) true);
      }
    }

    private void PlayAlbumButton_Click(object sender, RoutedEventArgs e)
    {
      if (!(sender is Button button))
        return;
      this.viewModel.LaunchZuneMedia(this.viewModel.MediaType, this.viewModel.Id, string.Empty, (object) null);
      if (!(((FrameworkElement) button).DataContext is ProviderViewModel dataContext))
        return;
      OmnitureAppMeasurement.Instance.TrackPlayOnConsoleEvent("played on Xbox", this.OmnitureEntryPointName, new OmnitureMediaContentTrackInfo()
      {
        Title = this.viewModel.Title,
        MediaType = "album",
        MediaId = this.viewModel.Id,
        Rank = -1,
        Genre = this.viewModel.Genre,
        Category = "na",
        Studio = "na",
        Network = "na"
      }, new OmnitureProviderTrackInfo()
      {
        Title = dataContext.ProviderName,
        TitleId = dataContext.TitleId,
        ProductId = dataContext.ProductId,
        LaunchType = dataContext.OfferDescription
      });
    }

    private void ImageSelection_Changed(object sender, SelectionChangedEventArgs e)
    {
      if (!(sender is ListBox listBox) || ((Selector) listBox).SelectedIndex == -1 || !(this.DataContext is AlbumItem dataContext) || dataContext.Photos == null || dataContext.Photos.Count == 0)
        return;
      ObservableCollection<string> observableCollection = new ObservableCollection<string>();
      for (int selectedIndex = ((Selector) listBox).SelectedIndex; selectedIndex < dataContext.Photos.Count; ++selectedIndex)
        observableCollection.Add(dataContext.Photos[selectedIndex]);
      for (int index = 0; index < ((Selector) listBox).SelectedIndex; ++index)
        observableCollection.Add(dataContext.Photos[index]);
      App.GlobalData.SelectedImageCollection = observableCollection;
      NavHelper.SafeNavigate((Page) this, NavHelper.ImageViewerUri);
      ((Selector) listBox).SelectedIndex = -1;
    }

    private void AlbumRelatedItemsListBox_SelectionChanged(
      object sender,
      SelectionChangedEventArgs e)
    {
      if (!(sender is ListBox listBox) || ((Selector) listBox).SelectedIndex == -1)
        return;
      AlbumItem selectedItem = (AlbumItem) ((Selector) listBox).SelectedItem;
      if (selectedItem != null)
      {
        App.GlobalData.SelectedMediaDetails = (ViewModelBase) selectedItem;
        NavHelper.SafeNavigate((Page) this, NavHelper.GetAlbumSearchDetailsUri(selectedItem.Id));
        OmnitureMediaContentTrackInfo mediaContent = new OmnitureMediaContentTrackInfo()
        {
          Title = selectedItem.Title,
          MediaType = "album",
          MediaId = selectedItem.Id,
          Rank = ((Selector) this.RelatedItemsListBox).SelectedIndex,
          Genre = selectedItem.Genre,
          Category = "na",
          Studio = "na",
          Network = "na"
        };
        App.GlobalData.OmnitureEntryPoint = "related";
        OmnitureAppMeasurement.Instance.MediaItemClickedEvent("media selected", "related", mediaContent);
      }
      ((Selector) listBox).SelectedIndex = -1;
    }

    private void AlbumItemsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!(sender is ListBoxWithCompression boxWithCompression) || ((Selector) boxWithCompression).SelectedIndex == -1)
        return;
      AlbumItem selectedItem = (AlbumItem) ((Selector) boxWithCompression).SelectedItem;
      if (selectedItem != null)
      {
        App.GlobalData.SelectedMediaDetails = (ViewModelBase) selectedItem;
        this.albumListOffset = new double?(boxWithCompression.ScrollViewer.VerticalOffset);
        NavHelper.SafeNavigate((Page) this, NavHelper.GetAlbumSearchDetailsUri(selectedItem.Id));
      }
      ((Selector) boxWithCompression).SelectedIndex = -1;
    }

    private void ShowAlbumsButton_Click(object sender, RoutedEventArgs e)
    {
      VisualStateManager.GoToState((Control) this, "ShowAlbums", true);
    }

    private void HideAlbumsButton_Click(object sender, RoutedEventArgs e)
    {
      VisualStateManager.GoToState((Control) this, "HideAlbums", true);
      this.ArtistScroll.ScrollToVerticalOffset(0.0);
    }

    private void ShowSongsButton_Click(object sender, RoutedEventArgs e)
    {
      VisualStateManager.GoToState((Control) this, "ShowSongs", true);
    }

    private void HideSongsButton_Click(object sender, RoutedEventArgs e)
    {
      VisualStateManager.GoToState((Control) this, "HideSongs", true);
      this.OverviewScroll.ScrollToVerticalOffset(0.0);
    }

    private void AlbumsOfSameArtistListBox_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      ListBoxWithCompression sameArtistListBox = this.AlbumsOfSameArtistListBox;
      if (sameArtistListBox == null || !this.albumListOffset.HasValue)
        return;
      sameArtistListBox.ScrollViewer.ScrollToVerticalOffset(this.albumListOffset.Value);
    }

    private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      this.OmnitureTrackPageVisit();
    }

    private void AlbumsOfSameArtistListBox_ItemsChanged(object sender, ItemsChangedEventArgs e)
    {
      ((DependencyObject) this).Dispatcher.BeginInvoke((Action) (() =>
      {
        lock (this.AlbumsOfSameArtistListBox)
        {
          if (((PresentationFrameworkCollection<object>) ((ItemsControl) this.AlbumsOfSameArtistListBox).Items).Count <= 2)
          {
            ((FrameworkElement) this.AlbumsOfSameArtistListBox).MaxHeight = double.PositiveInfinity;
          }
          else
          {
            UIElement uiElement = (UIElement) ((ItemsControl) this.AlbumsOfSameArtistListBox).ItemContainerGenerator.ContainerFromItem((object) this.viewModel.AlbumsBySameArtist[1]);
            if (uiElement == null)
              ((FrameworkElement) this.AlbumsOfSameArtistListBox).MaxHeight = double.PositiveInfinity;
            else
              ((FrameworkElement) this.AlbumsOfSameArtistListBox).MaxHeight = uiElement.TransformToVisual((UIElement) this.AlbumsOfSameArtistListBox).Transform(new Point()).Y + uiElement.DesiredSize.Height;
          }
        }
      }));
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      
        return;
      
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Views/Search/AlbumSearchDetailsPage.xaml", UriKind.Relative));
      this.BusyIndicator = (BusyIndicator) ((FrameworkElement) this).FindName("BusyIndicator");
      this.VisualStateGroup = (VisualStateGroup) ((FrameworkElement) this).FindName("VisualStateGroup");
      this.HideAlbums = (VisualState) ((FrameworkElement) this).FindName("HideAlbums");
      this.ShowAlbums = (VisualState) ((FrameworkElement) this).FindName("ShowAlbums");
      this.HideSongs = (VisualState) ((FrameworkElement) this).FindName("HideSongs");
      this.ShowSongs = (VisualState) ((FrameworkElement) this).FindName("ShowSongs");
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.reviews = (PivotItem) ((FrameworkElement) this).FindName("reviews");
      this.images = (PivotItem) ((FrameworkElement) this).FindName("images");
      this.ImagesListBox = (ListBox) ((FrameworkElement) this).FindName("ImagesListBox");
      this.related = (PivotItem) ((FrameworkElement) this).FindName("related");
      this.RelatedItemsListBox = (ListBox) ((FrameworkElement) this).FindName("RelatedItemsListBox");
      this.AlbumSearchDetailsControl = (XLPivot) ((FrameworkElement) this).FindName("AlbumSearchDetailsControl");
      this.overview = (PivotItem) ((FrameworkElement) this).FindName("overview");
      this.OverviewScroll = (ScrollViewer) ((FrameworkElement) this).FindName("OverviewScroll");
      this.albumArtImage = (Image) ((FrameworkElement) this).FindName("albumArtImage");
      this.titleTextBlock = (TextBlock) ((FrameworkElement) this).FindName("titleTextBlock");
      this.artistTextBlock = (TextBlock) ((FrameworkElement) this).FindName("artistTextBlock");
      this.explicitTextBlock = (TextBlock) ((FrameworkElement) this).FindName("explicitTextBlock");
      this.songsGroup = (StackPanel) ((FrameworkElement) this).FindName("songsGroup");
      this.SongsHeader = (TextBlock) ((FrameworkElement) this).FindName("SongsHeader");
      this.ShowSongsButton = (Button) ((FrameworkElement) this).FindName("ShowSongsButton");
      this.HideSongsButton = (Button) ((FrameworkElement) this).FindName("HideSongsButton");
      this.SongListBox = (ListBox) ((FrameworkElement) this).FindName("SongListBox");
      this.ProvidersHeader = (TextBlock) ((FrameworkElement) this).FindName("ProvidersHeader");
      this.ProvidersList = (ItemsControl) ((FrameworkElement) this).FindName("ProvidersList");
      this.OverviewErrorText = (TextBlock) ((FrameworkElement) this).FindName("OverviewErrorText");
      this.OverviewLoadingIndicator = (BusyIndicator) ((FrameworkElement) this).FindName("OverviewLoadingIndicator");
      this.artist = (PivotItem) ((FrameworkElement) this).FindName("artist");
      this.ArtistScroll = (ScrollViewer) ((FrameworkElement) this).FindName("ArtistScroll");
      this.AlbumsTitle = (TextBlock) ((FrameworkElement) this).FindName("AlbumsTitle");
      this.AlbumsOfSameArtistListBox = (ListBoxWithCompression) ((FrameworkElement) this).FindName("AlbumsOfSameArtistListBox");
      this.ShowAlbumsButton = (Button) ((FrameworkElement) this).FindName("ShowAlbumsButton");
      this.HideAlbumsButton = (Button) ((FrameworkElement) this).FindName("HideAlbumsButton");
      this.BioTitle = (TextBlock) ((FrameworkElement) this).FindName("BioTitle");
      this.DiscographyNotFound = (TextBlock) ((FrameworkElement) this).FindName("DiscographyNotFound");
      this.MediaControls = (MediaBar) ((FrameworkElement) this).FindName("MediaControls");
    }
  }
}
