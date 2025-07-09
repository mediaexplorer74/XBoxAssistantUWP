// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.AvatarManifest
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;
using Microsoft.XboxLive.Avatars.Internal.Parsers;
using Microsoft.XboxLive.Avatars.Internal.Version1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Xml;


namespace Microsoft.XboxLive.Avatars.Internal
{
  public abstract class AvatarManifest
  {
    public const int MANIFEST_LEN_IN_BYTES = 1000;
    internal int m_VersionNumber;

    private void ManifestUpdateWorker(object state)
    {
      AvatarManifest.AvatarManifestObjectState manifestObjectState = state as AvatarManifest.AvatarManifestObjectState;
      Exception e = (Exception) null;
      bool updateStatus = false;
      try
      {
        updateStatus = this.Update(manifestObjectState.DataManager, manifestObjectState.AssetId);
      }
      catch (Exception ex)
      {
        e = ex;
      }
      ManifestUpdateCompletedEventArgs completedEventArgs = new ManifestUpdateCompletedEventArgs(e, updateStatus);
      manifestObjectState.AsyncOp.PostOperationCompleted(new SendOrPostCallback(this.OnUpdateCompleted), (object) completedEventArgs);
    }

    private void OnUpdateCompleted(object operationState)
    {
      ManifestUpdateCompletedEventArgs e = operationState as ManifestUpdateCompletedEventArgs;
      if (this.UpdateCompleted == null)
        return;
      this.UpdateCompleted((object) this, e);
    }

    public event ManifestUpdateCompletedEventHandler UpdateCompleted;

    public void UpdateAsync(IDataManager dataManager, Guid newAssetId)
    {
      ThreadPool.QueueUserWorkItem(new WaitCallback(this.ManifestUpdateWorker), (object) new AvatarManifest.AvatarManifestObjectState(AsyncOperationManager.CreateOperation((object) null))
      {
        AssetId = newAssetId,
        DataManager = dataManager
      });
    }

    private void UpdateDependenciesWorker(object state)
    {
      AvatarManifest.AvatarManifestObjectState manifestObjectState = state as AvatarManifest.AvatarManifestObjectState;
      Exception error = (Exception) null;
      try
      {
        this.UpdateDependencies(manifestObjectState.DataManager);
      }
      catch (Exception ex)
      {
        error = ex;
      }
      AsyncCompletedEventArgs completedEventArgs = new AsyncCompletedEventArgs(error, false, (object) null);
      manifestObjectState.AsyncOp.PostOperationCompleted(new SendOrPostCallback(this.OnUpdateDependenciesCompleted), (object) completedEventArgs);
    }

    private void OnUpdateDependenciesCompleted(object operationState)
    {
      AsyncCompletedEventArgs e = operationState as AsyncCompletedEventArgs;
      if (this.UpdateDependenciesCompleted == null)
        return;
      this.UpdateDependenciesCompleted((object) this, e);
    }

    public event ManifestUpdateDependenciesCompletedEventHandler UpdateDependenciesCompleted;

    public void UpdateDependenciesAsync(IDataManager dataManager)
    {
      ThreadPool.QueueUserWorkItem(new WaitCallback(this.UpdateDependenciesWorker), (object) new AvatarManifest.AvatarManifestObjectState(AsyncOperationManager.CreateOperation((object) null))
      {
        DataManager = dataManager
      });
    }

    internal int Version => this.m_VersionNumber;

    public static AvatarManifest Create(byte[] description)
    {
      AvatarManifestV1 avatarManifestV1 = new AvatarManifestV1();
      EndianStream stream = new EndianStream((Stream) new MemoryStream(description));
      bool flag = false;
      if (stream.ReadUInt() == 0U)
        flag = avatarManifestV1.InitFromBinary(stream);
      return !flag ? (AvatarManifest) null : (AvatarManifest) avatarManifestV1;
    }

    public static Dictionary<string, AvatarManifest> Create(XmlReader description)
    {
      Dictionary<string, AvatarManifest> dictionary = new Dictionary<string, AvatarManifest>();
      try
      {
        if (!description.ReadToFollowing("AvatarManifests"))
          return (Dictionary<string, AvatarManifest>) null;
        if (description.ReadToDescendant("Manifests"))
        {
          if (description.ReadToDescendant(nameof (AvatarManifest)))
          {
            while (description.ReadToDescendant("Gamertag"))
            {
              try
              {
                string str = description.ReadElementContentAsString();
                byte[] numArray = new byte[1000];
                description.ReadElementContentAsBinHex(numArray, 0, 1000);
                AvatarManifest avatarManifest = AvatarManifest.Create(numArray);
                dictionary.Add(str.ToLower(), avatarManifest);
              }
              catch (Exception ex)
              {
                string message = ex.Message;
                Logger.Log((Log) new DebugLog((object) new AvatarManifestV1(), ex.Message));
              }
              finally
              {
                description.ReadToFollowing(nameof (AvatarManifest));
              }
            }
          }
        }
      }
      catch (XmlException ex)
      {
        string message = ex.Message;
        return (Dictionary<string, AvatarManifest>) null;
      }
      return dictionary;
    }

