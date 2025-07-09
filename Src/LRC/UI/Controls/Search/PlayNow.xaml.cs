// *********************************************************
// Type: LRC.PlayNow
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Service.Omniture;
using LRC.ViewModel;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;


namespace LRC
{
  public partial class PlayNow : UserControl
  {
ScrollViewer ScrollViewerWrapper;
VisualStateGroup VisualStateGroup;
VisualState Collapsed;
VisualState Expanded;
Grid MovieRootPanel;
Grid MovieGrid;
Image MovieImage;
TextBlock MovieDetailsName;
TextBlock MovieDetailsReleaseDetails;
StarRatingControl StarRating;
CriticRatingControl CriticRating;
Button MoreButton;
Button LessButton;
TextBlock Description;
TextBlock DescriptionTitle;
TextBlock playOnXboxTitle;
ItemsControl ProvidersList;
    

    public PlayNow() => this.InitializeComponent();

    private static void OmnitureTrackingPlayOnConsole(ProviderViewModel provider, LrcPage parent)
    {
      if (!(parent.DataContext is SearchDetailsViewModel dataContext))
        return;
      OmnitureMediaContentTrackInfo mediaContent = new OmnitureMediaContentTrackInfo()
      {
        Title = dataContext.SearchItem.Title,
        MediaType = dataContext.SearchItem.ItemType,
        MediaId = dataContext.SearchItem.Id,
        Rank = -1,
        Genre = dataContext.SearchItem.Genres == null || dataContext.SearchItem.Genres.Count <= 0 ? "na" : dataContext.SearchItem.Genres[0],
        Category = "na",
        Studio = "na",
        Network = "na"
      };
      OmnitureProviderTrackInfo provider1 = new OmnitureProviderTrackInfo()
      {
        Title = provider.ProviderName,
        TitleId = provider.TitleId,
        LaunchType = provider.OfferDescription,
        ProductId = provider.ProductId
      };
      OmnitureAppMeasurement.Instance.TrackPlayOnConsoleEvent("played on Xbox", parent.OmnitureEntryPointName, mediaContent, provider1);
    }

    private void ProviderButton_Click(object sender, RoutedEventArgs e)
    {
      Button button = sender as Button;
      if (button == null || !(((FrameworkElement) button).DataContext is ProviderViewModel dataContext) || dataContext.TitleId == 0U)
        return;
      LrcPage lrcPageParent = LrcPage.GetLrcPageParent((FrameworkElement) this);
      if (lrcPageParent == null)
        return;
      ((ViewModelBase) lrcPageParent.DataContext).LaunchApp(dataContext.TitleId, dataContext.DeepLinkInfo, (object) null);
      PlayNow.OmnitureTrackingPlayOnConsole(dataContext, lrcPageParent);
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

    // [Удалено для UWP портирования]
    // public void InitializeComponent()
    // {
      
        return;
      
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Controls/Search/PlayNow.xaml", UriKind.Relative));
      this.ScrollViewerWrapper = (ScrollViewer) ((FrameworkElement) this).FindName("ScrollViewerWrapper");
      this.VisualStateGroup = (VisualStateGroup) ((FrameworkElement) this).FindName("VisualStateGroup");
      this.Collapsed = (VisualState) ((FrameworkElement) this).FindName("Collapsed");
      this.Expanded = (VisualState) ((FrameworkElement) this).FindName("Expanded");
      this.MovieRootPanel = (Grid) ((FrameworkElement) this).FindName("MovieRootPanel");
      this.MovieGrid = (Grid) ((FrameworkElement) this).FindName("MovieGrid");
      this.MovieImage = (Image) ((FrameworkElement) this).FindName("MovieImage");
      this.MovieDetailsName = (TextBlock) ((FrameworkElement) this).FindName("MovieDetailsName");
      this.MovieDetailsReleaseDetails = (TextBlock) ((FrameworkElement) this).FindName("MovieDetailsReleaseDetails");
      this.StarRating = (StarRatingControl) ((FrameworkElement) this).FindName("StarRating");
      this.CriticRating = (CriticRatingControl) ((FrameworkElement) this).FindName("CriticRating");
      this.MoreButton = (Button) ((FrameworkElement) this).FindName("MoreButton");
      this.LessButton = (Button) ((FrameworkElement) this).FindName("LessButton");
      this.Description = (TextBlock) ((FrameworkElement) this).FindName("Description");
      this.DescriptionTitle = (TextBlock) ((FrameworkElement) this).FindName("DescriptionTitle");
      this.playOnXboxTitle = (TextBlock) ((FrameworkElement) this).FindName("playOnXboxTitle");
      this.ProvidersList = (ItemsControl) ((FrameworkElement) this).FindName("ProvidersList");
    }
  }
}
