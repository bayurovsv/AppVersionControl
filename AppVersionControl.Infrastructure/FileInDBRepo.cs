using System;
using System.Collections.Generic;
using System.IO;
using AppVersionControl.Domain.ApplicationVersion;
using AppVersionControl.Domain.Interfaces;
using System.Data;
using System.Linq;

namespace AppVersionControl.Infrastructure
{
    public class FileInDBRepo : IFileRepo
    {
        private static DB.SQLite.Logger logger = new DB.SQLite.Logger(AppDomain.CurrentDomain.BaseDirectory);
        private readonly string BDPath;//путь к базе данных
        private readonly string DirecroryPath;// путь к корневону каталогу при сохранении
        private DB.SQLite.SQLite mydb = null;
        private string sSql = string.Empty;
        public FileInDBRepo(string bd_path)
        {
            BDPath = bd_path ?? throw new ArgumentNullException(nameof(bd_path));
            DirecroryPath = bd_path ?? throw new ArgumentNullException(nameof(bd_path));
            DirecroryPath = string.Concat(DirecroryPath, "Versions");
        }
        #region IList 
        public IList<byte[]> FindByteFileOfVersions(AppVersion ver)
        {
            try
            {
                mydb = new DB.SQLite.SQLite(BDPath);
                sSql = "SELECT * FROM FilesContains WHERE Applications=" + "'" + ver.VersionName + "'";
                DataRow[] datarows = mydb.Select(sSql);
                if (datarows == null)// проверка что запрос что-то вернул
                {
                    logger.Add("Не удалось подключиться к базе данных");
                    throw new FilePathIsNullException();
                }
                string[] versss = (from vers in datarows orderby vers.ItemArray[0] descending select vers.ItemArray[1].ToString()).ToArray();//поиск наибольшей версии 
                VersionNumber Vers;
                VersionNumber.TryParse(versss[0], out Vers);
                List<byte[]> fileApp = new List<byte[]>();
                List<string> ListFile = new List<string>();
                foreach (var dr in from DataRow dr in datarows where Vers.ToString() == dr.ItemArray[1].ToString().Trim() select dr)// Заполнение списка мета данных файлов
                {
                        fileApp.Add((byte[])dr.ItemArray[4]);
                }

                return fileApp;
            }
            catch (VersionInFileSystemRepo.FilePathIsNullException ex)
            {
                logger.Add(ex);
                throw new FilePathIsNullException();
            }
        }
        public IList<FileOfVersion> FindFileOfVersions(AppVersion ver)
        {
            try
            {
                mydb = new DB.SQLite.SQLite(BDPath);
                sSql = "SELECT * FROM FilesContains WHERE Applications=" + "'" + ver.VersionName + "'";
                DataRow[] datarows = mydb.Select(sSql);
                if (datarows == null)
                {
                    logger.Add("Не удалось подключиться к базе данных");
                    throw new FilePathIsNullException();
                }
                string[] versss = (from vers in datarows orderby vers.ItemArray[0] descending select vers.ItemArray[1].ToString()).ToArray();//поиск наибольшей версии 
                VersionNumber Vers;
                VersionNumber.TryParse(versss[0], out Vers);
                List<FileOfVersion> fileApp = new List<FileOfVersion>();
                foreach (var dr in from DataRow dr in datarows where Vers.ToString() == dr.ItemArray[1].ToString().Trim() select dr)// Заполнение списка мета данных файлов
                {
                    FileOfVersion verss = new FileOfVersion(dr.ItemArray[0].ToString(),
                    (byte[])dr.ItemArray[4], File.GetCreationTime(dr.ItemArray[0].ToString()));
                    fileApp.Add(verss);

                }
                return fileApp;
            }
            catch (VersionInFileSystemRepo.FilePathIsNullException ex)
            {
                logger.Add(ex);
                throw new FilePathIsNullException();
            }
        }
        #endregion
        #region GetFile
        public byte[] GetFile(AppVersion version, FileOfVersion name)
        {
            try
            {
                return fileContain(DirecroryPath + "\\" + version.VersionName + "\\" + version.VersionNumber.ToString() + "\\" + name.FilePath);
            }
            catch
            {
                throw new FilePathIsNullException(); ;
            }
        }
        public byte[] GetFile(string versionName, string versionNumber, string relative_file_path)
        {
            try
            {
                return fileContain(DirecroryPath + "\\" + versionName + "\\" + versionNumber + "\\" + relative_file_path);
            }
            catch
            {
                throw new FilePathIsNullException(); ;
            }
        }
        #endregion
        public IDictionary<FileOfVersion, byte[]> FindFileOfVersionsWithData(AppVersion ver)// у всех файлов разный HashCode
        {
            IDictionary<FileOfVersion, byte[]> result = new Dictionary<FileOfVersion, byte[]>();
            try
            {
                IList<FileOfVersion> FileVersion = FindFileOfVersions(ver);
                IList<byte[]> ByteFile = FindByteFileOfVersions(ver);
                for (int i = 0; i < FileVersion.Count; i++)
                {
                    if (FileVersion[i].FileHash == HashCode(ByteFile[i]))
                        result.Add(FileVersion[i], ByteFile[i]);
                }
                return result;
            }
            catch (FileOrByteIsNullException)
            {
                throw new FileOrByteIsNullException();
            }
        }
        #region Save

