﻿namespace YZ.Utils
{
    using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
    using System.Text;
    /// <summary>
    /// 数据辅助类,提供从 DataTable 到 TEntity 的转换
    /// </summary>
    public class DataHelper
    {
        public static IList<TEntity> ConvertDataTableToObjects<TEntity>( DataTable dt )
        {
            if ( dt == null )
            {
                return null;
            }
            IList<TEntity> list = new List<TEntity>();
            foreach ( DataRow row in dt.Rows )
            {
                list.Add( ConvertRowToObject<TEntity>( row ) );
            }
            return list;
        }

        public static TEntity ConvertRowToObject<TEntity>( DataRow row )
        {
            if ( row == null )
            {
                return default( TEntity );
            }
            Type objType = typeof( TEntity );
            return ( TEntity )ConvertRowToObject( objType , row );
        }

        public static string GenUpdateSql<T>(T ins)
        {
            StringBuilder sb = new StringBuilder();
            Type obj = typeof(T);
            sb.Append("update ").Append(obj.Name).Append(" set ");
            PropertyInfo[] pInfos = obj.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            object id = obj.InvokeMember("id",
                    BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance
                    | BindingFlags.IgnoreCase, null, ins, null);
            string whsql = " where id=" + (int)id;
            foreach (var item in pInfos)
            {
                if (item.Name.ToLower() == "id")continue;
                sb.Append(item.Name).Append("=");

                object value = obj.InvokeMember(item.Name,
                    BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance
                    | BindingFlags.IgnoreCase, null, ins, null);
                sb.Append(ValueToDbStr(item.PropertyType, value)).Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(whsql);
            return sb.ToString();
        }

        public static string GenInsertSql<T>(T ins)
        {
            StringBuilder sb = new StringBuilder();
            Type obj = typeof(T);
            sb.Append("insert into ").Append(obj.Name).Append("(");
            PropertyInfo[] pInfos = obj.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            StringBuilder sbValues = new StringBuilder();
            foreach(var item in pInfos)
            {
                if (item.Name.ToLower() == "id") continue;
                sb.Append(item.Name).Append(",");
                
                object value = obj.InvokeMember(item.Name, 
                    BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance 
                    | BindingFlags.IgnoreCase, null, ins, null);
                sbValues.Append(ValueToDbStr(item.PropertyType, value)).Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sbValues.Remove(sbValues.Length - 1,1);
            sb.Append(") values(").Append(sbValues.ToString()).Append(")");
            return sb.ToString();
        }

        protected static string ValueToDbStr(Type dataType, object value)
        {
            if(dataType == typeof(string))
            {
                return "'" + (value == null ? "" : value.ToString()) + "'";
            }
            if(dataType == typeof(UInt32) || dataType == typeof(double) || dataType == typeof(int))
            {
                return value.ToString();
            }
            if(dataType == typeof(DateTime))
            {
                return "'" + (value==null ? "1970-01-01 00:00:00" :
                    ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss")) + "'";
            }
            TextLogger.LogError("not found datatype:" + dataType);
            return "";
        }

        /// <summary>
        /// 转换DataRow到实体对象
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public static object ConvertRowToObject( Type objType , DataRow row )
        {
            if ( row == null )
            {
                return null;
            }
            DataTable table = row.Table;
            object target = Activator.CreateInstance( objType );
            foreach ( DataColumn column in table.Columns )
            {
                PropertyInfo property = objType.GetProperty( column.ColumnName , BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase );
                if ( property == null )
                {
                    //throw new PropertyNotFoundException( column.ColumnName );
                    continue;
                }
                Type propertyType = property.PropertyType;
                object obj3 = null;
                bool flag = true;
                try
                {
                    obj3 = TypeHelper.ChangeType( propertyType , row[ column.ColumnName ] );
                }
                catch
                {
                    flag = false;
                }
                if ( flag )
                {
                    object[ ] args = new object[ ] { obj3 };
                    objType.InvokeMember( column.ColumnName , BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase , null , target , args );
                }
            }
            return target;
        }

        public static TEntity ConvertToEntity<TEntity>(MySqlDataReader reader) where TEntity:class
        {
            Type objType = typeof(TEntity);
            object target = Activator.CreateInstance(objType);
            for(int i = 0; i < reader.FieldCount; ++i)
            {
                PropertyInfo property = objType.GetProperty(reader.GetName(i),
                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (property == null)
                {
                    continue;
                }
                Type propertyType = property.PropertyType;
                object obj3 = null;
                bool flag = true;
                try
                {
                    obj3 = TypeHelper.ChangeType(propertyType, reader.GetValue(i));
                }
                catch (Exception ex)
                {
                    flag = false;
                    string msg = ex.Message;
                }
                if (flag)
                {
                    object[] args = new object[] { obj3 };
                    objType.InvokeMember(reader.GetName(i), BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase, null, target, args);
                }
            }
            return target as TEntity;
        }

        public static IList<TEntity> ConvertToLst<TEntity>(MySqlDataReader reader)where TEntity:class
        {
            IList<TEntity> list = new List<TEntity>();
            Type objType = typeof(TEntity);
            while(reader.Read())
            {  
                list.Add(ConvertToEntity<TEntity>(reader));
            } 
            return list;
        }

        /// <summary>
        /// 从SQL命令中提取参数
        /// </summary>
        /// <param name="sqlStatement"></param>
        /// <param name="paraPrefix"></param>
        /// <returns></returns>
        public static IList<string> DistillCommandParameter( string sqlStatement , string paraPrefix )
        {
            sqlStatement = sqlStatement + " ";
            IList<string> paraList = new List<string>();
            DoDistill( sqlStatement , paraList , paraPrefix );
            if ( paraList.Count > 0 )
            {
                string item = paraList[ paraList.Count - 1 ].Trim();
                if ( item.EndsWith( "\"" ) )
                {
                    item.TrimEnd( new char[ ] { '"' } );
                    paraList.RemoveAt( paraList.Count - 1 );
                    paraList.Add( item );
                }
            }
            return paraList;
        }
        /// <summary>
        /// 提取参数
        /// </summary>
        /// <param name="sqlStatement"></param>
        /// <param name="paraList"></param>
        /// <param name="paraPrefix"></param>
        private static void DoDistill( string sqlStatement , IList<string> paraList , string paraPrefix )
        {
            sqlStatement.TrimStart( new char[ ] { ' ' } );
            int index = sqlStatement.IndexOf( paraPrefix );
            if ( index >= 0 )
            {
                int startIndex = sqlStatement.IndexOf( " " , index );
                int length = startIndex - index;
                string str = sqlStatement.Substring( index , length );
                paraList.Add( str.Replace( paraPrefix , "" ) );
                DoDistill( sqlStatement.Substring( startIndex ) , paraList , paraPrefix );
            }
        }
        /// <summary>
        /// 填充命令参数值
        /// </summary>
        /// <param name="command"></param>
        /// <param name="entityOrRow"></param>
        public static void FillCommandParameterValue( IDbCommand command , object entityOrRow )
        {
            foreach ( IDbDataParameter parameter in command.Parameters )
            {
                parameter.Value = GetColumnValue( entityOrRow , parameter.SourceColumn );
                if ( parameter.Value == null )
                {
                    parameter.Value = DBNull.Value;
                }
            }
        }
        /// <summary>
        /// 获取列值
        /// </summary>
        /// <param name="entityOrRow"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static object GetColumnValue( object entityOrRow , string columnName )
        {
            DataRow row = entityOrRow as DataRow;
            if ( row != null )
            {
                return row[ columnName ];
            }
            return entityOrRow.GetType().InvokeMember( columnName , BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase , null , entityOrRow , null );
        }
        /// <summary>
        /// 获取安全的数据值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static object GetSafeDbValue( object val )
        {
            if ( val != null )
            {
                return val;
            }
            return DBNull.Value;
        }
        /// <summary>
        /// 刷新实体属性
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="row"></param>
        public static void RefreshEntityFields( object entity , DataRow row )
        {
            DataTable table = row.Table;
            IList<string> refreshFields = new List<string>();
            foreach ( DataColumn column in table.Columns )
            {
                refreshFields.Add( column.ColumnName );
            }
            RefreshEntityFields( entity , row , refreshFields );
        }

        /// <summary>
        /// 刷新实体属性
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="row"></param>
        /// <param name="refreshFields"></param>
        public static void RefreshEntityFields( object entity , DataRow row , IList<string> refreshFields )
        {
            Type type = entity.GetType();
            foreach ( string str in refreshFields )
            {
                string name = str;
                PropertyInfo property = type.GetProperty( name , BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase );
                if ( property == null )
                {
                    //throw new PropertyNotFoundException( name );
                    continue;
                }
                Type propertyType = property.PropertyType;
                object obj2 = null;
                bool flag = true;
                try
                {
                    obj2 = TypeHelper.ChangeType( propertyType , row[ name ] );
                }
                catch
                {
                    flag = false;
                }
                if ( flag )
                {
                    object[ ] args = new object[ ] { obj2 };
                    type.InvokeMember( name , BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase , null , entity , args );
                }
            }
        }

        /// <summary>
        /// 属性未找到异常类
        /// </summary>
        public class PropertyNotFoundException : UCException
        {
            private string targetPropertyName;
            /// <summary>
            /// 属性名称
            /// </summary>
            public string TargetPropertyName
            {
                get
                {
                    return this.targetPropertyName;
                }
                set
                {
                    this.targetPropertyName = value;
                }
            }
            /// <summary>
            /// 属性未找到异常类构造函数
            /// </summary>
            public PropertyNotFoundException( )
            {
            }
            /// <summary>
            /// 属性未找到异常类构造函数
            /// </summary>
            /// <param name="propertyName"></param>
            public PropertyNotFoundException( string propertyName )
                : base( string.Format( "The property named '{0}' not found in Entity definition." , propertyName ) )
            {
                this.targetPropertyName = propertyName;
            }
        }
    }
}

