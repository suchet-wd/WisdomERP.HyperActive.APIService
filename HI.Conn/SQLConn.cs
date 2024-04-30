using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml;

namespace HI.Conn
{
     public static class SQLConn
    {
        public static System.Data.SqlClient.SqlCommand Cmd;
        public static System.Data.SqlClient.SqlConnection Cnn;
        public static System.Data.SqlClient.SqlTransaction Tran;
        public static System.Data.SqlClient.SqlCommand Cmd2;
        public static System.Data.SqlClient.SqlConnection Cnn2;
        public static System.Data.SqlClient.SqlTransaction Tran2;
        public static bool WriteLog;

        public static string _ConnString;
        #region " CONNECTTION "

        public static void SqlConnectionOpen()
        {
            if (SQLConn.Cnn == null) { SQLConn.Cnn = new System.Data.SqlClient.SqlConnection(); };
            if (SQLConn.Cnn.State == ConnectionState.Open)
            {
                SQLConn.Cnn.Close();
            };
            SQLConn.Cnn.ConnectionString = _ConnString;
            SQLConn.Cnn.Open();
        }

        public static void SqlBeginTransaction()
        {
            if (SQLConn.Cnn == null) { SQLConn.Cnn = new System.Data.SqlClient.SqlConnection(); }

            if (SQLConn.Cnn.State == ConnectionState.Open)
            {
                SQLConn.Cnn.Close();
            };
            SQLConn.Cnn.ConnectionString = _ConnString;
            SQLConn.Cnn.Open();
            SQLConn.Tran = SQLConn.Cnn.BeginTransaction();
        }

        public static System.Data.SqlClient.SqlConnection SqlConnectionOpen(System.Data.SqlClient.SqlConnection _cnn)
        {
            if (_cnn == null) { _cnn = new System.Data.SqlClient.SqlConnection(); }  ;
            if (_cnn.State == ConnectionState.Open)
            {
                _cnn.Close();
            };
            _cnn.ConnectionString = "";
            _cnn.Open();
            return _cnn;
        }

        public static System.Data.SqlClient.SqlConnection SqlBeginTransaction(System.Data.SqlClient.SqlConnection _cnn)
        {
            if (_cnn == null) { _cnn = new System.Data.SqlClient.SqlConnection(); }

            if (_cnn.State == ConnectionState.Open)
            {
                _cnn.Close();
            };
            _cnn.ConnectionString = "";
            _cnn.Open();
            SQLConn.Tran = _cnn.BeginTransaction();
            return _cnn;
        }

        public static System.Data.SqlClient.SqlConnection SqlBeginTransaction(System.Data.SqlClient.SqlConnection _cnn, System.Data.SqlClient.SqlTransaction _tran)
        {
            if (_cnn == null) { _cnn = new System.Data.SqlClient.SqlConnection(); };

            if (_cnn.State == ConnectionState.Open)
            {
                _cnn.Close();
            };
            _cnn.ConnectionString = "";
            _cnn.Open();
            _tran = _cnn.BeginTransaction();
            return _cnn;
        }

        public static void DisposeSqlConnection(System.Data.SqlClient.SqlConnection _cnn)
        {
            if ((_cnn != null))
            {
                if (_cnn.State == ConnectionState.Open)
                {
                    _cnn.Close();
                }
                _cnn.Dispose();
            }
        }

        public static void DisposeSqlConnection(System.Data.SqlClient.SqlCommand _cmd)
        {
            if ((_cmd != null))
            {
                if ((_cmd.Connection != null))
                {
                    if (_cmd.Connection.State == ConnectionState.Open)
                    {
                        _cmd.Connection.Close();
                    }
                    _cmd.Connection.Dispose();
                }
                _cmd.Dispose();
            }
        }

        public static void DisposeSqlConnection(System.Data.SqlClient.SqlDataAdapter _adapter)
        {
            if ((_adapter != null))
            {
                if ((_adapter.SelectCommand != null))
                {
                    if ((_adapter.SelectCommand.Connection != null))
                    {
                        if (!(_adapter.SelectCommand.Connection.State == ConnectionState.Open))
                        {
                            _adapter.SelectCommand.Connection.Close();
                        }
                        _adapter.SelectCommand.Connection.Dispose();
                    }
                    _adapter.SelectCommand.Dispose();
                }
                _adapter.Dispose();
            }
        }

