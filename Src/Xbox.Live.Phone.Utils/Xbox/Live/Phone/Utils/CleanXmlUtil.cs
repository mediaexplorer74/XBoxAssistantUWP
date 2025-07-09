// *********************************************************
// Type: Xbox.Live.Phone.Utils.CleanXmlUtil
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using System;
using System.IO;
using System.Net;
using System.Text;


namespace Xbox.Live.Phone.Utils
{
  public class CleanXmlUtil
  {
    public static string ReadCleanedResponseXmlToString(WebResponse response)
    {
      byte[] numArray = response != null ? new byte[response.ContentLength] : throw new ArgumentNullException(nameof (response));
      int num = 0;
      using (Stream responseStream = response.GetResponseStream())
      {
        do
        {
          num += responseStream.Read(numArray, num, (int) response.ContentLength - num);
        }
        while (num < (int) response.ContentLength);
      }
      return CleanXmlUtil.TrimPrefixXml(Encoding.UTF8.GetString(numArray, 0, num));
    }

    public static string TrimPrefixXml(string xml)
    {
      if (string.IsNullOrWhiteSpace(xml))
        return xml;
      int startIndex = xml.IndexOf("<?xml", StringComparison.OrdinalIgnoreCase);
      xml = startIndex < 0 ? string.Empty : xml.Substring(startIndex).Replace("\r\n", string.Empty);
      return xml;
    }
  }
}
