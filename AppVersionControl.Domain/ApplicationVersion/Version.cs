using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AppVersionControl.Domain.ApplicationVersion
{
    public class AppVersion : IEquatable<AppVersion>
    {
        /// <summary>Имя приложения.</summary>
        public string VersionName { get; }
        /// <summary>Номер версии(сборки)</summary>
        public VersionNumber VersionNumber { get; }
        /// <summary>Общий размер версии в байтах.</summary>
        private List<FileOfVersion> _files = new List<FileOfVersion>();
        /// <summary>Список всех файлов версии.</summary>
        public ReadOnlyCollection<FileOfVersion> Files => _files.AsReadOnly();
        public AppVersion(string app_name, VersionNumber number)
        {
            if (String.IsNullOrWhiteSpace(app_name)) throw new ApplicationNameIsNullException();
            if (number ==null) throw new Exception(nameof(number));
            VersionName = app_name;
            VersionNumber = number;
        }

        public AppVersion(string app_name, VersionNumber number, IList<FileOfVersion> files)
        {
            if (String.IsNullOrWhiteSpace(app_name)) throw new ApplicationNameIsNullException();
            if (number == null) throw new Exception(nameof(number));
            if (files == null ||  files.Count == 0) throw new ApplicationNotFilesException();
            VersionName = app_name;
            VersionNumber = number;
            foreach (FileOfVersion file in files)
            {
                AddFile(file);
            }
        }
        public AppVersion(string app_name)
        {
            if (String.IsNullOrWhiteSpace(app_name)) throw new ApplicationNameIsNullException();
           
            VersionName = app_name;
            VersionNumber = new VersionNumber(1,0);
        }
        public AppVersion(string app_name,IList<FileOfVersion> files)
        {
            if (String.IsNullOrWhiteSpace(app_name)) throw new ApplicationNameIsNullException();
            if (files == null || files.Count == 0) throw new ApplicationNotFilesException();
            VersionName = app_name;
            VersionNumber = new VersionNumber(1, 0);
            foreach (FileOfVersion file in files)
            {
                AddFile(file);
            }
        }
        public void AddFile(FileOfVersion file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            if (_files.Contains(file)) throw new Exception("Такой файл уже добавлен.");
            _files.Add(file);
        }

        #region IEquatable
        /// <summary> метод вычисления хеш суммы файлов приложения</summary>
        public override int GetHashCode()
        {
            string str = $"{VersionName}{VersionNumber}";
            return str.GetHashCode();
        }
        /// <summary>Метод сравнения файлов приложения </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as AppVersion);
        }
        /// <summary>метод сравнения версий приложения</summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(AppVersion other)
        {
            if (other == null) return false;
            return VersionName == other.VersionName && VersionNumber == other.VersionNumber;
        }
        #endregion

        #region Domain exception
        public class ApplicationSizeIsNullException : Exception
        {
            public ApplicationSizeIsNullException()
                   : base("Это пустая папка(вес приложения равен 0)")
            {
            }
        }
        public class ApplicationNameIsNullException : Exception
        {
            public ApplicationNameIsNullException()
                : base("Имя приложения в версии не указано.")
            {
            }

        }
        public class ApplicationNotFilesException : Exception
        {
            public ApplicationNotFilesException()
                 : base("Файлы приложения не найдены.")
            {
            }
        }
        #endregion
    }
}

