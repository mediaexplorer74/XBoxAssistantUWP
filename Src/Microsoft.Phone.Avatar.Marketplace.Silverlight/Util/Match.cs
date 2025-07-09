// *********************************************************
// Type: Microsoft.Phone.Marketplace.Util.Match
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll


namespace Microsoft.Phone.Marketplace.Util
{
  internal class Match
  {
    private MatchState state;
    private int pos;
    private int len;
    private byte symbol;

    internal MatchState State
    {
      get => this.state;
      set => this.state = value;
    }

    internal int Position
    {
      get => this.pos;
      set => this.pos = value;
    }

    internal int Length
    {
      get => this.len;
      set => this.len = value;
    }

    internal byte Symbol
    {
      get => this.symbol;
      set => this.symbol = value;
    }
  }
}
