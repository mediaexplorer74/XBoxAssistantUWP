// *********************************************************
// Type: Microsoft.Phone.Marketplace.PaymentType
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll


namespace Microsoft.Phone.Marketplace
{
  internal enum PaymentType
  {
    CreditCard = 1,
    Token = 2,
    Points = 4,
    Wholesale = 8,
    NoTokens = 13, // 0x0000000D
    All = 15, // 0x0000000F
    Untrusted = 65536, // 0x00010000
  }
}
