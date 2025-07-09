// *********************************************************
// Type: LRC.AchievementEntry
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;


namespace LRC
{
  public partial class AchievementEntry : Button
  {
Image AchievementTile;
TextBlock AchievementTitle;
TextBlock GamerscoreText;
Image GamerscoreG;
TextBlock AchievementDateEarned;
    

    public AchievementEntry() => this.InitializeComponent();

    // [Удалено для UWP портирования]
    // public void InitializeComponent()
    // {
      
        return;
      
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Controls/AchievementEntry.xaml", UriKind.Relative));
      this.AchievementTile = (Image) ((FrameworkElement) this).FindName("AchievementTile");
      this.AchievementTitle = (TextBlock) ((FrameworkElement) this).FindName("AchievementTitle");
      this.GamerscoreText = (TextBlock) ((FrameworkElement) this).FindName("GamerscoreText");
      this.GamerscoreG = (Image) ((FrameworkElement) this).FindName("GamerscoreG");
      this.AchievementDateEarned = (TextBlock) ((FrameworkElement) this).FindName("AchievementDateEarned");
    }
  }
}
