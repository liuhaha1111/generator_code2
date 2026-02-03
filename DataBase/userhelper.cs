using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace WisdomGrowth.DataBase
{
    /// <summary>
    /// 用户管理类
    /// </summary>
    public class userhelper
    {
        public static readonly string DBName = "WisdomGrowthInfoDataBase.db3";
        //对SQLite数据库执行查询操作，返回DataSet 
        /// </summary>
        /// <param name="fileNames"></param>
        /// <param name="errorInfo"></param>
        public static DataTable Query(string SQLString)
        {
            string dataBaseFileName = string.Format("{0}\\{1}", Application.StartupPath + "\\database", DBName);
            string connectionString = @"Data Source=" + dataBaseFileName + "; Pooling=true;FailIfMissing=false;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                DataTable table = new DataTable();
                try
                {
                    connection.Open();
                    SQLiteDataAdapter command = new SQLiteDataAdapter(SQLString, connection);
                    command.Fill(table);
                }
                catch (SQLiteException ex)
                {
                    throw new Exception(ex.Message);
                }
                return table;
            }
        }
        /// <summary>
        //对SQLite数据库执行增删改操作，返回受影响的行数。 
        /// </summary>
        /// <param name="fileNames"></param>
        /// <param name="errorInfo"></param>
        public static int ExecuteNonQuery(string SQLString)
        {
            string dataBaseFileName = string.Format("{0}\\{1}", Application.StartupPath + "\\database", DBName);
            string connectionString = @"Data Source=" + dataBaseFileName + "; Pooling=true;FailIfMissing=false;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (SQLiteException E)
                    {
                        connection.Close();
                        throw new Exception(E.Message);
                    }
                }
            }
        }
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="fileNames"></param>
        /// <param name="errorInfo"></param>
        public static void ImportUserToDb(string userid, string username, string password)
        {
            string sql = string.Format(@"INSERT INTO  UserDatasTable ([Userid], [Username], [Password]) VALUES ('{0}', '{1}', '{2}')", userid, username, password);
            ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="fileNames"></param>
        /// <param name="errorInfo"></param>
        public static DataTable QueryUserToDb()
        {
            string sql = string.Format(@"select * from UserDatasTable");
            DataTable table = Query(sql);
            return table;
        }

        public static DataTable QueryUserToDb(string userid, string username)
        {
            string sql = string.Format(@"select * from UserDatasTable where Userid='{0}' and Username='{1}'", userid, username);
            DataTable table = Query(sql);
            return table;
        }
        public static DataTable QueryUserByIdToDb(string userid)
        {
            string sql = string.Format(@"select * from UserDatasTable where Userid='{0}'", userid);
            DataTable table = Query(sql);
            return table;
        }
        public static DataTable QueryUserByIdToDb(string userid, string password)
        {
            string sql = string.Format(@"select * from UserDatasTable where Userid='{0}' and Password='{1}'", userid, password);
            DataTable table = Query(sql);
            return table;
        }
    }
}

