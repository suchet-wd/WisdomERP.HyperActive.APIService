using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Xml;
using wisdomserverExchange.Class;

namespace wisdomserverExchange
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();
        //DateTime scheduleDateTime;

        private HttpClient httpClient;
        //private readonly string apiUrl = "http://203.150.53.240:96/api/SM/ProductionPlan";
        //private readonly string api2Url = "http://203.150.53.240:96/api/SM/CreateTaskCutting";
        private readonly string apiToken = "6HEZZHkAAiFuWb5h";

        public Service1()
        {
            InitializeComponent();
            httpClient = new HttpClient();
        }
        public void OnDebug()
        {
            this.OnStart(null);
        }
        protected override void OnStart(string[] args)
        {
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 60000;
            timer.Enabled = true;
        }

        protected override void OnStop()
        {

        }

        //private ResponseAPI PostDataToApi(string _apiNo, string _DocNo, string datajson)
        //{
        //    try
        //    {
        //        // data to be posted
        //        string postData = datajson;
        //        string apiUrl = "";
        //        string jsonContent = "";
        //        ResponseAPI responseAPI = new ResponseAPI("", "");
        //        //httpClient.Dispose();
        //        // Set up the HTTP request headers
        //        httpClient = new HttpClient();
        //        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiToken}");

        //        // Post data to the API
        //        if (_apiNo == "1")
        //        {
        //            //10.92.10.150:3002
        //            //203.150.53.240:96
        //            apiUrl = "http://203.150.53.240:96/api/SM/ProductionPlan";
        //            //apiUrl = "http://10.92.10.150:3002/api/SM/ProductionPlan";
        //        }
        //        else if (_apiNo == "2")
        //        {
        //            apiUrl = "http://203.150.53.240:96/api/SM/CreateTaskCutting";
        //            //apiUrl = "http://10.92.10.150:3002/api/SM/CreateTaskCutting";
        //        }

        //        WriteLogFile($"JSON string:" + _apiNo + " Failed Message is : " + datajson.ToString());

        //        HttpResponseMessage response = httpClient.PostAsync(apiUrl, new StringContent(postData, Encoding.UTF8, "application/json")).Result;

        //        // Handle the API response as needed
        //        if (response.IsSuccessStatusCode)
        //        {
        //            Console.WriteLine("Data posted successfully.");
        //            responseAPI.Code = "0";
        //            responseAPI.Msg = "";
        //            //WriteLogFile("Web Api called : Api Data:" + _apiNo + " Data posted successfully " + response.ToString());
        //            //WriteLogFile("Web Api called : Api Data:" + _apiNo + " Data   " + postData.ToString());
        //            //jsonContent = response.Content.ReadAsStringAsync().Result;
        //            //responseAPI = JsonConvert.DeserializeObject<ResponseAPI>(jsonContent);
        //            //WriteLogFile($"Web Api called : Api Data:" + _apiNo + " Failed Message is : " + responseAPI.Msg);
        //        }
        //        else
        //        {
        //            jsonContent = response.Content.ReadAsStringAsync().Result;
        //            responseAPI = JsonConvert.DeserializeObject<ResponseAPI>(jsonContent);
        //            responseAPI.Code = (responseAPI.Code == null) ? "" : responseAPI.Code.Trim();
        //            responseAPI.Msg = (responseAPI.Msg == null) ? "" : responseAPI.Msg.Trim();
        //            if (_apiNo == "1")
        //            {
        //                if (responseAPI.Msg.Trim() == "This Job : " + _DocNo + " already exists")
        //                {
        //                    responseAPI.Code = "0";
        //                    //responseAPI.Msg = "";
        //                }
        //            }

        //            if (_apiNo == "2")
        //            {
        //                if ((responseAPI.Msg.Trim().Substring(0, 8) == "PooNo : ") && (responseAPI.Msg.Trim().Substring(responseAPI.Msg.Trim().Length - 15, 15) == " already exists"))
        //                {
        //                    responseAPI.Code = "0";
        //                    //responseAPI.Msg = "";
        //                }
        //            }
        //            //WriteLogFile($"Web Api called : Api Data:" + _apiNo + " Failed to post data. Status code: {response.StatusCode}");
        //            //WriteLogFile($"Web Api called : Api Data:" + _apiNo + " Failed to post data.  " + postData.ToString());
        //            //WriteLogFile($"Web Api called : Api Data:" + _apiNo + " Failed Message is : " + responseAPI.Msg);
        //        }
        //        return responseAPI;
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLogFile($"An error occurred: {ex.Message}");
        //        return null;
        //    }
        //}

        //private bool SaveStateSendAPI(string _ApiNo, string _DocNo, string _State, ResponseAPI _responseAPI)
        //{
        //    try
        //    {
        //        string _Cmd = "";
        //        //"This Job : " + _DocNo + " already exists";
        //        if (_responseAPI == null)
        //        {
        //            _responseAPI = new ResponseAPI("", "Server not response !!!");

        //        }

        //        _Cmd = "DECLARE @Date varchar(10) = Convert(varchar(10), Getdate(), 111) \n";
        //        _Cmd += "DECLARE @Time varchar(10) = Convert(varchar(8), Getdate(), 114) \n\n";

        //        _Cmd += "IF EXISTS( SELECT TOP 1 l.FTApiNo + '.' + l.FTDocumentNo \n";
        //        _Cmd += "        FROM [HITECH_HYPERACTIVE].dbo.LOG_SR_HyperActive_API AS l WITH(NOLOCK) \n";
        //        _Cmd += "        WHERE l.FTApiNo = '" + _ApiNo + "' AND l.FTDocumentNo = '" + _DocNo + "') \n";

        //        _Cmd += "BEGIN \n";

        //        _Cmd += "      UPDATE [HITECH_HYPERACTIVE].dbo.LOG_SR_HyperActive_API \n";
        //        _Cmd += "      SET FTStateSend = '" + _State + "', FTResponseRemark = '" + _responseAPI.Msg + "' \n";
        //        _Cmd += "      , FDDocDate = @Date , FTDocTime = @Time , FTDocState = ''\n";

        //        _Cmd += "      WHERE FTApiNo = '" + _ApiNo + "' AND FTDocumentNo = '" + _DocNo + "' \n";

        //        _Cmd += "END \n";

        //        _Cmd += "ELSE \n";

        //        _Cmd += "BEGIN \n";

        //        _Cmd += "      INSERT INTO [HITECH_HYPERACTIVE].dbo.LOG_SR_HyperActive_API \n";
        //        _Cmd += "      (FTApiNo, FTDocumentNo, FTStateSend, FTResponseCode, FTResponseRemark, FDDocDate, FTDocTime, FTDocState) VALUES \n";
        //        _Cmd += "      ('" + _ApiNo + "', '" + _DocNo + "', '" + _State + "', '" + _responseAPI.Code + "', '" + _responseAPI.Msg + "' , @Date, @Time, '')\n";

        //        _Cmd += "END \n";

        //        //if (_responseAPI.Msg.Trim() == _Cmd)
        //        //{
        //        //    _Cmd = "INSERT INTO [HITECH_MERCHAN].dbo.LOG_SR_HyperActive_API (FTApiNo, FTDocumentNo, FTStateSend,FTResponseCode,FTResponseRemark) VALUES (";
        //        //    _Cmd += "'" + _ApiNo + "', '" + _DocNo + "', '1', '', '')";
        //        //}
        //        //else
        //        //{
        //        //    _Cmd = "INSERT INTO [HITECH_MERCHAN].dbo.LOG_SR_HyperActive_API (FTApiNo, FTDocumentNo, FTStateSend,FTResponseCode,FTResponseRemark) VALUES (";
        //        //    _Cmd += "'" + _ApiNo + "', '" + _DocNo + "', '" + _State + "', '" + _responseAPI.Code + "', '" + _responseAPI.Msg + "')";
        //        //}


        //        return HI.Conn.SQLConn.ExecuteOnly(_Cmd, HI.Conn.DB.DataBaseName.DB_HYPERACTIVE);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return false;
        //        //WriteLogFile($"An error occurred: {ex.Message}");
        //    }

        //}


        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            try
            {
                HyperActive.HyperActiveAPI hyper = new HyperActive.HyperActiveAPI();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //public bool ExecuteOnly(string QryStr)
        //{
        //    string _ConnString = "";
        //    System.Data.SqlClient.SqlConnection _Cnn = new System.Data.SqlClient.SqlConnection();
        //    System.Data.SqlClient.SqlCommand _Cmd = new System.Data.SqlClient.SqlCommand();

        //    try
        //    {
        //        _ConnString = "Server=hig00svr91;Database=HITECH_PRODUCTION;User Id=sa;Password=!@#5k,mew,;";
        //        //_ConnString = "Server=hig00svr91;Database=HITECH_PRODUCTION;User Id=wsmhig;Password=I-xfn-l6M.;";

        //        if (_Cnn.State == ConnectionState.Open) { _Cnn.Close(); };
        //        _Cnn.ConnectionString = _ConnString;
        //        _Cnn.Open();
        //        _Cmd = _Cnn.CreateCommand();
        //        _Cmd.CommandTimeout = 0;
        //        _Cmd.CommandType = CommandType.Text;
        //        _Cmd.CommandText = QryStr;
        //        _Cmd.ExecuteNonQuery();
        //        _Cmd.Parameters.Clear();

        //        _Cmd.Connection.Close();
        //        _Cmd.Dispose();
        //        if (_Cnn.State == ConnectionState.Open)
        //        {
        //            _Cnn.Close();
        //        }
        //        _Cnn.Dispose();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        _Cmd.Connection.Close();
        //        _Cmd.Dispose();
        //        if (_Cnn.State == ConnectionState.Open)
        //        {
        //            _Cnn.Close();
        //        }
        //        _Cnn.Dispose();
        //        //Interaction.MsgBox(ex.Message);
        //        return false;
        //    }

        //}
        //public static DataTable GetDataTable(string QryStr)
        //{
        //    DataTable objDT = new DataTable();

        //    try
        //    {
        //        string _ConnString = "";
        //        System.Data.SqlClient.SqlConnection _Cnn = new System.Data.SqlClient.SqlConnection();
        //        System.Data.SqlClient.SqlCommand _Cmd = new System.Data.SqlClient.SqlCommand();

        //        try
        //        {
        //            _ConnString = "Server=hig00svr91;Database=HITECH_PRODUCTION;User Id=sa;Password=!@#5k,mew,;";
        //            //_ConnString = "Server=hig00svr91;Database=HITECH_PRODUCTION;User Id=wsmhig;Password=I-xfn-l6M.;";

        //            if (_Cnn.State == ConnectionState.Open) { _Cnn.Close(); };
        //            _Cnn.ConnectionString = _ConnString;
        //            _Cnn.Open();
        //            _Cmd = _Cnn.CreateCommand();

        //            var _Adepter = new SqlDataAdapter(_Cmd);
        //            _Adepter.SelectCommand.CommandTimeout = 0;
        //            _Adepter.SelectCommand.CommandType = CommandType.Text;
        //            _Adepter.SelectCommand.CommandText = QryStr;
        //            _Adepter.Fill(objDT);
        //            _Adepter.Dispose();

        //            _Cmd.Connection.Close();
        //            _Cmd.Dispose();
        //            if (_Cnn.State == ConnectionState.Open)
        //            {
        //                _Cnn.Close();
        //            }
        //            _Cnn.Dispose();

        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //            _Cmd.Connection.Close();
        //            _Cmd.Dispose();
        //            if (_Cnn.State == ConnectionState.Open)
        //            {
        //                _Cnn.Close();
        //            }
        //            _Cnn.Dispose();
        //        }
        //    }
        //    catch { }

        //    return objDT;
        //}

        //public static XmlDocument GetDataXML(string QryStr)
        //{
        //    XmlDocument objXML = new XmlDocument();
        //    try
        //    {
        //        string _ConnString = "";
        //        System.Data.SqlClient.SqlConnection _Cnn = new System.Data.SqlClient.SqlConnection();
        //        System.Data.SqlClient.SqlCommand _Cmd = new System.Data.SqlClient.SqlCommand();

        //        try
        //        {
        //            _ConnString = "Server=hig00svr91;Database=HITECH_PRODUCTION;User Id=sa;Password=!@#5k,mew,;";
        //            //_ConnString = "Server=hig00svr91;Database=HITECH_PRODUCTION;User Id=wsmhig;Password=I-xfn-l6M.;";
        //            XmlReader xmlreader;
        //            if (_Cnn.State == ConnectionState.Open) { _Cnn.Close(); };
        //            _Cnn.ConnectionString = _ConnString;
        //            _Cnn.Open();
        //            _Cmd = _Cnn.CreateCommand();
        //            _Cmd.CommandText = QryStr;
        //            xmlreader = _Cmd.ExecuteXmlReader();

        //            while (xmlreader.Read())
        //            {
        //                objXML.Load(xmlreader);
        //            }
        //            xmlreader.Close();

        //            _Cmd.Connection.Close();
        //            _Cmd.Dispose();
        //            if (_Cnn.State == ConnectionState.Open)
        //            {
        //                _Cnn.Close();
        //            }
        //            _Cnn.Dispose();

        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //            _Cmd.Connection.Close();
        //            _Cmd.Dispose();
        //            if (_Cnn.State == ConnectionState.Open)
        //            {
        //                _Cnn.Close();
        //            }
        //            _Cnn.Dispose();
        //        }
        //    }
        //    catch { }

        //    return objXML;
        //}


        public void WriteLogFile(string message)
        {
            StreamWriter sw = null;
            sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\WisdomLogFile.txt", true);
            sw.WriteLine("{DateTime.Now.ToString()} :" + message);
            sw.Flush();
            sw.Close();
        }
    }
}



