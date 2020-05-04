using AppVersionControl.Domain.ApplicationVersion;
using AppVersionControl.Domain.Interfaces;
using System;
using Newtonsoft.Json;
using System.IO;

namespace AppVersionControl.Infrastructure
{
    public class VersionSaveInFileSystemRepo : IVersionSaveRepo
    {
        private readonly string RootPath;
        public VersionSaveInFileSystemRepo(string root_path)
        {
            RootPath = root_path ?? throw new ArgumentNullException(nameof(root_path));
            RootPath = Path.Combine(RootPath, "FilesApps");
        }
        public bool SaveApplication(AppVersion version) // Сохранение мета данных в xml 
        {
            try
            {
                string json = JsonConvert.SerializeObject(version, Formatting.Indented);
                File.WriteAllText(RootPath + "\\" + version.VersionName + "-" + version.VersionNumber.ToString() + ".xml", json);
                return true;
            }
            catch (SaveApplicationException ex) 
            {
                throw new SaveApplicationException();
            }
        }
        #region Domain exception
        public class SaveApplicationException : Exception
        {
            public SaveApplicationException()
                 : base("Ошибка сохранения")
            {
            }
        }
        #endregion
    }
}
