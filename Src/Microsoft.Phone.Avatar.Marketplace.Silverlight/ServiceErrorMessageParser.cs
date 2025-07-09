// *********************************************************
// Type: Microsoft.Phone.Marketplace.ServiceErrorMessageParser
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;
using System.IO;
using System.Xml.Linq;


namespace Microsoft.Phone.Marketplace
{
  internal class ServiceErrorMessageParser
  {
    internal ServiceFailureException Parse(Stream stream, Exception innerException)
    {
      XNamespace pdlcNs = XmlNamespaces.PdlcNs;
      XElement xelement = XElement.Load(stream);
      return ServiceFailureException.GetServiceFailure((ServiceErrorBucket) Enum.Parse(typeof (ServiceErrorBucket), (string) xelement.Element(pdlcNs + "Bucket"), false), (int) (uint) xelement.Element(pdlcNs + "ErrorCode"), (string) xelement.Element(pdlcNs + "ErrorMessage"), innerException);
    }
  }
}
