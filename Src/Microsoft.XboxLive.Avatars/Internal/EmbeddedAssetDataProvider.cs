// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.EmbeddedAssetDataProvider
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;
using System.Globalization;
using System.IO;
using System.Reflection;


namespace Microsoft.XboxLive.Avatars.Internal
{
  public class EmbeddedAssetDataProvider : IDataProvider
  {
    private Assembly resourceAssembly;
    private string m_stockAssetAddressFormat;
    private string m_nonStockAssetAddressFormat;
    private string m_manifestServiceAddressFormat;

    public EmbeddedAssetDataProvider(
      Assembly assembly,
      string stockAssetAddressFormat,
      string nonStockAssetAddressFormat)
    {
      this.m_stockAssetAddressFormat = stockAssetAddressFormat;
      this.m_nonStockAssetAddressFormat = nonStockAssetAddressFormat;
      this.resourceAssembly = assembly;
    }

    public EmbeddedAssetDataProvider(Assembly assembly, string manifestServiceAddressFormat)
    {
      this.m_manifestServiceAddressFormat = manifestServiceAddressFormat;
      this.resourceAssembly = assembly;
    }

    public EmbeddedAssetDataProvider(Assembly assembly) => this.resourceAssembly = assembly;

    private string MakeStockAssetAddress(string asset)
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, this.m_stockAssetAddressFormat, new object[1]
      {
        (object) asset
      });
    }

    private string MakeNonStockAssetAddress(string asset, string titleId)
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, this.m_nonStockAssetAddressFormat, new object[2]
      {
        (object) titleId,
        (object) asset
      });
    }

    private string MakeManifestAddress(string gamertag)
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, this.m_manifestServiceAddressFormat, new object[1]
      {
        (object) gamertag
      });
    }

    private string MakeAssetAddressFromGuid(Guid avatarAssetGuid)
    {
      return !AssetLoader.IsStockAsset(avatarAssetGuid) ? this.MakeNonStockAssetAddress(avatarAssetGuid.ToString(), AssetLoader.GetTitleIdFromAssetId(avatarAssetGuid)) : this.MakeStockAssetAddress(avatarAssetGuid.ToString());
    }

    public void GetDataAsync(DataRequest request, DataProvider dataProvider)
    {
      string name = (string) null;
      if (request is DataRequestGuid)
        name = this.MakeAssetAddressFromGuid((request as DataRequestGuid).DataId);
      else if (request is DataRequestString)
      {
        DataRequestString dataRequestString = request as DataRequestString;
        name = this.m_manifestServiceAddressFormat == null ? this.MakeStockAssetAddress(dataRequestString.DataId) : this.MakeManifestAddress(dataRequestString.DataId);
      }
      DataRequestCompletedEventArgs completedEventArgs;
      if (name == null)
        completedEventArgs = new DataRequestCompletedEventArgs((Exception) new ArgumentNullException("dataId", "Address cannot be null"), false, request.Context);
      if ((object) this.resourceAssembly == null)
        completedEventArgs = new DataRequestCompletedEventArgs((Exception) new ArgumentNullException("resourceAssembly", "resourceAssembly cannot be null"), false, request.Context);
      DataRequestCompletedEventArgs eventArguments;
      try
      {
        Stream manifestResourceStream = this.resourceAssembly.GetManifestResourceStream(name);
        if (manifestResourceStream != null)
        {
          eventArguments = new DataRequestCompletedEventArgs((Exception) null, false, request.Context);
          eventArguments.Result = manifestResourceStream;
        }
        else
          eventArguments = new DataRequestCompletedEventArgs((Exception) new InvalidOperationException("Failed to obtain resource."), false, request.Context);
      }
      catch (Exception ex)
      {
        eventArguments = new DataRequestCompletedEventArgs(ex, false, request.Context);
      }
      dataProvider.RequestProcessed(eventArguments, request);
    }

    public void CancelAsync()
    {
    }
  }
}
