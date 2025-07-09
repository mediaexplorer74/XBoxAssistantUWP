// *********************************************************
// Type: Microsoft.Phone.Marketplace.CategoryEnumerationRequest
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Linq;


namespace Microsoft.Phone.Marketplace
{
  public class CategoryEnumerationRequest
  {
    private const int ImageSize = 34;
    private const int XboxStore = 1;
    private const int DetailView = 5;
    private const int OfferFilterLevel = 2;
    private const int CategorySystemId = 14000;
    private const int LifeStyleCategory = 14001;
    private const int OrderBy = 11;
    private const int OrderDirection = 1;
    private const int QueryModifier = 8;
    private int _pageSize;
    private int _pageNum;
    private EventHandler<EnumerateCategoriesCompletedEventArgs> _enumerateCategoriesCompleted;
    private HttpRequest _httpRequest;
    private SynchronizationContext _syncContext;
    private Action _cancelGamerContext;
    private bool _cancelRequested;
    private object _userState;
    private object _syncObject = new object();
    private readonly XNamespace AtomNs = (XNamespace) "http://www.w3.org/2005/Atom";

    public event EventHandler<EnumerateCategoriesCompletedEventArgs> EnumerateCategoriesCompleted
    {
      add => this.DoMutableAction((Action) (() => this._enumerateCategoriesCompleted += value));
      remove => this.DoMutableAction((Action) (() => this._enumerateCategoriesCompleted -= value));
    }

    public void EnumerateCategoriesAsync(int pageSize, int pageNum)
    {
      this.EnumerateCategoriesAsync(pageSize, pageNum, 14001, (object) null);
    }

    public void EnumerateCategoriesAsync(
      int pageSize,
      int pageNum,
      int categoryId,
      object userstate)
    {
      this.EnumerateCategoriesAsyncCommon(pageSize, pageNum, categoryId, userstate);
    }

    public void CancelAsync()
    {
      lock (this._syncObject)
      {
        if (!this.IsBusy)
          return;
        this._cancelRequested = true;
        if (this._httpRequest != null && this._httpRequest.IsBusy)
        {
          this._httpRequest.CancelAsync();
        }
        else
        {
          if (this._cancelGamerContext == null)
            return;
          this._cancelGamerContext();
        }
      }
    }

    public bool IsBusy { get; private set; }

    private static bool HasCategoryFields(XElement entry, XNamespace ns)
    {
      XElement xelement = entry.Element(ns + "category");
      return xelement != null && xelement.Element(ns + "categoryName") != null && xelement.Element(ns + "categoryId") != null;
    }

    private static Category ParseCategory(XElement e, XNamespace ns)
    {
      XElement xelement1 = e.Element(ns + "category");
      Guid mediaGuid = xelement1.Element(ns + "logoImageMediaId") != null ? xelement1.Element(ns + "logoImageMediaId").ParseGuid() : Guid.Empty;
      int num = (int) xelement1.Element(ns + "categoryId");
      string str = (string) xelement1.Element(ns + "categoryName");
      XElement xelement2 = e.Element(ns + "subcategories");
      IEnumerable<XElement> source = e.Elements(ns + "images") != null ? e.Elements(ns + "images").Elements<XElement>(ns + "image") : (IEnumerable<XElement>) null;
      XElement xelement3 = (XElement) null;
      if (source != null && source.Count<XElement>() > 0)
        xelement3 = source.Single<XElement>((Func<XElement, bool>) (i => i.ParseGuid() == mediaGuid && (int) i.Element(ns + "size") == 34));
      return new Category()
      {
        Id = num,
        ThumbnailUrl = xelement3 != null ? (string) xelement3.Element(ns + "fileUrl") : string.Empty,
        Title = str,
        HasSubCategory = xelement2 != null && xelement2.HasElements
      };
    }

    private void DoMutableAction(Action action)
    {
      lock (this._syncObject)
      {
        if (this.IsBusy)
          throw new InvalidOperationException("Object cannot be modified while IsBusy is true.");
      }
      action();
    }

    private bool IsCancelRequested
    {
      get
      {
        lock (this._syncObject)
          return this._cancelRequested;
      }
    }

    private void RaiseEnumerateCategoriesCompleted(EnumerateCategoriesCompletedEventArgs e)
    {
      SynchronizationContext syncContext = this._syncContext;
      this.ResetRequestState();
      EventHandler<EnumerateCategoriesCompletedEventArgs> enumerateCategoriesCompleted = this._enumerateCategoriesCompleted;
      if (enumerateCategoriesCompleted == null)
        return;
      if (syncContext == null)
        enumerateCategoriesCompleted((object) this, e);
      else
        syncContext.Post((SendOrPostCallback) (state => enumerateCategoriesCompleted((object) this, e)), (object) null);
    }