//private ResponseAPI PostDataToApi2(string datajson)
//{
//    try
//    {
//        // data to be posted
//        string postData = datajson;
//        string jsonContent = "";
//        ResponseAPI responseAPI;

//        // Set up the HTTP request headers
//        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiToken}");

//        // Post data to the API
//        HttpResponseMessage response = httpClient.PostAsync(api2Url, new StringContent(postData, Encoding.UTF8, "application/json")).Result;

//        // Handle the API response as needed
//        if (response.IsSuccessStatusCode)
//        {
//            Console.WriteLine("Data posted successfully.");
//            WriteLogFile("Web Api called : Api Data:2 Data posted successfully " + response.ToString());
//            WriteLogFile("Web Api called : Api Data:2 Data   " + postData.ToString());
//            jsonContent = response.Content.ReadAsStringAsync().Result;
//            responseAPI = JsonConvert.DeserializeObject<ResponseAPI>(jsonContent);
//            //Console.WriteLine("Api Data:2 Failed Message is : " + responseAPI.Msg);
//            WriteLogFile($"Web Api called : Api Data:2 Failed Message is : " + responseAPI.Msg);
//        }
//        else
//        {
//            WriteLogFile($"Web Api called : Api Data:2 Failed to post data. Status code: {response.StatusCode}");
//            WriteLogFile($"Web Api called : Api Data:2 Failed to post data.  " + postData.ToString());
//            jsonContent = response.Content.ReadAsStringAsync().Result;
//            responseAPI = JsonConvert.DeserializeObject<ResponseAPI>(jsonContent);
//            //Console.WriteLine("Api Data:2 Failed Message is : " + responseAPI.Msg);
//            WriteLogFile($"Web Api called : Api Data:2 Failed Message is : " + responseAPI.Msg);
//        }
//        return responseAPI;
//    }
//    catch (Exception ex)
//    {
//        WriteLogFile($"An error occurred: {ex.Message}");
//        return null;
//    }
//}



