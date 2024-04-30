using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace HI.Conn
{
    public class API
    {
        public System.Data.DataTable GetDataTable(string cmdstring, HI.Conn.DB.DataBaseName DbName)
        {
            System.Data.DataTable dt = null;

            string urlEndPoint = Conn.DB.APIServiceLink;
            string ControllerName = "";

            switch (DbName)
            {
                case DB.DataBaseName.DB_SECURITY:
                    ControllerName = "BaseSercurity";
                    break;
                case DB.DataBaseName.DB_HR:
                    ControllerName = "BaseHR";
                    break;
                case DB.DataBaseName.DB_SYSTEM:
                    ControllerName = "BaseSystem";
                    break;
                case DB.DataBaseName.DB_MASTER:
                    ControllerName = "BaseMaster";
                    break;
                case DB.DataBaseName.DB_MERCHAN:
                    ControllerName = "BaseMerchan";
                    break;
                case DB.DataBaseName.DB_PUR:
                    ControllerName = "BasePurchase";
                    break;
                case DB.DataBaseName.DB_INVEN:
                    ControllerName = "BaseInventory";
                    break;
                case DB.DataBaseName.DB_PROD:
                    ControllerName = "BaseProduction";
                    break;
                case DB.DataBaseName.DB_ACCOUNT:
                    ControllerName = "BaseAccount";
                    break;
                case DB.DataBaseName.DB_LANG:
                    ControllerName = "BaseSyslang";
                    break;
                case DB.DataBaseName.DB_LOG:
                    ControllerName = "BaseSyslog";
                    break;
                case DB.DataBaseName.DB_MAIL:
                    ControllerName = "BaseMail";
                    break;
                case DB.DataBaseName.DB_HR_PAYROLL:
                    ControllerName = "BaseHR";
                    break;
                case DB.DataBaseName.DB_MEDC:
                    ControllerName = "BaseMedical";
                    break;
                case DB.DataBaseName.DB_FG:
                    ControllerName = "BaseFG";
                    break;
                case DB.DataBaseName.DB_PLANNING:
                    ControllerName = "BasePlanning";
                    break;
                case DB.DataBaseName.DB_DOC:
                    ControllerName = "BaseDocument";
                    break;
                case DB.DataBaseName.DB_FIXED:
                    ControllerName = "BaseFixedAsset";
                    break;
                case DB.DataBaseName.DB_SAMPLE:
                    ControllerName = "BaseSampleroom";
                    break;
                case DB.DataBaseName.DB_HYPERACTIVE:
                    ControllerName = "BaseHyperactive";
                    break;
                default:
                    ControllerName = "BaseSystem";
                    break;
            }


            urlEndPoint = urlEndPoint + "/" + ControllerName;

            try
            {

                System.Data.DataSet dts = new System.Data.DataSet("JsonDs");
                System.Data.DataTable dtd = new System.Data.DataTable();
                bool clearrowdata =false ;
                dtd.Columns.Add("cmdstring", typeof(string));

                dtd.Rows.Add(cmdstring);
                dts.Tables.Add(dtd);

                string json_data = JsonConvert.SerializeObject(dts);
                byte[] json_bytes = System.Text.Encoding.UTF8.GetBytes(json_data);


                //' -- Refresh the access token
                System.Net.WebRequest request = System.Net.HttpWebRequest.Create(urlEndPoint);

                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = json_bytes.Length;

                Stream postStream = request.GetRequestStream();
                postStream.Write(json_bytes, 0, json_bytes.Length);
                postStream.Flush();
                postStream.Close();

                try
                {

                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                    using (System.Net.WebResponse response = request.GetResponse())
                    {


                        try {

                            if (((System.Net.HttpWebResponse)response).StatusCode == HttpStatusCode.OK) {
                                clearrowdata = true;
                            };
                        }catch{ }


                        using (System.IO.StreamReader streamReader = new System.IO.StreamReader(response.GetResponseStream()))
                        {
                            // Parse the JSON the way you prefer

                                                
                            var jsonResponseText = streamReader.ReadToEnd();
                            System.Data.DataSet dsRet = JsonConvert.DeserializeObject<System.Data.DataSet>(jsonResponseText);

                            if (dsRet != null)
                            {
                                if (dsRet.Tables.Count > 0)
                                {
                                    dt = dsRet.Tables[0].Copy();

                                    if (clearrowdata) {
                                        dt.Rows.Clear(); 
                                    };

                                };

                            };

                            dsRet.Dispose();

                        }
                    }
                }
                catch (Exception ex2) { }

            }
            catch (Exception ex)
            {

            }

            return dt;

        }

        public string[] GetReportPDF(string cmpid, string username, string foldername, string fileRpt, string formula,int lang)
        {
            string[] rpt = null;
            string msgerror = "";
            string reportstring = "";
            string urlEndPoint = Conn.DB.APIReportServiceLink;
            string ControllerName  = "PDFReport";
     

            urlEndPoint = urlEndPoint + "/" + ControllerName;

          
            try
            {

                System.Data.DataSet dts = new System.Data.DataSet("JsonDs");
                System.Data.DataTable dtd = new System.Data.DataTable();
                dtd.Columns.Add("cmpid", typeof(string));
                dtd.Columns.Add("username", typeof(string));              
                dtd.Columns.Add("foldername", typeof(string));
                dtd.Columns.Add("reportname", typeof(string));
                dtd.Columns.Add("formula", typeof(string));
                dtd.Columns.Add("language", typeof(string));
                dtd.Rows.Add(cmpid, username, foldername, fileRpt, formula, lang.ToString());

                dts.Tables.Add(dtd);

                string json_data = JsonConvert.SerializeObject(dts);
                byte[] json_bytes = System.Text.Encoding.UTF8.GetBytes(json_data);


                //' -- Refresh the access token
                System.Net.WebRequest request = System.Net.HttpWebRequest.Create(urlEndPoint);

                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = json_bytes.Length;

                Stream postStream = request.GetRequestStream();
                postStream.Write(json_bytes, 0, json_bytes.Length);
                postStream.Flush();
                postStream.Close();

                try
                {

                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                    using (System.Net.WebResponse response = request.GetResponse())
                    {
                        using (System.IO.StreamReader streamReader = new System.IO.StreamReader(response.GetResponseStream()))
                        {
                            // Parse the JSON the way you prefer
                            var jsonResponseText = streamReader.ReadToEnd();
                            System.Data.DataSet dsRet = JsonConvert.DeserializeObject<System.Data.DataSet>(jsonResponseText);

                            if (dsRet != null)
                            {
                                if (dsRet.Tables.Count > 0)
                                {

                                    try {
                                        reportstring = dsRet.Tables[0].Rows[0]["report"].ToString();
                                    } catch { }

                                    if (dsRet.Tables.Count > 1) {
                                         msgerror = dsRet.Tables[1].Rows[0]["msgerror"].ToString();
                                    }
                                };

                            };

                            dsRet.Dispose();

                        }
                    }
                }
                catch (Exception ex2) { }

            }
            catch (Exception ex)
            {

            }

            rpt = new string[] { msgerror, reportstring };

            return rpt;
        }

        public bool PostData(string ControllerName, System.Data.DataSet ds)
        {
            bool statepost = false ;

            string urlEndPoint = Conn.DB.APIServiceLink;
          

            urlEndPoint = urlEndPoint + "/" + ControllerName;

            try
            {

                string json_data = JsonConvert.SerializeObject(ds);
                byte[] json_bytes = System.Text.Encoding.UTF8.GetBytes(json_data);


                //' -- Refresh the access token
                System.Net.WebRequest request = System.Net.HttpWebRequest.Create(urlEndPoint);

                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = json_bytes.Length;

                Stream postStream = request.GetRequestStream();
                postStream.Write(json_bytes, 0, json_bytes.Length);
                postStream.Flush();
                postStream.Close();

                try
                {

                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                    using (System.Net.WebResponse response = request.GetResponse())
                    {

                        try
                        {

                            if (((System.Net.HttpWebResponse)response).StatusCode == HttpStatusCode.Accepted )
                            {
                                statepost = true;
                            };
                        }
                        catch { }


                        using (System.IO.StreamReader streamReader = new System.IO.StreamReader(response.GetResponseStream()))
                        {
                            // Parse the JSON the way you prefer


                            var jsonResponseText = streamReader.ReadToEnd();
                            //System.Data.DataSet dsRet = JsonConvert.DeserializeObject<System.Data.DataSet>(jsonResponseText);

                            //if (dsRet != null)
                            //{
                   

                            //};

                            //dsRet.Dispose();

                        }
                    }
                }
                catch (Exception ex2) { }

            }
            catch (Exception ex)
            {

            }

            return statepost;

        }
    }
}
