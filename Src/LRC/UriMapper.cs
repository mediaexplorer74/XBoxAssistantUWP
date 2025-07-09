// *********************************************************
// Type: LRC.UriMapper
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.ViewModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Navigation;


namespace LRC
{
  public partial class UriMapper : UriMapperBase
  {
    private const string ComponentName = "UriMapper";
    private const char QueryStringIdentifier = '?';
    private const char QueryStringSeparator = '&';
    private const char ParamSeparator = '=';
    private const string DirectLaunchPath = "DirectLaunch";
    private const string TitleIdLaunchParam = "TitleId";
    private const string MediaIdLaunchParam = "MediaId";
    private bool needToDirectLaunch;
    private uint directLaunchTitleId;
    private string directLaunchPartnerMediaId;

    public bool NeedToDirectLaunch
    {
      get => this.needToDirectLaunch;
      set => this.needToDirectLaunch = value;
    }

    public string PartnerMediaId => this.directLaunchPartnerMediaId;

    public uint TitleId => this.directLaunchTitleId;

    public virtual Uri MapUri(Uri uri)
    {
      Uri uri1 = (Uri) null;
      if (uri != (Uri) null)
      {
        string str = uri.ToString();
        if (str.Contains("DirectLaunch"))
        {
          string uriToParse = HttpUtility.UrlDecode(str);
          this.needToDirectLaunch = false;
          this.directLaunchTitleId = 0U;
          this.directLaunchPartnerMediaId = (string) null;
          Dictionary<string, string> uriParameters = UriMapper.GetUriParameters(uriToParse, "DirectLaunch");
          if (uriParameters != null)
          {
            foreach (string key in uriParameters.Keys)
            {
              if (string.Equals(key, "TitleId", StringComparison.OrdinalIgnoreCase))
              {
                string s = uriParameters[key];
                if (!string.IsNullOrEmpty(s))
                {
                  uint result;
                  if (uint.TryParse(s, out result) && result > 0U)
                  {
                    this.needToDirectLaunch = true;
                    this.directLaunchTitleId = result;
                  }
                  else
                    this.needToDirectLaunch = false;
                }
              }
              else if (string.Equals(key, "MediaId", StringComparison.OrdinalIgnoreCase))
                this.directLaunchPartnerMediaId = HttpUtility.UrlEncode(uriParameters[key]);
            }
            uri1 = new Uri("/UI/Views/Connecting/HowToConnectPage.xaml", UriKind.RelativeOrAbsolute);
            this.NotifyNeedToDirectLaunch();
          }
        }
      }
      if (uri1 == (Uri) null)
        uri1 = uri;
      return uri1;
    }

    private static Dictionary<string, string> GetUriParameters(
      string uriToParse,
      string startString)
    {
      if (string.IsNullOrEmpty(startString))
        throw new ArgumentNullException(nameof (startString));
      Dictionary<string, string> uriParameters = new Dictionary<string, string>();
      if (!string.IsNullOrEmpty(uriToParse))
      {
        int startIndex = Math.Max(uriToParse.IndexOf(startString, StringComparison.OrdinalIgnoreCase), 0);
        int num = uriToParse.IndexOf('?', startIndex);
        if (num >= 0)
        {
          string[] strArray1 = uriToParse.Substring(num + 1).Split(new char[1]
          {
            '&'
          }, StringSplitOptions.RemoveEmptyEntries);
          if (strArray1 != null && strArray1.Length > 0)
          {
            foreach (string str in strArray1)
            {
              if (!string.IsNullOrEmpty(str))
              {
                string[] strArray2 = str.Split(new char[1]
                {
                  '='
                }, StringSplitOptions.RemoveEmptyEntries);
                if (strArray2 != null)
                {
                  switch (strArray2.Length)
                  {
                    case 0:
                      continue;
                    case 1:
                      uriParameters.Add(strArray2[0], (string) null);
                      continue;
                    default:
                      uriParameters.Add(strArray2[0], strArray2[1]);
                      continue;
                  }
                }
              }
            }
          }
        }
      }
      return uriParameters;
    }

    private void NotifyNeedToDirectLaunch()
    {
      if (!this.needToDirectLaunch || MainViewModel.Instance == null || MainViewModel.Instance.DirectLaunchViewModel == null)
        return;
      MainViewModel.Instance.DirectLaunchViewModel.TitleId = this.TitleId;
      MainViewModel.Instance.DirectLaunchViewModel.PartnerMediaId = this.PartnerMediaId;
      MainViewModel.Instance.DirectLaunchViewModel.NeedToDirectLaunch = true;
      MainViewModel.Instance.DirectLaunchViewModel.LaunchItem = (NowPlayingItemViewModel) null;
    }
  }
}
