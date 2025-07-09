// *********************************************************
// Type: LRC.RelatedItems
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Service;
using LRC.Service.Omniture;
using LRC.ViewModel;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;


namespace LRC
{
  public partial class RelatedItems : UserControl
  {
    private const string ComponentName = "RelatedItems";
ListBox RelatedItemsListBox;
TextBlock RelatedItemsNoItemsFound;
    

    public RelatedItems() => this.InitializeComponent();

    private void RelatedItemsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (((Selector) this.RelatedItemsListBox).SelectedIndex < 0)
        return;
      if (e.AddedItems != null && e.AddedItems.Count > 0)
      {
        bool flag = ((FrameworkElement) this).DataContext is RelatedItemsViewModel dataContext && dataContext.UseMicrosoftItemsOnly;
        if (e.AddedItems[0] is SearchItemViewModel addedItem)
        {
          LrcPage lrcPageParent = LrcPage.GetLrcPageParent((FrameworkElement) this);
          if (lrcPageParent != null)
          {
            if (flag && !addedItem.IsGame)
            {
              Uri page = (Uri) null;
              switch (addedItem.Filter)
              {
                case LRC.Service.Search.Filter.Movies:
                  MovieMediaItem movieMediaItem = new MovieMediaItem(addedItem.Id);
                  movieMediaItem.Title = addedItem.Title;
                  App.GlobalData.SelectedMediaDetails = (ViewModelBase) movieMediaItem;
                  page = NavHelper.GetZuneVideoDetailsPage(MediaType.movie);
                  break;
                case LRC.Service.Search.Filter.TV:
                  if (string.Equals(addedItem.ItemType, "TVEPISODE"))
                  {
                    TVEpisodeItem tvEpisodeItem = new TVEpisodeItem(addedItem.Id);
                    tvEpisodeItem.Title = addedItem.Title;
                    App.GlobalData.SelectedMediaDetails = (ViewModelBase) tvEpisodeItem;
                    page = NavHelper.GetZuneVideoDetailsPage(MediaType.tv_episode);
                    break;
                  }
                  TVMediaItem tvMediaItem = new TVMediaItem(addedItem.Id);
                  tvMediaItem.Title = addedItem.Title;
                  App.GlobalData.SelectedMediaDetails = (ViewModelBase) tvMediaItem;
                  page = NavHelper.GetZuneVideoDetailsPage(MediaType.tv_series);
                  break;
              }
              if (page != (Uri) null)
                NavHelper.SafeNavigate((Page) lrcPageParent, page);
            }
            else
            {
              Uri searchDetailsUri = NavHelper.GetSearchDetailsUri(addedItem);
              if (searchDetailsUri != (Uri) null)
              {
                if (lrcPageParent is SearchDetailsPageBase searchDetailsPageBase && ((Page) lrcPageParent).NavigationService.Source == searchDetailsUri)
                  searchDetailsPageBase.ResetToDefaultView();
                else if (addedItem.IsGame)
                {
                  App.GlobalData.SelectedMediaDetails = (ViewModelBase) new GameItem(addedItem.Id);
                }
                else
                {
                  App.GlobalData.SelectedSearchItem = new SearchDetailsViewModel(addedItem.Id, addedItem.ItemType, addedItem.Title, addedItem.DetailsUrl);
                  App.GlobalData.SelectedSearchItem.Title = addedItem.Title;
                  App.GlobalData.SelectedSearchItem.SearchItem.TelevisionSeriesHasSeasons = addedItem.TelevisionSeriesHasSeasons;
                }
                NavHelper.SafeNavigate((Page) lrcPageParent, searchDetailsUri);
              }
            }
            OmnitureMediaContentTrackInfo mediaContent = new OmnitureMediaContentTrackInfo()
            {
              Title = addedItem.Title,
              MediaType = addedItem.ItemType,
              MediaId = addedItem.Id,
              Rank = ((Selector) this.RelatedItemsListBox).SelectedIndex,
              Genre = addedItem.Genres.Count > 0 ? addedItem.Genres[0] : "na",
              Category = "na",
              Studio = "na",
              Network = "na"
            };
            App.GlobalData.OmnitureEntryPoint = "related";
            OmnitureAppMeasurement.Instance.MediaItemClickedEvent("media selected", "related", mediaContent);
          }
        }
      }
      ((Selector) this.RelatedItemsListBox).SelectedIndex = -1;
    }

    // [Удалено для UWP портирования]
    // public void InitializeComponent()
    // {
      
        return;
      
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Controls/Search/RelatedItems.xaml", UriKind.Relative));
      this.RelatedItemsListBox = (ListBox) ((FrameworkElement) this).FindName("RelatedItemsListBox");
      this.RelatedItemsNoItemsFound = (TextBlock) ((FrameworkElement) this).FindName("RelatedItemsNoItemsFound");
    }
  }
}
