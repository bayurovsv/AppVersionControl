using System;
using System.Data;
using System.Data.SQLite;

namespace DB.SQLite
{
    public class SQLite
    {
        string cs = "Data Source=:memory:";//
        private static Logger logger = new Logger(AppDomain.CurrentDomain.BaseDirectory);
        private readonly string BDPath;//= @"D:\AppVersionControl\WpfApp1\bin\Debug\DB\updatefile.db";//путь к базе данных
        public SQLite(string bd_path)
        {
            BDPath = bd_path ?? throw new ArgumentNullException(nameof(bd_path));
            BDPath = string.Concat(BDPath, @"\DB\updatefile.db");
        }
        public bool Insert(string sSql)
        {
            using (SQLiteConnection con = new SQLiteConnection(cs))
            {
                con.ConnectionString = @"Data Source=" + BDPath + ";New=False;Version=3";
                con.Open();
                SQLiteTransaction transaction = con.BeginTransaction();
                try
                {
                    using (SQLiteCommand sqlCommand = con.CreateCommand())
                    {
                        sqlCommand.CommandText = sSql;
                        sqlCommand.ExecuteNonQuery();
                    }
                    transaction.Commit();
                    con.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Add(ex);
                    throw new Exception();
                }
            }
        }
        public DataRow[] Select(string sSql)
        {
            DataRow[] datarows = null;
            SQLiteDataAdapter dataadapter = null;
            DataSet dataset = new DataSet();
            DataTable datatable = new DataTable();
            using (SQLiteConnection con = new SQLiteConnection(cs))
            {
                con.ConnectionString = @"Data Source=" + BDPath +
               ";New=False;Version=3";
                con.Open();
                SQLiteTransaction transaction = con.BeginTransaction();

                try
                {
                    using (SQLiteCommand sqlCommand = con.CreateCommand())
                    {
                        dataadapter = new SQLiteDataAdapter(sSql, con);
                        dataset.Reset();
                        dataadapter.Fill(dataset);
                        datatable = dataset.Tables[0];
                        datarows = datatable.Select();
                    }
                    transaction.Commit();
                    con.Close();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Add(ex);
                    throw new Exception();
                }
                return datarows;
            }
        }
    }
}
