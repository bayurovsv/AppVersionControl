using AppVersionControl.Domain.ApplicationVersion;
using System.Collections.Generic;

namespace AppVersionControl.Domain.Interfaces
{
    public interface IFileRepo
    {
        IList<FileOfVersion> FindFileOfVersions(AppVersion ver);
        IList<byte[]> FindByteFileOfVersions(AppVersion ver);
        IDictionary<FileOfVersion, byte[]> FindFileOfVersionsWithData(AppVersion ver);
        byte[] GetFile(AppVersion version, FileOfVersion name);
        byte[] GetFile(string versionName, string versionNumber, string relative_file_path);
        bool Save(AppVersion ver, IDictionary<FileOfVersion, byte[]> file);
    }
}
