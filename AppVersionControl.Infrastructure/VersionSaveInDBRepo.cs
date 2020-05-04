using AppVersionControl.Domain.ApplicationVersion;
using AppVersionControl.Domain.Interfaces;
using AppVersionControl.Domain.Iterfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace AppVersionControl.Infrastructure
{
    public class VersionSaveInDBRepo : IVersionSaveRepo
    {
        private static DB.SQLite.Logger logger = new DB.SQLite.Logger(AppDomain.CurrentDomain.BaseDirectory);
        private readonly string BDPath;
        private DB.SQLite.SQLite mydb = null;
        private string sSql = string.Empty;
        public VersionSaveInDBRepo(string bd_path)
        {
            BDPath = bd_path ?? throw new ArgumentNullException(nameof(bd_path));
        }
        public bool SaveApplication(AppVersion version)//Сохранение метаданных
        {
            try
            {
                mydb = new DB.SQLite.SQLite(BDPath);
                IVersionRepo repo = new VersionInDBRepo(AppDomain.CurrentDomain.BaseDirectory);
                IList<AppVersion> apps = repo.FindAll(version.VersionName);
                if (apps.Count == 0)
                {
                    sSql = "INSERT INTO Applications(ApplicationName) VALUES(" + "'" + version.VersionName + "'" + ")";
                    mydb.Insert(sSql);
                }
                for (int i = 0; i < version.Files.Count; i++)
                {
                    mydb = new DB.SQLite.SQLite(BDPath);
                    sSql = "INSERT INTO FilesVersions(AppVersions,FileHash,CreationDate,FileSize,FilePath,DateCreateDB,Applications)" +
                    "VALUES(" + "'" + version.VersionNumber.ToString() + "'" + "," + "'" + version.Files[i].FileHash + "'" + "," + "'" + DateTime.Now.ToString()
                    + "'" + "," + "'" + version.Files[i].FileSize.FileSize + "'" + "," + "'" + version.Files[i].FilePath + "'" + "," + "'" + DateTime.Now.ToString()
                    + "'" + "," + "'" + version.VersionName + "'" + ");";
                    mydb.Insert(sSql);
                }
                mydb = null;
                logger.Add("Запись добавлена!");
                return true;
            }
            catch (SaveException ex)
            {
                logger.Add(ex);
                throw new SaveException();
            }
        }
    }
}
