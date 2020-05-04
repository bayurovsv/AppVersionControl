using System;

namespace AppVersionControl.Domain.ApplicationVersion
{
    public class VersionNumber : IComparable, IComparable<VersionNumber>, IEquatable<VersionNumber>
    {
        /// <summary>Номер версии</summary>
        public int Number { get; } 

        /// <summary>Номер подверсии</summary>
        public int VersionBuildNumber { get; }

        public VersionNumber(int number, int versionbild)
        {
            if (number<= 0) throw new Exception(nameof(number));
            if (versionbild < 0) throw new Exception(nameof(versionbild));
            Number = number;
            VersionBuildNumber = versionbild;
        }
        public override string ToString()
        {
            return $"{Number}_b{VersionBuildNumber}";
        }

        public static bool TryParse(object obj, out VersionNumber result)
        {
            string[] ver;
            result = null;
            string versnum="";
            char[] num = obj.ToString().ToCharArray();
            foreach(char chr in num)
            {
                if(char.IsLetter(chr))
                {
                    versnum += '.';
                }
                if(char.IsDigit(chr))
                {
                    versnum += chr;
                }
            }
            ver = versnum.Split('.');
            result = new VersionNumber(Convert.ToInt32(ver[0]), Convert.ToInt32(ver[1]));
            return false;
        }
        #region Перегрузка операций сравнения 
        public static bool operator ==(VersionNumber number, VersionNumber otherNumber)
        {
            if (ReferenceEquals(number, null))
            {
                return ReferenceEquals(otherNumber, null);
            }
            return number.Equals(otherNumber);
        }

        public static bool operator !=(VersionNumber number, VersionNumber otherNumber)
        {
            return !(number == otherNumber);
        }

        public static bool operator <(VersionNumber number, VersionNumber otherNumber)
        {
            return (Compare(number, otherNumber) < 0);
        }

        public static bool operator >(VersionNumber number, VersionNumber otherNumber)
        {
            return (Compare(number, otherNumber) > 0);
        }
        
        public static bool operator <=(VersionNumber number, VersionNumber otherNumber)
        {
            return (Compare(number, otherNumber) <= 0);
        }

        public static bool operator >=(VersionNumber number, VersionNumber otherNumber)
        {
            return (Compare(number, otherNumber) >= 0);
        }

        #endregion

        #region IComapable
        public int CompareTo(VersionNumber other)
        {
            int res = Number.CompareTo(other.Number);
            if (res == 0)
            {
                return VersionBuildNumber.CompareTo(other.VersionBuildNumber);
            }
            return res;
        }

        public int CompareTo(object obj)
        {
            return CompareTo(obj as VersionNumber);
        }
        public static int Compare(VersionNumber number, VersionNumber otherNumber)
        {
            if (ReferenceEquals(number, otherNumber))
            {
                return 0;
            }
            if (ReferenceEquals(number, null))
            {
                return -1;
            }
            return number.CompareTo(otherNumber);
        }

        #endregion

        #region IEquatable

        public override bool Equals(object obj)
        {
            return Equals(obj as VersionNumber);
        }
        public override int GetHashCode()
        {
            string str = $"{Number}{VersionBuildNumber}";
            return str.GetHashCode();
        }

        public bool Equals(VersionNumber other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            return CompareTo(other) == 0;
        }
        #endregion

    }
}
