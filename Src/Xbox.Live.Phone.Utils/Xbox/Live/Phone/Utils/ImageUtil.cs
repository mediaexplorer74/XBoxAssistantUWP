// *********************************************************
// Type: Xbox.Live.Phone.Utils.ImageUtil
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace Xbox.Live.Phone.Utils
{
  public static class ImageUtil
  {
    private static event EventHandler<EventArgs> EventImageLoadedInternal;

    private static event EventHandler<EventArgs> EventImageUnloadedInternal;

    public static event EventHandler<EventArgs> EventImageLoaded
    {
      add
      {
        ImageUtil.EventImageLoadedInternal -= value;
        ImageUtil.EventImageLoadedInternal += value;
      }
      remove => ImageUtil.EventImageLoadedInternal -= value;
    }

    public static event EventHandler<EventArgs> EventImageUnloaded
    {
      add
      {
        ImageUtil.EventImageUnloadedInternal -= value;
        ImageUtil.EventImageUnloadedInternal += value;
      }
      remove => ImageUtil.EventImageUnloadedInternal -= value;
    }

    [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#", Justification = "parameter set in xaml")]
    public static void LoadImage(Image imgCtrl, string imageUrl)
    {
      if (string.IsNullOrWhiteSpace(imageUrl) || imgCtrl == null)
        return;
      if (imgCtrl.Source != null)
        ImageUtil.UnloadImage(imgCtrl);
      imgCtrl.Source = !imageUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase) ? (ImageSource) new BitmapImage(new Uri(imageUrl, UriKind.RelativeOrAbsolute)) : (ImageSource) new BitmapImage(new Uri(imageUrl, UriKind.Absolute));
      EventHandler<EventArgs> imageLoadedInternal = ImageUtil.EventImageLoadedInternal;
      if (imageLoadedInternal == null)
        return;
      imageLoadedInternal((object) imgCtrl, EventArgs.Empty);
    }

    public static Uri UnloadImage(Image imageCtrl)
    {
      Uri uri = (Uri) null;
      if (imageCtrl != null)
      {
        if (imageCtrl.Source is BitmapImage source)
        {
          uri = source.UriSource;
          source.UriSource = (Uri) null;
        }
        if (imageCtrl.Source != null)
          imageCtrl.Source = (ImageSource) null;
      }
      EventHandler<EventArgs> unloadedInternal = ImageUtil.EventImageUnloadedInternal;
      if (unloadedInternal != null)
        unloadedInternal((object) imageCtrl, EventArgs.Empty);
      return uri;
    }

    public static Uri UnloadImageBrush(ImageBrush imageBrush)
    {
      Uri uri = (Uri) null;
      if (imageBrush != null)
      {
        if (imageBrush.ImageSource is BitmapImage imageSource)
        {
          uri = imageSource.UriSource;
          imageSource.UriSource = (Uri) null;
        }
        imageBrush.ImageSource = (ImageSource) null;
      }
      return uri;
    }
  }
}
