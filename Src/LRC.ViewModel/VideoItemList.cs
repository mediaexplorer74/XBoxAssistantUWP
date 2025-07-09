// *********************************************************
// Type: LRC.ViewModel.VideoItemList
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using System;
using System.Collections.ObjectModel;


namespace LRC.ViewModel
{
  public class VideoItemList : MediaItemList
  {
    private string style;
    private string template;
    private ObservableCollection<ViewModelBase> displayedItems;

    public string Style
    {
      get => this.style;
      set => this.SetPropertyValue<string>(ref this.style, value, nameof (Style));
    }

    public string Template
    {
      get => this.template;
      set => this.SetPropertyValue<string>(ref this.template, value, nameof (Template));
    }

    public int MinimumItems { get; set; }

    public bool HasMoreItemToLoad
    {
      get => this.Items != null && this.Items.Count > this.DisplayedItems.Count;
    }

    public ObservableCollection<ViewModelBase> DisplayedItems
    {
      get
      {
        if (this.displayedItems == null)
          this.displayedItems = new ObservableCollection<ViewModelBase>();
        return this.displayedItems;
      }
      set
      {
        this.SetPropertyValue<ObservableCollection<ViewModelBase>>(ref this.displayedItems, value, nameof (DisplayedItems));
      }
    }

    public void LoadMoreData()
    {
      if (this.Items == null || this.DisplayedItems.Count >= this.Items.Count)
        return;
      int num = this.displayedItems.Count == 0 ? Math.Min(this.displayedItems.Count + this.MinimumItems, this.displayedItems.Count + (this.Items.Count - this.DisplayedItems.Count)) : this.Items.Count;
      for (int count = this.DisplayedItems.Count; count < num; ++count)
        this.DisplayedItems.Add(this.Items[count]);
    }
  }
}