        public static void DisposeSqlTransaction(System.Data.SqlClient.SqlTransaction _tran)
        {
            if ((_tran != null))
            {
                if ((_tran.Connection != null))
                {
                    if (_tran.Connection.State == ConnectionState.Open)
                    {
                        _tran.Connection.Close();
                    }
                    _tran.Connection.Dispose();
                }
                _tran.Dispose();
            }
        }

        public static void ClearParameterObject(System.Data.SqlClient.SqlCommand _cmd)
        {
            if (_cmd.Parameters.Count > 0)
            {
                _cmd.Parameters.Clear();
            }
        }
        #endregion

        #region "   SQL TRANSACTION    "

        public static bool Execute_Tran(string[] QryStr, HI.Conn.DB.DataBaseName DbName)
        {
            try
            {
                int Complete = 0;

                _ConnString = HI.Conn.DB.ConnectionString(DbName);

                SQLConn.SqlConnectionOpen();
                SQLConn.Cmd = SQLConn.Cnn.CreateCommand();
                SQLConn.Tran = SQLConn.Cnn.BeginTransaction();

                foreach (string Str in QryStr)
                {
      
                    SQLConn.Cmd.CommandType = CommandType.Text;
                    SQLConn.Cmd.CommandText = Str;
                    SQLConn.Cmd.Transaction = SQLConn.Tran;
                    Complete = SQLConn.Cmd.ExecuteNonQuery();
                    SQLConn.Cmd.Parameters.Clear();

                    if (Complete <= 0)
                    {
                        SQLConn.Tran.Rollback();
                        SQLConn.DisposeSqlTransaction(SQLConn.Tran);
                        SQLConn.DisposeSqlConnection(SQLConn.Cmd);
                        return false;
                    }

                    CreateLogCommand(Str);

                }

                SQLConn.Tran.Commit();
                SQLConn.DisposeSqlTransaction(SQLConn.Tran);
                SQLConn.DisposeSqlConnection(SQLConn.Cmd);

               

                return true;

            }
            catch (Exception ex)
            {
                SQLConn.Tran.Rollback();
                SQLConn.DisposeSqlTransaction(SQLConn.Tran);
                SQLConn.DisposeSqlConnection(SQLConn.Cmd);
                return false;
            }
        }

