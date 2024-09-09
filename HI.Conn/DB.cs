using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Text ;

namespace HI.Conn
{
    public static class DB

    {

        public static string _ConnectString;
        public static string _AppName;
        public static string APIServiceLink;
        public static string APILink;
        public static string APILinkPort;
        public static string APIReportServiceLink;
        public static string APIReportLink;
        public static string APIReportLinkPort;
        private static string[] _ServerName = new string[30];
        private static string[] _DBName = new string[30];
        private static string[] _UserName = new string[30];
        private static string[] _PasswordName = new string[30];
        private static string[] _DBAPILink = new string[30];
        private static string[] _HPName = new string[30];
        public static string _StartDate;
        public static string _Timer;
        public static string _apiToken;

        private static string[] _SystemDBName = { "DB_TEMPDB", "DB_SECURITY", "DB_HR", "DB_SYSTEM", 
            "DB_MASTER", "DB_MER", "DB_PUR", "DB_INVEN", "DB_PROD", "DB_ACCOUNT", "DB_LANG", "DB_LOG", 
            "DB_MAIL", "DB_HR_PAYROLL", "DB_MEDC", "DB_FG", "DB_PLANNING", "DB_DOC", "DB_FIXED", 
            "DB_SAMPLE", "DB_TIME", "DB_FHS", "DB_HYPERACTIVE" };
        public enum DataBaseName : int
        {
            DB_TEMPDB = 0,
            DB_SECURITY = 1,
            DB_HR = 2,
            DB_SYSTEM = 3,
            DB_MASTER = 4,
            DB_MERCHAN = 5,
            DB_PUR = 6,
            DB_INVEN = 7,
            DB_PROD = 8,
            DB_ACCOUNT = 9,
            DB_LANG = 10,
            DB_LOG = 11,
            DB_MAIL = 12,
            DB_HR_PAYROLL = 13,
            DB_MEDC=14,
            DB_FG = 15,
            DB_PLANNING =16,
            DB_DOC =17,
            DB_FIXED =18,
            DB_SAMPLE = 19,
            DB_TIME = 20,
            DB_FHS = 21,
            DB_HYPERACTIVE = 22
        }

        private static string[] _SystemHPName = { "api1", "api2", "api5", "api6", "api7", "api9", "api10", "api11" };
        public enum HyperActiveName : int
        {
            api1 = 0,
            api2 = 1,
            api5 = 2,
            api6 = 3,
            api7 = 4,
            api9 = 5,
            api10 = 6,
            api11 = 7
        }

        public static string GetHyperActiveAPIName(HyperActiveName hyperActiveName)
        {
            try
            {
                return _HPName[(int)hyperActiveName];
            }
            catch
            {
                return "";
            }
        }


        public static string GetDataBaseName(DataBaseName DbName)
        {
            try
            {
                return _DBName[(int)DbName];
            }
            catch 
            {
                return "";
            }
        }



        public static string GetServerName(DataBaseName DbName)
        {
            try
            {
                return _ServerName[(int)DbName];
            }
            catch 
            {
                return "";
            }
        }

        public static string GetDataBaseName(string DbName)
        {
            try
            {
                int I = 0;
                foreach (string StrDBName in _DBName)
                {

                    if (StrDBName.ToUpper().Contains(DbName.ToUpper()) == true)
                    {
                        return _DBName[I];

                    }
                    I = I + 1;
                }
                return "";
            }
            catch 
            {
                return "";
            }
        }

