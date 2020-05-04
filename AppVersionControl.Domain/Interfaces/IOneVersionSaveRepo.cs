using AppVersionControl.Domain.ApplicationVersion;
using System.Collections.Generic;

namespace AppVersionControl.Domain.Interfaces
{
    public interface IOneVersionSaveRepo
    {
        bool SaveOneVersionApplication(AppVersion ver, IDictionary<FileOfVersion, byte[]> file);
    }
}
