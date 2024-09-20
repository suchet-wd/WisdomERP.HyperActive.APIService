using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Xml;

namespace HyperActive
{
    public class HyperActiveAPI
    {
        private HttpClient httpClient;
        private readonly string apiToken = HI.Conn.DB._apiToken;
        //"6HEZZHkAAiFuWb5h";

        public HyperActiveAPI()
        {

            //string startDate = HI.Conn.DB._StartDate; //"2023/10/01";
            DataTable dt = GetDataList();

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

            //// Start API 6: Bundle Update
            foreach (DataRow R in dt.Select("APINo = '6'"))
            {
                API6(R[1].ToString());
            }
            Console.WriteLine("End API#6 : Bundle Update");

            //// Start API 7: 
            foreach (DataRow R in dt.Select("APINo = '7'"))
            {
                API7(R[1].ToString());
            }
            Console.WriteLine("End API#7 Get Station Results");

            //// Start API 9: 
            foreach (DataRow R in dt.Select("APINo = '9'"))
            {
                API9(R[1].ToString());
            }
            Console.WriteLine("End API#9 Get Pack Results");

            //// Start API 10: 
            foreach (DataRow R in dt.Select("APINo = '10'"))
            {
                API10(R[1].ToString());
            }
            //// Start API 11: 
            foreach (DataRow R in dt.Select("APINo = '11'"))
            {
                API11(R[1].ToString());
            }
            Console.WriteLine("End API#10 Get OutSourse Status");
        }


        private DataTable GetDataList()
        //string startDate
        {
            string QryStr = "EXEC " + HI.Conn.DB.GetDataBaseName(HI.Conn.DB.DataBaseName.DB_HYPERACTIVE) +
                ".[dbo].[SP_GET_DocumentNo_For_Hyperconvert_API] ";
            //'" + startDate + "' "; //2023/06/01
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
                ResponseAPI responseAPI = new ResponseAPI();
                //httpClient.Dispose();
                // Set up the HTTP request headers
                httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiToken}");

                // Post data to the API  //10.92.10.150:3002  //203.150.53.240:96
                if (_apiNo == "1")
                {
                    apiUrl = HI.Conn.DB.GetHyperActiveAPIName(HI.Conn.DB.HyperActiveName.api1);
                }
                else if (_apiNo == "2")
                {
                    apiUrl = HI.Conn.DB.GetHyperActiveAPIName(HI.Conn.DB.HyperActiveName.api2);
                }
                else if (_apiNo == "6")
                {
                    apiUrl = HI.Conn.DB.GetHyperActiveAPIName(HI.Conn.DB.HyperActiveName.api6);
                }
                else if (_apiNo == "7")
                {
                    apiUrl = HI.Conn.DB.GetHyperActiveAPIName(HI.Conn.DB.HyperActiveName.api7);
                }
                else if (_apiNo == "9")
                {
                    apiUrl = HI.Conn.DB.GetHyperActiveAPIName(HI.Conn.DB.HyperActiveName.api9);
                }
                else if (_apiNo == "10")
                {
                    apiUrl = HI.Conn.DB.GetHyperActiveAPIName(HI.Conn.DB.HyperActiveName.api10);
                }
                else if (_apiNo == "11")
                {
                    apiUrl = HI.Conn.DB.GetHyperActiveAPIName(HI.Conn.DB.HyperActiveName.api11);
                }
                //WriteLogFile($"JSON string:" + _apiNo + " Failed Message is : " + datajson.ToString());

                HttpResponseMessage response = httpClient.PostAsync(apiUrl, new StringContent(postData, Encoding.UTF8, "application/json")).Result;

