// *********************************************************
// Type: Microsoft.Phone.Marketplace.ExtensionMethods
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;
using System.Text.RegularExpressions;
using System.Xml.Linq;


namespace Microsoft.Phone.Marketplace
{
  internal static class ExtensionMethods
  {
    private static readonly Regex GuidRegex = new Regex("([0-9a-f]{8}\\-?[0-9a-f]{4}\\-?[0-9a-f]{4}\\-?[0-9a-f]{4}\\-?[0-9a-f]{12})", RegexOptions.IgnoreCase);

    internal static Guid ParseGuid(this XElement guidElement) => ((string) guidElement).ParseGuid();

    internal static Guid ParseGuid(this string guidString)
    {
      if (!string.IsNullOrEmpty(guidString))
      {
        Match match = ExtensionMethods.GuidRegex.Match(guidString);
        if (match.Success)
          return new Guid(match.Groups[1].Captures[0].Value);
      }
      return Guid.Empty;
    }

    internal static bool IsNotNull(this XElement e)
    {
      if (e == null)
        return false;
      XAttribute xattribute = e.Attribute(XmlNamespaces.SchemaNs + "nil");
      return xattribute == null || !(xattribute.Value == "true");
    }
  }
}
