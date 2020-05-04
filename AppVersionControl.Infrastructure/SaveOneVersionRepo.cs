using AppVersionControl.Domain.ApplicationVersion;
using AppVersionControl.Domain.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace AppVersionControl.Infrastructure
{
    public class SaveOneVersionRepo : IOneVersionSaveRepo
    {
        private readonly string RootPath;
        public SaveOneVersionRepo(string root_path)
        {
            RootPath = root_path ?? throw new ArgumentNullException(nameof(root_path));
            RootPath = Path.Combine(RootPath, "Versions");
        }

        public bool SaveOneVersionApplication(AppVersion version, IDictionary<FileOfVersion, byte[]> file)
        {
            try
            {
                Directory.CreateDirectory(RootPath + "\\" + version.VersionName);
                List<FileOfVersion> fileVersion = (from dir in file.Keys select dir).ToList();
                List<byte[]> fileContains = (from contains in file.Values select contains).ToList();
                for (int i = 0; i < fileVersion.Count; i++)
                {
                    if (fileVersion[i].FileHash == HashCode(fileContains[i]))
                    {
                        Directory.CreateDirectory(RootPath + "\\" + version.VersionName + "\\" + Path.GetDirectoryName(fileVersion[i].FilePath));
                        using (FileStream fileStream = new FileStream(RootPath + "\\" + version.VersionName + "\\" + fileVersion[i].FilePath, FileMode.Create))
                        {
                            fileStream.Write(file[fileVersion[i]], 0, fileContains[i].Length);
                        }
                    }
                    else
                    {
                        throw new Exception("Метаданные не соответствуют содержимому файла!");
                    }
                }
                SaveApplication(version, RootPath + "\\" + version.VersionName);
                return true;
            }
            catch (SaveException)
            {
                throw new SaveException();
            }
        }
        #region Tools
        private static string HashCode(byte[] text)//HashCode
        {
            string hash;
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                hash = BitConverter.ToString(md5.ComputeHash(text)).Replace("-", string.Empty);
            }
            return hash;
        }
        private bool SaveApplication(AppVersion version, string path) // Сохранение мета данных в xml 
        {
            try
            {
                string json = JsonConvert.SerializeObject(version, Formatting.Indented);
                File.WriteAllText(RootPath + "\\" + version.VersionName + "\\" + version.VersionNumber.ToString() + ".xml", json);
                return true;
            }
            catch (SaveApplicationException ex)
            {
                throw new SaveApplicationException();
            }
        }
        #endregion
        #region Domain exception
        public class SaveApplicationException : Exception
        {
            public SaveApplicationException()
                 : base("Ошибка сохранения дынных о приложении")
            {
            }
        }
        #endregion
    }
}
