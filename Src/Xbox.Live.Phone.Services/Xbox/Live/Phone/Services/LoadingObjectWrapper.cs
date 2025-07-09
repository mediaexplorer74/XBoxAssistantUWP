// *********************************************************
// Type: Xbox.Live.Phone.Services.LoadingObjectWrapper
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Microsoft.Xna.Framework.GamerServices;


namespace Xbox.Live.Phone.Services
{
  public class LoadingObjectWrapper
  {
    private byte[] objectValue;

    public LoadingObjectWrapper()
    {
      this.LoadingState = LoadingObjectWrapperState.Loading;
      this.objectValue = (byte[]) null;
    }

    public LoadingObjectWrapperState LoadingState { get; private set; }

    public bool IsNull => this.objectValue == null;

    public bool IsInvalid
    {
      get
      {
        bool flag = this.LoadingState == LoadingObjectWrapperState.Loaded && this.IsNull;
        return this.LoadingState == LoadingObjectWrapperState.Failed || flag;
      }
    }

    public bool IsLoadingOrInvalid
    {
      get => this.LoadingState == LoadingObjectWrapperState.Loading || this.IsInvalid;
    }

    public AvatarDescription AvatarDescription
    {
      get
      {
        return this.objectValue == null ? (AvatarDescription) null : new AvatarDescription(this.GetValue());
      }
    }

    public byte[] GetValue() => this.objectValue;

    public void SetLoaded(byte[] value)
    {
      this.LoadingState = LoadingObjectWrapperState.Loaded;
      this.objectValue = value;
    }

    public void SetFailed()
    {
      this.LoadingState = LoadingObjectWrapperState.Failed;
      this.objectValue = (byte[]) null;
    }
  }
}
