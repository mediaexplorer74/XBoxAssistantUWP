// *********************************************************
// Type: LRC.LrcNavigation
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.ViewModel;
using Microsoft.Phone.Controls;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Xml.Serialization;
using Xbox.Live.Phone.Utils.Cache;
using Xbox.Live.Phone.Utils.Serialization;


namespace LRC
{
  [XmlRoot(Namespace = "")]
  [DataContract(Namespace = "")]
  public class LrcNavigation
  {
    private const string ComponentName = "LrcNavigation";
    private const string BaseScreenName = "LrcApp";
    private const string MainViewModelKey = "MainViewModel";
    private const string WPTombstoneTarget = "app://external/";
    private const int StackDepth = 100;
    private static LrcNavigation instance;
    private PhoneApplicationFrame appRootFrame;
    private Uri targetUri;
    private Uri currentUri;
    private NavigationMode navigationMode;
    private bool recursiveBackNavigation;
    private Uri loopStartTargetPageUri;
    private Uri lastCanceledUri;
    private double opacityBeforeRecurisveNavigation;

    public LrcNavigation()
    {
      this.Locator = new ViewModelLocator();
      this.Stack = new XmlSerializableList<string>();
      this.Stack.Capacity = 100;
      this.Stack.Add("LrcApp");
      this.StackPointer = 0;
    }

    public static LrcNavigation Instance
    {
      get
      {
        if (LrcNavigation.instance == null)
          LrcNavigation.instance = new LrcNavigation();
        return LrcNavigation.instance;
      }
    }

    [DataMember]
    public ViewModelLocator Locator { get; set; }

    [DataMember]
    public XmlSerializableList<string> Stack { get; set; }

    [DataMember]
    public int StackPointer { get; set; }

    [XmlIgnore]
    [IgnoreDataMember]
    public bool IsPreviousPageMainPage
    {
      get
      {
        return 0 == string.Compare(this.Stack[this.StackPointer - 1], NavHelper.MainPageUri.ToString(), StringComparison.OrdinalIgnoreCase);
      }
    }

    public static void RestoreStaticInstanceState(string path)
    {
      LrcNavigation lrcNavigation = CacheManager.Load<LrcNavigation>(path);
      LrcNavigation.instance.Stack = lrcNavigation.Stack;
      LrcNavigation.instance.StackPointer = lrcNavigation.StackPointer;
      LrcNavigation.instance.Locator = lrcNavigation.Locator;
      MainViewModel.Instance = LrcNavigation.Instance.Locator.Locate("MainViewModel") as MainViewModel;
    }

    public void Initialize(PhoneApplicationFrame root)
    {
      if (root == null)
        throw new ArgumentNullException(nameof (root), "root frame cannot be null");
      lock (this)
      {
        if (this.appRootFrame != null)
          return;
        this.appRootFrame = root;
        ((Frame) this.appRootFrame).Navigated += new NavigatedEventHandler(this.Root_Navigated);
      }
    }

    public string Top() => this.Stack[this.StackPointer];

    public string GetViewModelKeyForRestore(LrcPage sourcePage, bool allowMultipleInstance)
    {
      if (sourcePage == null)
        return (string) null;
      return allowMultipleInstance ? this.Stack[this.StackPointer - 1] + ((object) sourcePage).GetType().ToString() : ((object) sourcePage).GetType().ToString();
    }

    public string GetViewModelKeyForSave(
      LrcPage sourcePage,
      bool allowMultipleInstance,
      bool adjustStack)
    {
      if (sourcePage == null)
        return (string) null;
      if (!allowMultipleInstance)
        return ((object) sourcePage).GetType().ToString();
      return adjustStack ? this.Stack[this.StackPointer - 2] + ((object) sourcePage).GetType().ToString() : this.Stack[this.StackPointer] + ((object) sourcePage).GetType().ToString();
    }

    public void SaveState(string path)
    {
      this.Locator.AddOrSet("MainViewModel", (ViewModelBase) MainViewModel.Instance);
      CacheManager.Save(path, (object) LrcNavigation.instance);
    }

