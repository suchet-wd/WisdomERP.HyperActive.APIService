using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Xml;
using System.Xml.Serialization;
using wisdomserverExchange.Class;

namespace wisdomserverExchange
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();
        DateTime scheduleDateTime;

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

        private ResponseAPI PostDataToApi(string _apiNo, string _DocNo, string datajson)
        {
            try
            {
                // data to be posted
                string postData = datajson;
                string apiUrl = "";
                string jsonContent = "";
                ResponseAPI responseAPI = new ResponseAPI("", "");
                //httpClient.Dispose();
                // Set up the HTTP request headers
                httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiToken}");

                // Post data to the API
                if (_apiNo == "1")
                {
                    apiUrl = "http://203.150.53.240:96/api/SM/ProductionPlan";
                }
                else if (_apiNo == "2")
                {
                    apiUrl = "http://203.150.53.240:96/api/SM/CreateTaskCutting";
                }

                HttpResponseMessage response = httpClient.PostAsync(apiUrl, new StringContent(postData, Encoding.UTF8, "application/json")).Result;

                // Handle the API response as needed
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Data posted successfully.");
                    responseAPI.Code = "0";
                    responseAPI.Msg = "";
                    //WriteLogFile("Web Api called : Api Data:" + _apiNo + " Data posted successfully " + response.ToString());
                    //WriteLogFile("Web Api called : Api Data:" + _apiNo + " Data   " + postData.ToString());
                    //jsonContent = response.Content.ReadAsStringAsync().Result;
                    //responseAPI = JsonConvert.DeserializeObject<ResponseAPI>(jsonContent);
                    //WriteLogFile($"Web Api called : Api Data:" + _apiNo + " Failed Message is : " + responseAPI.Msg);
                }
                else
                {
                    jsonContent = response.Content.ReadAsStringAsync().Result;
                    responseAPI = JsonConvert.DeserializeObject<ResponseAPI>(jsonContent);
                    responseAPI.Code = (responseAPI.Code == null) ? "" : responseAPI.Code.Trim();
                    responseAPI.Msg = (responseAPI.Msg == null) ? "" : responseAPI.Msg.Trim();

                    if (responseAPI.Msg.Trim() == "This Job : " + _DocNo + " already exists")
                    {
                        responseAPI.Code = "0";
                        responseAPI.Msg = "";
                    }
                    //WriteLogFile($"Web Api called : Api Data:" + _apiNo + " Failed to post data. Status code: {response.StatusCode}");
                    //WriteLogFile($"Web Api called : Api Data:" + _apiNo + " Failed to post data.  " + postData.ToString());
                    //WriteLogFile($"Web Api called : Api Data:" + _apiNo + " Failed Message is : " + responseAPI.Msg);
                }
                return responseAPI;
            }
            catch (Exception ex)
            {
                WriteLogFile($"An error occurred: {ex.Message}");
                return null;
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

        private bool SaveStateSendAPI(string _ApiNo, string _DocNo, string _State, ResponseAPI _responseAPI)
        {
            try
            {
                string _Cmd = "This Job : " + _DocNo + " already exists";
                if (_responseAPI != null)
                {
                    _Cmd = "IF EXISTS( SELECT TOP 1 l.FTApiNo + '.' + l.FTDocumentNo \n";
                    _Cmd += "        FROM HITECH_MERCHAN.dbo.LOG_SR_HyperActive_API AS l WITH(NOLOCK) \n";
                    _Cmd += "        WHERE l.FTApiNo = '" + _ApiNo + "' AND l.FTDocumentNo = '" + _DocNo + "') \n";

                    _Cmd += "   BEGIN\n";

                    _Cmd += "      UPDATE HITECH_MERCHAN.dbo.LOG_SR_HyperActive_API \n";
                    _Cmd += "      SET FTStateSend = '" + _State + "', FTResponseRemark = '" + _responseAPI.Msg + "' \n";
                    _Cmd += "      WHERE FTApiNo = '" + _ApiNo + "' AND FTDocumentNo = '" + _DocNo + "' \n";

                    _Cmd += "   END\n";

                    _Cmd += "ELSE\n";

                    _Cmd += "   BEGIN\n";

                    _Cmd += "      INSERT INTO HITECH_MERCHAN.dbo.LOG_SR_HyperActive_API \n";
                    _Cmd += "      (FTApiNo, FTDocumentNo, FTStateSend, FTResponseCode, FTResponseRemark) VALUES \n";
                    _Cmd += "      ('" + _ApiNo + "', '" + _DocNo + "', '" + _State + "', '" + _responseAPI.Code + "', '" + _responseAPI.Msg + "')\n";

                    _Cmd += "   END\n";

                    //if (_responseAPI.Msg.Trim() == _Cmd)
                    //{
                    //    _Cmd = "INSERT INTO [HITECH_MERCHAN].dbo.LOG_SR_HyperActive_API (FTApiNo, FTDocumentNo, FTStateSend,FTResponseCode,FTResponseRemark) VALUES (";
                    //    _Cmd += "'" + _ApiNo + "', '" + _DocNo + "', '1', '', '')";
                    //}
                    //else
                    //{
                    //    _Cmd = "INSERT INTO [HITECH_MERCHAN].dbo.LOG_SR_HyperActive_API (FTApiNo, FTDocumentNo, FTStateSend,FTResponseCode,FTResponseRemark) VALUES (";
                    //    _Cmd += "'" + _ApiNo + "', '" + _DocNo + "', '" + _State + "', '" + _responseAPI.Code + "', '" + _responseAPI.Msg + "')";
                    //}
                }


                return ExecuteOnly(_Cmd);
            }
            catch (Exception ex)
            {
                return false;
                //WriteLogFile($"An error occurred: {ex.Message}");
            }

        }


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


        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            try
            {
                string JSONresult = "";
                DataTable dt = new DataTable();
                ResponseAPI responseAPI;
                XmlDocument docXML = new XmlDocument();

                string _cmd = "EXEC [HITECH_MERCHAN].[dbo].[SP_GET_DocumentNo_For_Hyperconvert_API] '2023/12/01' ";
                dt = GetDataTable(_cmd);

                // Start API 1: Production Plan
                foreach (DataRow R in dt.Select("APINo = '1'"))
                {
                    _cmd = "EXEC [HITECH_MERCHAN].dbo.SP_Send_Data_To_Hyperconvert_API1 " + R[1].ToString();
                    docXML = GetDataXML(_cmd);
                    Console.WriteLine("Get Data for API No #1 DocNo = " + R[1].ToString());
                    JSONresult = JsonConvert.SerializeObject(docXML);
                    //JsonConvert.SerializeXmlNode(docXML); //, Newtonsoft.Json.Formatting.Indented
                    JSONresult = JSONresult.Replace("\"_", "\"");
                    JSONresult = JSONresult.Replace("\"\",", "");
                    JSONresult = JSONresult.Replace("\"[]\"", "[]");
                    JSONresult = JSONresult.Replace("[[],", "[");
                    JSONresult = JSONresult.Replace("{\"root\":", "");
                    JSONresult = JSONresult.Replace("}}", "}");

                    if (JSONresult.Length > 0)
                    {
                        WriteLogFile("----------------------------------------------------");
                        WriteLogFile("API1 ### " + R[1].ToString());
                        WriteLogFile("----------------------------------------------------");
                        WriteLogFile(JSONresult);
                        WriteLogFile("----------------------------------------------------");
                        // ----------- Start Send API Process -----------
                        Console.WriteLine("Start Send API No #1 DocNo = " + R[1].ToString());

                        //responseAPI = PostDataToApi("1", R[1].ToString(), JSONresult);

                        //if (responseAPI != null)
                        //{
                        //    if (responseAPI.Code == "0")
                        //    {
                        //        SaveStateSendAPI("1", R[1].ToString(), "1", responseAPI);
                        //        Console.WriteLine("Send API No #1 DocNo = " + R[1].ToString() + " Successful.");
                        //    }
                        //    else
                        //    {
                        //        Console.WriteLine("!!! Error API No #1 DocNo = " + R[1].ToString() + " !!! @ " + responseAPI.Msg + " [Code:" + responseAPI.Code + "]");
                        //        SaveStateSendAPI("1", R[1].ToString(), "2", responseAPI);
                        //    }
                        //}
                        //else
                        //{
                        //    Console.WriteLine("Error API No #1 DocNo = " + R[1].ToString() + "!!!");
                        //}
                    }
                }

                Console.WriteLine("End API#1");


                //// Start API 2: Create Task Cutting
                foreach (DataRow R in dt.Select("APINo = '2'"))
                {
                    _cmd = "EXEC [HITECH_MERCHAN].dbo.SP_Send_Data_To_Hyperconvert_API2 " + R[1].ToString();
                    docXML = GetDataXML(_cmd);

                    JSONresult = JsonConvert.SerializeObject(docXML);
                    //JsonConvert.SerializeXmlNode(docXML); //, Newtonsoft.Json.Formatting.Indented
                    JSONresult = JSONresult.Replace("\"_", "\"");
                    JSONresult = JSONresult.Replace("\"\",", "");
                    JSONresult = JSONresult.Replace("\"[]\"", "[]");
                    JSONresult = JSONresult.Replace("[[],", "[");
                    JSONresult = JSONresult.Replace("{\"root\":", "");
                    JSONresult = JSONresult.Replace("}}", "}");

                    if (JSONresult.Length > 0)
                    {
                        WriteLogFile("----------------------------------------------------");
                        WriteLogFile("API2 ### " + R[1].ToString());
                        WriteLogFile("----------------------------------------------------");
                        WriteLogFile(JSONresult);
                        WriteLogFile("----------------------------------------------------");
                        // ----------- Start Send API Process -----------
                        Console.WriteLine("Start Send API No #2 DocNo = " + R[1].ToString());
                        //responseAPI = PostDataToApi("2", R[1].ToString(), JSONresult);

                        //if (responseAPI != null)
                        //{
                        //    if (responseAPI.Code == "0")
                        //    {
                        //        SaveStateSendAPI("2", R[1].ToString(), "1", responseAPI);
                        //        Console.WriteLine("Send API No #2 DocNo = " + R[1].ToString() + " Successful.");
                        //    }
                        //    else
                        //    {
                        //        Console.WriteLine("!!! Error API No #2 DocNo = " + R[1].ToString() + " !!! @ " + responseAPI.Msg + " [Code:" + responseAPI.Code + "]");
                        //        SaveStateSendAPI("2", R[1].ToString(), "2", responseAPI);
                        //    }
                        //}
                        //else
                        //{
                        //    Console.WriteLine("Error API No #2 DocNo = " + R[1].ToString() + "!!!");
                        //}
                    }
                }
                Console.WriteLine("End API#2");
                //// Start API 3:  Bundle Info
                foreach (DataRow R in dt.Select("APINo = '3'"))
                {
                    _cmd = "EXEC [HITECH_MERCHAN].dbo.SP_Send_Data_To_Hyperconvert_API3 " + R[1].ToString();
                    docXML = GetDataXML(_cmd);

                    JSONresult = JsonConvert.SerializeObject(docXML);
                    //JsonConvert.SerializeXmlNode(docXML); //, Newtonsoft.Json.Formatting.Indented
                    JSONresult = JSONresult.Replace("\"_", "\"");
                    JSONresult = JSONresult.Replace("\"\",", "");
                    JSONresult = JSONresult.Replace("\"[]\"", "[]");
                    JSONresult = JSONresult.Replace("[[],", "[");
                    JSONresult = JSONresult.Replace("{\"root\":", "");
                    JSONresult = JSONresult.Replace("}}", "}");

                    if (JSONresult.Length > 0)
                    {
                        WriteLogFile("----------------------------------------------------");
                        WriteLogFile("API3 ### " + R[1].ToString());
                        WriteLogFile("----------------------------------------------------");
                        WriteLogFile(JSONresult);
                        WriteLogFile("----------------------------------------------------");
                        // ----------- Start Send API Process -----------
                        Console.WriteLine("Start Send API No #3 DocNo = " + R[1].ToString());
                        //responseAPI = PostDataToApi("2", R[1].ToString(), JSONresult);

                        //if (responseAPI != null)
                        //{
                        //    if (responseAPI.Code == "0")
                        //    {
                        //        SaveStateSendAPI("2", R[1].ToString(), "1", responseAPI);
                        //        Console.WriteLine("Send API No #3 DocNo = " + R[1].ToString() + " Successful.");
                        //    }
                        //    else
                        //    {
                        //        Console.WriteLine("!!! Error API No #3 DocNo = " + R[1].ToString() + " !!! @ " + responseAPI.Msg + " [Code:" + responseAPI.Code + "]");
                        //        SaveStateSendAPI("2", R[1].ToString(), "2", responseAPI);
                        //    }
                        //}
                        //else
                        //{
                        //    Console.WriteLine("Error API No #3 DocNo = " + R[1].ToString() + "!!!");
                        //}
                    }
                }
                Console.WriteLine("End API#3");

            }
            catch (Exception ex)
            {

            }
        }

        public bool ExecuteOnly(string QryStr)
        {
            string _ConnString = "";
            System.Data.SqlClient.SqlConnection _Cnn = new System.Data.SqlClient.SqlConnection();
            System.Data.SqlClient.SqlCommand _Cmd = new System.Data.SqlClient.SqlCommand();

            try
            {
                _ConnString = "Server=hig00svr91;Database=HITECH_PRODUCTION;User Id=sa;Password=5k,mew,;";

                if (_Cnn.State == ConnectionState.Open) { _Cnn.Close(); };
                _Cnn.ConnectionString = _ConnString;
                _Cnn.Open();
                _Cmd = _Cnn.CreateCommand();
                _Cmd.CommandTimeout = 0;
                _Cmd.CommandType = CommandType.Text;
                _Cmd.CommandText = QryStr;
                _Cmd.ExecuteNonQuery();
                _Cmd.Parameters.Clear();

                _Cmd.Connection.Close();
                _Cmd.Dispose();
                if (_Cnn.State == ConnectionState.Open)
                {
                    _Cnn.Close();
                }
                _Cnn.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                _Cmd.Connection.Close();
                _Cmd.Dispose();
                if (_Cnn.State == ConnectionState.Open)
                {
                    _Cnn.Close();
                }
                _Cnn.Dispose();
                //Interaction.MsgBox(ex.Message);
                return false;
            }

        }
        public static DataTable GetDataTable(string QryStr)
        {
            DataTable objDT = new DataTable();

            try
            {
                string _ConnString = "";
                System.Data.SqlClient.SqlConnection _Cnn = new System.Data.SqlClient.SqlConnection();
                System.Data.SqlClient.SqlCommand _Cmd = new System.Data.SqlClient.SqlCommand();

                try
                {
                    _ConnString = "Server=hig00svr91;Database=HITECH_PRODUCTION;User Id=sa;Password=5k,mew,;";

                    if (_Cnn.State == ConnectionState.Open) { _Cnn.Close(); };
                    _Cnn.ConnectionString = _ConnString;
                    _Cnn.Open();
                    _Cmd = _Cnn.CreateCommand();

                    var _Adepter = new SqlDataAdapter(_Cmd);
                    _Adepter.SelectCommand.CommandTimeout = 0;
                    _Adepter.SelectCommand.CommandType = CommandType.Text;
                    _Adepter.SelectCommand.CommandText = QryStr;
                    _Adepter.Fill(objDT);
                    _Adepter.Dispose();

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

            return objDT;
        }

        public static XmlDocument GetDataXML(string QryStr)
        {
            XmlDocument objXML = new XmlDocument();
            try
            {
                string _ConnString = "";
                System.Data.SqlClient.SqlConnection _Cnn = new System.Data.SqlClient.SqlConnection();
                System.Data.SqlClient.SqlCommand _Cmd = new System.Data.SqlClient.SqlCommand();

                try
                {
                    _ConnString = "Server=hig00svr91;Database=HITECH_PRODUCTION;User Id=sa;Password=5k,mew,;";
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
        }


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
