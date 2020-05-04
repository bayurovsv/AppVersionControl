using AppVersionControl.Domain.ApplicationVersion;
using System.Collections.Generic;

namespace AppVersionControl.Domain.Iterfaces
{
    public interface IVersionRepo
    {
        AppVersion Find(string version_name);
        AppVersion Find(AppVersion app);
        AppVersion Find(string version_name, VersionNumber versionNumber);
        IList<AppVersion> FindAll();
        IList<AppVersion> FindAll(string version_name);
    }
}
