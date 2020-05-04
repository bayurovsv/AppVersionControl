using System;
using System.Collections.Generic;
using System.IO;

namespace AppVersionControl.Domain.ApplicationVersion
{
    public class FileOfVersion : IEquatable<FileOfVersion>
    {
        /// <summary>Имя файла</summary>
        public string FileName { get; }
        /// <summary>Хеш сумма файла</summary>
        public string FileHash { get; }
        /// <summary>Дата создания</summary>
        DateTime CreationDate { get; }
        /// <summary>Размер файла в байтах</summary>
        public AppFileSize FileSize { get; }//метод
        /// <summary>Расширение файла</summary>
        public string FileExtension { get; }
        /// <summary>Путь к файлу</summary>
        public string FilePath { get; }

        //public enum FILE_PATH_TYPE { FTP, FULL_FILE_PATH, FILE_PATH };
       
        public FileOfVersion(string hash_file, DateTime date_create, AppFileSize size_file, string file_full_path)
        {
            if (String.IsNullOrWhiteSpace(file_full_path)) throw new FilePathIsNullException();
            FileName = Path.GetFileNameWithoutExtension(file_full_path);
            FileHash = hash_file;
            CreationDate = date_create;
            FileSize = size_file;
            FileExtension = Path.GetExtension(file_full_path);
            FilePath = file_full_path;
        }

        public FileOfVersion(string full_path,byte[] file_contains, DateTime createDate)
        {
              if (String.IsNullOrWhiteSpace(full_path)) throw new FilePathIsNullException();
              FileName = Path.GetFileNameWithoutExtension(full_path);
              FileHash = HashCode(file_contains);
              CreationDate = createDate;
              AppFileSize fileSize = new AppFileSize(file_contains.Length);
              FileSize = fileSize;
              FileExtension = Path.GetExtension(full_path);
              FilePath = full_path;
        }
        private static string HashCode(byte[] text)
        {
            string hash;
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                hash = BitConverter.ToString(md5.ComputeHash(text)).Replace("-", string.Empty);
            }
            return hash;
        }
        #region IEquatable
        public override bool Equals(object obj)
        {
            return Equals(obj as FileOfVersion);
        }

        public bool Equals(FileOfVersion other)
        {
            if (other == null) return false;
            return FileName == other.FileName &&
                FileHash == other.FileHash &&
                CreationDate == other.CreationDate &&
                FileSize == other.FileSize &&
                FileExtension == other.FileExtension &&
                FilePath == other.FilePath;
        }

        public override int GetHashCode()
        {
            var hashCode = -1754082123;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FileName);
            hashCode = hashCode * -1521134295 + FileSize.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FileExtension);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FilePath);
            return hashCode;
        }
        #endregion
        #region Domain exception
        public class FileNameIsNullException : Exception
        {
            public FileNameIsNullException()
                   : base("Имя файла не найдено")
            {
            }
        }
        public class HashFileIsNullException : Exception
        {
            public HashFileIsNullException()
                : base("Хеш сумма не найдена")
            {
            }

        }
        public class FileIsNullException : Exception
        {
            public FileIsNullException() : base("Файл не найден")
            {
            }
        }
        public class FileSizeIsNullException : Exception
        {
            public FileSizeIsNullException()
                 : base("Размер файла равен 0")
            {
            }
        }
        public class FileExtIsNullException : Exception
        {
            public FileExtIsNullException()
                 : base("Расширение файла не определено")
            {
            }
        }
        public class FilePathIsNullException : Exception
        {
            public FilePathIsNullException()
                 : base("Путь к файлу не найден")
            {
            }
        }
        #endregion
    }
}
