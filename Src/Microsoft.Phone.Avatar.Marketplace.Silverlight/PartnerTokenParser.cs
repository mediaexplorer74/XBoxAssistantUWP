// *********************************************************
// Type: Microsoft.Phone.Marketplace.PartnerTokenParser
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;


namespace Microsoft.Phone.Marketplace
{
  internal class PartnerTokenParser
  {
    internal DateTime ParseExpiration(string token)
    {
      return DateTime.Parse((string) XElement.Parse(token).Descendants(XmlNamespaces.SamlNs + "Conditions").First<XElement>().Attribute((XName) "NotOnOrAfter"), (IFormatProvider) null, DateTimeStyles.AdjustToUniversal);
    }
  }
}
