// *********************************************************
// Type: LRC.MediaItemBoxArtConverter
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Service;
using LRC.ViewModel;
using System;
using System.Globalization;
using System.Windows.Data;


namespace LRC
{
  public class MediaItemBoxArtConverter : IValueConverter
  {
    private const string ComponentName = "MediaItemBoxArtConverter";

    public static string GetBoxArtImagePath(MediaType mediaType)
    {
      string boxArtImagePath;
      switch (mediaType)
      {
        case MediaType.movie:
        case MediaType.movietrailer:
          boxArtImagePath = "/UI/Images/DefaultBoxArt/movie.png";
          break;
        case MediaType.music_album:
        case MediaType.music_artist:
        case MediaType.music_playlist:
          boxArtImagePath = "/UI/Images/DefaultBoxArt/music.png";
          break;
        case MediaType.music_track:
          boxArtImagePath = "/UI/Images/DefaultBoxArt/musicTrack.png";
          break;
        case MediaType.tv_episode:
        case MediaType.tv_series:
          boxArtImagePath = "/UI/Images/DefaultBoxArt/tv.png";
          break;
        case MediaType.music_musicvideo:
          boxArtImagePath = "/UI/Images/DefaultBoxArt/musicVideo.png";
          break;
        case MediaType.hub:
          boxArtImagePath = "/UI/Images/DefaultBoxArt/hub.png";
          break;
        default:
          boxArtImagePath = "/UI/Images/DefaultBoxArt/unknown.png";
          break;
      }
      return boxArtImagePath;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      string str = (string) null;
      if (value is MediaItem mediaItem)
        str = MediaItemBoxArtConverter.GetBoxArtImagePath(mediaItem.MediaType);
      return (object) str;
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
