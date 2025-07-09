// *********************************************************
// Type: LRC.ArtistSearchDetailsPage
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
  public class ArtistSearchDetailsPage : SearchDetailsPageBase
  {
    public static readonly DependencyProperty IsBioAvailableProperty = DependencyProperty.Register("IsBioAvailable", typeof (bool), typeof (ArtistSearchDetailsPage), new PropertyMetadata((object) false, (PropertyChangedCallback) ((d, e) => ((ArtistSearchDetailsPage) d).AddBioPivotItem(e))));
    public static readonly DependencyProperty IsRelatedAvailableProperty = DependencyProperty.Register("IsRelatedAvailable", typeof (bool), typeof (ArtistSearchDetailsPage), new PropertyMetadata((object) false, (PropertyChangedCallback) ((d, e) => ((ArtistSearchDetailsPage) d).AddRelatedPivotItem(e))));
    private MusicArtistViewModel viewModel;
BusyIndicator BusyIndicator;
Grid LayoutRoot;
PivotItem bio;
PivotItem related;
ListBox RelatedItemsListBox;
XLPivot DetailsControl;
PivotItem albums;
TextBlock DiscographyNotFound;
ListBox AlbumsOfSameArtistListBox;
TextBlock OverviewErrorText;
BusyIndicator OverviewLoadingIndicator;
MediaBar MediaControls;
    

    public ArtistSearchDetailsPage()
    {
      this.InitializeComponent();
      this.AllowMultipleInstances = true;
      this.Persistent = false;
      this.NeedToSaveViewModel = true;
      Binding binding1 = new Binding("IsBioAvailable")
      {
        Source = this.DataContext
      };
      BindingOperations.SetBinding((DependencyObject) this, ArtistSearchDetailsPage.IsBioAvailableProperty, (BindingBase) binding1);
      Binding binding2 = new Binding("IsRelatedAvailable")
      {
        Source = this.DataContext
      };
      BindingOperations.SetBinding((DependencyObject) this, ArtistSearchDetailsPage.IsRelatedAvailableProperty, (BindingBase) binding2);
      this.OmniturePageName = "wp:lrc:search:artistdetail";
      this.OmnitureChannelName = "wp:lrc:search";
    }

    protected override Pivot Pivot => (Pivot) this.DetailsControl;

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      this.viewModel = this.DataContext as MusicArtistViewModel;
      if (this.viewModel != null)
      {
        this.viewModel.Load();
      }
      else
      {
        if (!((Page) this).NavigationContext.QueryString.ContainsKey("item"))
          return;
        SearchDetailsViewModel selectedSearchItem = App.GlobalData.SelectedSearchItem;
        App.GlobalData.SelectedSearchItem = (SearchDetailsViewModel) null;
        this.viewModel = new MusicArtistViewModel(selectedSearchItem.SearchItem.Id, selectedSearchItem.Title);
        this.viewModel.Load();
        this.DataContext = (object) this.viewModel;
      }
    }

    private void AddBioPivotItem(DependencyPropertyChangedEventArgs e)
    {
      if (!(bool) e.NewValue)
        return;
      ((PresentationFrameworkCollection<UIElement>) ((Panel) this.LayoutRoot).Children).Remove((UIElement) this.bio);
      ((UIElement) this.bio).Visibility = (Visibility) 0;
      ((PresentationFrameworkCollection<object>) this.Pivot.Items).Add((object) this.bio);
    }

    private void AddRelatedPivotItem(DependencyPropertyChangedEventArgs e)
    {
      if (!(bool) e.NewValue)
        return;
      ((PresentationFrameworkCollection<UIElement>) ((Panel) this.LayoutRoot).Children).Remove((UIElement) this.related);
      ((UIElement) this.related).Visibility = (Visibility) 0;
      ((PresentationFrameworkCollection<object>) this.Pivot.Items).Add((object) this.related);
    }

    private void AlbumItemsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

    private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      this.OmnitureTrackPageVisit();
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      
        return;
      
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Views/Search/ArtistSearchDetailsPage.xaml", UriKind.Relative));
      this.BusyIndicator = (BusyIndicator) ((FrameworkElement) this).FindName("BusyIndicator");
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.bio = (PivotItem) ((FrameworkElement) this).FindName("bio");
      this.related = (PivotItem) ((FrameworkElement) this).FindName("related");
      this.RelatedItemsListBox = (ListBox) ((FrameworkElement) this).FindName("RelatedItemsListBox");
      this.DetailsControl = (XLPivot) ((FrameworkElement) this).FindName("DetailsControl");
      this.albums = (PivotItem) ((FrameworkElement) this).FindName("albums");
      this.DiscographyNotFound = (TextBlock) ((FrameworkElement) this).FindName("DiscographyNotFound");
      this.AlbumsOfSameArtistListBox = (ListBox) ((FrameworkElement) this).FindName("AlbumsOfSameArtistListBox");
      this.OverviewErrorText = (TextBlock) ((FrameworkElement) this).FindName("OverviewErrorText");
      this.OverviewLoadingIndicator = (BusyIndicator) ((FrameworkElement) this).FindName("OverviewLoadingIndicator");
      this.MediaControls = (MediaBar) ((FrameworkElement) this).FindName("MediaControls");
    }
  }
}
