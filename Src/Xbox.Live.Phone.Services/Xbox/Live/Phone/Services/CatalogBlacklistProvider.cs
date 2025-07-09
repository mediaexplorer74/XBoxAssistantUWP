// *********************************************************
// Type: Xbox.Live.Phone.Services.CatalogBlacklistProvider
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Xml.Linq;
using Xbox.Live.Phone.Utils;
using Xbox.Live.Phone.Utils.Cache;


namespace Xbox.Live.Phone.Services
{
  public sealed class CatalogBlacklistProvider
  {
    public const string CatalogBlacklistCacheKey = "CatalogBlacklist";
    private const string BlacklistedGamesAndCategoriesResourceName = "Xbox.Live.Phone.Services.Avatar.MarketPlace.BlacklistedGamesAndCategories.xml";
    private const string BlacklistUrl = "https://download-ssl.xboxlive.com/content/584d07d1/BlackListedAssets.xml";
    private const string ComponentName = "CatalogBlacklistProvider";
    private static readonly XNamespace blacklistNamespace = (XNamespace) "http://xboxlive/avatar/BlacklistedAssets.xsd/";
    private static CatalogBlacklistProvider catalogBlacklistProviderInstance = (CatalogBlacklistProvider) null;
    private Xbox.Live.Phone.Utils.Etc.HashSet<int> blacklistedCategories;
    private Xbox.Live.Phone.Utils.Etc.HashSet<Guid> blacklistedGames;
    private CatalogBlacklistCache blacklistCache;

    private CatalogBlacklistProvider()
    {
      this.LoadingState = CatalogBlacklistProvider.State.Uninitialized;
    }

    public event EventHandler<ServiceProxyEventArgs<bool>> EventGetCatalogBlacklistCompleted;

    public static CatalogBlacklistProvider Instance
    {
      get
      {
        if (CatalogBlacklistProvider.catalogBlacklistProviderInstance == null)
          CatalogBlacklistProvider.catalogBlacklistProviderInstance = new CatalogBlacklistProvider();
        return CatalogBlacklistProvider.catalogBlacklistProviderInstance;
      }
    }

    public List<Guid> BlacklistedAssetIds
    {
      get
      {
        lock (this)
        {
          this.DownloadBlacklistIfNeeded();
          while (this.blacklistCache.Blacklist == null)
            Monitor.Wait((object) this);
          return this.blacklistCache.Blacklist;
        }
      }
    }

    public Xbox.Live.Phone.Utils.Etc.HashSet<int> BlacklistedCategories
    {
      get
      {
        this.DownloadBlacklistIfNeeded();
        return this.blacklistedCategories;
      }
    }

    public Xbox.Live.Phone.Utils.Etc.HashSet<Guid> BlacklistedGames
    {
      get
      {
        this.DownloadBlacklistIfNeeded();
        return this.blacklistedGames;
      }
    }

    public CatalogBlacklistProvider.State LoadingState { get; private set; }

    public void ResetFailureState()
    {
      if (this.LoadingState != CatalogBlacklistProvider.State.Failed)
        return;
      this.LoadingState = CatalogBlacklistProvider.State.Uninitialized;
    }

    public void DownloadBlackListedItemsAsync()
    {
      lock (this)
      {
        if (this.LoadingState == CatalogBlacklistProvider.State.Uninitialized)
        {
          this.LoadingState = CatalogBlacklistProvider.State.Loading;
          this.blacklistCache = CacheManager.Load<CatalogBlacklistCache>("CatalogBlacklist");
          if (this.blacklistCache == null || this.blacklistCache.Blacklist == null)
          {
            this.blacklistCache = new CatalogBlacklistCache("CatalogBlacklist");
            this.blacklistCache.Blacklist = (List<Guid>) null;
            this.blacklistCache.LastModified = (string) null;
          }
          else
            Monitor.PulseAll((object) this);
          this.LoadCategoryAndGameBlacklists();
          if (this.blacklistCache.Blacklist == null || this.blacklistCache.IsExpired)
          {
            this.GetBlacklistAsync();
          }
          else
          {
            this.LoadingState = CatalogBlacklistProvider.State.Loaded;
            this.FireEventGetCatalogBlacklistCompleted();
          }
        }
        else
          this.FireEventGetCatalogBlacklistCompleted();
      }
    }

