// *********************************************************
// Type: Delay.LowProfileImageLoader
// Assembly: PhonePerformance, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E90E6BDA-1ECA-4098-8295-FCCAB2C4D18A
// *********************************************************PhonePerformance.dll

using Microsoft.Phone;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using Xbox.Live.Phone.Utils;


namespace Delay
{
  public static class LowProfileImageLoader
  {
    private const string ComponentName = "LowProfileImageLoader";
    private const int UIThrottlingWait = 50;
    private const int WorkItemQuantum = 2;
    private static readonly Thread _thread = new Thread(new ParameterizedThreadStart(LowProfileImageLoader.WorkerThreadProc));
    private static readonly Queue<LowProfileImageLoader.PendingRequest> _pendingRequests = new Queue<LowProfileImageLoader.PendingRequest>();
    private static readonly Queue<IAsyncResult> _pendingResponses = new Queue<IAsyncResult>();
    private static readonly object _syncBlock = new object();
    private static bool _exiting;
    public static readonly DependencyProperty UriSourceProperty = DependencyProperty.RegisterAttached("UriSource", typeof (Uri), typeof (LowProfileImageLoader), new PropertyMetadata(new PropertyChangedCallback(LowProfileImageLoader.OnUriSourceChanged)));
    public static readonly DependencyProperty DefaultUriSourceProperty = DependencyProperty.RegisterAttached("DefaultUriSource", typeof (Uri), typeof (LowProfileImageLoader), new PropertyMetadata(new PropertyChangedCallback(LowProfileImageLoader.OnDefaultUriSourceChanged)));

    public static int PendingImageCount { get; set; }

    [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "UriSource is applicable only to Image elements.")]
    public static Uri GetUriSource(Image obj)
    {
      return obj != null ? (Uri) ((DependencyObject) obj).GetValue(LowProfileImageLoader.UriSourceProperty) : throw new ArgumentNullException(nameof (obj));
    }

