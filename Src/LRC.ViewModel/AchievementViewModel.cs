// *********************************************************
// Type: LRC.ViewModel.AchievementViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using Leet.UserGameData.DataContracts;
using LRC.Resources;
using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace LRC.ViewModel
{
  [XmlRoot(Namespace = "")]
  [DataContract(Namespace = "")]
  public class AchievementViewModel : ViewModelBase
  {
    private Achievement achievement;
    private string gamerscoreText;
    private string dateEarnedText;
    private string descriptionText;

    public AchievementViewModel()
    {
    }

    public AchievementViewModel(Achievement achievement)
    {
      this.Achievement = achievement;
      if (this.Achievement.IsEarned)
      {
        this.GamerscoreText = this.Achievement.Gamerscore.ToString((IFormatProvider) CultureInfo.CurrentCulture);
        this.DescriptionText = this.Achievement.Description;
        if (this.Achievement.EarnedOnline)
          this.DateEarnedText = string.Format((IFormatProvider) CultureInfo.CurrentUICulture, Resource.Achievements_DateEarned, new object[1]
          {
            (object) this.Achievement.EarnedDateTime.ToLocalTime().ToString("d", (IFormatProvider) CultureInfo.CurrentCulture)
          });
        else
          this.DateEarnedText = string.Empty;
      }
      else if (this.Achievement.DisplayBeforeEarned)
      {
        this.DateEarnedText = string.Empty;
        this.GamerscoreText = this.Achievement.Gamerscore.ToString((IFormatProvider) CultureInfo.CurrentCulture);
        this.DescriptionText = this.Achievement.HowToEarn;
      }
      else
      {
        this.Achievement.Name = Resource.Achievements_SecretTitle;
        this.DateEarnedText = string.Empty;
        this.GamerscoreText = Resource.Achievements_SecretGamerscore;
        this.DescriptionText = Resource.Achievements_SecretDescription;
      }
    }

    [DataMember]
    public Achievement Achievement
    {
      get => this.achievement;
      set => this.SetPropertyValue<Achievement>(ref this.achievement, value, nameof (Achievement));
    }

    [DataMember]
    public string GamerscoreText
    {
      get => this.gamerscoreText;
      set => this.SetPropertyValue<string>(ref this.gamerscoreText, value, nameof (GamerscoreText));
    }

    [DataMember]
    public string DateEarnedText
    {
      get => this.dateEarnedText;
      set => this.SetPropertyValue<string>(ref this.dateEarnedText, value, nameof (DateEarnedText));
    }

    [DataMember]
    public string DescriptionText
    {
      get => this.descriptionText;
      set
      {
        this.SetPropertyValue<string>(ref this.descriptionText, value, nameof (DescriptionText));
      }
    }
  }
}