    public void RemoveBackEntry()
    {
      JournalEntry journalEntry;
      try
      {
        journalEntry = this.appRootFrame.RemoveBackEntry();
      }
      catch (NullReferenceException ex)
      {
        return;
      }
      if (journalEntry == null || journalEntry.Source == (Uri) null)
        return;
      lock (this.Stack)
      {
        string topscreen = this.Top();
        this.PopScreen();
        if (string.Compare(this.Top(), journalEntry.Source.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
          this.PopScreen();
        this.PushScreen(topscreen);
      }
    }

    public void AdjustStackComingBackFromTombstoning()
    {
      if (string.Compare("app://external/", this.Top(), StringComparison.OrdinalIgnoreCase) != 0)
        return;
      this.PopScreen();
    }

    private void PopScreen()
    {
      lock (this.Stack)
      {
        if (this.StackPointer <= 0)
          return;
        this.Stack.RemoveAt(this.StackPointer);
        --this.StackPointer;
      }
    }

    private void PushScreen(string topscreen)
    {
      if (string.IsNullOrEmpty(topscreen))
        return;
      lock (this.Stack)
      {
        this.Stack.Add(topscreen.ToUpperInvariant());
        ++this.StackPointer;
      }
    }

    private void Root_Navigated(object sender, NavigationEventArgs e)
    {
      ((Frame) this.appRootFrame).Navigated -= new NavigatedEventHandler(this.Root_Navigated);
      ((Frame) this.appRootFrame).Navigated += new NavigatedEventHandler(this.LrcNavigation_Navigated);
      ((Frame) this.appRootFrame).Navigating += new NavigatingCancelEventHandler(this.LrcNavigation_Navigating);
      ((Frame) this.appRootFrame).NavigationStopped += new NavigationStoppedEventHandler(this.LrcNavigation_NavigationStopped);
    }

    private void LrcNavigation_Navigating(object sender, NavigatingCancelEventArgs e)
    {
      this.currentUri = ((Frame) this.appRootFrame).CurrentSource;
      this.navigationMode = e.NavigationMode;
      this.targetUri = e.Uri;
      if (e.NavigationMode != null || !(((Frame) this.appRootFrame).CurrentSource != e.Uri) || !this.Stack.Contains(e.Uri.ToString().ToUpperInvariant()))
        return;
      this.loopStartTargetPageUri = e.Uri;
      this.recursiveBackNavigation = true;
      this.opacityBeforeRecurisveNavigation = ((UIElement) this.appRootFrame).Opacity;
      ((CancelEventArgs) e).Cancel = true;
    }

    private void LrcNavigation_Navigated(object sender, NavigationEventArgs e)
    {
      switch ((int) this.navigationMode)
      {
        case 0:
          if (this.Stack.Contains(this.targetUri.ToString().ToUpperInvariant()))
            break;
          this.PushScreen(this.targetUri.ToString());
          break;
        case 1:
          this.PopScreen();
          if (!this.recursiveBackNavigation)
            break;
          if (this.loopStartTargetPageUri != e.Uri)
          {
            try
            {
              ((Frame) this.appRootFrame).GoBack();
              break;
            }
            catch (InvalidOperationException ex)
            {
              this.EndRecurisiveNavigation();
              this.lastCanceledUri = (Uri) null;
              break;
            }
          }
          else
          {
            this.EndRecurisiveNavigation();
            this.lastCanceledUri = (Uri) null;
            break;
          }
      }
    }

    private void LrcNavigation_NavigationStopped(object sender, NavigationEventArgs e)
    {
      if (!this.recursiveBackNavigation)
        return;
      if (this.lastCanceledUri == (Uri) null)
      {
        this.lastCanceledUri = ((Frame) this.appRootFrame).CurrentSource;
        try
        {
          ((Frame) this.appRootFrame).GoBack();
        }
        catch (InvalidOperationException ex)
        {
          this.EndRecurisiveNavigation();
          this.lastCanceledUri = (Uri) null;
        }
      }
      else
      {
        this.EndRecurisiveNavigation();
        this.lastCanceledUri = (Uri) null;
      }
    }

    private void EndRecurisiveNavigation()
    {
      this.recursiveBackNavigation = false;
      this.loopStartTargetPageUri = (Uri) null;
      ((UIElement) this.appRootFrame).Opacity = this.opacityBeforeRecurisveNavigation;
    }
  }
}
