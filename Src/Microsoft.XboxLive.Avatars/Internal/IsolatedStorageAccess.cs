// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.IsolatedStorageAccess
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System.IO;
using System.IO.IsolatedStorage;


namespace Microsoft.XboxLive.Avatars.Internal
{
  internal static class IsolatedStorageAccess
  {
    internal static IsolatedStorageFileStream CreateIsolatedStorageFileStream(
      string path,
      FileMode mode,
      FileAccess access,
      FileShare share,
      IsolatedStorageFile isf)
    {
      IsolatedStorageFileStream storageFileStream = (IsolatedStorageFileStream) null;
      try
      {
        storageFileStream = new IsolatedStorageFileStream(path, mode, access, share, isf);
      }
      catch (IOException ex)
      {
      }
      catch (IsolatedStorageException ex)
      {
      }
      return storageFileStream;
    }
  }
}
