using AppVersionControl.Domain.ApplicationVersion;
using AppVersionControl.Domain.Iterfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AppVersionControl.Infrastructure
{
    public class VersionInDBRepo : IVersionRepo
    {
        private static DB.SQLite.Logger logger = new DB.SQLite.Logger(AppDomain.CurrentDomain.BaseDirectory);
        private readonly string BDPath;
        private DB.SQLite.SQLite mydb = null;
        private string sSql = string.Empty;
        public VersionInDBRepo(string bd_path)
        {
            BDPath = bd_path ?? throw new ArgumentNullException(nameof(bd_path));
        }
        #region Find
        public AppVersion Find(string version_name)
        {
            try
            {
                mydb = new DB.SQLite.SQLite(BDPath);
                sSql = "SELECT * FROM FilesVersions WHERE Applications=" + "'" + version_name + "'";
                logger.Add("Версия найдена");
                return Result(sSql);
            }
            catch (FilePathIsNullException ex)
            {
                logger.Add(ex);
                throw new FilePathIsNullException();
            }
        }
        public AppVersion Find(AppVersion app)
        {
            try
            {
                mydb = new DB.SQLite.SQLite(BDPath);
                sSql = "SELECT * FROM FilesVersions WHERE Applications=" + "'" + app.VersionName + "'";
                logger.Add("Версия найдена");
                return Result(sSql);
            }
            catch (FilePathIsNullException ex)
            {
                logger.Add(ex);
                throw new FilePathIsNullException();
            }
        }
        public AppVersion Find(string version_name, VersionNumber versionNumber)
        {
            try
            {
                mydb = new DB.SQLite.SQLite(BDPath);
                sSql = "SELECT * FROM FilesVersions WHERE Applications=" + "'" + version_name + "'" + "and AppVersions=" + "'" + versionNumber.ToString() + "'";
                logger.Add("Версия найдена");
                return Result(sSql);
            }
            catch (FilePathIsNullException ex)
            {
                logger.Add(ex);
                throw new FilePathIsNullException();
            }
        }
        #endregion
        #region FindAll
        public IList<AppVersion> FindAll(string version_name)
        {
            try
            {
                mydb = new DB.SQLite.SQLite(BDPath);
                sSql = "SELECT * FROM FilesVersions WHERE Applications=" + "'" + version_name + "'";
                return AppVersions(sSql);
            }
            catch (FilePathIsNullException ex)
            {
                logger.Add(ex);
                throw new FilePathIsNullException();
            }
        }
        public IList<AppVersion> FindAll()
        {
            try
            {
                mydb = new DB.SQLite.SQLite(BDPath);
                sSql = "SELECT * FROM FilesVersions";
                return AppVersions(sSql);
            }
            catch (FilePathIsNullException ex)
            {
                logger.Add(ex);
                throw new FilePathIsNullException();
            }
        }
        #endregion
        #region Tools
        private AppVersion Result(string sSql)
        {
            string appVersion = "";
            string app_name = "";
            DataRow[] datarows = mydb.Select(sSql);
            if (datarows == null)
            {
                logger.Add("Не удалось подключиться к базе данных");
                throw new FilePathIsNullException();
            }
            string[] versss = (from vers in datarows orderby vers.ItemArray[0] descending select vers.ItemArray[0].ToString()).ToArray();//поиск наибольшей версии 
            VersionNumber Vers;
            VersionNumber.TryParse(versss[0], out Vers);
            List<FileOfVersion> fileApp = new List<FileOfVersion>();
            foreach (var dr in from DataRow dr in datarows where Vers.ToString() == dr.ItemArray[0].ToString().Trim() select dr)
            {
                app_name = dr.ItemArray[6].ToString();
                appVersion = dr.ItemArray[0].ToString().Trim();
                FileOfVersion verss = new FileOfVersion(dr.ItemArray[1].ToString().Trim(), Convert.ToDateTime(dr.ItemArray[2].ToString().Trim()), new AppFileSize(Convert.ToInt32(dr.ItemArray[3].ToString().Trim())), dr.ItemArray[4].ToString().Trim());
                fileApp.Add(verss);
            }
            VersionNumber version;
            VersionNumber.TryParse(appVersion, out version);
            AppVersion res = new AppVersion(app_name, version, fileApp);
            return res;
        }
        private List<AppVersion> AppVersions(string sSql)
        {
            List<AppVersion> AppVersions = new List<AppVersion>();
            DataRow[] datarows = mydb.Select(sSql);
            if (datarows == null)
            {
                logger.Add("Не удалось подключиться к базе данных");
                throw new FilePathIsNullException();
            }
            foreach (DataRow dr in datarows)
            {
                VersionNumber VerNum;
                VersionNumber.TryParse(dr[0].ToString().Trim(), out VerNum);
                AppVersion AppVer = new AppVersion(dr.ItemArray[6].ToString(), VerNum);
                if (!AppVersions.Contains(AppVer))
                    AppVersions.Add(AppVer);
            }
            return AppVersions;
        }
        #endregion
        #region Domain exception
        public class FilePathIsNullException : Exception
        {
            public FilePathIsNullException()
                 : base("Версии не найдены")
            {
            }
        }
        #endregion
    }
}