//private void PostDataToApi3(string datajson)
//{
//    try
//    {
//        // data to be posted
//        string postData = datajson;

//        // Set up the HTTP request headers
//        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiToken}");

//        // Post data to the API
//        HttpResponseMessage response = httpClient.PostAsync(api2Url, new StringContent(postData, Encoding.UTF8, "application/json")).Result;

//        // Handle the API response as needed
//        if (response.IsSuccessStatusCode)
//        {
//            Console.WriteLine("Data posted successfully.");
//            WriteLogFile("Web Api called : Api Data:3 Data posted successfully " + response.ToString());
//            WriteLogFile("Web Api called : Api Data:3 Data   " + postData.ToString());
//        }
//        else
//        {
//            WriteLogFile($"Web Api called : Api Data:3 Failed to post data. Status code: {response.StatusCode}");
//            WriteLogFile($"Web Api called : Api Data:3 Failed to post data.  " + postData.ToString());
//            string jsonContent = response.Content.ReadAsStringAsync().Result;
//            var responseAPI = JsonConvert.DeserializeObject<ResponseAPI>(jsonContent);
//            Console.WriteLine("Api Data:3 Failed Message is : " + responseAPI.Msg);
//            WriteLogFile($"Web Api called : Api Data:3 Failed Message is : " + responseAPI.Msg);
//        }
//    }
//    catch (Exception ex)
//    {
//        WriteLogFile($"An error occurred: {ex.Message}");
//    }
//}