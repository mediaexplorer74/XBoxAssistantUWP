// *********************************************************
// Type: Microsoft.Xmedia.Client.WindowsPhone.MediaState
// Assembly: Microsoft.Xmedia.Client.WindowsPhone.Interface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B6D5468-129C-4117-AE93-587315C3ADB1
// *********************************************************Microsoft.Xmedia.Client.WindowsPhone.Interface.dll


namespace Microsoft.Xmedia.Client.WindowsPhone
{
  public class MediaState
  {
    public uint TitleId { get; set; }

    public string ContentId { get; set; }

    public string ContentTitle { get; set; }

    public ulong Duration { get; set; }

    public ulong Position { get; set; }

    public ulong MinimumSeekPosition { get; set; }

    public ulong MaximumSeekPosition { get; set; }

    public float Rate { get; set; }

    public MediaTransportState MediaTransportState { get; set; }

    public MediaTransportCapabilities MediaTransportCapabilities { get; set; }
  }
}
