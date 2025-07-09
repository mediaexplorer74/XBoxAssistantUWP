// *********************************************************
// Type: LRC.ViewModel.FormattedMediaItems
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll


namespace LRC.ViewModel
{
  public class FormattedMediaItems : ViewModelBase
  {
    private ViewModelBase displayedItems1;
    private ViewModelBase displayedItems2;

    public ViewModelBase DisplayedItems1
    {
      get => this.displayedItems1;
      set
      {
        this.SetPropertyValue<ViewModelBase>(ref this.displayedItems1, value, nameof (DisplayedItems1));
        this.NotifyPropertyChanged("IsDisplayedItems1Available");
      }
    }

    public ViewModelBase DisplayedItems2
    {
      get => this.displayedItems2;
      set
      {
        this.SetPropertyValue<ViewModelBase>(ref this.displayedItems2, value, nameof (DisplayedItems2));
        this.NotifyPropertyChanged("IsDisplayedItems2Available");
      }
    }

    public bool IsDisplayedItems1Available => this.displayedItems1 != null;

    public bool IsDisplayedItems2Available => this.displayedItems2 != null;
  }
}
