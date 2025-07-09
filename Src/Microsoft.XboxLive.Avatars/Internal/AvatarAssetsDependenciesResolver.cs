// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.AvatarAssetsDependenciesResolver
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;


namespace Microsoft.XboxLive.Avatars.Internal
{
  internal class AvatarAssetsDependenciesResolver
  {
    private static volatile Dictionary<Guid, AvatarAssetDependency> s_ShapeOverrides;
    private static object s_ShapeOverridesLock = new object();

    private AvatarAssetsDependenciesResolver()
    {
    }

    private static void InitializeShapeDependencies(Stream stream)
    {
      if (stream == null)
        return;
      Dictionary<Guid, AvatarAssetDependency> dictionary = new Dictionary<Guid, AvatarAssetDependency>();
      byte[] numArray = new byte[16];
      while (stream.Read(numArray, 0, 16) == 16)
      {
        Guid key = new Guid(numArray);
        if (stream.Read(numArray, 0, 16) == 16)
        {
          AvatarAssetDependency avatarAssetDependency = new AvatarAssetDependency();
          avatarAssetDependency.m_DependentAssetId = new Guid(numArray);
          int length = 0;
          for (int index = 0; index < 4; ++index)
            length += stream.ReadByte() << (index << 3);
          if (length > 0)
          {
            switch (length)
            {
              case 0:
              case 1:
              case 2:
              case 3:
              case 4:
                avatarAssetDependency.m_ModifiedAssetList = new Guid[length];
                for (int index = 0; index < length; ++index)
                {
                  stream.Read(numArray, 0, 16);
                  avatarAssetDependency.m_ModifiedAssetList[index] = new Guid(numArray);
                }
                break;
              default:
                goto label_16;
            }
          }
          else
            avatarAssetDependency.m_ModifiedAssetList = (Guid[]) null;
          if (!dictionary.ContainsKey(key))
            dictionary.Add(key, avatarAssetDependency);
        }
        else
          break;
      }
label_16:
      AvatarAssetsDependenciesResolver.s_ShapeOverrides = dictionary;
    }

    private static void DownloadDependedAssetCompleted(
      object sender,
      DataRequestCompletedEventArgs e)
    {
      if (e.Error == null && !e.Cancelled)
        AvatarAssetsDependenciesResolver.InitializeShapeDependencies(e.Result);
      ((EventWaitHandle) e.UserState).Set();
    }

    internal static void InvalidateDependenciesTable()
    {
      lock (AvatarAssetsDependenciesResolver.s_ShapeOverridesLock)
        AvatarAssetsDependenciesResolver.s_ShapeOverrides = (Dictionary<Guid, AvatarAssetDependency>) null;
    }

    internal static AvatarAssetDependency GetDependentAssets(IDataManager dataManager, Guid id)
    {
      Dictionary<Guid, AvatarAssetDependency> shapeOverrides = AvatarAssetsDependenciesResolver.s_ShapeOverrides;
      if (shapeOverrides == null)
      {
        lock (AvatarAssetsDependenciesResolver.s_ShapeOverridesLock)
        {
          if (AvatarAssetsDependenciesResolver.s_ShapeOverrides == null)
          {
            AutoResetEvent context = new AutoResetEvent(false);
            dataManager.GetAssetAsync("TocAssetDependencies".ToLower(), new DownloadRequestEventHandler(AvatarAssetsDependenciesResolver.DownloadDependedAssetCompleted), (object) context);
            context.WaitOne();
            if (AvatarAssetsDependenciesResolver.s_ShapeOverrides == null)
              throw new AvatarException(Resources.ShapeOverridesFailedToLoad);
          }
          shapeOverrides = AvatarAssetsDependenciesResolver.s_ShapeOverrides;
        }
      }
      AvatarAssetDependency dependentAssets = new AvatarAssetDependency();
      shapeOverrides.TryGetValue(id, out dependentAssets);
      return dependentAssets;
    }
  }
}
