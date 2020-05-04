using AppVersionControl.Domain.ApplicationVersion;

namespace AppVersionControl.Domain.Interfaces
{
    public interface IVersionSaveRepo 
    {
        bool SaveApplication(AppVersion version);
    }
}