// *********************************************************
// Type: Microsoft.Phone.Marketplace.Purchase.UI.PurchaseRequestComponent
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using Microsoft.Phone.Marketplace.Resources;
using Microsoft.Phone.Shell;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;


namespace Microsoft.Phone.Marketplace.Purchase.UI
{
  internal class PurchaseRequestComponent : UserControl, IPurchaseRequestUI
  {
    private const string AvatarNonStockAssetsThumbnailUrlFormat = "http://avatar.xboxlive.com/global/t.{0}/avataritem/{1}/128";
    private Offer _purchaseOffer;
    private BusyMode _busyMode;
    private int? currentPointsBalance = new int?();
    private bool _isActive;
    private bool _wasTrayVisible;
    private Page _page;
    private Popup _popup;
    private ObservableCollection<object> _viewModel = new ObservableCollection<object>();
    internal Storyboard BlockingBusyStoryboard;
    internal Image BackgroundImage;
    internal Grid BlockingBusyIndicator;
    internal Image BlockingBusyBig;
    internal Image BlockingBusySmall;
    internal Image OfferImage;
    internal Grid InsufficientPoints;
    internal Grid CannotRetrivePointsBalance;
    internal TextBlock RedeemText;
    internal TextBlock TermsOfUseText;
    internal Run TermsOfUseRun1;
    internal Run TermsOfUseRun2;
    internal Run TermsOfUseRun3;
    internal Button AddPointsButton;
    internal Button BuyButton;
    internal Button CancelButton;
    private bool _contentLoaded;

    public PurchaseRequestComponent(Page page)
    {
      string lowerInvariant = Thread.CurrentThread.CurrentUICulture.Name.ToLowerInvariant();
      if (lowerInvariant == "ja" || lowerInvariant == "ja-jp")
        ((FrameworkElement) this).Language = XmlLanguage.GetLanguage("ja-jp");
      this._page = page;
      this.InitializeComponent();
      this.BackgroundImage.Source = this.SynchronouslyLoad("Purchase.UI.background.jpg");
      this._popup = new Popup();
      this._popup.Child = (UIElement) this;
      ((DependencyObject) this).Dispatcher.BeginInvoke((Action) (() =>
      {
        this._viewModel.Add((object) null);
        this._viewModel.Add((object) (Visibility) 1);
        this._viewModel.Add((object) null);
        this._viewModel.Add((object) null);
        this._viewModel.Add((object) null);
        this._viewModel.Add((object) null);
        this._viewModel.Add((object) null);
        this._viewModel.Add((object) null);
        this._viewModel.Add((object) "---");
        this._viewModel.Add((object) null);
        this._viewModel.Add((object) null);
        this._viewModel.Add((object) null);
        this._viewModel.Add((object) null);
        this._viewModel.Add((object) null);
        this._viewModel.Add((object) null);
        this._viewModel.Add((object) (Visibility) 0);
        this._viewModel.Add((object) (Visibility) 1);
        this._viewModel.Add((object) (Visibility) 1);
        this._viewModel.Add((object) true);
        this._viewModel.Add((object) true);
        this._viewModel.Add((object) true);
        this._viewModel.Add((object) (Visibility) 1);
        ((FrameworkElement) this).DataContext = (object) this._viewModel;
      }));
      this.LoadResourceStrings();
    }

    public void LoadResourceStrings()
    {
      ((DependencyObject) this).Dispatcher.BeginInvoke((Action) (() =>
      {
        this._viewModel[3] = (object) UIResources.ConfirmPurchase;
        this._viewModel[5] = (object) UIResources.CurrentBalance;
        this._viewModel[7] = (object) UIResources.NewBalance;
        this._viewModel[10] = (object) UIResources.InsufficientPointsMessage;
        this._viewModel[11] = (object) UIResources.RedeemCode;
        this._viewModel[12] = (object) UIResources.AddPoints;
        this._viewModel[13] = (object) UIResources.BuyButton;
        this._viewModel[14] = (object) UIResources.Cancel;
        this._viewModel[9] = (object) UIResources.CantRetrievePointsMessage;
        string[] strArray = UIResources.TermsOfUse.Split(new string[1]
        {
          "(*link*)"
        }, StringSplitOptions.RemoveEmptyEntries);
        this.TermsOfUseRun1.Text = AvatarUIResources.PurchaseLegalText + " " + strArray[0];
        this.TermsOfUseRun2.Text = strArray[1];
        this.TermsOfUseRun3.Text = strArray[2];
      }));
    }