                if (_apiNo != "7")
                {
                    // Handle the API response as needed
                    // 0 = normal, 1 = error
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
                            if ((responseAPI.Msg.Trim().Substring(0, 8) == "PooNo : ") &&
                                (responseAPI.Msg.Trim().Substring(responseAPI.Msg.Trim().Length - 15, 15) == " already exists"))
                            {
                                responseAPI.Code = "0";
                            }
                        }
                    }
                }
                else if (_apiNo == "7") // For API #7 Only
                {
                    //List<BundleRfidBarcodeList> bundleRfidBarcodeList = null;
                    //ResponseAPI(string boxRfid, string boxBarcode, string code, string msg, List<BundleRfidBarcodeList> bundleRfidBarcodeList)

                    ResponseAPI responseAPI7 = new ResponseAPI();
                    // Handle the API response as needed
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Data posted successfully.");
                        jsonContent = response.Content.ReadAsStringAsync().Result;
                        responseAPI7 = JsonConvert.DeserializeObject<ResponseAPI>(jsonContent);
                        //responseAPI7.BoxBarcode 
                        //responseAPI7.BoxRfid
                        //responseAPI7.Code = "0";
                        //responseAPI7.Msg = "";
                    }
                    else
                    {
                        jsonContent = response.Content.ReadAsStringAsync().Result;
                        responseAPI7 = JsonConvert.DeserializeObject<ResponseAPI>(jsonContent);
                        responseAPI7.Code = (responseAPI7.Code == null) ? "" : responseAPI7.Code.Trim();
                        responseAPI7.Msg = (responseAPI7.Msg == null) ? "" : responseAPI7.Msg.Trim();
                    }
                    return responseAPI7;
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

        // -----------------------------------------------------------------------------------------------------------------

        private void API1(string DocNo)
        {
            string QryStr = "EXEC " + HI.Conn.DB.GetDataBaseName(HI.Conn.DB.DataBaseName.DB_HYPERACTIVE) +
                ".dbo.SP_Send_Data_To_Hyperconvert_API1 '" + DocNo + "'";
            XmlDocument docXML = HI.Conn.SQLConn.GetDataXML(QryStr, HI.Conn.DB.DataBaseName.DB_HYPERACTIVE);
            Console.WriteLine("Get Data for API No #1 DocNo = " + DocNo);
            string JSONresult = JsonConvert.SerializeObject(docXML);
            //JsonConvert.SerializeXmlNode(docXML); //, Newtonsoft.Json.Formatting.Indented
            JSONresult = JSONresult.Replace("\"[]\"", "[]");
            JSONresult = JSONresult.Replace("[[],", "[");
            JSONresult = JSONresult.Replace("{\"root\":", "");
            JSONresult = JSONresult.Replace("\"_\",", "\"\",");
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
                        //_ApiNo, _DocNo, _State, _responseAPI, JSONresult
                        SaveStateSendAPI("1", DocNo, "1", responseAPI, JSONresult);
                        Console.WriteLine("Send API No #1 DocNo = " + DocNo + " Successful.");
                    }
                    else
                    {
                        //_ApiNo, _DocNo, _State, _responseAPI, JSONresult
                        SaveStateSendAPI("1", DocNo, "2", responseAPI, JSONresult);
                        Console.WriteLine("Error API No #1 DocNo = " + DocNo + " !!! @ " + responseAPI.Msg + " [Code:" + responseAPI.Code + "]");
                    }
                }
                else
                {
                    //_ApiNo, _DocNo, _State, _responseAPI, JSONresult
                    SaveStateSendAPI("1", DocNo, "2", responseAPI, JSONresult);
                    Console.WriteLine("Error API No #1 DocNo = " + DocNo + "!!!");
                }
            }
        } // End API1

        // -----------------------------------------------------------------------------------------------------------------

        private void API2(string DocNo)
        {
            string QryStr = "EXEC " + HI.Conn.DB.GetDataBaseName(HI.Conn.DB.DataBaseName.DB_HYPERACTIVE) + ".dbo.SP_Send_Data_To_Hyperconvert_API2 '" + DocNo + "'";
            XmlDocument docXML = HI.Conn.SQLConn.GetDataXML(QryStr, HI.Conn.DB.DataBaseName.DB_HYPERACTIVE);
            string JSONresult = JsonConvert.SerializeObject(docXML);
            JSONresult = JSONresult.Replace("PartDetail\":[\"[]\",{", "PartDetail\":[{");               // Case One Record
            JSONresult = JSONresult.Replace("PooDetail\":[\"[]\",", "PooDetail\":[");                   // Case One Record
            JSONresult = JSONresult.Replace("\"SpreadingRatio\":[\"[]\",", "\"SpreadingRatio\":[");     // Case one record
            JSONresult = JSONresult.Replace("\"SpreadingRatio\":\"[]\",", "\"SpreadingRatio\":[],");  // Case Null
            JSONresult = JSONresult.Replace("\"\",", "");
            JSONresult = JSONresult.Replace("{\"root\":", "");
            JSONresult = JSONresult.Replace("\"Route\":[\"[]\",{\"Station", "\"Route\":[{\"Station");
            JSONresult = JSONresult.Replace("\"Route\": \"[]\",", "\"Route\": [],");                    // Case Null
            JSONresult = JSONresult.Replace("PartDetail\":[\"[]\"],{", "PartDetail\":[],{");
            JSONresult = JSONresult.Replace("\"PartDetail\":\"[]\",", "\"PartDetail\":[],");          // Case Null
            JSONresult = JSONresult.Replace("\"PartDetail\":\"[]\"}", "\"PartDetail\":[]}");          // Case Null
            JSONresult = JSONresult.Replace("[{\"Station\":null,\"FactoryNo\":null}]", "[]");
            JSONresult = JSONresult.Substring(0, JSONresult.Length - 1);

            if (JSONresult.Length > 0)
            {
                ResponseAPI responseAPI = PostDataToApi("2", DocNo, JSONresult);

                if (responseAPI != null)
                {
                    if (responseAPI.Code == "0")
                    {
                        //_ApiNo, _DocNo, _State, _responseAPI, JSONresult
                        SaveStateSendAPI("2", DocNo, "1", responseAPI, JSONresult);
                        Console.WriteLine("Send API No #2 DocNo = " + DocNo + " Successful.");
                    }
                    else
                    {
                        //if (JSONresult.Contains("\"PartDetail\":\"[]\""))
                        //{
                        //    //_ApiNo, _DocNo, _State, _responseAPI, JSONresult
                        //    SaveStateSendAPI("2", DocNo, "2",
                        //        responseAPI.Msg.ToString() == "" ? new ResponseAPI("", "No PartDetails !!!") : responseAPI,
                        //        JSONresult);
                        //    SaveAllSendAPI("1", DocNo, "1", responseAPI, JSONresult);
                        //    Console.WriteLine("Error API No #2 DocNo = " + DocNo + " No PartDetails !!!");
                        //}
                        //else
                        //{
                        //    //_ApiNo, _DocNo, _State, _responseAPI, JSONresult
                        //    SaveStateSendAPI("2", DocNo, "2", responseAPI, JSONresult);
                        //    SaveAllSendAPI("1", DocNo, "1", responseAPI, JSONresult);
                        //    Console.WriteLine("Error API No #2 DocNo = " + DocNo + " !!! @ " + responseAPI.Msg + " [Code:" + responseAPI.Code + "]");
                        //}

                        SaveStateSendAPI("2", DocNo, "2", responseAPI, JSONresult);
                        Console.WriteLine("Error API No #2 DocNo = " + DocNo + " !!! @ " + responseAPI.Msg + " [Code:" + responseAPI.Code + "]");
                    }
                }
                else
                {
                    //if (JSONresult.Contains("\"PartDetail\":\"[]\""))
                    //{
                    //    //_ApiNo, _DocNo, _State, _responseAPI, JSONresult
                    //    SaveStateSendAPI("2", DocNo, "2",
                    //        responseAPI.Msg.ToString() == "" ? new ResponseAPI("", "No PartDetails !!!") : responseAPI,
                    //        JSONresult);
                    //    Console.WriteLine("Error API No #2 DocNo = " + DocNo + " No PartDetails !!!");
                    //}
                    //else
                    //{
                    //    //_ApiNo, _DocNo, _State, _responseAPI, JSONresult
                    //    SaveStateSendAPI("2", DocNo, "2", responseAPI, JSONresult);
                    //    Console.WriteLine("Error API No #2 DocNo = " + DocNo + " !!! @ No Response");
                    //}
                    ////_ApiNo, _DocNo, _State, _responseAPI, JSONresult
                    SaveStateSendAPI("2", DocNo, "2", responseAPI, JSONresult);
                    Console.WriteLine("Error API No #2 DocNo = " + DocNo + " !!! @ No Response");
                }
            }
        } // End API2

        // -----------------------------------------------------------------------------------------------------------------

        private void API6(string DocNo)     //API_6 : Bundle Update
        {
            string QryStr = "EXEC " + HI.Conn.DB.GetDataBaseName(HI.Conn.DB.DataBaseName.DB_HYPERACTIVE) +
                ".dbo.SP_Send_Data_To_Hyperconvert_API6 @BundleBarCode = '" + DocNo + "'";
            XmlDocument docXML = HI.Conn.SQLConn.GetDataXML(QryStr, HI.Conn.DB.DataBaseName.DB_HYPERACTIVE);
            Console.WriteLine("Get Data for API No #6 : Bundle Update - DocNo = " + DocNo);
            string JSONresult = JsonConvert.SerializeObject(docXML);
            //JsonConvert.SerializeXmlNode(docXML); //, Newtonsoft.Json.Formatting.Indented
            JSONresult = JSONresult.Replace("\"[]\"", "[]");
            JSONresult = JSONresult.Replace("[[],", "[");
            JSONresult = JSONresult.Replace("{\"root\":", "");
            JSONresult = JSONresult.Replace("\"_\",", "\"\",");
            JSONresult = JSONresult.Substring(0, JSONresult.Length - 1);
            JSONresult = JSONresult.Replace("}]}", "}]");
            JSONresult = JSONresult.Replace("{\"DefectDetails\":[", "[");

            if (JSONresult.Length > 0)
            {
                ResponseAPI responseAPI = PostDataToApi("6", DocNo, JSONresult);

                if (responseAPI != null)
                {
                    if (responseAPI.Code == "0")
                    {
                        //_ApiNo, _DocNo, _State, _responseAPI, JSONresult
                        SaveStateSendAPI("6", DocNo, "1", responseAPI, JSONresult);
                        Console.WriteLine("Send API No #6 : Bundle Update - DocNo = " + DocNo + " Successful.");
                    }
                    else
                    {
                        //_ApiNo, _DocNo, _State, _responseAPI, JSONresult
                        SaveStateSendAPI("6", DocNo, "2", responseAPI, JSONresult);
                        Console.WriteLine("Error API No #6 : Bundle Update - DocNo = " + DocNo + " !!! @ " + responseAPI.Msg + " [Code:" + responseAPI.Code + "]");
                    }
                }
                else
                {
                    //_ApiNo, _DocNo, _State, _responseAPI, JSONresult
                    SaveStateSendAPI("6", DocNo, "2", responseAPI, JSONresult);
                    Console.WriteLine("Error API No #6 : Bundle Update - DocNo = " + DocNo + "!!!");
                }
            }
        } // End API6 : Bundle Update

        // -----------------------------------------------------------------------------------------------------------------

        private void API7(string DocNo)     //API_7 : Station Results
        {
            string QryStr = "EXEC " + HI.Conn.DB.GetDataBaseName(HI.Conn.DB.DataBaseName.DB_HYPERACTIVE) +
                ".dbo.SP_Send_Data_To_Hyperconvert_API7 '" + DocNo + "'";
            XmlDocument docXML = HI.Conn.SQLConn.GetDataXML(QryStr, HI.Conn.DB.DataBaseName.DB_HYPERACTIVE);
            Console.WriteLine("Get Data for API No #7 : Station Results - DocNo = " + DocNo);
            string JSONresult = JsonConvert.SerializeObject(docXML);
            //JsonConvert.SerializeXmlNode(docXML); //, Newtonsoft.Json.Formatting.Indented
            JSONresult = JSONresult.Replace("\"[]\"", "[]");
            JSONresult = JSONresult.Replace("[[],", "[");
            JSONresult = JSONresult.Replace("{\"root\":", "");
            JSONresult = JSONresult.Replace("\"_\",", "\"\",");
            JSONresult = JSONresult.Substring(0, JSONresult.Length - 1);

            if (JSONresult.Length > 0)
            {
                ResponseAPI responseAPI = PostDataToApi("7", DocNo, JSONresult);

                if (responseAPI != null && responseAPI.BundleList != null)
                {
                    // 0 = normal, 1 = error
                    if (responseAPI.Code == "0")
                    {
                        //_ApiNo, _DocNo, _State, _responseAPI, JSONresult
                        SaveStateSendAPI("7", DocNo, "1", responseAPI, JSONresult);
                        Console.WriteLine("Send API No #7 : Station Results - DocNo = " + DocNo + " Successful.");
                    }
                    else if (responseAPI.Code == "1")
                    {
                        //_ApiNo, _DocNo, _State, _responseAPI, JSONresult
                        SaveStateSendAPI("7", DocNo, "2", responseAPI, JSONresult);
                        Console.WriteLine("Error API No #7 : Station Results - DocNo = " + DocNo + " !!! @ " + responseAPI.Msg + " [Code:" + responseAPI.Code + "]");
                    }
                    else
                    {
                        //_ApiNo, _DocNo, _State, _responseAPI, JSONresult
                        SaveStateSendAPI("7", DocNo, "2", responseAPI, JSONresult);
                        Console.WriteLine("Error API No #7 : Station Results - DocNo = " + DocNo + " !!! @ " + responseAPI.Msg + " [Code:" + responseAPI.Code + "]");
                    }
                }
                else
                {
                    //_ApiNo, _DocNo, _State, _responseAPI, JSONresult
                    SaveStateSendAPI("7", DocNo, "2", responseAPI, JSONresult);
                    Console.WriteLine("Error API No #7 : Station Results - DocNo = " + DocNo + "!!!");
                }

            } // End if (JSONresult.Length > 0)

        } // End API7 : Station Results

        // -----------------------------------------------------------------------------------------------------------------

        private void API9(string DocNo)     //API_9 : Pack Results
        {
            string QryStr = "EXEC " + HI.Conn.DB.GetDataBaseName(HI.Conn.DB.DataBaseName.DB_HYPERACTIVE) +
                ".dbo.SP_Send_Data_To_Hyperconvert_API9 ";
            //".dbo.SP_Send_Data_To_Hyperconvert_API9 @DateStart ='" + DocNo + "', @DateEnd = '" + DocNo + "'";
            XmlDocument docXML = HI.Conn.SQLConn.GetDataXML(QryStr, HI.Conn.DB.DataBaseName.DB_HYPERACTIVE);
            Console.WriteLine("Get Data for API No #9 : Pack Results -  DocNo = " + DocNo);
            string JSONresult = JsonConvert.SerializeObject(docXML);
            JSONresult = JsonConvert.SerializeObject(docXML);
            JSONresult = JSONresult.Replace("\"[]\"", "[]");
            JSONresult = JSONresult.Replace(":\"_", ":\"");
            JSONresult = JSONresult.Replace(":[[],{\"", ":[{\"");
            JSONresult = JSONresult.Replace("{\"root\":", "");
            JSONresult = JSONresult.Replace("\"_\",", "\"\",");
            JSONresult = JSONresult.Replace("{\"PackResults\":[", "[");
            JSONresult = JSONresult.Substring(0, JSONresult.Length - 2);
            ////////JsonConvert.SerializeXmlNode(docXML); //, Newtonsoft.Json.Formatting.Indented
            //////JSONresult = JSONresult.Replace("\"[]\"", "[]");
            //////JSONresult = JSONresult.Replace("[[],", "[");
            //////JSONresult = JSONresult.Replace(":\"_", ":\"");
            ////////JSONresult = JSONresult.Replace("}}", "}");
            //////JSONresult = JSONresult.Replace("{\"root\":", "");
            //////JSONresult = JSONresult.Replace("\"_\",", "\"\",");
            //////JSONresult = JSONresult.Replace("{\"PackResults\":[", "[");
            //////JSONresult = JSONresult.Replace("]}", "]");
            //////JSONresult = JSONresult.Substring(0, JSONresult.Length - 1);

            if (JSONresult.Length > 0)
            {
                ResponseAPI responseAPI = PostDataToApi("9", DocNo, JSONresult);

                if (responseAPI != null)
                {
                    if (responseAPI.Code == "0" && UpdateStateAPI9())
                    {
                        //_ApiNo, _DocNo, _State, _responseAPI, JSONresult
                        SaveStateSendAPI("9", DocNo, "1", responseAPI, JSONresult);
                        Console.WriteLine("Send API No #9 : Pack Results -  DocNo = " + DocNo + " Successful.");
                    }
                    else
                    {
                        RollBackStateAPI9();
                        //_ApiNo, _DocNo, _State, _responseAPI, JSONresult
                        SaveStateSendAPI("9", DocNo, "2", responseAPI, JSONresult);
                        Console.WriteLine("Error API No #9 : Pack Results -  DocNo = " + DocNo + " !!! @ " + responseAPI.Msg + " [Code:" + responseAPI.Code + "]");
                    }
                }
                else
                {
                    RollBackStateAPI9();
                    //_ApiNo, _DocNo, _State, _responseAPI, JSONresult
                    SaveStateSendAPI("9", DocNo, "2", responseAPI, JSONresult);
                    Console.WriteLine("Error API No #9 : Pack Results - DocNo = " + DocNo + "!!!");
                }
            }
        } // End API_9 : Pack Results

        private void API10(string DocNo)
        {
            string QryStr = "EXEC " + HI.Conn.DB.GetDataBaseName(HI.Conn.DB.DataBaseName.DB_HYPERACTIVE) +
                ".dbo.SP_Send_Data_To_Hyperconvert_API10 @BarcodeSendSuplNo = '" + DocNo + "'";
            XmlDocument docXML = HI.Conn.SQLConn.GetDataXML(QryStr, HI.Conn.DB.DataBaseName.DB_HYPERACTIVE);
            Console.WriteLine("Get Data for API No #10 : Out Sourse Status DocNo = " + DocNo);
            string JSONresult = JsonConvert.SerializeObject(docXML);
            //JsonConvert.SerializeXmlNode(docXML); //, Newtonsoft.Json.Formatting.Indented
            JSONresult = JSONresult.Replace("\"[]\"", "[]");
            JSONresult = JSONresult.Replace("[[],", "[");
            JSONresult = JSONresult.Replace("{\"root\":", "");
            JSONresult = JSONresult.Replace("\"_\",", "\"\",");
            JSONresult = JSONresult.Substring(0, JSONresult.Length - 1);
            JSONresult = "[" + JSONresult + "]";

            if (JSONresult.Length > 0)
            {
                ResponseAPI responseAPI = PostDataToApi("10", DocNo, JSONresult);

                if (responseAPI != null)
                {
                    if (responseAPI.Code == "0")
                    {
                        //_ApiNo, _DocNo, _State, _responseAPI, JSONresult
                        SaveStateSendAPI("10", DocNo, "1", responseAPI, JSONresult);
                        Console.WriteLine("Send API No #10 : Out Sourse Status DocNo = " + DocNo + " Successful.");
                    }
                    else
                    {
                        //_ApiNo, _DocNo, _State, _responseAPI, JSONresult
                        SaveStateSendAPI("10", DocNo, "2", responseAPI, JSONresult);
                        Console.WriteLine("Error API No #10 : Out Sourse Status DocNo = " + DocNo + " !!! @ " + responseAPI.Msg + " [Code:" + responseAPI.Code + "]");
                    }
                }
                else
                {
                    //_ApiNo, _DocNo, _State, _responseAPI, JSONresult
                    SaveStateSendAPI("10", DocNo, "2", responseAPI, JSONresult);
                    Console.WriteLine("Error API No #10 : Out Sourse Status DocNo = " + DocNo + "!!!");
                }
            }
        } // End API_10 : Out Sourse Status


        private void API11(string DocNo) //Send Acc Info 
        {
            string QryStr = "EXEC " + HI.Conn.DB.GetDataBaseName(HI.Conn.DB.DataBaseName.DB_HYPERACTIVE) +
                ".dbo.SP_Send_Data_To_Hyperconvert_API11 @Doc = '" + DocNo + "'";
            XmlDocument docXML = HI.Conn.SQLConn.GetDataXML(QryStr, HI.Conn.DB.DataBaseName.DB_HYPERACTIVE);
            Console.WriteLine("Get Data for API No #11 : Send Acc Info  DocNo = " + DocNo);
            string JSONresult = JsonConvert.SerializeObject(docXML);
            //JsonConvert.SerializeXmlNode(docXML); //, Newtonsoft.Json.Formatting.Indented
            JSONresult = JSONresult.Replace("\"[]\"", "[]");
            JSONresult = JSONresult.Replace("[[],", "[");
            JSONresult = JSONresult.Replace("{\"root\":", "");
            JSONresult = JSONresult.Replace("\"_\",", "\"\",");
            JSONresult = JSONresult.Substring(0, JSONresult.Length - 1);
            JSONresult = "[" + JSONresult + "]";

            if (JSONresult.Length > 0)
            {
                ResponseAPI responseAPI = PostDataToApi("11", DocNo, JSONresult);

                if (responseAPI != null)
                {
                    if (responseAPI.Code == "0")
                    {
                        //_ApiNo, _DocNo, _State, _responseAPI, JSONresult
                        SaveStateSendAPI("11", DocNo, "1", responseAPI, JSONresult);
                        Console.WriteLine("Send API No #11 : Send Acc Info DocNo = " + DocNo + " Successful.");
                    }
                    else
                    {
                        //_ApiNo, _DocNo, _State, _responseAPI, JSONresult
                        SaveStateSendAPI("11", DocNo, "2", responseAPI, JSONresult);
                        Console.WriteLine("Error API No #11 : Send Acc Info DocNo = " + DocNo + " !!! @ " + 
                            responseAPI.Msg + " [Code:" + responseAPI.Code + "]");
                    }
                }
                else
                {
                    //_ApiNo, _DocNo, _State, _responseAPI, JSONresult
                    SaveStateSendAPI("11", DocNo, "2", responseAPI, JSONresult);
                    Console.WriteLine("Error API No #11 : Send Acc Info DocNo = " + DocNo + "!!!");
                }
            }
        } // End API_11 : Send ACC Info

        private bool SaveStateSendAPI(string _ApiNo, string _DocNo, string _State, ResponseAPI _responseAPI, string JSONresult)
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

                _Cmd += SaveAllSendAPI(_ApiNo, _DocNo, _State, _responseAPI, JSONresult);

                _Cmd += "IF EXISTS( SELECT TOP 1 l.FTApiNo + '.' + l.FTDocumentNo \n";
                _Cmd += "        FROM " + HI.Conn.DB.GetDataBaseName(HI.Conn.DB.DataBaseName.DB_HYPERACTIVE) + ".dbo.LOG_SR_HyperActive_API AS l WITH(NOLOCK) \n";
                _Cmd += "        WHERE l.FTApiNo = '" + _ApiNo + "' AND l.FTDocumentNo = '" + _DocNo + "') \n";
                _Cmd += "BEGIN \n";
                _Cmd += "      UPDATE " + HI.Conn.DB.GetDataBaseName(HI.Conn.DB.DataBaseName.DB_HYPERACTIVE) + ".dbo.LOG_SR_HyperActive_API \n";
                _Cmd += "      SET FTStateSend = '" + _State + "', FTResponseRemark = '" + _responseAPI.Msg + "' \n";
                _Cmd += "      , FDDocDate = @Date , FTDocTime = @Time , FTDocState = ''\n";
                _Cmd += "      , FTJson = '" + JSONresult + "'";
                //_Cmd += "      , FTJsonResponse = '" + _responseAPI + "'";
                _Cmd += "      WHERE FTApiNo = '" + _ApiNo + "' AND FTDocumentNo = '" + _DocNo + "' \n";
                _Cmd += "\n\n";

                _Cmd += UpdateStatus(_ApiNo, _State, _DocNo); // Add Query Update Status

                _Cmd += "END \n";
                _Cmd += "ELSE \n";
                _Cmd += "BEGIN \n";
                _Cmd += "      INSERT INTO " + HI.Conn.DB.GetDataBaseName(HI.Conn.DB.DataBaseName.DB_HYPERACTIVE) + ".dbo.LOG_SR_HyperActive_API \n";
                _Cmd += "      (FTApiNo, FTDocumentNo, FTStateSend, FTResponseCode, FTResponseRemark, FDDocDate, FTDocTime, FTDocState, FTJson) VALUES \n";
                _Cmd += "      ('" + _ApiNo + "', '" + _DocNo + "', '" + _State + "', '" + _responseAPI.Code + "', '" + _responseAPI.Msg + "' , @Date, @Time, '', '" + JSONresult + "')\n";
                _Cmd += "\n\n";

                _Cmd += UpdateStatus(_ApiNo, _State, _DocNo);           // Add Query Update Status

                _Cmd += "END \n";

                return HI.Conn.SQLConn.ExecuteOnly(_Cmd, HI.Conn.DB.DataBaseName.DB_HYPERACTIVE);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        } // End SaveStateSendAPI

        // Start SaveAllSendAPI
        private string SaveAllSendAPI(string _ApiNo, string _DocNo, string _State, ResponseAPI _responseAPI, string JSONresult)
        {
            try
            {
                string _Cmd = "";
                //_Cmd = "DECLARE @Date varchar(10) = Convert(varchar(10), Getdate(), 111) \n";
                //_Cmd += "DECLARE @Time varchar(10) = Convert(varchar(8), Getdate(), 114) \n\n";
                _Cmd = "BEGIN \n";
                _Cmd += "INSERT INTO " + HI.Conn.DB.GetDataBaseName(HI.Conn.DB.DataBaseName.DB_HYPERACTIVE) + ".dbo.LOG_AllSend_HyperActive_API \n";
                _Cmd += "(FTApiNo, FTDocumentNo, FTStateSend, FTResponseCode, FTResponseRemark, FDDocDate, FTDocTime, FTDocState, FTJson) VALUES \n";
                _Cmd += "('" + _ApiNo + "', '" + _DocNo + "', '" + _State + "', '" + _responseAPI.Code + "', '" + _responseAPI.Msg + "' , @Date, @Time, '', '" + JSONresult + "')\n";
                _Cmd += "END \n";

                return _Cmd;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        } // End SaveAllSendAPI

        // Start UpdateStatus(string _ApiNo, string _State)
        private string UpdateStatus(string _ApiNo, string _State, string _DocNo)
        {
            string _Cmd = "";
            if (_ApiNo == "1" && _State == "1")
            {
                _Cmd += "      UPDATE " + HI.Conn.DB.GetDataBaseName(HI.Conn.DB.DataBaseName.DB_PROD) + ".dbo.TPRODTBundle \n";
                _Cmd += "      SET FTStateExport = '1', FTExportDate = @Date , FTExportTime = @Time \n";
                _Cmd += "      WHERE FTOrderProdNo IN (SELECT DISTINCT op.FTOrderProdNo FROM " + HI.Conn.DB.GetDataBaseName(HI.Conn.DB.DataBaseName.DB_PROD) + ".dbo.TPRODTOrderProd AS op WITH (NOLOCK) WHERE op.FTOrderNo = '" + _DocNo + "') \n";
            }
            if (_ApiNo == "2" && _State == "1")
            {
                _Cmd += "      UPDATE " + HI.Conn.DB.GetDataBaseName(HI.Conn.DB.DataBaseName.DB_PROD) + ".dbo.TPRODTBundle \n";
                _Cmd += "      SET FTStateExport2 = '1', FTExportDate2 = @Date , FTExportTime2 = @Time \n";
                _Cmd += "      WHERE FTOrderProdNo IN (SELECT DISTINCT op.FTOrderProdNo FROM " + HI.Conn.DB.GetDataBaseName(HI.Conn.DB.DataBaseName.DB_PROD) + ".dbo.TPRODTOrderProd AS op WITH (NOLOCK) WHERE op.FTOrderNo = '" + _DocNo + "') \n";
            }
            if (_ApiNo == "6" && _State == "1")
            {
                _Cmd += "      UPDATE " + HI.Conn.DB.GetDataBaseName(HI.Conn.DB.DataBaseName.DB_HYPERACTIVE) + ".dbo.TPROSendSuplDefect \n";
                _Cmd += "      SET FTStateSendAPI6 = '1', FTStateSendAPI6 = @Date , FTSendTime6 = @Time  \n";
                _Cmd += "      WHERE FTBarcodeSendSuplNo = '" + _DocNo + "' \n";
            }
            if (_ApiNo == "7" && _State == "1")
            {
                _Cmd += "      UPDATE " + HI.Conn.DB.GetDataBaseName(HI.Conn.DB.DataBaseName.DB_HYPERACTIVE) + ".dbo.TPROSendSuplDefect \n";
                _Cmd += "      SET FTStateSend_API7 = '1', FDSendDate_API7 = @Date , FTSendTime_API7 = @Time  \n";
                _Cmd += "      WHERE FTBarcodeSendSuplNo = '" + _DocNo + "' \n";
            }
            if (_ApiNo == "9" && _State == "1")
            {
                _Cmd += "      UPDATE " + HI.Conn.DB.GetDataBaseName(HI.Conn.DB.DataBaseName.DB_PROD) + ".dbo.TPRODBarcodeScanOutline \n";
                _Cmd += "      SET FTStateExport = '1', FDExportDate = @Date , FTExportTime = @Time \n";
                _Cmd += "      WHERE FDInsDate = '" + _DocNo + "' \n";
            }
            if (_ApiNo == "10" && _State == "1")
            {
                _Cmd += "      UPDATE " + HI.Conn.DB.GetDataBaseName(HI.Conn.DB.DataBaseName.DB_HYPERACTIVE) + ".dbo.TPROSendSuplDefect \n";
                _Cmd += "      SET FTStateSend_API10 = '1', FDSendDate_API10 = @Date , FTSendTime_API10 = @Time  \n";
                _Cmd += "      WHERE FTBarcodeSendSuplNo = '" + _DocNo + "' \n";
            }
            if (_ApiNo == "11" && _State == "1")
            {
                _Cmd += "      UPDATE " + HI.Conn.DB.GetDataBaseName(HI.Conn.DB.DataBaseName.DB_INVEN) + ".dbo.TINVENDCPackingList \n";
                _Cmd += "      SET FTStateExport = '1', FDExportDate = @Date , FTExportTime = @Time \n";
                _Cmd += "      WHERE FTBarcodeSendSuplNo = '" + _DocNo + "' \n";
            }
            return _Cmd;
        } // End UpdateStatus(string _ApiNo, string _State)


        private bool UpdateStateAPI9()
        {
            string _Cmd = "";
            _Cmd = "DECLARE @Date varchar(10) = Convert(varchar(10), Getdate(), 111) \n";
            _Cmd += "DECLARE @Time varchar(10) = Convert(varchar(8), Getdate(), 114) \n";
            _Cmd += "UPDATE " + HI.Conn.DB.GetDataBaseName(HI.Conn.DB.DataBaseName.DB_PROD) + ".dbo.TPRODBarcodeScanOutline \n";
            _Cmd += "SET FTStateExport = '1', FDExportDate = @Date , FTExportTime = @Time \n";
            _Cmd += "WHERE FTStateExport = '2' AND FDInsDate >= Convert(varchar(10), Getdate(), 111) ";

            return HI.Conn.SQLConn.ExecuteOnly(_Cmd, HI.Conn.DB.DataBaseName.DB_PROD);
        } // End UpdateStateAPI9

        private bool RollBackStateAPI9()
        {
            string _Cmd = "";
            _Cmd = "DECLARE @Date varchar(10) = Convert(varchar(10), Getdate(), 111) \n";
            _Cmd += "DECLARE @Time varchar(10) = Convert(varchar(8), Getdate(), 114) \n";
            _Cmd += "UPDATE " + HI.Conn.DB.GetDataBaseName(HI.Conn.DB.DataBaseName.DB_PROD) + ".dbo.TPRODBarcodeScanOutline \n";
            _Cmd += "SET FTStateExport = '0', FDExportDate = @Date , FTExportTime = @Time \n";
            _Cmd += "WHERE FTStateExport = '2' AND FDInsDate >= Convert(varchar(10), Getdate(), 111) ";

            return HI.Conn.SQLConn.ExecuteOnly(_Cmd, HI.Conn.DB.DataBaseName.DB_PROD);
        } // End UpdateStateAPI9
    } // End class HyperActiveAPI
}