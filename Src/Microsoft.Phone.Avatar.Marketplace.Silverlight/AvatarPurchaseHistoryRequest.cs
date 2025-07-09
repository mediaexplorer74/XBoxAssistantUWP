// *********************************************************
// Type: Microsoft.Phone.Marketplace.AvatarPurchaseHistoryRequest
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using Microsoft.Phone.Marketplace.Resources;
using System;
using System.Collections.Generic;
using System.Xml.Linq;


namespace Microsoft.Phone.Marketplace
{
  public class AvatarPurchaseHistoryRequest : PurchaseHistoryRequest
  {
    public void EnumerateAvatarPurchaseHistoryAsync(
      int titleId,
      int pageSize,
      int pageNum,
      IEnumerable<Guid> transactionIds,
      DateTime? startDateUtc,
      object userData)
    {
      if (this._enumeratePurchaseHistoryCompleted == null)
        throw new InvalidOperationException(NonLocalizedResources.CompletedEventNotHooked);
      this.EnumeratePurchaseHistoryAsyncCommon(titleId, pageSize, pageNum, transactionIds, 47, startDateUtc, userData, false);
    }

    public void EnumerateSignedAvatarPurchaseHistoryAsync(
      int titleId,
      int pageSize,
      int pageNum,
      IEnumerable<Guid> transactionIds,
      DateTime? startDateUtc,
      object userData)
    {
      if (this._enumerateSignedPurchaseHistoryCompleted == null)
        throw new InvalidOperationException(NonLocalizedResources.CompletedEventNotHooked);
      this.EnumeratePurchaseHistoryAsyncCommon(titleId, pageSize, pageNum, transactionIds, 47, startDateUtc, userData, true);
    }

    protected override XElement CreateReceiptRequestXml(XNamespace ns)
    {
      return new XElement(ns + "ReceiptRequest", new object[4]
      {
        (object) new XElement(ns + "PageNumber", (object) this._pageNum),
        (object) new XElement(ns + "PageSize", (object) this._pageSize),
        (object) new XElement(ns + "TitleId", (object) this._titleId),
        (object) new XElement(ns + "StoreId", (object) 1)
      });
    }
  }
}
