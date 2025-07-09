// *********************************************************
// Type: LRC.PersonGroup
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace LRC
{
  public partial class PersonGroup : UserControl
  {
    private const string ComponentName = "PersonGroup";
    private static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof (Title), typeof (string), typeof (PersonGroup), new PropertyMetadata((object) null, (PropertyChangedCallback) null));
    private static readonly DependencyProperty PeopleProperty = DependencyProperty.Register(nameof (People), typeof (ObservableCollection<string>), typeof (PersonGroup), new PropertyMetadata((object) null, new PropertyChangedCallback(PersonGroup.DependencyPropertyChangedCallback)));
UserControl PersonGroupRoot;
TextBlock GroupTitle;
ItemsControl GroupList;
    

    public PersonGroup() => this.InitializeComponent();

    public string Title
    {
      get => (string) ((DependencyObject) this).GetValue(PersonGroup.TitleProperty);
      set => ((DependencyObject) this).SetValue(PersonGroup.TitleProperty, (object) value);
    }

    public ObservableCollection<string> People
    {
      get
      {
        return (ObservableCollection<string>) ((DependencyObject) this).GetValue(PersonGroup.PeopleProperty);
      }
      set => ((DependencyObject) this).SetValue(PersonGroup.PeopleProperty, (object) value);
    }

    private static void DependencyPropertyChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is PersonGroup personGroup) || e.Property != PersonGroup.PeopleProperty)
        return;
      ((UIElement) personGroup).Visibility = personGroup.People == null || personGroup.People.Count <= 0 ? (Visibility) 1 : (Visibility) 0;
    }

    private void PersonName_Tap(object sender, GestureEventArgs e)
    {
      if (!(sender is TextBlock textBlock) || string.IsNullOrEmpty(textBlock.Text))
        return;
      LrcPage lrcPageParent = LrcPage.GetLrcPageParent((FrameworkElement) this);
      if (lrcPageParent == null)
        return;
      SearchViewModel searchViewModel = new SearchViewModel(textBlock.Text, LRC.Service.Search.Filter.All);
      App.GlobalData.CurrentSearch = (ViewModelBase) searchViewModel;
      NavHelper.SafeNavigate((Page) lrcPageParent, NavHelper.GetSearchResultsUri(searchViewModel.SearchText, searchViewModel.SearchFilter));
    }

    // [Удалено для UWP портирования]
    // public void InitializeComponent()
    // {
      
        return;
      
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Controls/Search/PersonGroup.xaml", UriKind.Relative));
      this.PersonGroupRoot = (UserControl) ((FrameworkElement) this).FindName("PersonGroupRoot");
      this.GroupTitle = (TextBlock) ((FrameworkElement) this).FindName("GroupTitle");
      this.GroupList = (ItemsControl) ((FrameworkElement) this).FindName("GroupList");
    }
  }
}