    public Offer PurchaseOffer
    {
      get => this._purchaseOffer;
      set
      {
        this._purchaseOffer = value;
        ((DependencyObject) this).Dispatcher.BeginInvoke((Action) (() =>
        {
          if (this._purchaseOffer != null)
          {
            this._viewModel[0] = (object) this._purchaseOffer;
            string str = this._purchaseOffer.AvatarAssetId.ToString();
            this.OfferImage.Source = (ImageSource) new BitmapImage(new Uri(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "http://avatar.xboxlive.com/global/t.{0}/avataritem/{1}/128", new object[2]
            {
              (object) str.Substring(str.Length - 8, 8),
              (object) str
            }), UriKind.Absolute));
          }
          else
          {
            this._viewModel[0] = (object) null;
            this._viewModel[20] = (object) "";
          }
        }));
      }
    }

    public BusyMode BusyMode
    {
      get => this._busyMode;
      set
      {
        if (value == this._busyMode)
          return;
        this._busyMode = value;
        ((DependencyObject) this).Dispatcher.BeginInvoke((Action) (() =>
        {
          this._viewModel[18] = (object) (this._busyMode == BusyMode.None);
          this._viewModel[19] = (object) (this._busyMode != BusyMode.Blocking);
          this._viewModel[1] = (object) (Visibility) (this._busyMode == BusyMode.NonBlocking ? 0 : 1);
          this._viewModel[17] = (object) (Visibility) (this._busyMode == BusyMode.Blocking ? 0 : 1);
        }));
      }
    }

    public int? CurrentPointsBalance
    {
      get => this.currentPointsBalance;
      set
      {
        this.currentPointsBalance = value;
        ((DependencyObject) this).Dispatcher.BeginInvoke((Action) (() =>
        {
          this._viewModel[2] = (object) value;
          if (this.currentPointsBalance.HasValue)
          {
            int num = this.currentPointsBalance.Value;
            this._viewModel[6] = (object) num;
            this._viewModel[8] = (object) (num - this.PurchaseOffer.PointsPrice);
          }
          else
          {
            this._viewModel[6] = (object) "---";
            this._viewModel[8] = (object) "---";
          }
        }));
      }
    }

    public bool IsActive => this._isActive;

    public void Show(PurchaseUIMode mode)
    {
      this._isActive = true;
      ((DependencyObject) this).Dispatcher.BeginInvoke((Action) (() =>
      {
        switch (mode)
        {
          case PurchaseUIMode.BuyUnknownPointsBalance:
            this._viewModel[21] = (object) (Visibility) 0;
            this._viewModel[15] = (object) (Visibility) 0;
            this._viewModel[16] = (object) (Visibility) 1;
            break;
          case PurchaseUIMode.AddPoints:
            this._viewModel[15] = (object) (Visibility) 1;
            this._viewModel[21] = (object) (Visibility) 1;
            this._viewModel[16] = (object) (Visibility) 0;
            break;
          default:
            this._viewModel[15] = (object) (Visibility) 0;
            this._viewModel[16] = (object) (Visibility) 1;
            this._viewModel[21] = (object) (Visibility) 1;
            break;
        }
        this._wasTrayVisible = SystemTray.GetIsVisible((DependencyObject) this._page);
        SystemTray.SetIsVisible((DependencyObject) this._page, false);
        this._popup.IsOpen = true;
        this.OnScreenShown((object) this, EventArgs.Empty);
        this.BlockingBusyStoryboard.Begin();
      }));
    }

    public void Remove()
    {
      this._isActive = false;
      ((DependencyObject) this).Dispatcher.BeginInvoke((Action) (() =>
      {
        SystemTray.SetIsVisible((DependencyObject) this._page, this._wasTrayVisible);
        this._popup.IsOpen = false;
        this.OnScreenHidden((object) this, EventArgs.Empty);
      }));
    }

    public void DelayOperation(Action action, int frames)
    {
      ((DependencyObject) this).Dispatcher.BeginInvoke((Action) (() =>
      {
        DispatcherTimer timer = new DispatcherTimer()
        {
          Interval = TimeSpan.FromMilliseconds((double) (32 * frames))
        };
        timer.Tick += (EventHandler) ((s, e) =>
        {
          action();
          timer.Stop();
        });
        timer.Start();
      }));
    }

    internal bool HandleBackButton()
    {
      if (this._popup == null || !this._popup.IsOpen)
        return false;
      this.OnCancel((object) this, (RoutedEventArgs) null);
      return true;
    }

    private void OnScreenShown(object sender, EventArgs e)
    {
      EventHandler<EventArgs> screenShown = this.ScreenShown;
      if (screenShown == null)
        return;
      screenShown((object) this, EventArgs.Empty);
    }

    private void OnScreenHidden(object sender, EventArgs e)
    {
      EventHandler<EventArgs> screenHidden = this.ScreenHidden;
      if (screenHidden == null)
        return;
      screenHidden((object) this, EventArgs.Empty);
    }

    private void OnRedeem(object sender, RoutedEventArgs e)
    {
      if (this._busyMode != BusyMode.None)
        return;
      EventHandler<EventArgs> redeemPressed = this.RedeemPressed;
      if (redeemPressed == null)
        return;
      redeemPressed((object) this, EventArgs.Empty);
    }

