using AppVersionControl.Domain.ApplicationVersion;
using System.Collections.Generic;

namespace AppVersionControl.Domain.Interfaces
{
    public interface IAssemblyAppRepo
    {
        IDictionary<FileOfVersion, byte[]> FindFileOfVersionsWithData();
        IList<byte[]> FindByteFileOfVersions();
        IList<FileOfVersion> FindFileOfVersions();
    }
}
