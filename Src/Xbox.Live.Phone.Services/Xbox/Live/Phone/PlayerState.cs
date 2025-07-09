// *********************************************************
// Type: Xbox.Live.Phone.PlayerState
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Microsoft.Xna.Framework.GamerServices;


namespace Xbox.Live.Phone
{
  public class PlayerState
  {
    private static PlayerState staticInstance;
    private SignedInGamer gamer;

    private PlayerState()
    {
    }

    public static PlayerState Instance
    {
      get
      {
        if (PlayerState.staticInstance == null)
          PlayerState.staticInstance = new PlayerState();
        return PlayerState.staticInstance;
      }
    }

    public string Gamertag
    {
      get
      {
        string gamertag = string.Empty;
        if (this.gamer != null)
          gamertag = this.gamer.Gamertag;
        return gamertag;
      }
    }

    public void SignedInGamer_SignedOut(object sender, SignedOutEventArgs e)
    {
      this.gamer = (SignedInGamer) null;
    }

    public void SignedInGamerSignedIn(object sender, SignedInEventArgs e)
    {
      if (e == null)
        return;
      this.gamer = e.Gamer;
    }
  }
}