        public bool Save(AppVersion ver, IDictionary<FileOfVersion, byte[]> file)
        {
            try
            {
                List<FileOfVersion> fileVersion = (from dir in file.Keys select dir).ToList();
                List<byte[]> fileContains = (from contains in file.Values select contains).ToList();
                for (int i = 0; i < fileVersion.Count; i++)
                {
                    if (fileVersion[i].FileHash == HashCode(fileContains[i]))
                    {
                        mydb = new DB.SQLite.SQLite(BDPath);
                        sSql = "INSERT INTO FilesContains (Applications,AppVersions,FilePath,DateCreateDB,Contains) VALUES(" + "'" + ver.VersionName + "'" + "," + "'" + ver.VersionNumber.ToString() + "'" + "," + "'" + fileVersion[i].FilePath + "'" + "," + "'" + DateTime.Now.ToString() + "'" + "," + "'" + fileContains[i] + "'" + ");";
                        mydb.Insert(sSql);
                        logger.Add("Запись добавлена!");
                        mydb = null;
                    }
                    else
                    {
                        throw new Exception("Метаданные не соответствуют содержимому файла!");
                    }
                }
                return true;
            }
            catch (SaveException ex)
            {
                logger.Add(ex);
                throw new SaveException();
            }
        }
        #endregion
        #region tools
        private static byte[] fileContain(string file_path) // получение содержимого файла
        {
            byte[] file_contain = null;
            try
            {
                using (FileStream fstream = File.OpenRead(file_path))
                {
                    file_contain = new byte[fstream.Length];
                    fstream.Read(file_contain, 0, file_contain.Length);
                }
                return file_contain;
            }
            catch (FilePathIsNullException ex)
            {
                logger.Add(ex);
                throw new FilePathIsNullException();
            }
        }
        private List<string> GetFiles_app(string nameApp, VersionNumber version)//получение списка всех файлов версии(путей к файлам)
        {
            List<string> ls = new List<string>();
            try
            {
                if (version != null)
                {
                    string z = Path.Combine(DirecroryPath, nameApp);
                    z = Path.Combine(z, version.ToString());
                    var dirs = Directory.GetFiles(z, "*", SearchOption.AllDirectories).Select(x => x.Replace(z, ""));
                    return new List<string>(dirs);
                }
                return ls;
            }
            catch (FilePathIsNullException ex)
            {
                logger.Add(ex);
                throw new FilePathIsNullException();
            }
        }
        private static string HashCode(byte[] text)//Hashcode
        {
            string hash;
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                hash = BitConverter.ToString(md5.ComputeHash(text)).Replace("-", string.Empty);
            }
            return hash;
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
