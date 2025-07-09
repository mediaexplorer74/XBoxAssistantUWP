// *********************************************************
// Type: Microsoft.Phone.Marketplace.Util.DeflateInput
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll


namespace Microsoft.Phone.Marketplace.Util
{
  internal class DeflateInput
  {
    private byte[] buffer;
    private int count;
    private int startIndex;

    internal byte[] Buffer
    {
      get => this.buffer;
      set => this.buffer = value;
    }

    internal int Count
    {
      get => this.count;
      set => this.count = value;
    }

    internal int StartIndex
    {
      get => this.startIndex;
      set => this.startIndex = value;
    }

    internal void ConsumeBytes(int n)
    {
      this.startIndex += n;
      this.count -= n;
    }

    internal DeflateInput.InputState DumpState()
    {
      DeflateInput.InputState inputState;
      inputState.count = this.count;
      inputState.startIndex = this.startIndex;
      return inputState;
    }

    internal void RestoreState(DeflateInput.InputState state)
    {
      this.count = state.count;
      this.startIndex = state.startIndex;
    }

    internal struct InputState
    {
      internal int count;
      internal int startIndex;
    }
  }
}
