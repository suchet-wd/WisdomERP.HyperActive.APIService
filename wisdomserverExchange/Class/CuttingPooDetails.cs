using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace wisdomserverExchange
{
    public class CuttingPooDetails
    {

        [XmlElement, JsonProperty("PooNo")]
        public string PooNo { get; set; }

        [XmlElement, JsonProperty("TableNo")]
        public string TableNo { get; set; }

        [XmlElement, JsonProperty("SpreadingRatio")]
        public string SpreadingRatio { get; set; }

        [XmlElement, JsonProperty("NumberofLayer")]
        public string NumberofLayer { get; set; }

        public List<CuttingPartDetails> CuttingPartDetails { get; set; }


        //public static bool isJob(string jobno)
        //{
        //    DataTable _dataTable = new DataTable();
        //    bool state = false;

        //    string _Qry = "SELECT od.FTSubOrderNo FROM HITECH_MERCHAN.dbo.TMERTOrderSub AS od WITH ( NOLOCK ) \n";
        //    if (jobno != "")
        //    {
        //        _Qry += " WHERE od.FTOrderNo  = '" + jobno + "'";
        //    }

        //    try
        //    {
        //        _dataTable = Conn.GetNewDataTable(_Qry);
        //        if (_dataTable.Rows.Count > 0)
        //        {
        //            state = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //    }
        //    return state;
        //}

    }
}