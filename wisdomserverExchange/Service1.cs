using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using wisdomserverExchange.Class;

namespace wisdomserverExchange
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();
        DateTime scheduleDateTime;

        private readonly HttpClient httpClient;
        private readonly string apiUrl = "http://203.150.53.240:96/api/SM/ProductionPlan";
        private readonly string api2Url = "http://203.150.53.240:96/api/SM/CreateTaskCutting";
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

        private void PostDataToApi(string datajson)
        {
            try
            {
                // data to be posted
                string postData = datajson;

                // Set up the HTTP request headers
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiToken}");



                // Post data to the API
                HttpResponseMessage response = httpClient.PostAsync(apiUrl, new StringContent(postData, Encoding.UTF8, "application/json")).Result;

                // Handle the API response as needed
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Data posted successfully.");
                    WriteLogFile("Web Api called : Api Data:1 Data posted successfully " + response.ToString());
                    WriteLogFile("Web Api called : Api Data:1 Data   " + postData.ToString());
                }
                else
                {
                    WriteLogFile($"Web Api called : Api Data:1 Failed to post data. Status code: {response.StatusCode}");
                    WriteLogFile($"Web Api called : Api Data:1 Failed to post data.  " + postData.ToString());
                    string jsonContent = response.Content.ReadAsStringAsync().Result;
                    var responseAPI = JsonConvert.DeserializeObject<ResponseAPI>(jsonContent);
                    Console.WriteLine("Api Data:1 Failed Message is : " + responseAPI.Msg);
                    WriteLogFile($"Web Api called : Api Data:1 Failed Message is : " + responseAPI.Msg);
                    ////////////JsonTextReader reader = new JsonTextReader(new StringReader(jsonContent));
                    ////////////while (reader.Read())
                    ////////////{
                    ////////////    Console.WriteLine("Msg: {0}", reader.Value);
                    ////////////}
                }
            }
            catch (Exception ex)
            {
                WriteLogFile($"An error occurred: {ex.Message}");

            }
        }

        private void PostDataToApi2(string datajson)
        {
            try
            {
                // data to be posted
                string postData = datajson;

                // Set up the HTTP request headers
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiToken}");

                // Post data to the API
                HttpResponseMessage response = httpClient.PostAsync(api2Url, new StringContent(postData, Encoding.UTF8, "application/json")).Result;

                // Handle the API response as needed
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Data posted successfully.");
                    WriteLogFile("Web Api called : Api Data:2 Data posted successfully " + response.ToString());
                    WriteLogFile("Web Api called : Api Data:2 Data   " + postData.ToString());
                }
                else
                {
                    WriteLogFile($"Web Api called : Api Data:2 Failed to post data. Status code: {response.StatusCode}");
                    WriteLogFile($"Web Api called : Api Data:2 Failed to post data.  " + postData.ToString());
                    string jsonContent = response.Content.ReadAsStringAsync().Result;
                    var responseAPI = JsonConvert.DeserializeObject<ResponseAPI>(jsonContent);
                    Console.WriteLine("Api Data:2 Failed Message is : " + responseAPI.Msg);
                    WriteLogFile($"Web Api called : Api Data:2 Failed Message is : " + responseAPI.Msg);
                }
            }
            catch (Exception ex)
            {
                WriteLogFile($"An error occurred: {ex.Message}");

            }
        }


        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                XmlDocument docXML = new XmlDocument();
                string _cmd = "exec [HITECH_MERCHAN].dbo.SP_Send_Data_To_Hyperconvert_API1 ";
                docXML = GetDataXML(_cmd);

                string JSONresult = "";
                JSONresult = JsonConvert.SerializeXmlNode(docXML);
                JSONresult = JSONresult.Replace("\"_", "\"");
                JSONresult = JSONresult.Replace("{\"root\":", "");
                JSONresult = JSONresult.Replace("}}", "}");

                if (JSONresult.Length > 0)
                {
                    //PostDataToApi(JSONresult);
                }

                _cmd = "exec [HITECH_MERCHAN].dbo.SP_Send_Data_To_Hyperconvert_API2 ";
                docXML = GetDataXML(_cmd);

                JSONresult = "";
                JSONresult = JsonConvert.SerializeXmlNode(docXML);
                JSONresult = JSONresult.Replace("\"_", "\"");
                JSONresult = JSONresult.Replace("{\"root\":", "");
                JSONresult = JSONresult.Replace("}}", "}");

                if (JSONresult.Length > 0)
                {
                    PostDataToApi2(JSONresult);
                }
                //Console.WriteLine(JSONresult);
                //if (dt.Rows.Count > 0)
                //{
                //    string JSONresult;
                //    //JSONresult = JsonConvert.SerializeObject(dt);
                //    JSONresult = JsonConvert.SerializeXmlNode(doc);
                //    PostDataToApi(JSONresult);
                //}

            }
            catch (Exception ex)
            {

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
                        //objXML.DataSet.ReadXml(xmlreader);
                        objXML.Load(xmlreader);
                    }
                    xmlreader.Close();

                    //var _Adepter = new SqlDataAdapter(_Cmd);
                    //_Adepter.SelectCommand.CommandTimeout = 0;
                    //_Adepter.SelectCommand.CommandType = CommandType.Text;
                    //_Adepter.SelectCommand.CommandText = QryStr;
                    //_Adepter.Fill(objDT);
                    //_Adepter.Dispose();

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
