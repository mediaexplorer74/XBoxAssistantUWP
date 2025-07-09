// *********************************************************
// Type: Xbox.Live.Phone.Utils.Linq.LinqHelper
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;


namespace Xbox.Live.Phone.Utils.Linq
{
  public static class LinqHelper
  {
    private const string ComponentName = "LinqHelper";

    [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "prevent random failure crash the app")]
    public static XElement SafeParseXElement(string toParse)
    {
      XElement xelement = (XElement) null;
      if (!string.IsNullOrWhiteSpace(toParse))
      {
        try
        {
          xelement = XElement.Parse(toParse);
        }
        catch (Exception ex)
        {
        }
      }
      return xelement;
    }

    public static string TryGetElementValue(XElement item, XName xchildName)
    {
      return item == null ? (string) null : LinqHelper.TryGetStringValue(item.Element(xchildName));
    }

    public static string TryGetStringValue(XElement item) => item?.Value;

    public static string TryGetStringValue(XAttribute item) => item?.Value;

    public static string TryGetScrubbedStringValue(XElement item)
    {
      string stringToUnescape = item?.Value;
      string scrubbedStringValue = string.Empty;
      if (stringToUnescape != null)
      {
        try
        {
          scrubbedStringValue = Uri.UnescapeDataString(stringToUnescape);
        }
        catch (UriFormatException ex)
        {
          scrubbedStringValue = stringToUnescape;
        }
      }
      return scrubbedStringValue;
    }

    public static bool? TryGetBoolValue(XElement item)
    {
      bool? boolValue = new bool?();
      bool result;
      if (item != null && bool.TryParse(item.Value, out result))
        boolValue = new bool?(result);
      return boolValue;
    }

    public static double? TryGetDoubleValue(XElement item)
    {
      double? doubleValue = new double?();
      double result;
      if (item != null && double.TryParse(item.Value, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result))
        doubleValue = new double?(result);
      return doubleValue;
    }

    public static int? TryGetIntValue(XElement item)
    {
      return item == null ? new int?() : LinqHelper.TryGetIntValue(item.Value);
    }

    public static int? TryGetIntValue(string stringVal)
    {
      int? intValue = new int?();
      int result;
      if (!string.IsNullOrWhiteSpace(stringVal) && int.TryParse(stringVal, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result))
        intValue = new int?(result);
      return intValue;
    }

    public static uint TryGetUintTitleIdValue(XElement item)
    {
      uint uintTitleIdValue = 0;
      uint result;
      if (item != null && uint.TryParse(item.Value, NumberStyles.None, (IFormatProvider) NumberFormatInfo.InvariantInfo, out result))
        uintTitleIdValue = result;
      return uintTitleIdValue;
    }

    public static uint TryGetHexTitleIdValue(XElement item)
    {
      uint hexTitleIdValue = 0;
      if (item != null)
      {
        string s = item.Value;
        if (!string.IsNullOrEmpty(s) && s.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
          s = s.Substring(2);
        uint result;
        if (uint.TryParse(s, NumberStyles.HexNumber, (IFormatProvider) NumberFormatInfo.InvariantInfo, out result))
          hexTitleIdValue = result;
      }
      return hexTitleIdValue;
    }

    public static DateTime? TryGetDateTimeValue(XElement item)
    {
      DateTime? dateTimeValue = new DateTime?();
      if (item != null)
        dateTimeValue = new DateTime?(XmlConvert.ToDateTime(item.Value, XmlDateTimeSerializationMode.Utc));
      return dateTimeValue;
    }

    public static TimeSpan? TryGetTimeSpanValue(XElement item)
    {
      TimeSpan? timeSpanValue = new TimeSpan?();
      if (item != null)
        timeSpanValue = new TimeSpan?(XmlConvert.ToTimeSpan(item.Value));
      return timeSpanValue;
    }

    public static TimeSpan TryGetTimeSpan(XElement item, XName name)
    {
      TimeSpan timeSpan = new TimeSpan();
      try
      {
        if (item != null)
        {
          if (item.Element(name) != null)
            timeSpan = XmlConvert.ToTimeSpan(item.Element(name).Value);
        }
      }
      catch (FormatException ex)
      {
      }
      return timeSpan;
    }

    public static List<string> TryGetStringList(XElement listElement, XName xname)
    {
      IEnumerable<string> collection = (IEnumerable<string>) null;
      if (listElement != null)
        collection = listElement.Elements(xname).Select<XElement, string>((Func<XElement, string>) (itemElement => itemElement.Value));
      return collection != null ? new List<string>(collection) : (List<string>) null;
    }

    public static List<string> TryGetScrubbedStringList(XElement listElement, XName xname)
    {
      IEnumerable<string> collection = (IEnumerable<string>) null;
      if (listElement != null)
        collection = listElement.Elements(xname).Select<XElement, string>((Func<XElement, string>) (itemElement => Uri.UnescapeDataString(itemElement.Value)));
      return collection != null ? new List<string>(collection) : (List<string>) null;
    }

    public static int GetIntValue(XElement entry, XName name, int defaultValue)
    {
      string str = LinqHelper.GetValue(entry, name, string.Empty);
      int intValue = defaultValue;
      try
      {
        intValue = Convert.ToInt32(str, (IFormatProvider) CultureInfo.InvariantCulture);
      }
      catch (FormatException ex)
      {
      }
      catch (OverflowException ex)
      {
      }
      return intValue;
    }

    public static string GetValue(XElement entry, XName name, string defaultValue)
    {
      if (entry == null)
        return defaultValue;
      XElement xelement = entry.Element(name);
      return xelement == null ? defaultValue : xelement.Value;
    }

    public static string GetValue(
      XElement entry,
      XName name,
      XName secondName,
      string defaultValue)
    {
      return entry == null || entry.Element(name) == null || entry.Element(name).Element(secondName) == null ? defaultValue : entry.Element(name).Element(secondName).Value;
    }
  }
}