    public static AvatarManifest Create(XmlReader description, string gamerTag)
    {
      try
      {
        if (!description.ReadToFollowing("AvatarManifests"))
          return (AvatarManifest) null;
        if (description.ReadToDescendant("Manifests") && description.ReadToDescendant(nameof (AvatarManifest)))
        {
          while (description.ReadToDescendant("Gamertag"))
          {
            if (description.ReadElementContentAsString().ToLower() == gamerTag.ToLower())
            {
              byte[] numArray = new byte[1000];
              description.ReadElementContentAsBinHex(numArray, 0, 1000);
              return AvatarManifest.Create(numArray);
            }
            description.ReadToFollowing(nameof (AvatarManifest));
          }
        }
        throw new NoAvatarManifestException("Manifest doesn't exists. Either Gamertag is invalid or there is no avatar associted with current gamertag.");
      }
      catch (XmlException ex)
      {
        return (AvatarManifest) null;
      }
    }

    public abstract AvatarManifest Clone();

    public abstract AvatarGender BodyType { get; }

    public static bool operator ==(AvatarManifest a, AvatarManifest b)
    {
      if ((object) a == (object) b)
        return true;
      return (object) (a as AvatarManifestV1) != null && (object) (b as AvatarManifestV1) != null && a as AvatarManifestV1 == b as AvatarManifestV1;
    }

    public static bool operator !=(AvatarManifest a, AvatarManifest b) => !(a == b);

    public override bool Equals(object obj)
    {
      return (object) (obj as AvatarManifest) != null && this == (AvatarManifest) obj;
    }

    public override int GetHashCode() => 0;

    public abstract List<ComponentInfo> GetComponents(AvatarComponentMasks mask);

    public abstract bool RemoveComponents(AvatarComponentMasks mask);

    public abstract void ReplaceComponent(ComponentInfo newComponent);

    public abstract bool IsComponentPresent(AvatarComponentType componentId);

    public abstract void UpdateDependencies(IDataManager dataManager);

    public abstract byte[] SaveToBinary();

    private static void DownloadAssetCompleted(object sender, DataRequestCompletedEventArgs e)
    {
      SyncDownloadContext userState = (SyncDownloadContext) e.UserState;
      if (e.Error != null)
      {
        userState.error = e.Error;
        userState.stream = (Stream) null;
      }
      else
        userState.stream = !e.Cancelled ? e.Result : (Stream) null;
      userState.syncEvent.Set();
    }

    public bool Update(IDataManager dataManager, Guid newAssetId)
    {
      if (dataManager == null)
        throw new ArgumentNullException(nameof (dataManager));
      using (SyncDownloadContext context = new SyncDownloadContext())
      {
        dataManager.GetAssetAsync(newAssetId, new DownloadRequestEventHandler(AvatarManifest.DownloadAssetCompleted), (object) context);
        context.syncEvent.WaitOne();
        Stream stream = context.stream;
        if (stream == null)
          return false;
        StructuredBinary structuredBinary = new StructuredBinary();
        if (!structuredBinary.Open(stream))
          return false;
        BlockIterator iterator = structuredBinary.Iterator;
        AssetMetadataParser assetMetadataParser = new AssetMetadataParser();
        if (!assetMetadataParser.LoadFromStrb(iterator))
          throw new AvatarException(Resources.InvalidAnimationAssetFileText);
        if ((assetMetadataParser.BodyTypeMask & this.BodyType) == AvatarGender.Unknown || assetMetadataParser.AssetType != BinaryAssetType.Component)
          return false;
        this.ReplaceComponent(new ComponentInfo()
        {
          m_AssetId = newAssetId,
          m_ComponentMask = (AvatarComponentMasks) assetMetadataParser.AssetTypeDetails
        });
      }
      return true;
    }

    public static AvatarManifest[] CreateRandom(AvatarGender bodyMask, int avatarsCount)
    {
      return bodyMask == AvatarGender.Male || bodyMask == AvatarGender.Female || bodyMask == AvatarGender.Both ? new RandomAvatar().CreateAvatars(bodyMask, avatarsCount) : throw new ArgumentOutOfRangeException(nameof (bodyMask));
    }

    public static AvatarManifest[] CreateRandom(
      IDataManager dataManager,
      AvatarGender bodyMask,
      int avatarsCount)
    {
      if (bodyMask != AvatarGender.Male && bodyMask != AvatarGender.Female && bodyMask != AvatarGender.Both)
        throw new ArgumentOutOfRangeException(nameof (bodyMask));
      Logger.Log((Log) new DebugLog(new object(), "deprecated funtion call, use AvatarManifest.CreateRandom(bodymask, avatarscount) instead."));
      return new RandomAvatar().CreateAvatars(bodyMask, avatarsCount);
    }

    private class AvatarManifestObjectState
    {
      private AsyncOperation m_asyncOp;

      internal IDataManager DataManager { get; set; }

      internal Guid AssetId { get; set; }

      internal AsyncOperation AsyncOp => this.m_asyncOp;

      internal AvatarManifestObjectState(AsyncOperation asyncOp) => this.m_asyncOp = asyncOp;
    }
  }
}