    [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "UriDefaultSourceProperty is applicable only to Image elements.")]
    public static Uri GetDefaultUriSource(Image obj)
    {
      return obj != null ? (Uri) ((DependencyObject) obj).GetValue(LowProfileImageLoader.DefaultUriSourceProperty) : throw new ArgumentNullException(nameof (obj));
    }

    public static void QueueImageToDownload(Image obj, Uri value)
    {
      LowProfileImageLoader.SetUriSource(obj, value);
    }

    [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "UriSource is applicable only to Image elements.")]
    public static void SetUriSource(Image obj, Uri value)
    {
      if (obj == null)
        throw new ArgumentNullException(nameof (obj));
      LowProfileImageLoader.AddImageToQueue(obj, value);
      ((DependencyObject) obj).SetValue(LowProfileImageLoader.UriSourceProperty, (object) value);
    }

    [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "UriSource is applicable only to Image elements.")]
    public static void SetDefaultUriSource(Image obj, Uri value)
    {
      if (obj == null)
        throw new ArgumentNullException(nameof (obj));
      LowProfileImageLoader.SetImageDefaultUri(obj, value);
      ((DependencyObject) obj).SetValue(LowProfileImageLoader.DefaultUriSourceProperty, (object) value);
    }

    public static bool IsEnabled { get; set; }

    public static Uri FailedUriSource { get; set; }

    [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "Static constructor performs additional tasks.")]
    static LowProfileImageLoader()
    {
      LowProfileImageLoader._thread.Start();
      Application.Current.Exit += new EventHandler(LowProfileImageLoader.HandleApplicationExit);
      ImageUtil.EventImageUnloaded += new EventHandler<EventArgs>(LowProfileImageLoader.ImageUnloaded);
      LowProfileImageLoader.IsEnabled = true;
    }

    private static void HandleApplicationExit(object sender, EventArgs e)
    {
      LowProfileImageLoader._exiting = true;
      if (!Monitor.TryEnter(LowProfileImageLoader._syncBlock, 100))
        return;
      Monitor.Pulse(LowProfileImageLoader._syncBlock);
      Monitor.Exit(LowProfileImageLoader._syncBlock);
    }

    [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Linear flow is easy to understand.")]
    [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Relevant exceptions don't have a common base class.")]
    private static void WorkerThreadProc(object unused)
    {
      Random random = new Random();
      List<LowProfileImageLoader.PendingRequest> pendingRequestList = new List<LowProfileImageLoader.PendingRequest>();
      Queue<IAsyncResult> asyncResultQueue = new Queue<IAsyncResult>();
      while (!LowProfileImageLoader._exiting)
      {
        lock (LowProfileImageLoader._syncBlock)
        {
          if (LowProfileImageLoader._pendingRequests.Count == 0 && LowProfileImageLoader._pendingResponses.Count == 0 && pendingRequestList.Count == 0 && asyncResultQueue.Count == 0)
          {
            Monitor.Wait(LowProfileImageLoader._syncBlock);
            if (LowProfileImageLoader._exiting)
              break;
          }
          while (0 < LowProfileImageLoader._pendingRequests.Count)
          {
            LowProfileImageLoader.PendingRequest pendingRequest = LowProfileImageLoader._pendingRequests.Dequeue();
            for (int index = 0; index < pendingRequestList.Count; ++index)
            {
              if (pendingRequestList[index].Image == pendingRequest.Image)
              {
                pendingRequestList[index] = pendingRequest;
                pendingRequest = (LowProfileImageLoader.PendingRequest) null;
                break;
              }
            }
            if (pendingRequest != null)
            {
              pendingRequestList.Add(pendingRequest);
              ++LowProfileImageLoader.PendingImageCount;
            }
          }
          while (0 < LowProfileImageLoader._pendingResponses.Count)
            asyncResultQueue.Enqueue(LowProfileImageLoader._pendingResponses.Dequeue());
        }
        Queue<LowProfileImageLoader.PendingCompletion> pendingCompletionQueue = new Queue<LowProfileImageLoader.PendingCompletion>();
        int count = pendingRequestList.Count;
        for (int index1 = 0; 0 < count && index1 < 2; ++index1)
        {
          int index2 = random.Next(count);
          LowProfileImageLoader.PendingRequest pendingRequest = pendingRequestList[index2];
          pendingRequestList[index2] = pendingRequestList[count - 1];
          pendingRequestList.RemoveAt(count - 1);
          --count;
          if (pendingRequest.Uri != (Uri) null)
          {
            if (pendingRequest.Uri.IsAbsoluteUri)
            {
              HttpWebRequest http = WebRequest.CreateHttp(pendingRequest.Uri);
              http.AllowReadStreamBuffering = true;
              http.BeginGetResponse(new AsyncCallback(LowProfileImageLoader.HandleGetResponseResult), (object) new LowProfileImageLoader.ResponseState((WebRequest) http, pendingRequest.Image, pendingRequest.Uri));
            }
            else
            {
              Stream streamFromResource = LowProfileImageLoader.GetStreamFromResource(pendingRequest.Uri);
              if (streamFromResource != null)
                pendingCompletionQueue.Enqueue(new LowProfileImageLoader.PendingCompletion(pendingRequest.Image, pendingRequest.Uri, streamFromResource));
            }
          }
          Thread.Sleep(1);
        }
        for (int index = 0; 0 < asyncResultQueue.Count && index < 2; ++index)
        {
          IAsyncResult asyncResult = asyncResultQueue.Dequeue();
          LowProfileImageLoader.ResponseState responseState = (LowProfileImageLoader.ResponseState) asyncResult.AsyncState;
          try
          {
            Stream responseStream = responseState.WebRequest.EndGetResponse(asyncResult).GetResponseStream();
            Uri uri = responseState.Uri;
            pendingCompletionQueue.Enqueue(new LowProfileImageLoader.PendingCompletion(responseState.Image, uri, responseStream));
          }
          catch (WebException ex)
          {
            if (responseState != null && responseState.Image != null)
              ((DependencyObject) Deployment.Current).Dispatcher.BeginInvoke((Action) (() =>
              {
                if (((FrameworkElement) responseState.Image).Tag is LowProfileImageLoader.ImageLoaderData tag2)
                  tag2.UriAppliedStatus = LowProfileImageLoader.UriAppliedStatus.Source;
                LowProfileImageLoader.HandleImageLoadFailure(responseState.Image);
              }));
            --LowProfileImageLoader.PendingImageCount;
          }
          Thread.Sleep(1);
        }
        if (0 < pendingCompletionQueue.Count)
        {
          while (0 < pendingCompletionQueue.Count)
          {
            LowProfileImageLoader.ProcessCompletion(pendingCompletionQueue.Dequeue());
            Thread.Sleep(50);
          }
        }
      }
    }

    private static void ProcessCompletion(
      LowProfileImageLoader.PendingCompletion pendingCompletion)
    {
      ((DependencyObject) Deployment.Current).Dispatcher.BeginInvoke((Action) (() =>
      {
        if (LowProfileImageLoader.GetUriSource(pendingCompletion.Image) == pendingCompletion.Uri)
        {
          int actualHeight = (int) ((FrameworkElement) pendingCompletion.Image).ActualHeight;
          int actualWidth = (int) ((FrameworkElement) pendingCompletion.Image).ActualWidth;
          if (actualHeight > 0 && actualWidth > 0)
            LowProfileImageLoader.TrySetJpegSource(pendingCompletion.Image, pendingCompletion.Stream, actualWidth, actualHeight, LowProfileImageLoader.UriAppliedStatus.Source);
          else
            LowProfileImageLoader.TrySetBitmapSource(pendingCompletion.Image, pendingCompletion.Stream, LowProfileImageLoader.UriAppliedStatus.Source);
        }
        try
        {
          pendingCompletion.Stream.Dispose();
        }
        catch
        {
        }
        --LowProfileImageLoader.PendingImageCount;
      }));
    }

    private static void TrySetJpegSource(
      Image image,
      Stream sourceStream,
      int width,
      int height,
      LowProfileImageLoader.UriAppliedStatus uriToApply)
    {
      try
      {
        WriteableBitmap writeableBitmap = PictureDecoder.DecodeJpeg(sourceStream, height, width);
        LowProfileImageLoader.TrySetImageSource(image, (ImageSource) writeableBitmap);
      }
      catch
      {
        LowProfileImageLoader.TrySetBitmapSource(image, sourceStream, uriToApply);
      }
    }

    private static Stream GetStreamFromResource(Uri sourceUri)
    {
      Stream streamFromResource = (Stream) null;
      if (sourceUri != (Uri) null && !sourceUri.IsAbsoluteUri)
      {
        string originalString = sourceUri.OriginalString;
        Uri uri;
        if (!originalString.StartsWith("/", StringComparison.Ordinal))
          uri = sourceUri;
        else
          uri = new Uri(originalString.TrimStart('/'), UriKind.Relative);
        StreamResourceInfo resourceStream = Application.GetResourceStream(uri);
        if (resourceStream != null)
          streamFromResource = resourceStream.Stream;
      }
      return streamFromResource;
    }

    private static void TrySetBitmapSource(
      Image image,
      Stream sourceStream,
      LowProfileImageLoader.UriAppliedStatus uriToApply)
    {
      BitmapImage bitmapImage = new BitmapImage();
      bitmapImage.CreateOptions = (BitmapCreateOptions) 16;
      try
      {
        ((BitmapSource) bitmapImage).SetSource(sourceStream);
        LowProfileImageLoader.TrySetImageSource(image, (ImageSource) bitmapImage, uriToApply);
      }
      catch
      {
        if (((FrameworkElement) image).Tag is LowProfileImageLoader.ImageLoaderData tag)
          tag.UriAppliedStatus = uriToApply;
        LowProfileImageLoader.HandleImageLoadFailure(image);
      }
    }

    private static void AddImageToQueue(Image image, Uri uri)
    {
      if (image == null || uri == (Uri) null)
        return;
      if (!LowProfileImageLoader.IsEnabled || DesignerProperties.IsInDesignTool)
      {
        LowProfileImageLoader.TrySetImageSource(image, (ImageSource) new BitmapImage(uri));
      }
      else
      {
        if (((FrameworkElement) image).Tag is LowProfileImageLoader.ImageLoaderData imageLoaderData)
        {
          if (imageLoaderData.Source != uri)
          {
            ImageUtil.UnloadImage(image);
            imageLoaderData.UriAppliedStatus = LowProfileImageLoader.UriAppliedStatus.None;
          }
          else if (imageLoaderData.InQueue)
            return;
        }
        else
          imageLoaderData = new LowProfileImageLoader.ImageLoaderData();
        imageLoaderData.Source = uri;
        ((FrameworkElement) image).Tag = (object) imageLoaderData;
        imageLoaderData.InQueue = true;
        lock (LowProfileImageLoader._syncBlock)
        {
          LowProfileImageLoader._pendingRequests.Enqueue(new LowProfileImageLoader.PendingRequest(image, uri));
          Monitor.Pulse(LowProfileImageLoader._syncBlock);
        }
      }
    }

    private static void OnUriSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
      Image image = (Image) o;
      Uri newValue = (Uri) e.NewValue;
      if (newValue != (Uri) null)
        LowProfileImageLoader.AddImageToQueue(image, newValue);
      else if (((FrameworkElement) image).Tag is LowProfileImageLoader.ImageLoaderData tag && tag.DefaultSource != (Uri) null)
        LowProfileImageLoader.SetDefaultUriSource(image, tag.DefaultSource);
      else if (LowProfileImageLoader.FailedUriSource != (Uri) null)
        LowProfileImageLoader.SetDefaultUriSource(image, LowProfileImageLoader.FailedUriSource);
      else
        ImageUtil.UnloadImage(image);
    }

    private static void OnDefaultUriSourceChanged(
      DependencyObject o,
      DependencyPropertyChangedEventArgs e)
    {
      if (!DesignerProperties.IsInDesignTool)
        ThreadManager.UIThread.AssertIsCurrentThread();
      LowProfileImageLoader.SetImageDefaultUri(o as Image, e.NewValue as Uri);
    }

    private static void SetImageDefaultUri(Image image, Uri uri)
    {
      if (image == null || !(uri != (Uri) null))
        return;
      if (!(((FrameworkElement) image).Tag is LowProfileImageLoader.ImageLoaderData imageLoaderData))
      {
        imageLoaderData = new LowProfileImageLoader.ImageLoaderData();
        ((FrameworkElement) image).Tag = (object) imageLoaderData;
      }
      imageLoaderData.DefaultSource = uri;
      if (imageLoaderData.UriAppliedStatus == LowProfileImageLoader.UriAppliedStatus.Source && image.Source != null)
        return;
      Stream streamFromResource = LowProfileImageLoader.GetStreamFromResource(uri);
      LowProfileImageLoader.TrySetBitmapSource(image, streamFromResource, LowProfileImageLoader.UriAppliedStatus.Default);
    }

    private static void TrySetImageSource(Image image, ImageSource imageSource)
    {
      LowProfileImageLoader.TrySetImageSource(image, imageSource, LowProfileImageLoader.UriAppliedStatus.Source);
    }

    private static void TrySetImageSource(
      Image image,
      ImageSource imageSource,
      LowProfileImageLoader.UriAppliedStatus uriToApply)
    {
      if (image == null)
        return;
      image.ImageOpened -= new EventHandler<RoutedEventArgs>(LowProfileImageLoader.Image_ImageOpened);
      image.ImageOpened += new EventHandler<RoutedEventArgs>(LowProfileImageLoader.Image_ImageOpened);
      image.ImageFailed -= new EventHandler<ExceptionRoutedEventArgs>(LowProfileImageLoader.Image_ImageFailed);
      image.ImageFailed += new EventHandler<ExceptionRoutedEventArgs>(LowProfileImageLoader.Image_ImageFailed);
      if (((FrameworkElement) image).Tag is LowProfileImageLoader.ImageLoaderData tag)
      {
        tag.UriAppliedStatus = uriToApply;
        if (uriToApply == LowProfileImageLoader.UriAppliedStatus.Source)
          tag.InQueue = false;
      }
      image.Source = imageSource;
    }

    private static void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
    {
      if (!(sender is Image image))
        return;
      image.ImageOpened -= new EventHandler<RoutedEventArgs>(LowProfileImageLoader.Image_ImageOpened);
      image.ImageFailed -= new EventHandler<ExceptionRoutedEventArgs>(LowProfileImageLoader.Image_ImageFailed);
      LowProfileImageLoader.HandleImageLoadFailure(image);
    }

    private static void Image_ImageOpened(object sender, RoutedEventArgs e)
    {
      if (!(sender is Image image))
        return;
      image.ImageOpened -= new EventHandler<RoutedEventArgs>(LowProfileImageLoader.Image_ImageOpened);
      image.ImageFailed -= new EventHandler<ExceptionRoutedEventArgs>(LowProfileImageLoader.Image_ImageFailed);
    }

    private static void ImageUnloaded(object sender, EventArgs e)
    {
      if (!(sender is Image image) || !(((FrameworkElement) image).Tag is LowProfileImageLoader.ImageLoaderData))
        return;
      ((FrameworkElement) image).Loaded -= new RoutedEventHandler(LowProfileImageLoader.ImageLoaded);
      ((FrameworkElement) image).Loaded += new RoutedEventHandler(LowProfileImageLoader.ImageLoaded);
    }

    private static void ImageLoaded(object sender, RoutedEventArgs e)
    {
      if (!(sender is Image image))
        return;
      ((FrameworkElement) image).Loaded -= new RoutedEventHandler(LowProfileImageLoader.ImageLoaded);
      if (!(((FrameworkElement) image).Tag is LowProfileImageLoader.ImageLoaderData tag))
        return;
      LowProfileImageLoader.SetImageDefaultUri(image, tag.DefaultSource);
      LowProfileImageLoader.AddImageToQueue(image, tag.Source);
    }

    private static void HandleImageLoadFailure(Image image)
    {
      if (!(((FrameworkElement) image).Tag is LowProfileImageLoader.ImageLoaderData tag))
        return;
      int num = tag.Source != (Uri) null ? 1 : 0;
      LowProfileImageLoader.UriAppliedStatus uriAppliedStatus = tag.UriAppliedStatus;
      Uri defaultUriSource = LowProfileImageLoader.GetDefaultUriSource(image);
      tag.UriAppliedStatus = LowProfileImageLoader.UriAppliedStatus.None;
      switch (uriAppliedStatus)
      {
        case LowProfileImageLoader.UriAppliedStatus.Source:
          if (defaultUriSource != (Uri) null)
          {
            Stream streamFromResource = LowProfileImageLoader.GetStreamFromResource(defaultUriSource);
            LowProfileImageLoader.TrySetBitmapSource(image, streamFromResource, LowProfileImageLoader.UriAppliedStatus.Default);
            break;
          }
          if (!(LowProfileImageLoader.FailedUriSource != (Uri) null))
            break;
          Stream streamFromResource1 = LowProfileImageLoader.GetStreamFromResource(LowProfileImageLoader.FailedUriSource);
          LowProfileImageLoader.TrySetBitmapSource(image, streamFromResource1, LowProfileImageLoader.UriAppliedStatus.Failed);
          break;
        case LowProfileImageLoader.UriAppliedStatus.Default:
          if (!(LowProfileImageLoader.FailedUriSource != (Uri) null))
            break;
          Stream streamFromResource2 = LowProfileImageLoader.GetStreamFromResource(LowProfileImageLoader.FailedUriSource);
          LowProfileImageLoader.TrySetBitmapSource(image, streamFromResource2, LowProfileImageLoader.UriAppliedStatus.Failed);
          break;
      }
    }

    private static void HandleGetResponseResult(IAsyncResult result)
    {
      lock (LowProfileImageLoader._syncBlock)
      {
        LowProfileImageLoader._pendingResponses.Enqueue(result);
        Monitor.Pulse(LowProfileImageLoader._syncBlock);
      }
    }

    private class PendingRequest
    {
      public Image Image { get; private set; }

      public Uri Uri { get; private set; }

      public PendingRequest(Image image, Uri uri)
      {
        this.Image = image;
        this.Uri = uri;
      }
    }

    private class ResponseState
    {
      public WebRequest WebRequest { get; private set; }

      public Image Image { get; private set; }

      public Uri Uri { get; private set; }

      public ResponseState(WebRequest webRequest, Image image, Uri uri)
      {
        this.WebRequest = webRequest;
        this.Image = image;
        this.Uri = uri;
      }
    }

    private class PendingCompletion
    {
      public Image Image { get; private set; }

      public Uri Uri { get; private set; }

      public Stream Stream { get; private set; }

      public PendingCompletion(Image image, Uri uri, Stream stream)
      {
        this.Image = image;
        this.Uri = uri;
        this.Stream = stream;
      }
    }

    private class ImageLoaderData
    {
      public Uri Source { get; set; }

      public Uri DefaultSource { get; set; }

      public LowProfileImageLoader.UriAppliedStatus UriAppliedStatus { get; set; }

      public bool InQueue { get; set; }
    }

    private enum UriAppliedStatus
    {
      None,
      Source,
      Default,
      Failed,
    }
  }
}
