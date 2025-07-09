// *********************************************************
// Type: LRC.ViewModel.IItemList
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using System.Collections.ObjectModel;


namespace LRC.ViewModel
{
  public interface IItemList
  {
    string Title { get; set; }

    string Description { get; set; }

    string ImageUrl { get; set; }

    ObservableCollection<ViewModelBase> Items { get; set; }
  }
}
