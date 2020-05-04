using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppVersionControl.Domain.ApplicationVersion
{
   public class AppFileSize : IComparable, IComparable<AppFileSize>, IEquatable<AppFileSize>
    {/// <summary>Размер файла</summary>
        public long FileSize { get; }
        public AppFileSize(long fileSize)
        {
            if (fileSize < 0) throw new AppFileSizeIsNullException();
            FileSize = fileSize;
        }
        public AppFileSize(byte fileSize)
        {
            if (fileSize < 0) throw new AppFileSizeIsNullException();
            FileSize = fileSize;
        }
        public AppFileSize(short fileSize)
        {
            if (fileSize < 0) throw new AppFileSizeIsNullException();
            FileSize = fileSize;
        }
        public AppFileSize(int fileSize)
        {
            if (fileSize < 0) throw new AppFileSizeIsNullException();
            FileSize = fileSize;
        }
        #region IComapable
        public int CompareTo(object obj)
        {
            return CompareTo(obj as AppFileSize);
        }

        public int CompareTo(AppFileSize other)
        {
            int res = FileSize.CompareTo(other.FileSize);
            if (res == 0) 
            {
                return FileSize.CompareTo(other.FileSize);
            }
            return res;
           
        }
        #endregion
        #region IEquatable
        public bool Equals(AppFileSize other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            return CompareTo(other)==0;
        }
        #endregion
        #region Domain exception
        public class AppFileSizeIsNullException : Exception
        {
            public AppFileSizeIsNullException()
                   : base("Вес файла не найден")
            {
            }
        }
        #endregion
    }
}
