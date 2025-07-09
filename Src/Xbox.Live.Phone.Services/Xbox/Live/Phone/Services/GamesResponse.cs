// *********************************************************
// Type: Xbox.Live.Phone.Services.GamesResponse
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Gds.Contracts;
using Leet.Silverlight.XLiveWeb;
using System;
using Xbox.Live.Phone.Utils;


namespace Xbox.Live.Phone.Services
{
  public sealed class GamesResponse
  {
    private uint pageNumber;

    public GamesResponse(uint pageNumber) => this.pageNumber = pageNumber;

    public event EventHandler<ServiceProxyEventArgs<Games>> EventGetGamesCompleted;

    public void HttpClient_OnGetGamesCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      GameDataServiceManager.OngoingRequestCounter.Subtract(1);
      EventHandler<ServiceProxyEventArgs<Games>> getGamesCompleted = this.EventGetGamesCompleted;
      if (getGamesCompleted != null)
      {
        ServiceProxyEventArgs<Games> e1;
        if (e.ResultAvailable)
        {
          if (!(e.Result is Games result) || result.UserGamesCollection == null || result.UserGamesCollection.Count < 1)
          {
            XLiveMobileException exception = XLiveMobileException.CreateException(4000, "The game data response is invalid. ", (string[]) null);
            exception.StatusCode = 20;
            e1 = new ServiceProxyEventArgs<Games>((object) null, (Exception) exception, false, (object) null);
          }
          else
          {
            result.PageNumber = this.pageNumber;
            e1 = new ServiceProxyEventArgs<Games>((object) result, (Exception) null, false, (object) null);
          }
        }
        else
          e1 = new ServiceProxyEventArgs<Games>((object) null, (Exception) XLiveMobileException.CreateException(e.Error, 4000, "failed to get games", (string[]) null), false, (object) null);
        getGamesCompleted((object) this, e1);
      }
      if (!(sender is XLiveWebHttpClient xliveWebHttpClient))
        return;
      xliveWebHttpClient.Dispose();
    }
  }
}
