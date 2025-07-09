// *********************************************************
// Type: Microsoft.Phone.Marketplace.Util.InvalidDataException
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;


namespace Microsoft.Phone.Marketplace.Util
{
  public sealed class InvalidDataException : SystemException
  {
    public InvalidDataException()
      : base("Data invalid")
    {
    }

    public InvalidDataException(string message)
      : base(message)
    {
    }

    public InvalidDataException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
