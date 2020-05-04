using System;
using System.IO;
using System.Text;

namespace DB.SQLite
{
    public class Logger
    {
        public readonly string LOGPath;
        public Logger(string log_path)
        {
            LOGPath = log_path ?? throw new ArgumentNullException(nameof(log_path));
            LOGPath = Path.Combine(LOGPath, "Log\\log.txt");
        }
        #region ILogger
        public long Add(string message)
        {
            long id = AddToLog(message);
            return id;
        }
        public long Add(string message, Exception ex)
        {
            long id = AddToLog(message, ex);
            return id;
        }
        public long Add(Exception ex)
        {
            long id = AddToLog(ex);
            return id;
        }
        #endregion
        #region AddToLog
        private long AddToLog(string message)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(LOGPath, true, Encoding.Default))
                {
                    DateTime time = DateTime.Now;
                    sw.WriteLine(time + ": " + message);
                }
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        private long AddToLog(string message, Exception ex)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(LOGPath, true, Encoding.Default))
                {
                    DateTime time = DateTime.Now;
                    sw.WriteLine(time + ": " + message + "  (" + (ex) + ")");
                }
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        private long AddToLog(Exception ex)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(LOGPath, true, Encoding.Default))
                {
                    DateTime time = DateTime.Now;
                    sw.WriteLine(time + ": " + "  (" + (ex) + ")");
                }
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        #endregion
    }
}

