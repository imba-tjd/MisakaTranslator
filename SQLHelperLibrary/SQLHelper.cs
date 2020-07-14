// 本代码设计不好，生命周期混乱，而且保有状态就不能叫Helper了。
// 修改：CreateNewDatabase什么也不做。原本还将Open放到构造函数中的，但为了减少改动，现不再那样。
// 原修改：https://github.com/imba-tjd/MisakaTranslator/commit/4aaa46ad6325cc78e986e9830fd55bca22564c1f
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace SQLHelperLibrary
{
    public class SQLHelper
    {
        private readonly SqliteConnection _mDbConnection;//数据库连接
        private string _errorInfo;//最后一次错误信息

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="dataSource">数据库文件路径</param>
        public SQLHelper(string dataSource)
        {
            _mDbConnection = new SqliteConnection("Filename="+dataSource);
        }

        /// <summary>
        /// 专用于游戏库的初始化
        /// </summary>
        public SQLHelper() : this($"{Environment.CurrentDirectory}\\MisakaGameLibrary.sqlite") { }


        /// <summary>
        /// 创建一个新的sqlite数据库，后缀名.sqlite
        /// </summary>
        /// <param name="Filepath">数据库路径</param>
        public static void CreateNewDatabase(string Filepath)
        {

        }

        /// <summary>
        /// 执行一条非查询语句,失败会返回-1，可通过getLastError获取失败原因
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>返回影响的结果数</returns>
        public int ExecuteSql(string sql)
        {
            var command = new SqliteCommand(sql, _mDbConnection);
            try
            {
                _mDbConnection.Open();
                var res = command.ExecuteNonQuery();
                _mDbConnection.Close();
                return res;
            }
            catch (SqliteException ex)
            {
                _mDbConnection.Close();
                _errorInfo = ex.Message;
                return -1;
            }
        }

        /// <summary>
        /// 执行查询语句，返回单行结果（适用于执行查询可确定只有一条结果的）,失败返回null,可通过getLastError获取失败原因
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="columns">结果应包含的列数</param>
        /// <returns></returns>
        public List<string> ExecuteReader_OneLine(string sql, int columns)
        {
            var cmd = new SqliteCommand(sql, _mDbConnection);
            try
            {
                _mDbConnection.Open();
                var myReader = cmd.ExecuteReader();

                if (myReader.HasRows == false)
                {
                    _mDbConnection.Close();
                    return new List<string>();
                }
                
                var ret = new List<string>();
                while (myReader.Read())
                {
                    for (var i = 0; i < columns; i++)
                    {
                        ret.Add(myReader[i].ToString());
                    }
                }

                _mDbConnection.Close();
                return ret;
            }
            catch (SqliteException e)
            {
                _mDbConnection.Close();
                _errorInfo = e.Message;
                return null;
            }
        }

        /// <summary>
        /// 执行查询语句,返回结果,失败返回null,可通过getLastError获取失败原因
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="columns">结果应包含的列数</param>
        /// <returns></returns>
        public List<List<string>> ExecuteReader(string sql, int columns)
        {
            var cmd = new SqliteCommand(sql, _mDbConnection);
            try
            {
                _mDbConnection.Open();
                var myReader = cmd.ExecuteReader();

                if (myReader.HasRows == false)
                {
                    _mDbConnection.Close();
                    return new List<List<string>>();
                }

                
                var ret = new List<List<string>>();
                while (myReader.Read())
                {
                    var lst = new List<string>();
                    for (var i = 0; i < columns; i++)
                    {
                        lst.Add(myReader[i].ToString());
                    }
                    ret.Add(lst);
                    
                }

                _mDbConnection.Close();
                return ret;
            }
            catch (SqliteException e)
            {
                _mDbConnection.Close();
                _errorInfo = e.Message;
                return null;
            }
        }

        /// <summary>
        /// 获取最后一次失败原因
        /// </summary>
        /// <returns></returns>
        public string GetLastError() {
            return _errorInfo;
        }
    }
}
