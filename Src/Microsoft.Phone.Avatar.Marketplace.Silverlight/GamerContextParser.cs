// *********************************************************
// Type: Microsoft.Phone.Marketplace.GamerContextParser
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;
using System.Globalization;
using System.IO;
using System.Xml.Linq;


namespace Microsoft.Phone.Marketplace
{
  internal class GamerContextParser
  {
    internal GamerContext Parse(Stream stream, out int points)
    {
      XNamespace pdlcNs = XmlNamespaces.PdlcNs;
      GamerContext context = new GamerContext();
      XElement xelement = XElement.Load(stream);
      context.Expires = DateTime.Parse((string) xelement.Element(pdlcNs + "Expires"), (IFormatProvider) null, DateTimeStyles.AdjustToUniversal);
      context.XblMarketplaceCatalogUrl = HttpRequest.SafeCombine(new Uri((string) xelement.Element(pdlcNs + "XblMarketplaceCatalogUrl")), "v1/product/");
      context.MOPageAddPointsUrl = new Uri((string) xelement.Element(pdlcNs + "MOPageAddPointsUrl"));
      context.MOPageSetupBillingUrl = new Uri((string) xelement.Element(pdlcNs + "MOPageSetupBillingUrl"));
      context.MOPageTouUrl = new Uri((string) xelement.Element(pdlcNs + "MOPageTouUrl"));
      context.LegalLocale = (string) xelement.Element(pdlcNs + "LegalLocale");
      this.SetLegalLanguage(context);
      context.UserType = (int) xelement.Element(pdlcNs + "UserType");
      context.CanPurchasePDLC = (bool) xelement.Element(pdlcNs + "CanPurchasePDLC");
      context.IsLightweightAccount = (bool) xelement.Element(pdlcNs + "IsLightweightAccount");
      context.AvatarMarketplaceUrl = (string) xelement.Element(pdlcNs + "AvatarMarketplaceUrl");
      points = (int) xelement.Element(pdlcNs + "PointsBalance");
      return context;
    }

    private void SetLegalLanguage(GamerContext context)
    {
      GamerContext.LastKnownLegalLanguage = context.LegalLocale;
    }

    internal static void ParseLanguageAndRegion(
      string locale,
      out string language,
      out string region)
    {
      language = string.Empty;
      region = string.Empty;
      if (string.IsNullOrEmpty(locale))
        return;
      language = locale;
      int length = locale.IndexOf('-');
      if (length < 0)
        return;
      language = locale.Substring(0, length);
      region = locale.Substring(length + 1);
    }
  }
}
