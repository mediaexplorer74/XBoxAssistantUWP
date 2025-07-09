// *********************************************************
// Type: Microsoft.Phone.Marketplace.PointsBalanceParser
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System.IO;
using System.Xml.Linq;


namespace Microsoft.Phone.Marketplace
{
  internal class PointsBalanceParser
  {
    internal int Parse(Stream stream) => (int) XElement.Load(stream);
  }
}