    private void OnTermsOfUse(object sender, RoutedEventArgs e)
    {
      if (this._busyMode != BusyMode.None)
        return;
      EventHandler<EventArgs> termsOfUsePressed = this.TermsOfUsePressed;
      if (termsOfUsePressed == null)
        return;
      termsOfUsePressed((object) this, EventArgs.Empty);
    }

    private void OnAddPoints(object sender, RoutedEventArgs e)
    {
      if (this._busyMode != BusyMode.None)
        return;
      EventHandler<EventArgs> addPointsPressed = this.AddPointsPressed;
      if (addPointsPressed == null)
        return;
      addPointsPressed((object) this, EventArgs.Empty);
    }

    private void OnBuy(object sender, RoutedEventArgs e)
    {
      if (this._busyMode != BusyMode.None)
        return;
      EventHandler<EventArgs> buyPressed = this.BuyPressed;
      if (buyPressed == null)
        return;
      buyPressed((object) this, EventArgs.Empty);
    }

    private void OnCancel(object sender, RoutedEventArgs e)
    {
      EventHandler<EventArgs> cancelPressed = this.CancelPressed;
      if (cancelPressed == null)
        return;
      cancelPressed((object) this, EventArgs.Empty);
    }

    private ImageSource SynchronouslyLoad(string path)
    {
      Assembly executingAssembly = Assembly.GetExecutingAssembly();
      BitmapImage bitmapImage = new BitmapImage();
      bitmapImage.CreateOptions = (BitmapCreateOptions) 0;
      string str = executingAssembly.FullName.Split(',')[0];
      using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(str + "." + path))
        ((BitmapSource) bitmapImage).SetSource(manifestResourceStream);
      return (ImageSource) bitmapImage;
    }

    public event EventHandler<EventArgs> RedeemPressed;

    public event EventHandler<EventArgs> TermsOfUsePressed;

    public event EventHandler<EventArgs> BuyPressed;

    public event EventHandler<EventArgs> AddPointsPressed;

    public event EventHandler<EventArgs> CancelPressed;

    public event EventHandler<EventArgs> ScreenShown;

    public event EventHandler<EventArgs> ScreenHidden;

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Microsoft.Phone.Avatar.Marketplace.Silverlight;component/Purchase/UI/PurchaseRequestComponent.xaml", UriKind.Relative));
      this.BlockingBusyStoryboard = (Storyboard) ((FrameworkElement) this).FindName("BlockingBusyStoryboard");
      this.BackgroundImage = (Image) ((FrameworkElement) this).FindName("BackgroundImage");
      this.BlockingBusyIndicator = (Grid) ((FrameworkElement) this).FindName("BlockingBusyIndicator");
      this.BlockingBusyBig = (Image) ((FrameworkElement) this).FindName("BlockingBusyBig");
      this.BlockingBusySmall = (Image) ((FrameworkElement) this).FindName("BlockingBusySmall");
      this.OfferImage = (Image) ((FrameworkElement) this).FindName("OfferImage");
      this.InsufficientPoints = (Grid) ((FrameworkElement) this).FindName("InsufficientPoints");
      this.CannotRetrivePointsBalance = (Grid) ((FrameworkElement) this).FindName("CannotRetrivePointsBalance");
      this.RedeemText = (TextBlock) ((FrameworkElement) this).FindName("RedeemText");
      this.TermsOfUseText = (TextBlock) ((FrameworkElement) this).FindName("TermsOfUseText");
      this.TermsOfUseRun1 = (Run) ((FrameworkElement) this).FindName("TermsOfUseRun1");
      this.TermsOfUseRun2 = (Run) ((FrameworkElement) this).FindName("TermsOfUseRun2");
      this.TermsOfUseRun3 = (Run) ((FrameworkElement) this).FindName("TermsOfUseRun3");
      this.AddPointsButton = (Button) ((FrameworkElement) this).FindName("AddPointsButton");
      this.BuyButton = (Button) ((FrameworkElement) this).FindName("BuyButton");
      this.CancelButton = (Button) ((FrameworkElement) this).FindName("CancelButton");
    }

    private enum ViewModelProperty
    {
      PurchaseOffer,
      IsBusy,
      CurrentPointsBalance,
      ConfirmPurcahse,
      DownloadSize,
      CurrentBalanceLabel,
      CurrentPointsBalanceText,
      NewPointsBalance,
      NewPointsBalanceText,
      CantRetrievePointsText,
      InsufficientPointsMessage,
      RedeemCode,
      AddPoints,
      BuyButton,
      CancelButton,
      BuyModeVisibility,
      AddPointsModeVisibility,
      IsBlockingBusy,
      BuyEnabled,
      CancelEnabled,
      OfferImage,
      UnknownPointsBalanceModeVisibility,
    }
  }
}
