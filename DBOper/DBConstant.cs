using Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YZ.Utils;

namespace DataInput.DBOper
{
    public class DBUtil
    {
        protected MySqlConnection m_mysqlConn;
        protected const string m_connFormatStr = "server={0};uid={1};pwd={2};database={3};port={4};CharSet={5}";
        //protected const string m_create
        protected static DBUtil m_Instance;
        protected MySqlCommand m_myCommand;
        protected MySqlDataReader m_myReader;
        protected List<station> m_lstStation;
        protected bool m_bStationDirty = true;
        public static DBUtil GetInstance()
        {
            if(m_Instance == null)
            {
                m_Instance = new DBUtil();
                m_Instance.InitData();
                m_Instance.InitDB();
            }
            return m_Instance;
        }

        protected void InitData()
        {
            m_lstStation = new List<station>();
        }
        protected void InitDB()
        {
            //init varables
            if(m_mysqlConn == null)
            {
                string filePath = "Config/config.xml";
                string connStr = string.Format(m_connFormatStr,
                    XMLHelper.GetXmlAttribute(filePath, "//Config//DBParam", "server").Value,
                    XMLHelper.GetXmlAttribute(filePath, "//Config//DBParam", "uid").Value,
                    XMLHelper.GetXmlAttribute(filePath, "//Config/DBParam", "pwd").Value,
                    XMLHelper.GetXmlAttribute(filePath, "//Config/DBParam", "database").Value,
                    XMLHelper.GetXmlAttribute(filePath, "//Config/DBParam", "port").Value,
                    XMLHelper.GetXmlAttribute(filePath, "//Config/DBParam", "CharSet").Value);
                m_mysqlConn = new MySqlConnection(connStr);
            }
            try
            {
                if (m_mysqlConn.State == ConnectionState.Open)
                {
                    m_myCommand = new MySqlCommand();
                    m_myCommand.Connection = m_mysqlConn;
                }
                else m_mysqlConn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception: " + ex.Message);
            }
        }
        protected bool ExcuteNoQuery(string sql)
        {
            if(m_mysqlConn.IsPasswordExpired)
            {
                InitDB();
            }
            m_myCommand.CommandText = sql;
            m_myCommand.CommandType = System.Data.CommandType.Text;
            return m_myCommand.ExecuteNonQuery() > 0;
        }
        public bool AddNewVoyage(string ItemName, string Member, string TimeRange, string Power, double Length, double Width, int SurveyItems)
        {
            return true;
        }

        public IList<T> GetLstBySql<T>(string sql)where T:class
        {
            BeginTrans();
            IList<T> ret = new List<T>();
            m_myCommand.CommandText = sql;
            m_myCommand.CommandType = CommandType.Text;
            m_myReader = m_myCommand.ExecuteReader();
            ret = DataHelper.ConvertToLst<T>(m_myReader);
            EndTrans();
            return ret;
        }

        public IList<T> GetLstItems<T>()where T:class
        {   
            string sql = "select * from " + typeof(T).Name;
            return GetLstBySql<T>(sql);
        }

        public IList<T> GetLstItems<T>(string where)where T:class
        {
            string sql = "select * from " + typeof(T).Name + " where " + where;
            return GetLstBySql<T>(sql);
        }

        public T GetEntityByWhere<T>(string where) where T : class
        {
            T ret = default(T);
            BeginTrans();
            m_myCommand.CommandText = "select * from " + typeof(T).Name + (string.IsNullOrEmpty(where) ?
                "" : " where " + where);
            m_myCommand.CommandType = CommandType.Text;
            m_myReader = m_myCommand.ExecuteReader();
            if (m_myReader.Read())
            {
                ret = DataHelper.ConvertToEntity<T>(m_myReader);
            }
            EndTrans();
            return ret;
        }

        public T GetEntityBySql<T>(string sql) where T : class
        {
            BeginTrans();
            IList<T> ret;
            m_myCommand.CommandText = sql;
            m_myCommand.CommandType = CommandType.Text;
            m_myReader = m_myCommand.ExecuteReader();
            ret = DataHelper.ConvertToLst<T>(m_myReader);
            EndTrans();
            if(ret.Count == 0)
            {
                return default(T);
            }
            return ret[0];
        }

        public void BeginTrans()
        {
            if (m_mysqlConn.State == ConnectionState.Closed ||
                m_mysqlConn.State == ConnectionState.Broken)
            {
                m_mysqlConn.Open();
            }
            m_myCommand = new MySqlCommand();
            m_myCommand.Connection = m_mysqlConn;
        }

        public void EndTrans()
        {
            //m_mysqlConn.Close();
            if(m_myReader != null && !m_myReader.IsClosed)
                m_myReader.Close();
        }

        public bool CheckAndAdd<T>(string where, T ins)
        {
            try
            {
                if (CheckExist<T>(where))
                {
                    if (!Update<T>(ins))
                        return false;
                }
                else
                {
                    if (!SaveObj<T>(ins))
                        return false;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public bool CheckExist<T>(string where)
        {
            BeginTrans();
            bool ret = false;
            try
            {
                m_myCommand.CommandText = "select Id from " + typeof(T).Name + " where " + where;
                m_myCommand.CommandType = CommandType.Text;
                m_myReader = m_myCommand.ExecuteReader();
                if (m_myReader.Read())
                {
                    ret = true;
                }
                else
                    ret = false;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                EndTrans();
            }
            return ret;
        }

        public int GetLastId()
        {
            BeginTrans();
            int lastId = 0;
            m_myCommand.CommandText = "select @@IDENTITY";
            m_myCommand.CommandType = CommandType.Text;
            m_myReader = m_myCommand.ExecuteReader();
            if (m_myReader.Read())
            {
                lastId = m_myReader.GetInt32(0);
            }
            else
            {
                lastId = 0;
            }
            EndTrans();
            return lastId;
        }

        public bool SaveObj<T>(T ins)
        {
            BeginTrans();
            int ret = 0;
            try
            {
                m_myCommand.CommandText = DataHelper.GenInsertSql<T>(ins);
                m_myCommand.CommandType = CommandType.Text;
                ret = m_myCommand.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                ret = 0;
                throw ex;
            }
            finally
            {
                EndTrans();
            }
            return ret > 0;
        }
        public bool Update<T>(T ins)
        {
            BeginTrans();
            int ret = 0;
            try
            {
                m_myCommand.CommandText = DataHelper.GenUpdateSql<T>(ins);
                m_myCommand.CommandType = CommandType.Text;
                ret = m_myCommand.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                EndTrans();
            }
            return ret > 0;
        }

        public IList<station> GetStations()
        {
            IList<station> ret = null;
            if (m_bStationDirty)
            {
                m_lstStation.Clear();
                m_lstStation.AddRange(GetLstItems<station>());
                m_bStationDirty = false;
            }
            ret = m_lstStation;
            return ret;
        }

        public void SetStationDirty(bool param)
        {
            m_bStationDirty = param;
        }
    }
}
