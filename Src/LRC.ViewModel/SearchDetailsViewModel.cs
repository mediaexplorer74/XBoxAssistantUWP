// *********************************************************
// Type: LRC.ViewModel.SearchDetailsViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace LRC.ViewModel
{
  [DataContract(Namespace = "")]
  [XmlRoot(Namespace = "")]
  public class SearchDetailsViewModel : ViewModelBase
  {
    private const string ComponentName = "SearchDetailsViewModel";
    private SearchItemViewModel item;

    public SearchDetailsViewModel()
    {
    }

    public SearchDetailsViewModel(string itemId, string itemType)
      : this(itemId, itemType, (string) null, (string) null)
    {
    }

    [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification = "Naming convention matches existing data structures.")]
    public SearchDetailsViewModel(
      string itemId,
      string itemType,
      string itemName,
      string itemDetailsUrl)
      : this()
    {
      if (string.IsNullOrEmpty(itemId))
        throw new ArgumentNullException(nameof (itemId));
      if (string.IsNullOrEmpty(itemType))
        throw new ArgumentNullException(nameof (itemType));
      this.SearchItem = new SearchItemViewModel(itemId, itemType, itemName, itemDetailsUrl);
    }

    public override event EventHandler<LRCAsyncCompletedEventArgs> EventOnAsyncOperationError
    {
      add
      {
        this.InternalEventOnAsyncOperationError -= value;
        this.InternalEventOnAsyncOperationError += value;
        this.SearchItem.EventOnAsyncOperationError += value;
      }
      remove
      {
        this.InternalEventOnAsyncOperationError -= value;
        this.SearchItem.EventOnAsyncOperationError -= value;
      }
    }

    [DataMember]
    public SearchItemViewModel SearchItem
    {
      get => this.item;
      set => this.SetPropertyValue<SearchItemViewModel>(ref this.item, value, nameof (SearchItem));
    }

    public override void Load()
    {
      if (this.SearchItem == null)
        return;
      this.SearchItem.Load();
    }

    public void FetchMoreData()
    {
      if (this.SearchItem == null)
        return;
      this.SearchItem.FetchMoreData();
    }
  }
}
