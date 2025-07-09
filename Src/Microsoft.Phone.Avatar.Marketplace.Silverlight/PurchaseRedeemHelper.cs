// *********************************************************
// Type: Microsoft.Phone.Marketplace.PurchaseRedeemHelper
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using Microsoft.Phone.Marketplace.Resources;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.GamerServices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Xml.Linq;


namespace Microsoft.Phone.Marketplace
{
  internal static class PurchaseRedeemHelper
  {
    internal static void ShowMessageBox(
      IPurchaseRequestUI ui,
      string title,
      string message,
      string button1,
      string button2,
      Action<bool> action)
    {
      if (message.Length > (int) byte.MaxValue)
        message = message.Substring(0, (int) byte.MaxValue);
      if (!Guide.IsVisible)
      {
        string[] strArray;
        if (button1 != null)
          strArray = new string[2]{ button1, button2 };
        else
          strArray = new string[1]{ button2 };
        string[] buttons = strArray;
        Guide.BeginShowMessageBox(title, message, (IEnumerable<string>) buttons, buttons.Length - 1, MessageBoxIcon.Alert, (AsyncCallback) (result =>
        {
          int? nullable1 = new int?();
          int? nullable2;
          try
          {
            nullable2 = Guide.EndShowMessageBox(result);
          }
          catch
          {
            nullable2 = new int?();
          }
          action(nullable2.HasValue && nullable2.Value == 0);
        }), (object) null);
      }
      else
        ui.DelayOperation((Action) (() => PurchaseRedeemHelper.ShowMessageBox(ui, title, message, button1, button2, action)), 1);
    }

    internal static Uri GetPurchaseUri()
    {
      string relative = "purchase";
      return HttpRequest.SafeCombine(GamerContext.MarketplaceUrl, relative);
    }

    internal static Stream CreatePurchaseRequestStream(
      Offer offer,
      Guid transactionId,
      string billingToken)
    {
      XNamespace pdlcNs = XmlNamespaces.PdlcNs;
      XElement xelement = new XElement(pdlcNs + "PurchaseRequest", new object[5]
      {
        (object) new XElement(pdlcNs + "TransactionId", (object) transactionId),
        (object) new XElement(pdlcNs + "OfferId", (object) offer.OfferId),
        (object) new XElement(pdlcNs + "PointsPrice", (object) offer.PointsPrice),
        (object) new XElement(pdlcNs + "MediaTypeId", (object) offer.MediaTypeId),
        (object) new XElement(pdlcNs + "StoreId", (object) offer.StoreId)
      });
      if (billingToken != null)
        xelement.Add((object) new XElement(pdlcNs + "PaymentType", (object) 2U));
      else
        xelement.Add((object) new XElement(pdlcNs + "PaymentType", (object) 4U));
      if (offer.Asset != null)
        xelement.Add((object) new XElement(pdlcNs + "AssetId", (object) offer.Asset.AssetId));
      if (billingToken != null)
        xelement.Add((object) new XElement(pdlcNs + "BillingToken", (object) billingToken));
      MemoryStream purchaseRequestStream = new MemoryStream();
      xelement.Save((Stream) purchaseRequestStream, SaveOptions.DisableFormatting | SaveOptions.OmitDuplicateNamespaces);
      purchaseRequestStream.Position = 0L;
      return (Stream) purchaseRequestStream;
    }

    internal static void SetLastKnownLegalLocale(IPurchaseRequestUI ui)
    {
      string name = Thread.CurrentThread.CurrentUICulture.Name;
      if (name == null)
        return;
      UIResources.Culture = new CultureInfo(name);
      ui.LoadResourceStrings();
    }

    internal static Action CompleteWithBrowserTask(
      Uri uri,
      string locale,
      uint titleId,
      string sessionId,
      Action<Exception> errorHandler)
    {
      WebBrowserTask browser = new WebBrowserTask();
      try
      {
        string str1 = uri.ToString();
        if (str1.Contains("{titleid}") || str1.Contains("{sessionid}"))
          SecureLIVEnRequest.ValidateSecure(uri);
        string language;
        string region;
        GamerContextParser.ParseLanguageAndRegion(locale, out language, out region);
        sessionId = sessionId == null ? string.Empty : sessionId;
        string str2 = "rehydrate";
        while (sessionId.StartsWith(str2))
          sessionId = sessionId.Substring(str2.Length);
        string uriString = str1.Replace("{titleid}", Uri.EscapeDataString(string.Format("{0:x}", (object) titleId))).Replace("{sessionid}", Uri.EscapeDataString(sessionId)).Replace("{locale}", Uri.EscapeDataString(locale)).Replace("{language}", Uri.EscapeDataString(language)).Replace("{countryorregion}", Uri.EscapeDataString(region));
        browser.Uri = new Uri(uriString);
        errorHandler((Exception) new BrowserNavigationException());
        return (Action) (() => browser.Show());
      }
      catch (Exception ex)
      {
        ServiceFailureException failureException = ExceptionHelper.Transform(ex);
        errorHandler((Exception) failureException);
      }
      return (Action) null;
    }
  }
}
