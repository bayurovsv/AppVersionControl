using AppVersionControl.Domain.ApplicationVersion;
using AppVersionControl.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace AppVersionControl.Infrastructure
{
    public class AssemblyAppRepo:IAssemblyAppRepo
    {
        private readonly string RootPath;
        public AssemblyAppRepo(string root_path)
        {
            RootPath = root_path ?? throw new ArgumentNullException(nameof(root_path));
        }
        public IDictionary<FileOfVersion, byte[]> FindFileOfVersionsWithData()
        {
            IDictionary<FileOfVersion, byte[]> result = new Dictionary<FileOfVersion, byte[]>();
            try
            {
                IList<FileOfVersion> FileVersion = FindFileOfVersions();
                IList<byte[]> ByteFile = FindByteFileOfVersions();
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
        public IList<byte[]> FindByteFileOfVersions()
        {
            try
            {
                List<byte[]> fileApp = new List<byte[]>();
                List<string> ListFile = new List<string>();
                ListFile = GetFiles_app(RootPath);
                for (int x = 0; x < ListFile.Count; x++)
                {
                    fileApp.Add(fileContain(RootPath + "\\" + ListFile[x]));
                }
                return fileApp;
            }
            catch (FilePathIsNullException ex)
            {
                throw new FilePathIsNullException();
            }
        }
        public IList<FileOfVersion> FindFileOfVersions()
        {
            try
            {
                List<FileOfVersion> fileApp = new List<FileOfVersion>();
                List<string> ListFile = new List<string>();
                ListFile = GetFiles_app(RootPath);
                for (int x = 0; x < ListFile.Count; x++)
                {
                    FileOfVersion verss = new FileOfVersion(ListFile[x], fileContain(RootPath + ListFile[x]), File.GetCreationTime(RootPath + ListFile[x]));
                    fileApp.Add(verss);
                }
                return fileApp;
            }
            catch (FilePathIsNullException ex)
            {
                throw new FilePathIsNullException();
            }
        }

        #region Tools
        private List<string> GetFiles_app(string z)
        {
            List<string> ls = new List<string>();
            try
            {
                var dirs = Directory.GetFiles(z, "*", SearchOption.AllDirectories).Select(x => x.Replace(z, ""));
                return new List<string>(dirs);
            }
            catch (FilePathIsNullException ex)
            {

                throw new FilePathIsNullException();
            }
        }
        private byte[] fileContain(string file_path)
        {
            byte[] file_contain = null;
            try
            {
                using (FileStream fstream = File.OpenRead(file_path))
                {
                    file_contain = new byte[fstream.Length];
                    fstream.Read(file_contain, 0, file_contain.Length);
                    fstream.Close();
                }
            }
            catch (VersionInFileSystemRepo.FilePathIsNullException ex)
            {
                throw new NotImplementedException();
            }
            return file_contain;
        }
        private string HashCode(byte[] text)//HashCode
        {
            string hash;
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                hash = BitConverter.ToString(md5.ComputeHash(text)).Replace("-", string.Empty);
            }
            return hash;
        }
        #endregion
    }
}