        public static int Execute_Tran(string sqlStr, System.Data.SqlClient.SqlCommand sqlcmd, System.Data.SqlClient.SqlTransaction Tr)
        {
            try
            {
                int Complete = 0;

                sqlcmd.CommandType = CommandType.Text;
                sqlcmd.CommandText = sqlStr;
                sqlcmd.CommandTimeout = 0;
                sqlcmd.Transaction = Tr;
                Complete = sqlcmd.ExecuteNonQuery();
                sqlcmd.Parameters.Clear();

                if (Complete > 0) {   CreateLogCommand(sqlStr);}
              


                return Complete;

            }

            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message);
                return -1;
            }

        }

        public static int ExecuteTran(string sqlStr, System.Data.SqlClient.SqlCommand sqlcmd, System.Data.SqlClient.SqlTransaction Tr)
        {
            int Complete = 0;
            try
            {

                sqlcmd.CommandType = CommandType.Text;
                sqlcmd.CommandText = sqlStr;
                sqlcmd.Transaction = Tr;
                Complete = sqlcmd.ExecuteNonQuery();
                sqlcmd.Parameters.Clear();

                CreateLogCommand(sqlStr);

                return Complete;

            }
            catch 
            {
                return Complete;
            }
        }

        #endregion

        #region "NonTransection"

        public static bool ExecuteOnly(string QryStr, HI.Conn.DB.DataBaseName DbName)
        {

            System.Data.SqlClient.SqlConnection _Cnn = new System.Data.SqlClient.SqlConnection();
            System.Data.SqlClient.SqlCommand _Cmd = new System.Data.SqlClient.SqlCommand();
        
            try
            {

                _ConnString = HI.Conn.DB.ConnectionString(DbName);

                if (_Cnn.State == ConnectionState.Open) { _Cnn.Close(); };           
                _Cnn.ConnectionString = HI.Conn.SQLConn._ConnString;
                _Cnn.Open();

                _Cmd = _Cnn.CreateCommand();
                _Cmd.CommandTimeout = 0;
                _Cmd.CommandType = CommandType.Text;
                _Cmd.CommandText = QryStr;
                _Cmd.ExecuteNonQuery();
                _Cmd.Parameters.Clear();

                DisposeSqlConnection(_Cmd);
                DisposeSqlConnection(_Cnn);

                CreateLogCommand(QryStr);

                return true;
            }
            catch (Exception ex)
            {
                DisposeSqlConnection(_Cmd);
                DisposeSqlConnection(_Cnn);
                //Interaction.MsgBox(ex.Message);
                return false;
            }

        }


        private static bool ExecuteLog(string QryStr, HI.Conn.DB.DataBaseName DbName)
        {

            System.Data.SqlClient.SqlConnection _Cnn = new System.Data.SqlClient.SqlConnection();
            System.Data.SqlClient.SqlCommand _Cmd = new System.Data.SqlClient.SqlCommand();

            try
            {
                _ConnString = HI.Conn.DB.ConnectionString(DbName);

                if (_Cnn.State == ConnectionState.Open) { _Cnn.Close(); };
                _Cnn.ConnectionString = HI.Conn.SQLConn._ConnString;
                _Cnn.Open();
                _Cmd = _Cnn.CreateCommand();
                _Cmd.CommandTimeout = 0;
                _Cmd.CommandType = CommandType.Text;
                _Cmd.CommandText = QryStr;
                _Cmd.ExecuteNonQuery();
                _Cmd.Parameters.Clear();

                DisposeSqlConnection(_Cmd);
                DisposeSqlConnection(_Cnn);
                return true;
            }
            catch (Exception ex)
            {
                DisposeSqlConnection(_Cmd);
                DisposeSqlConnection(_Cnn);
                //Interaction.MsgBox(ex.Message);
                return false;
            }

        }

        public static bool ExecuteNonQuery(string QryStr, HI.Conn.DB.DataBaseName DbName)
        {
            try
            {
                int Complete = 0;

                _ConnString = HI.Conn.DB.ConnectionString(DbName);
                SQLConn.SqlConnectionOpen();
                SQLConn.Cmd = SQLConn.Cnn.CreateCommand();
                HI.Conn.SQLConn.Tran = HI.Conn.SQLConn.Cnn.BeginTransaction();

                SQLConn.Cmd.CommandType = CommandType.Text;
                SQLConn.Cmd.CommandText = QryStr;
                SQLConn.Cmd.Transaction = HI.Conn.SQLConn.Tran;
                Complete = SQLConn.Cmd.ExecuteNonQuery();
                SQLConn.Cmd.Parameters.Clear();

                if (Complete <= 0)
                {
                    SQLConn.Tran.Rollback();
                    SQLConn.DisposeSqlTransaction(SQLConn.Tran);
                    SQLConn.DisposeSqlConnection(SQLConn.Cmd);
                    return false;
                }

                SQLConn.Tran.Commit();
                SQLConn.DisposeSqlTransaction(SQLConn.Tran);
                SQLConn.DisposeSqlConnection(SQLConn.Cmd);

                CreateLogCommand(QryStr);

                return true;

            }

            catch (Exception ex)
            {
                SQLConn.Tran.Rollback();
                SQLConn.DisposeSqlTransaction(SQLConn.Tran);
                SQLConn.DisposeSqlConnection(SQLConn.Cmd);
                return false;
            }
        }
        public static bool ExecuteNonQuery(ref SqlCommand _Cmd, HI.Conn.DB.DataBaseName DbName)
        {
            try
            {
                int Complete = 0;

                _ConnString = HI.Conn.DB.ConnectionString(DbName);
                SQLConn.SqlConnectionOpen();
                HI.Conn.SQLConn.Tran = HI.Conn.SQLConn.Cnn.BeginTransaction();

                _Cmd.Connection = SQLConn.Cnn;
                _Cmd.CommandTimeout = 0;
                _Cmd.Transaction = HI.Conn.SQLConn.Tran;
                Complete = _Cmd.ExecuteNonQuery();
                _Cmd.Parameters.Clear();

                if (Complete <= 0)
                {
                    SQLConn.Tran.Rollback();
                    SQLConn.DisposeSqlTransaction(SQLConn.Tran);
                    SQLConn.DisposeSqlConnection(_Cmd);
                    return false;
                }

                SQLConn.Tran.Commit();
                SQLConn.DisposeSqlTransaction(SQLConn.Tran);
                SQLConn.DisposeSqlConnection(_Cmd);

                CreateLogCommand(_Cmd.CommandText);
                return true;

            }
            catch (Exception ex)
            {
                SQLConn.Tran.Rollback();
                SQLConn.DisposeSqlTransaction(SQLConn.Tran);
                SQLConn.DisposeSqlConnection(SQLConn.Cmd);
                Interaction.MsgBox(ex.Message);
                return false;
            }
        }

        public static object ExecuteScalar(string QryStr, HI.Conn.DB.DataBaseName DbName)
        {

            try
            {
                _ConnString = HI.Conn.DB.ConnectionString(DbName);
                SQLConn.SqlConnectionOpen();
                SQLConn.Cmd = SQLConn.Cnn.CreateCommand();
                SQLConn.Cmd.CommandType = CommandType.Text;
                SQLConn.Cmd.CommandText = QryStr;


              
                return SQLConn.Cmd.ExecuteScalar();

            }
            catch (SqlException ex)
            {
                return null;
            }
            finally
            {
                SQLConn.DisposeSqlConnection(SQLConn.Cmd);
            }

        }

        public static object ExecuteScalar(ref SqlCommand _Cmd, HI.Conn.DB.DataBaseName DbName)
        {

            try
            {
                _ConnString = HI.Conn.DB.ConnectionString(DbName);
                SQLConn.SqlConnectionOpen();

                _Cmd.Connection = SQLConn.Cnn;
                return _Cmd.ExecuteScalar();

            }
            catch (SqlException ex)
            {
                return null;
            }
            finally
            {
                SQLConn.DisposeSqlConnection(_Cmd);
            }

        }


        public static bool ExecuteStoredProcedure( string username, string StoredProcedureName ,string ParameterNameTable ,DataTable dt  , HI.Conn.DB.DataBaseName DbName)
        {
            try
            {
                int Complete = 0;

                _ConnString = HI.Conn.DB.ConnectionString(DbName);
    
                using (SqlConnection con = new SqlConnection(_ConnString))
                {
                    using (SqlCommand cmd = new SqlCommand(StoredProcedureName))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@UserLogin", username);
                        cmd.Parameters.AddWithValue(ParameterNameTable, dt);
                        con.Open();
                        Complete = cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }

                return true;

            }
            catch (Exception ex)
            {
               
                return false;
            }
        }




        public class StoreTableParameter
        {

   

            public string ParameterName { get; set; }
            public string ParameterValue { get; set; }


        }

        public class StoreTableParameterTable
        {
            public string ParameterName { get; set; }
            public System.Data.DataTable ParameterValue { get; set; }


        }

        public static DataTable GetDataTableExecuteStoredProcedureTable(List<StoreTableParameter> pStoreTableParameter, string StoredProcedureName, List<StoreTableParameterTable> pStoreTableParameterTable, HI.Conn.DB.DataBaseName DbNamee)
        {
            DataTable objDT = new DataTable();


            try
            {
                int Complete = 0;

                _ConnString = HI.Conn.DB.ConnectionString(DbNamee);

                using (SqlConnection con = new SqlConnection(_ConnString))
                {
                    using (SqlCommand cmd = new SqlCommand(StoredProcedureName))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;

                        for (int i = 0; i <= pStoreTableParameter.Count - 1; i++) {

                            cmd.Parameters.AddWithValue(pStoreTableParameter[i].ParameterName, pStoreTableParameter[i].ParameterValue);

                        }

                        for (int i = 0; i <= pStoreTableParameterTable.Count - 1; i++)
                        {

                            cmd.Parameters.AddWithValue(pStoreTableParameterTable[i].ParameterName, pStoreTableParameterTable[i].ParameterValue);

                        }

                        using (SqlDataAdapter Adpt = new SqlDataAdapter(cmd))
                        {

                            Adpt.Fill(objDT);
                        }

                            
                        con.Close();
                    }
                }

                return objDT;

            }
            catch (Exception ex)
            {

            }


          
            return objDT;
        }

        public static void CreateLogCommand(string commandstring)
        {

            try
            {
                if (WriteLog) { 
                     string StrSql = "";

                    StrSql = "INSERT INTO [" + HI.Conn.DB.GetDataBaseName(DB.DataBaseName.DB_LOG) + "].dbo.HSysCommandLog ";
                    StrSql += Environment.NewLine + " (FTCommandUser, FDCommandDate, FTCommandTime, FTMnuName, FTFormName, FTCommand)  ";
                    StrSql += Environment.NewLine + " SELECT '" + (HI.Conn.DB.UserNameLogIn) + "'";
                    StrSql += Environment.NewLine + ",Convert(varchar(10),Getdate(),111)";
                    StrSql += Environment.NewLine + ",Convert(varchar(8),Getdate(),114)";
                    StrSql += Environment.NewLine + ",''";
                    StrSql += Environment.NewLine + ",''";
                    StrSql += Environment.NewLine + ",'" + HI.Conn.DB.Quoted(commandstring) + "'";

                    ExecuteLog(StrSql, DB.DataBaseName.DB_LOG);             
                }
               

            }
            catch
            {
            }

        }

        #endregion

        #region " GETDATA  "


        public static DataTable GetDataTable(string QryStr, HI.Conn.DB.DataBaseName DbName, string TableName = "DataTalble1",bool useapi =true )
        {
            DataTable objDT = new DataTable(TableName);

            try {

                if (Conn.DB.APIServiceLink != "" && useapi)
                {
                    Conn.API ERPAPI = new Conn.API();

                    objDT = ERPAPI.GetDataTable(QryStr, DbName);
                }
                else
                {

                    System.Data.SqlClient.SqlConnection _Cnn = new System.Data.SqlClient.SqlConnection();
                    System.Data.SqlClient.SqlCommand _Cmd = new System.Data.SqlClient.SqlCommand();

                    try
                    {
                        _ConnString = HI.Conn.DB.ConnectionString(DbName);

                        if (_Cnn.State == ConnectionState.Open) { _Cnn.Close(); };
                        _Cnn.ConnectionString = HI.Conn.SQLConn._ConnString;
                        _Cnn.Open();
                        _Cmd = _Cnn.CreateCommand();

                        var _Adepter = new SqlDataAdapter(_Cmd);
                        _Adepter.SelectCommand.CommandTimeout = 0;
                        _Adepter.SelectCommand.CommandType = CommandType.Text;
                        _Adepter.SelectCommand.CommandText = QryStr;
                        _Adepter.Fill(objDT);
                        _Adepter.Dispose();

                        DisposeSqlConnection(_Cmd);
                        DisposeSqlConnection(_Cnn);

                    }
                    catch (Exception ex)
                    {
                        DisposeSqlConnection(_Cmd);
                        DisposeSqlConnection(_Cnn);
                    }

                }


            } catch { }
           
            return objDT;
        }
 
        public static void GetDataSet(string QryStr, HI.Conn.DB.DataBaseName DbName, ref DataSet objDataSet, string DefaultTableName = null)
        {
            
                //SqlDataAdapter objDA = new SqlDataAdapter(QryStr, HI.Conn.DB.ConnectionString(DbName));
                //if (DefaultTableName == null)
                //{
                //    objDA.Fill(objDataSet);
                //}
                //else
                //{
                //    objDA.Fill(objDataSet, DefaultTableName);
                //}
                //objDA.Dispose(); 

                System.Data.SqlClient.SqlConnection _Cnn = new System.Data.SqlClient.SqlConnection();
                System.Data.SqlClient.SqlCommand _Cmd = new System.Data.SqlClient.SqlCommand();

                try
                {
                    _ConnString = HI.Conn.DB.ConnectionString(DbName);

                    if (_Cnn.State == ConnectionState.Open) { _Cnn.Close(); };
                    _Cnn.ConnectionString = HI.Conn.SQLConn._ConnString;
                    _Cnn.Open();
                    _Cmd = _Cnn.CreateCommand();

                    var _Adepter = new SqlDataAdapter(_Cmd);
                    _Adepter.SelectCommand.CommandTimeout = 0;
                    _Adepter.SelectCommand.CommandType = CommandType.Text;
                    _Adepter.SelectCommand.CommandText = QryStr;
                    _Adepter.Fill(objDataSet);
                    _Adepter.Dispose();

                    DisposeSqlConnection(_Cmd);
                    DisposeSqlConnection(_Cnn);

                }
                catch (Exception ex)
                {
                    DisposeSqlConnection(_Cmd);
                    DisposeSqlConnection(_Cnn);
                }

            //return objDataSet;
        }

        public static XmlDocument GetDataXML(string QryStr, HI.Conn.DB.DataBaseName DbName)
        {
            XmlDocument objXML = new XmlDocument();
            try
            {
                string _ConnString = "";
                System.Data.SqlClient.SqlConnection _Cnn = new System.Data.SqlClient.SqlConnection();
                System.Data.SqlClient.SqlCommand _Cmd = new System.Data.SqlClient.SqlCommand();

                try
                {
                    //_ConnString = "Server=hig00svr91;Database=HITECH_PRODUCTION;User Id=sa;Password=!@#5k,mew,;";
                    //_ConnString = "Server=hig00svr91;Database=HITECH_PRODUCTION;User Id=wsmhig;Password=I-xfn-l6M.;";
                    _ConnString = HI.Conn.DB.ConnectionString(DbName);

                    XmlReader xmlreader;
                    if (_Cnn.State == ConnectionState.Open) { _Cnn.Close(); };
                    _Cnn.ConnectionString = _ConnString;
                    _Cnn.Open();
                    _Cmd = _Cnn.CreateCommand();
                    _Cmd.CommandText = QryStr;
                    xmlreader = _Cmd.ExecuteXmlReader();

                    while (xmlreader.Read())
                    {
                        objXML.Load(xmlreader);
                    }
                    xmlreader.Close();

                    _Cmd.Connection.Close();
                    _Cmd.Dispose();
                    if (_Cnn.State == ConnectionState.Open)
                    {
                        _Cnn.Close();
                    }
                    _Cnn.Dispose();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _Cmd.Connection.Close();
                    _Cmd.Dispose();
                    if (_Cnn.State == ConnectionState.Open)
                    {
                        _Cnn.Close();
                    }
                    _Cnn.Dispose();
                }
            }
            catch { }

            return objXML;
        } // End GetDataXML


        public static DataTable GetDataTableConectstring(string QryStr, string _ConnectionString, string TableName = "DataTalble1")
        {
            DataTable objDT = new DataTable(TableName);

            System.Data.SqlClient.SqlConnection _Cnn = new System.Data.SqlClient.SqlConnection();
            System.Data.SqlClient.SqlCommand _Cmd = new System.Data.SqlClient.SqlCommand();
            try
            {
                _ConnString = _ConnectionString;

                if (_Cnn.State == ConnectionState.Open) { _Cnn.Close(); };
                _Cnn.ConnectionString = HI.Conn.SQLConn._ConnString;
                _Cnn.Open();
                _Cmd = _Cnn.CreateCommand();

                var _Adepter = new SqlDataAdapter(_Cmd);
                _Adepter.SelectCommand.CommandTimeout = 0;
                _Adepter.SelectCommand.CommandType = CommandType.Text;
                _Adepter.SelectCommand.CommandText = QryStr;
                _Adepter.Fill(objDT);
                _Adepter.Dispose();

                DisposeSqlConnection(_Cmd);
                DisposeSqlConnection(_Cnn);
            }
            catch (Exception ex)
            {
                DisposeSqlConnection(_Cmd);
                DisposeSqlConnection(_Cnn);
            }

            return objDT;
        }

        public static DataTable GetDataTableOnbeginTrans(string QryStr, string DefaultTableName = "DataTalble1")
        {
            DataTable objDT = new DataTable(DefaultTableName);
            SqlCommand _cmd = null;

            try
            {

                if (HI.Conn.SQLConn.Tran != null)
                {
                    _cmd = new SqlCommand(QryStr, SQLConn.Cnn, HI.Conn.SQLConn.Tran);
                }
                else
                {
                    _cmd = new SqlCommand(QryStr, SQLConn.Cnn);
                }

                var _Adepter = new SqlDataAdapter(_cmd);
                _Adepter.SelectCommand.CommandTimeout = 0;
                _Adepter.Fill(objDT);
                _Adepter.Dispose();

                _cmd.Dispose();

            }
            catch (Exception ex)
            {
                _cmd.Dispose();
            }

            return objDT;

        }

        public static string GetField(string strSql, HI.Conn.DB.DataBaseName DbName, object defaultValue = null, bool useapi = true)
        {
            DataTable dt = new DataTable();
            string _Value = Convert.ToString(defaultValue);

            try
            {
                dt = GetDataTable(strSql, DbName,"Table1", useapi);


                if (dt.Rows.Count != 0)
                {
                    foreach (DataRow R in dt.Rows)
                    {
                        if (R[0] == DBNull.Value) {}
                        else{_Value = R[0].ToString();};
                        break;
                    };
                }

                dt.Dispose();
            }
            catch (Exception ex)
            {
            }
            
           
            return _Value;
        }

        public static string GetFieldConectstring(string strSql, string _ConnecttionString, object defaultValue = null)
        {
            DataTable dt = new DataTable();
            string _Value = Convert.ToString(defaultValue);

            try
            {
                dt = GetDataTableConectstring(strSql, _ConnecttionString);


                if (dt.Rows.Count != 0)
                {
                    foreach (DataRow R in dt.Rows)
                    {
                        if (R[0] == DBNull.Value) { }
                        else { _Value = R[0].ToString(); };
                        break;
                    };
                }

                dt.Dispose();
            }
            catch (Exception ex)
            {
            }

         
            return _Value;
        }

        public static string GetFieldByName(string strSql, HI.Conn.DB.DataBaseName DbName, string FieldName, object defaultValue = null,bool useapi=true )
        {
            DataTable dt = new DataTable();
            string _Value = Convert.ToString(defaultValue);

            try
            {
                dt = GetDataTable(strSql, DbName,"table1", useapi);
                if (dt.Rows.Count != 0)
                {
                    foreach (DataRow R in dt.Rows)
                    {
                        if (!string.IsNullOrEmpty(FieldName) & dt.Columns.IndexOf(FieldName) >= 0)
                        {
                            if (R[FieldName] == DBNull.Value) {}
                            else { _Value = R[FieldName].ToString(); };
                        }
                        else
                        {
                            if (R[0] == DBNull.Value) { }
                            else { _Value = R[0].ToString(); };
                        }
                        break; 
                    }
                }
                else
                {
                    _Value = defaultValue.ToString();
                }

            }
            catch (Exception ex)
            {
            }

            dt.Dispose();
            return _Value;
        }

        public static string GetFieldOnBeginTrans(string strSql, HI.Conn.DB.DataBaseName DbName, object defaultValue = null)
        {
            DataTable dt = new DataTable();
            string _Value = defaultValue.ToString();

            try
            {
                dt = HI.Conn.SQLConn.GetDataTableOnbeginTrans( strSql,DbName.ToString());

                if (dt.Rows.Count != 0)
                {
                    foreach (DataRow R in dt.Rows)
                    {

                        if (R[0] == DBNull.Value) {}
                        else { _Value = R[0].ToString(); };
                        break; 
                    }
                };
               

            }
            catch (Exception ex)
            {
            }

            dt.Dispose();
            return _Value;

        }

        public static string GetFieldByNameOnBeginTrans(string strSql, HI.Conn.DB.DataBaseName DbName, string FieldName, object defaultValue = null)
        {
            DataTable dt = new DataTable();
            string _Value = Convert.ToString(defaultValue);

            try
            {             
                dt = GetDataTableOnbeginTrans(strSql, DbName.ToString() );

                if (dt.Rows.Count != 0)
                {                 
                    
                    foreach (DataRow R in dt.Rows)
                    {
                        if (!string.IsNullOrEmpty(FieldName) & dt.Columns.IndexOf(FieldName) >= 0)
                        {
                            if (R[FieldName] == DBNull.Value){}
                            else{ _Value = R[FieldName].ToString();};
                        }
                        else
                        {
                            if (R[0] == DBNull.Value){ }
                            else { _Value = R[0].ToString(); };
                        }
                        break; 
                    }

                };

            }
            catch (Exception ex)
            {
            }

            dt.Dispose();
            return _Value;

        }

        #endregion
    }





}
