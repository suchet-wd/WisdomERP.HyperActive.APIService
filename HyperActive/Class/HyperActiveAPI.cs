using Newtonsoft.Json;
using System;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Xml;

namespace HyperActive
{
    public class HyperActiveAPI
    {
        private HttpClient httpClient;
        private readonly string apiToken = "6HEZZHkAAiFuWb5h";

        public HyperActiveAPI()
        {
            string startDate = "2023/06/01";
            DataTable dt = GetDataList(startDate);

            //Start API 1: Production Plan
            foreach (DataRow R in dt.Select("APINo = '1'"))
            {
                API1(R[1].ToString());
            }

            Console.WriteLine("End API#1");


            //// Start API 2: Create Task Cutting
            foreach (DataRow R in dt.Select("APINo = '2'"))
            {
                API2(R[1].ToString());
            }
            Console.WriteLine("End API#2");
        }


        private DataTable GetDataList(string startDate)
        {
            string QryStr = "EXEC " + HI.Conn.DB.GetDataBaseName(HI.Conn.DB.DataBaseName.DB_HYPERACTIVE) + 
                ".[dbo].[SP_GET_DocumentNo_For_Hyperconvert_API] '"+ startDate+ "' "; //2023/06/01
                                                                                                                                                                            //dt = GetDataTable(QryStr);
            return HI.Conn.SQLConn.GetDataTable(QryStr, HI.Conn.DB.DataBaseName.DB_HYPERACTIVE);
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

                // Post data to the API  //10.92.10.150:3002  //203.150.53.240:96
                if (_apiNo == "1")
                {
                    apiUrl = "http://203.150.53.240:96/api/SM/ProductionPlan";
                    //apiUrl = "http://10.92.10.150:3002/api/SM/ProductionPlan";
                }
                else if (_apiNo == "2")
                {
                    apiUrl = "http://203.150.53.240:96/api/SM/CreateTaskCutting";
                    //apiUrl = "http://10.92.10.150:3002/api/SM/CreateTaskCutting";
                }

                //WriteLogFile($"JSON string:" + _apiNo + " Failed Message is : " + datajson.ToString());

                HttpResponseMessage response = httpClient.PostAsync(apiUrl, new StringContent(postData, Encoding.UTF8, "application/json")).Result;

                // Handle the API response as needed
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Data posted successfully.");
                    responseAPI.Code = "0";
                    responseAPI.Msg = "";
                }
                else
                {
                    jsonContent = response.Content.ReadAsStringAsync().Result;
                    responseAPI = JsonConvert.DeserializeObject<ResponseAPI>(jsonContent);
                    responseAPI.Code = (responseAPI.Code == null) ? "" : responseAPI.Code.Trim();
                    responseAPI.Msg = (responseAPI.Msg == null) ? "" : responseAPI.Msg.Trim();
                    if (_apiNo == "1")
                    {
                        if (responseAPI.Msg.Trim() == "This Job : " + _DocNo + " already exists")
                        {
                            responseAPI.Code = "0";
                        }
                    }

                    if (_apiNo == "2")
                    {
                        if ((responseAPI.Msg.Trim().Substring(0, 8) == "PooNo : ") && (responseAPI.Msg.Trim().Substring(responseAPI.Msg.Trim().Length - 15, 15) == " already exists"))
                        {
                            responseAPI.Code = "0";
                        }
                    }
                }
                return responseAPI;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //WriteLogFile($"An error occurred: {ex.Message}");
                return null;
            }
        }

        private void API1(string DocNo)
        {
            string QryStr = "EXEC " + HI.Conn.DB.GetDataBaseName(HI.Conn.DB.DataBaseName.DB_HYPERACTIVE) + ".dbo.SP_Send_Data_To_Hyperconvert_API1 '" + DocNo + "'";
            XmlDocument docXML = HI.Conn.SQLConn.GetDataXML(QryStr, HI.Conn.DB.DataBaseName.DB_HYPERACTIVE);
            Console.WriteLine("Get Data for API No #1 DocNo = " + DocNo);
            string JSONresult = JsonConvert.SerializeObject(docXML);
            //JsonConvert.SerializeXmlNode(docXML); //, Newtonsoft.Json.Formatting.Indented
            JSONresult = JSONresult.Replace("\"[]\"", "[]");
            JSONresult = JSONresult.Replace("[[],", "[");
            JSONresult = JSONresult.Replace("{\"root\":", "");
            JSONresult = JSONresult.Replace("\"_\",", "");
            JSONresult = JSONresult.Replace("AssortQuantity\":{", "AssortQuantity\":[{");
            JSONresult = JSONresult.Replace("},\"ProductOperation", "}],\"ProductOperation");
            JSONresult = JSONresult.Substring(0, JSONresult.Length - 1);

            if (JSONresult.Length > 0)
            {
                ResponseAPI responseAPI = PostDataToApi("1", DocNo, JSONresult);

                if (responseAPI != null)
                {
                    if (responseAPI.Code == "0")
                    {
                        SaveStateSendAPI("1", DocNo, "1", responseAPI);
                        Console.WriteLine("Send API No #1 DocNo = " + DocNo + " Successful.");
                    }
                    else
                    {
                        SaveStateSendAPI("1", DocNo, "2", responseAPI);
                        Console.WriteLine("Error API No #1 DocNo = " + DocNo + " !!! @ " + responseAPI.Msg + " [Code:" + responseAPI.Code + "]");
                    }
                }
                else
                {
                    SaveStateSendAPI("1", DocNo, "2", responseAPI);
                    Console.WriteLine("Error API No #1 DocNo = " + DocNo + "!!!");
                }
            }
        } // End API1

        private void API2(string DocNo)
        {
            string QryStr = "EXEC " + HI.Conn.DB.GetDataBaseName(HI.Conn.DB.DataBaseName.DB_HYPERACTIVE) + ".dbo.SP_Send_Data_To_Hyperconvert_API2 '" + DocNo + "'";
            XmlDocument docXML = HI.Conn.SQLConn.GetDataXML(QryStr, HI.Conn.DB.DataBaseName.DB_HYPERACTIVE);
            string JSONresult = JsonConvert.SerializeObject(docXML);
            JSONresult = JSONresult.Replace("PartDetail\":[\"[]\",{", "PartDetail\":[{");
            JSONresult = JSONresult.Replace("PooDetail\":[\"[]\",", "PooDetail\":[");
            JSONresult = JSONresult.Replace("\"SpreadingRatio\":[\"[]\",", "\"SpreadingRatio\":[");
            JSONresult = JSONresult.Replace("\"\",", "");
            JSONresult = JSONresult.Replace("{\"root\":", "");
            JSONresult = JSONresult.Replace("\"Route\":[\"[]\",{\"Station", "\"Route\":[{\"Station");
            JSONresult = JSONresult.Substring(0, JSONresult.Length - 1);

            if (JSONresult.Length > 0)
            {
                ResponseAPI responseAPI = PostDataToApi("2", DocNo, JSONresult);

                if (responseAPI != null)
                {
                    if (responseAPI.Code == "0")
                    {
                        SaveStateSendAPI("2", DocNo, "1", responseAPI);
                        Console.WriteLine("Send API No #2 DocNo = " + DocNo + " Successful.");
                    }
                    else
                    {
                        if (JSONresult.Contains("\"PartDetail\":\"[]\""))
                        {
                            SaveStateSendAPI("2", DocNo, "1", new ResponseAPI("", "No PartDetails !!!"));
                            Console.WriteLine("Error API No #2 DocNo = " + DocNo + " No PartDetails !!!");
                        }
                        else
                        {
                            SaveStateSendAPI("2", DocNo, "2", responseAPI);
                            Console.WriteLine("Error API No #2 DocNo = " + DocNo + " !!! @ " + responseAPI.Msg + " [Code:" + responseAPI.Code + "]");
                        }
                    }
                }
                else
                {
                    if (JSONresult.Contains("\"PartDetail\":\"[]\""))
                    {
                        SaveStateSendAPI("2", DocNo, "1", new ResponseAPI("", "No PartDetails !!!"));
                        Console.WriteLine("Error API No #2 DocNo = " + DocNo + " No PartDetails !!!");
                    }
                    else
                    {
                        SaveStateSendAPI("2", DocNo, "2", responseAPI);
                        Console.WriteLine("Error API No #2 DocNo = " + DocNo + " !!! @ No Response");
                    }
                }
            }
        } // End API2

        private bool SaveStateSendAPI(string _ApiNo, string _DocNo, string _State, ResponseAPI _responseAPI)
        {
            try
            {
                string _Cmd = "";
                if (_responseAPI == null)
                {
                    _responseAPI = new ResponseAPI("", "Server not response !!!");

                }

                _Cmd = "DECLARE @Date varchar(10) = Convert(varchar(10), Getdate(), 111) \n";
                _Cmd += "DECLARE @Time varchar(10) = Convert(varchar(8), Getdate(), 114) \n\n";
                _Cmd += "IF EXISTS( SELECT TOP 1 l.FTApiNo + '.' + l.FTDocumentNo \n";
                _Cmd += "        FROM [HITECH_HYPERACTIVE].dbo.LOG_SR_HyperActive_API AS l WITH(NOLOCK) \n";
                _Cmd += "        WHERE l.FTApiNo = '" + _ApiNo + "' AND l.FTDocumentNo = '" + _DocNo + "') \n";
                _Cmd += "BEGIN \n";
                _Cmd += "      UPDATE [HITECH_HYPERACTIVE].dbo.LOG_SR_HyperActive_API \n";
                _Cmd += "      SET FTStateSend = '" + _State + "', FTResponseRemark = '" + _responseAPI.Msg + "' \n";
                _Cmd += "      , FDDocDate = @Date , FTDocTime = @Time , FTDocState = ''\n";
                _Cmd += "      WHERE FTApiNo = '" + _ApiNo + "' AND FTDocumentNo = '" + _DocNo + "' \n";
                _Cmd += "END \n";
                _Cmd += "ELSE \n";
                _Cmd += "BEGIN \n";
                _Cmd += "      INSERT INTO [HITECH_HYPERACTIVE].dbo.LOG_SR_HyperActive_API \n";
                _Cmd += "      (FTApiNo, FTDocumentNo, FTStateSend, FTResponseCode, FTResponseRemark, FDDocDate, FTDocTime, FTDocState) VALUES \n";
                _Cmd += "      ('" + _ApiNo + "', '" + _DocNo + "', '" + _State + "', '" + _responseAPI.Code + "', '" + _responseAPI.Msg + "' , @Date, @Time, '')\n";
                _Cmd += "END \n";

                return HI.Conn.SQLConn.ExecuteOnly(_Cmd, HI.Conn.DB.DataBaseName.DB_HYPERACTIVE);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        } // End SaveStateSendAPI
    }
}
