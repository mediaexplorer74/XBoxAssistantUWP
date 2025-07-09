// *********************************************************
// Type: Xbox.Live.Phone.Services.Beacons.StubBeaconsServiceManager
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Xbox.Live.Phone.Utils;


namespace Xbox.Live.Phone.Services.Beacons
{
  public sealed class StubBeaconsServiceManager : IBeaconsServiceManager
  {
    private static string nextActivityStubFile = string.Empty;
    private static string nextBeaconDefaultTextStubFile = string.Empty;
    private static string nextBeaconForTitleStubFile = string.Empty;

    public event EventHandler<ServiceProxyEventArgs<ActivityListForTitleData>> EventGetActivityListForTitleCompleted;

    public event EventHandler<ServiceProxyEventArgs<BeaconDefaultTextData>> EventGetBeaconDefaultTextCompleted;

    public event EventHandler<ServiceProxyEventArgs<BeaconInfo>> EventGetBeaconForTitleCompleted;

    public event EventHandler<ServiceProxyEventArgs<string>> EventSetBeaconForTitleCompleted;

    public event EventHandler<ServiceProxyEventArgs<string>> EventDeleteBeaconForTitleCompleted;

    public static string NextActivityStubFile
    {
      get => StubBeaconsServiceManager.nextActivityStubFile;
      set => StubBeaconsServiceManager.nextActivityStubFile = value;
    }

    public static string NextBeaconDefaultTextStubFile
    {
      get => StubBeaconsServiceManager.nextBeaconDefaultTextStubFile;
      set => StubBeaconsServiceManager.nextBeaconDefaultTextStubFile = value;
    }

    public static string NextBeaconForTitleStubFile
    {
      get => StubBeaconsServiceManager.nextBeaconForTitleStubFile;
      set => StubBeaconsServiceManager.nextBeaconForTitleStubFile = value;
    }

    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "environmentName", Justification = "This is stub")]
    public void Initialize(ServiceCommon.Environment environmentName)
    {
    }

    public void GetActivityListForTitle(uint gameId)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        string response = ServiceCommon.ReadResource(string.IsNullOrEmpty(StubBeaconsServiceManager.NextActivityStubFile) ? "Xbox.Live.Phone.Services.StubData.ActivityListForTitle.txt" : StubBeaconsServiceManager.NextActivityStubFile);
        StubBeaconsServiceManager.NextActivityStubFile = string.Empty;
        ActivityListForTitleData activityListForTitleData = BeaconsResponseParser.ParseGetActivityListForTitleResponse(response);
        Thread.Sleep(2000);
        ThreadManager.UIThreadPost((SendOrPostCallback) delegate
        {
          EventHandler<ServiceProxyEventArgs<ActivityListForTitleData>> forTitleCompleted = this.EventGetActivityListForTitleCompleted;
          if (forTitleCompleted == null)
            return;
          forTitleCompleted((object) this, new ServiceProxyEventArgs<ActivityListForTitleData>((object) activityListForTitleData, (Exception) null, false, (object) null));
        }, (object) this);
      });
    }

    public void GetBeaconDefaultText(uint titleId)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        string response = ServiceCommon.ReadResource(string.IsNullOrEmpty(StubBeaconsServiceManager.NextBeaconDefaultTextStubFile) ? "Xbox.Live.Phone.Services.StubData.BeaconDefaultText.txt" : StubBeaconsServiceManager.NextBeaconDefaultTextStubFile);
        StubBeaconsServiceManager.NextBeaconDefaultTextStubFile = string.Empty;
        BeaconDefaultTextData beaconDefaultTextData = BeaconsResponseParser.ParseGetBeaconDefaultTextResponse(response);
        Thread.Sleep(2000);
        ThreadManager.UIThreadPost((SendOrPostCallback) delegate
        {
          EventHandler<ServiceProxyEventArgs<BeaconDefaultTextData>> defaultTextCompleted = this.EventGetBeaconDefaultTextCompleted;
          if (defaultTextCompleted == null)
            return;
          defaultTextCompleted((object) this, new ServiceProxyEventArgs<BeaconDefaultTextData>((object) beaconDefaultTextData, (Exception) null, false, (object) null));
        }, (object) this);
      });
    }

    public void GetBeaconForTitle(uint gameId)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        string response = ServiceCommon.ReadResource(string.IsNullOrEmpty(StubBeaconsServiceManager.NextBeaconForTitleStubFile) ? "Xbox.Live.Phone.Services.StubData.BeaconForTitle.txt" : StubBeaconsServiceManager.NextBeaconForTitleStubFile);
        StubBeaconsServiceManager.NextBeaconForTitleStubFile = string.Empty;
        BeaconInfo beaconInfo = BeaconsResponseParser.ParseGetBeaconForTitleResponse(response);
        Thread.Sleep(2000);
        ThreadManager.UIThreadPost((SendOrPostCallback) delegate
        {
          EventHandler<ServiceProxyEventArgs<BeaconInfo>> forTitleCompleted = this.EventGetBeaconForTitleCompleted;
          if (forTitleCompleted == null)
            return;
          forTitleCompleted((object) this, new ServiceProxyEventArgs<BeaconInfo>((object) beaconInfo, (Exception) null, false, (object) null));
        }, (object) this);
      });
    }

    public void SetBeaconForTitle(uint gameId, string beaconText)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        Thread.Sleep(2000);
        ThreadManager.UIThreadPost((SendOrPostCallback) delegate
        {
          EventHandler<ServiceProxyEventArgs<string>> forTitleCompleted = this.EventSetBeaconForTitleCompleted;
          if (forTitleCompleted == null)
            return;
          forTitleCompleted((object) this, new ServiceProxyEventArgs<string>((object) null, (Exception) null, false, (object) null));
        }, (object) this);
      });
    }

    public void DeleteBeaconForTitle(uint gameId)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        Thread.Sleep(2000);
        ThreadManager.UIThreadPost((SendOrPostCallback) delegate
        {
          EventHandler<ServiceProxyEventArgs<string>> forTitleCompleted = this.EventDeleteBeaconForTitleCompleted;
          if (forTitleCompleted == null)
            return;
          forTitleCompleted((object) this, new ServiceProxyEventArgs<string>((object) null, (Exception) null, false, (object) null));
        }, (object) this);
      });
    }
  }
}