        public static string GetDataBaseAPI(string DbName)
        {
            try
            {
                int I = 0;
                foreach (string StrDBName in _DBName)
                {

                    if (StrDBName.ToUpper().Contains(DbName.ToUpper()) == true)
                    {
                        return _DBAPILink[I];

                    }
                    I = I + 1;
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        public static bool UsedDB(DataBaseName DbName)
        {
            try
            {

                SerVerName = _ServerName[(int)DbName];
                UIDName = _UserName[(int)DbName];
                PWDName = _PasswordName[(int)DbName];
                BaseName = _DBName[(int)DbName];
                DBAPIServiceLink = _DBAPILink[(int)DbName];

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool UsedDB(string DbName)
        {
            try
            {
                int I = 0;
                foreach (string StrDBName in _DBName)
                {
                    if (StrDBName.ToUpper().Contains(DbName.ToUpper()) == true)
    
                    {
                        SerVerName = _ServerName[I];
                        UIDName = _UserName[I];
                        PWDName = _PasswordName[I];
                        BaseName = _DBName[I];
                        DBAPIServiceLink = _DBAPILink[I];
                        return true;
                        break; 
                    }
                    I = I + 1;
                }
                return false;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static string SerVerName { get; set;}
        public static string UIDName{ get; set;}
        public static string PWDName { get; set; }
        public static string BaseName { get; set; }
        public static string DBAPIServiceLink { get; set; }
        public static string AppService { get; set; }
        public static string AppServicePath { get; set; }
        public static string AppServiceName { get; set; }
        public static bool UsedADUser { get; set; }
        public static string UsedADUserIP { get; set; }
        public static string UserNameLogIn { get; set; }
        public static string Quoted(string tmpStr)
        {

            if (!string.IsNullOrEmpty(tmpStr))
            {
                return Strings.Replace(tmpStr, (Strings.Chr(39)).ToString(), (Strings.Chr(39)).ToString() + (Strings.Chr(39)).ToString());
            }
            else
            {
                return tmpStr;
            }

        }

        public static string ConnectionString(DataBaseName DbName)
        {

            try
            {
                _ConnectString = "SERVER=" + _ServerName[(int)DbName] + ";UID=" + _UserName[(int)DbName] + ";PWD=" + _PasswordName[(int)DbName] + ";Initial Catalog=" + _DBName[(int)DbName];
                return _ConnectString;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void GetXmlConnectionString()
        {
            try
            {
                
                APILink = "";
                APILinkPort = "";
                APIServiceLink = "";

                APIReportLink = "";
                APIReportLinkPort = "";
                APIReportServiceLink = "";

                DataSet DS = new DataSet();
                int i = 0;
                DataTable _Dt = null;
                DS.ReadXml(Application.StartupPath + "\\Database.xml");
                string dbServerName = null;
                string dbName = null;
                string dbUserName = null;
                string dbPassword = null;
                DataTable _HyperActive = null;

                AppService ="";
                AppServicePath ="";
                AppServiceName ="";

                UsedADUser = false;
                UsedADUserIP = "";

                _AppName = "Wisdom System";
                if (DS.Tables.IndexOf("APPLICATION") != -1) { _AppName = DS.Tables["APPLICATION"].Rows[0]["Name"].ToString(); };

                if (DS.Tables.IndexOf("AppService") != -1)
                {

                    try {
                        AppService = DS.Tables["AppService"].Rows[0]["Name"].ToString();
                        AppServicePath = DS.Tables["AppService"].Rows[0]["Path"].ToString();
                        AppServiceName = DS.Tables["AppService"].Rows[0]["Sevice"].ToString();
                    }
                    catch (Exception ex22)
                    {
                    }

                };

                if (DS.Tables.IndexOf("ADUser") != -1)
                {
                    try
                    {
                        UsedADUser =( DS.Tables["ADUser"].Rows[0]["Used"].ToString()=="1");
                        UsedADUserIP = DS.Tables["ADUser"].Rows[0]["ADUserIP"].ToString();
                        
                    }
                    catch (Exception ex22)
                    {
                    }
                };

                if (DS.Tables.IndexOf("APIService") != -1)
                {
                    try
                    {
                        APILink = DS.Tables["APIService"].Rows[0]["APILink"].ToString();
                        APILinkPort = DS.Tables["APIService"].Rows[0]["APIPort"].ToString();

                        if (APILink != "") {
                            if (APILinkPort != "")
                            {
                                APIServiceLink = "http://" + APILink + ':' + APILinkPort + "/api/";
                            }
                            else {
                                APIServiceLink = "http://" + APILink  + "/api/";

                            };
                        }
                       
                    }
                    catch (Exception ex229)
                    {
                    }
                };

                if (DS.Tables.IndexOf("APIReportService") != -1)
                {
                    try
                    {
                        APIReportLink = DS.Tables["APIReportService"].Rows[0]["APILink"].ToString();
                        APIReportLinkPort = DS.Tables["APIReportService"].Rows[0]["APIPort"].ToString();

                        if (APIReportLink != "")
                        {
                            if (APIReportLinkPort != "")
                            {
                                APIReportServiceLink = "http://" + APIReportLink + ':' + APIReportLinkPort + "/api/";
                            }
                            else
                            {
                                APIReportServiceLink = "http://" + APIReportLink + "/api/";

                            };
                        }

                    }
                    catch (Exception ex229)
                    {
                    }
                };


                i = 0;
                _Dt = DS.Tables["SQLSERVER"].Copy();

                string dbapi = null;
                string dbapiport = null;
                string dbserviceapi = null;
                foreach (string StrDBName in _SystemDBName)
                {
                    dbServerName = "";
                    dbName = "";
                    dbUserName = "";
                    dbPassword = "";
                    dbapi = "";
                    dbapiport = "";
                    foreach (DataRow Row in _Dt.Select("name='" + StrDBName + "' "))
                    {
                        dbServerName = (Row["server"]).ToString();
                        dbName = (Row["dbname"]).ToString();
                        dbUserName = (Row["username"]).ToString();
                        dbPassword = FuncDecryptDataServer((Row["password"]).ToString());

                        try {
                            dbapi = (Row["APILink"]).ToString();
                            dbapiport = (Row["APIPort"]).ToString();


                            if (dbapi != "")
                            {
                                if (dbapiport != "")
                                {
                                    dbserviceapi = "http://" + dbapi + ':' + dbapiport + "/api/";
                                }
                                else
                                {
                                    dbserviceapi = "http://" + dbapi + "/api/";

                                };
                            }


                        } catch {
                            dbserviceapi = "";
                        }
                    }

                    _ServerName[i] = dbServerName;
                    _DBName[i] = dbName;
                    _UserName[i] = dbUserName;
                    _PasswordName[i] = dbPassword;
                    _DBAPILink[i] = dbserviceapi;
                    i = i + 1;
                };

                i = 0;
                _HyperActive = DS.Tables["APIHyperActive"].Copy();

                //string hpapi = null;
                //string hpapiport = null;
                //string hpserviceapi = null;
                foreach (string StrHPName in _SystemHPName)
                {
                    //dbServerName = "";
                    dbName = "";
                    foreach (DataRow Row in _HyperActive.Select("name='" + StrHPName + "' "))
                    {
                       // dbServerName = (Row["name"]).ToString();
                        dbName = (Row["apiUrl"]).ToString();
                    }

                    //_ServerName[i] = dbServerName;
                    _HPName[i] = dbName;
                    i = i + 1;
                };

                if (DS.Tables.IndexOf("StartDate") != -1) { _StartDate = DS.Tables["StartDate"].Rows[0]["Name"].ToString(); };
                if (DS.Tables.IndexOf("Timer") != -1) { _Timer = DS.Tables["Timer"].Rows[0]["Name"].ToString(); };
                if (DS.Tables.IndexOf("apiToken") != -1) { _apiToken = DS.Tables["apiToken"].Rows[0]["Name"].ToString(); };


                DS.Dispose();
                DS = null;

            }
            catch (Exception ex)
            {
            }

        }

        #region " Function Decrypt FuncEncryptData"

        public static string FuncDecryptData(string DecryTxt, bool FirtsTime = true)
        {

            string txtDecry = "";
            try
            {
                char Buff1 = '\0';
                char Buff2 = '\0';
                string TxtBuff1 = "";
                string TxtBuff2 = "";
                int i = 0;
                int DecryCode = 0;


                if (!string.IsNullOrEmpty(Strings.Trim(DecryTxt)))
                {
                    DecryCode = Strings.Asc(Strings.Right(DecryTxt, 1)) - Strings.Asc(Strings.Mid(DecryTxt, 2, 1));

                    for (i = Strings.Len(DecryTxt) - 1; i >= 1; i += -1)
                    {
                        TxtBuff1 += Strings.Mid(DecryTxt, i, 1);
                    }

                    for (i = 1; i <= Strings.Len(TxtBuff1); i++)
                    {
                        Buff1 = Convert.ToChar(Strings.Mid(TxtBuff1, i, 1));
                        Buff2 = '\0';
                        Buff2 = Strings.Chr(Strings.Asc(Buff1) - DecryCode);
                        TxtBuff2 += Buff2;
                    }
                    txtDecry = TxtBuff2;
                }

                if ((FirtsTime))
                {
                    if (txtDecry.Length > 1)
                    {
                        txtDecry = FuncDecryptData(Strings.Right(txtDecry, txtDecry.Length - 1), false);
                    }
                    else
                    {
                        txtDecry = FuncDecryptData(txtDecry, false);
                    }

                }

            }
            catch (Exception ex)
            {
            }
            return txtDecry;
        }

        public static string FuncEncryptData(string EncryTxt, bool FirtsTime = true)
        {
            string txtEncry = "";
            try
            {
                int EncryCode = 0;
                char Buff1 = '\0';
                char Buff2 = '\0';
                string TxtBuff1 = "";
                string TxtBuff2 = "";
                int i = 0;
                VBMath.Randomize();
                EncryCode = Convert.ToInt32((9 * VBMath.Rnd()) + 1);


                for (i = 1; i <= Strings.Len(EncryTxt); i++)
                {
                    Buff1 = Convert.ToChar(Strings.Mid(EncryTxt, i, 1));
                    Buff2 = '\0';
                    Buff2 = Strings.Chr(Strings.Asc(Buff1) + EncryCode);
                    TxtBuff1 += Buff2;

                }

                for (i = Strings.Len(TxtBuff1); i >= 1; i += -1)
                {
                    TxtBuff2 += Strings.Mid(TxtBuff1, i, 1);
                }

                EncryCode = Strings.Asc(Strings.Mid(TxtBuff2, 2, 1)) + EncryCode;
                txtEncry = TxtBuff2 + Strings.Chr(EncryCode);

                if ((FirtsTime))
                {
                    if (txtEncry.Length > 1)
                    {
                        txtEncry = FuncEncryptData("H" + txtEncry, false);
                    }
                    else
                    {
                        txtEncry = FuncEncryptData(txtEncry, false);
                    }

                }

            }
            catch (Exception ex)
            {
            }
            return txtEncry;
        }

        #endregion

        #region " Function Decrypt FuncEncryptData Server"

        public static string FuncDecryptDataServer(string DecryTxt)
        {
            string txtDecry = "";
            try
            {
                char Buff1 = '\0';
                char Buff2 = '\0';
                string TxtBuff1 = "";
                string TxtBuff2 = "";
                int i = 0;
                int DecryCode = 0;

                if (!string.IsNullOrEmpty(Strings.Trim(DecryTxt)))
                {
                    DecryCode = Strings.Asc(Strings.Right(DecryTxt, 1)) - Strings.Asc(Strings.Mid(DecryTxt, 2, 1));

                    for (i = Strings.Len(DecryTxt) - 1; i >= 1; i += -1)
                    {
                        TxtBuff1 += Strings.Mid(DecryTxt, i, 1);
                    }

                    for (i = 1; i <= Strings.Len(TxtBuff1); i++)
                    {
                        Buff1 =  Convert.ToChar(Strings.Mid(TxtBuff1, i, 1));
                        Buff2 = '\0';
                        Buff2 = Strings.Chr(Strings.Asc(Buff1) - DecryCode);
                        TxtBuff2 += Buff2;
                    }

                    txtDecry = TxtBuff2;
                    txtDecry = Strings.Right(txtDecry, txtDecry.Length - 1);
                    txtDecry = Strings.Left(txtDecry, txtDecry.Length - 1);

                    int txtlangth = 0;
                    int txtsplit1 = 0;

                    try {

                        txtlangth =int.Parse((txtDecry.Split('|'))[0]);
                        txtsplit1 = int.Parse((txtDecry.Split('|'))[1]);
                        txtDecry = ((txtDecry.Split('|'))[2]);

                    }
                    catch { }

                    if (txtlangth > 0)
                    {
                        txtDecry = Strings.Right(txtDecry, txtlangth - txtsplit1) + Strings.Left(txtDecry, txtsplit1);                    
                    };

                }

            }
            catch (Exception ex)
            {
            }

            return txtDecry;
        }

        public static string FuncEncryptDataServer(string EncryTxt)
        {
            string txtEncry = "";
            try
            {
                int EncryCode = 0;
                char Buff1 = '\0';
                char Buff2 = '\0';
                string TxtBuff1 = "";
                string TxtBuff2 = "";
                int i = 0;
                int txtlangth = 0;
                int txtsplit1 = 0;

                txtlangth = EncryTxt.Length;
                txtsplit1 = txtlangth / 2;
                
                if (txtlangth <=0) {
                    txtlangth =0;
                    txtsplit1=0;
                };

                EncryTxt = txtlangth.ToString() + "|" + txtsplit1.ToString() + "|" + Strings.Right(EncryTxt, txtsplit1) + Strings.Left(EncryTxt, txtlangth - txtsplit1);

                VBMath.Randomize();
                EncryTxt = "L" + EncryTxt + "H";

                EncryCode = Convert.ToInt32 ((9 * VBMath.Rnd()) + 1);

                for (i = 1; i <= Strings.Len(EncryTxt); i++)
                {
                    Buff1 =  Convert.ToChar(Strings.Mid(EncryTxt, i, 1));
                    Buff2 = '\0';
                    Buff2 = Strings.Chr(Strings.Asc(Buff1) + EncryCode);
                    TxtBuff1 += Buff2;

                }

                for (i = Strings.Len(TxtBuff1); i >= 1; i += -1)
                {
                    TxtBuff2 += Strings.Mid(TxtBuff1, i, 1);
                }

                EncryCode = Strings.Asc(Strings.Mid(TxtBuff2, 2, 1)) + EncryCode;
                txtEncry = TxtBuff2 + Strings.Chr(EncryCode);

            }
            catch (Exception ex)
            {
            }

            return txtEncry;
        }

        #endregion
    }

}