    private static List<Guid> GetBlacklistFromResponse(HttpWebResponse response)
    {
      List<Guid> guidList = new List<Guid>();
      using (StringReader stringReader = new StringReader(CleanXmlUtil.ReadCleanedResponseXmlToString((WebResponse) response)))
        return XDocument.Load((TextReader) stringReader).Root.Descendants(CatalogBlacklistProvider.blacklistNamespace + "Assets").Where<XElement>((Func<XElement, bool>) (assets => (string) assets.Attribute((XName) "target") == "global")).First<XElement>().Descendants(CatalogBlacklistProvider.blacklistNamespace + "AssetID").Select<XElement, Guid>((Func<XElement, Guid>) (assets => new Guid(assets.Value))).ToList<Guid>();
    }

    private void FireEventGetCatalogBlacklistCompleted()
    {
      EventHandler<ServiceProxyEventArgs<bool>> tempEvent = this.EventGetCatalogBlacklistCompleted;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        if (tempEvent == null)
          return;
        tempEvent((object) null, (ServiceProxyEventArgs<bool>) null);
      }, (object) this);
    }

    private void DownloadBlacklistIfNeeded()
    {
      if (this.LoadingState != CatalogBlacklistProvider.State.Uninitialized)
        return;
      this.DownloadBlackListedItemsAsync();
    }

    private void GetBlacklistAsync()
    {
      HttpWebRequestRetry httpWebRequestRetry = new HttpWebRequestRetry(new Uri("https://download-ssl.xboxlive.com/content/584d07d1/BlackListedAssets.xml", UriKind.Absolute));
      httpWebRequestRetry.ResponseAvailable += new EventHandler<HttpWebRequestRetryEventArgs>(this.OnResponseComplete);
      httpWebRequestRetry.BeginGetResponse();
    }

    private void OnResponseComplete(object sender, HttpWebRequestRetryEventArgs e)
    {
      lock (this)
      {
        if (sender is HttpWebRequestRetry httpWebRequestRetry)
          httpWebRequestRetry.ResponseAvailable -= new EventHandler<HttpWebRequestRetryEventArgs>(this.OnResponseComplete);
        if (e.Response != null)
        {
          this.blacklistCache.Blacklist = CatalogBlacklistProvider.GetBlacklistFromResponse(e.Response);
          this.blacklistCache.LastModified = e.Response.Headers["Last-Modified"];
          this.blacklistCache.LastUpdateTime = DateTime.UtcNow;
          Monitor.PulseAll((object) this);
          CacheManager.Instance.SaveAsync((CacheItem) this.blacklistCache);
          this.LoadingState = CatalogBlacklistProvider.State.Loaded;
        }
        else
        {
          this.LoadingState = this.blacklistCache.Blacklist != null ? CatalogBlacklistProvider.State.Loaded : CatalogBlacklistProvider.State.Failed;
          Monitor.PulseAll((object) this);
          Exception exception = e.Exception;
        }
        this.FireEventGetCatalogBlacklistCompleted();
      }
    }

    private void LoadCategoryAndGameBlacklists()
    {
      this.blacklistedGames = new Xbox.Live.Phone.Utils.Etc.HashSet<Guid>();
      this.blacklistedCategories = new Xbox.Live.Phone.Utils.Etc.HashSet<int>();
      using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Xbox.Live.Phone.Services.Avatar.MarketPlace.BlacklistedGamesAndCategories.xml"))
      {
        if (manifestResourceStream == null)
          return;
        XDocument xdocument = XDocument.Load(manifestResourceStream);
        foreach (int element in xdocument.Root.Descendants((XName) "Category").Select<XElement, int>((Func<XElement, int>) (category => (int) category.Attribute((XName) "Id"))))
          this.blacklistedCategories.Add(element);
        foreach (Guid element in xdocument.Root.Descendants((XName) "Game").Select<XElement, Guid>((Func<XElement, Guid>) (game => new Guid(game.Attribute((XName) "Id").Value))))
          this.blacklistedGames.Add(element);
      }
    }

    public enum State
    {
      Uninitialized,
      Loading,
      Loaded,
      Failed,
    }
  }
}