    private void EnumerateCategoriesAsyncCommon(
      int pageSize,
      int pageNum,
      int categoryId,
      object userState)
    {
      if (pageSize <= 0 || pageSize > 100)
        throw new ArgumentOutOfRangeException(nameof (pageSize));
      if (pageNum <= 0)
        throw new ArgumentOutOfRangeException(nameof (pageNum));
      this.DoMutableAction((Action) (() => this.IsBusy = true));
      this._syncContext = SynchronizationContext.Current;
      this._userState = userState;
      this._pageSize = pageSize;
      this._pageNum = pageNum;
      this._cancelGamerContext = GamerContext.Acquire((Action<GamerContext, Exception, bool>) ((gc, error, cancelled) =>
      {
        this._cancelGamerContext = (Action) null;
        if (this.HandleCancelAndError(cancelled, error))
          return;
        this._httpRequest = new HttpRequest(new Uri(this.BuildCatalogUri(gc.AvatarMarketplaceUrl, gc.LegalLocale, categoryId, pageSize, pageNum)));
        this._httpRequest.GetResponseCompleted += (EventHandler<GetResponseCompletedEventArgs>) ((s, e) => this.CatalogQueryComplete(e));
        this._httpRequest.GetResponseAsync(userState);
      }));
    }

    private void CatalogQueryComplete(GetResponseCompletedEventArgs e)
    {
      if (this.HandleCancelAndError(e.Cancelled, e.Error))
        return;
      try
      {
        XElement xelement = XElement.Load(XmlReader.Create(e.Response));
        XNamespace ns = xelement.GetNamespaceOfPrefix("live");
        ReadOnlyCollection<Category> categories = new ReadOnlyCollection<Category>((IList<Category>) xelement.Elements(this.AtomNs + "entry").Where<XElement>((Func<XElement, bool>) (cat => CategoryEnumerationRequest.HasCategoryFields(cat, ns))).Select<XElement, Category>((Func<XElement, Category>) (x => CategoryEnumerationRequest.ParseCategory(x, ns))).ToList<Category>());
        this.HandleSuccess(int.Parse(xelement.Element(ns + "totalItems").Value), categories);
      }
      catch (Exception ex)
      {
        this.HandleCancelAndError(false, ex);
      }
    }

    private void ResetRequestState()
    {
      lock (this._syncObject)
      {
        this.IsBusy = false;
        this._cancelRequested = false;
        this._httpRequest = (HttpRequest) null;
        this._userState = (object) null;
        this._syncContext = (SynchronizationContext) null;
        this._cancelGamerContext = (Action) null;
      }
    }

    private string BuildCatalogUri(
      string marketplaceWebServiceUrl,
      string locale,
      int categoryId,
      int pageSize,
      int pageNumber)
    {
      Dictionary<string, string> source = new Dictionary<string, string>()
      {
        {
          "Locale",
          locale
        },
        {
          "LegalLocale",
          locale
        },
        {
          "Store",
          1.ToString()
        },
        {
          "PageSize",
          pageSize.ToString()
        },
        {
          "PageNum",
          pageNumber.ToString()
        },
        {
          "DetailView",
          5.ToString()
        },
        {
          "OfferFilterLevel",
          2.ToString()
        },
        {
          "CategorySystemId",
          14000.ToString()
        },
        {
          "CategoryIds",
          categoryId.ToString()
        },
        {
          "OrderBy",
          11.ToString()
        },
        {
          "OrderDirection",
          1.ToString()
        },
        {
          "QueryModifiers",
          8.ToString()
        }
      };
      return marketplaceWebServiceUrl + "/Query?methodName=FindCategories&" + CategoryEnumerationRequest.CreateQueryParameters(source.ToDictionary<KeyValuePair<string, string>, string, string>((Func<KeyValuePair<string, string>, string>) (k => "Names=" + k.Key), (Func<KeyValuePair<string, string>, string>) (v => "Values=" + v.Value)));
    }

    private static string CreateQueryParameters(Dictionary<string, string> keyValues)
    {
      return string.Join("&", keyValues.Select<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (e => e.Key.ToString() + "&" + e.Value)).ToArray<string>());
    }

    private bool HandleCancelAndError(bool canceled, Exception error)
    {
      if (this.IsCancelRequested || canceled)
      {
        this.HandleCancel();
        return true;
      }
      if (error == null)
        return false;
      this.HandleError(error);
      return true;
    }

    private void HandleSuccess(int totalAvailable, ReadOnlyCollection<Category> categories)
    {
      this.RaiseEnumerateCategoriesCompleted(new EnumerateCategoriesCompletedEventArgs((Exception) null, false, this._userState)
      {
        TotalAvailable = totalAvailable,
        Categories = categories
      });
    }

    private void HandleError(Exception e)
    {
      e = (Exception) ExceptionHelper.Transform(e);
      this.RaiseEnumerateCategoriesCompleted(new EnumerateCategoriesCompletedEventArgs(e, false, this._userState));
    }

    private void HandleCancel()
    {
      this.RaiseEnumerateCategoriesCompleted(new EnumerateCategoriesCompletedEventArgs((Exception) null, true, this._userState));
    }
  }
}
