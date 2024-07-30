using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HyperActive.Class
{
    //class DataManagement
    //{
    //    public bool ExecuteOnly(string QryStr)
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
    //            _Cmd.CommandTimeout = 0;
    //            _Cmd.CommandType = CommandType.Text;
    //            _Cmd.CommandText = QryStr;
    //            _Cmd.ExecuteNonQuery();
    //            _Cmd.Parameters.Clear();

    //            _Cmd.Connection.Close();
    //            _Cmd.Dispose();
    //            if (_Cnn.State == ConnectionState.Open)
    //            {
    //                _Cnn.Close();
    //            }
    //            _Cnn.Dispose();
    //            return true;
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
    //            //Interaction.MsgBox(ex.Message);
    //            return false;
    //        }

    //    }
    //    public static DataTable GetDataTable(string QryStr)
    //    {
    //        DataTable objDT = new DataTable();

    //        try
    //        {
    //            string _ConnString = "";
    //            System.Data.SqlClient.SqlConnection _Cnn = new System.Data.SqlClient.SqlConnection();
    //            System.Data.SqlClient.SqlCommand _Cmd = new System.Data.SqlClient.SqlCommand();

    //            try
    //            {
    //                _ConnString = "Server=hig00svr91;Database=HITECH_PRODUCTION;User Id=sa;Password=!@#5k,mew,;";
    //                //_ConnString = "Server=hig00svr91;Database=HITECH_PRODUCTION;User Id=wsmhig;Password=I-xfn-l6M.;";

    //                if (_Cnn.State == ConnectionState.Open) { _Cnn.Close(); };
    //                _Cnn.ConnectionString = _ConnString;
    //                _Cnn.Open();
    //                _Cmd = _Cnn.CreateCommand();

    //                var _Adepter = new SqlDataAdapter(_Cmd);
    //                _Adepter.SelectCommand.CommandTimeout = 0;
    //                _Adepter.SelectCommand.CommandType = CommandType.Text;
    //                _Adepter.SelectCommand.CommandText = QryStr;
    //                _Adepter.Fill(objDT);
    //                _Adepter.Dispose();

    //                _Cmd.Connection.Close();
    //                _Cmd.Dispose();
    //                if (_Cnn.State == ConnectionState.Open)
    //                {
    //                    _Cnn.Close();
    //                }
    //                _Cnn.Dispose();

    //            }
    //            catch (Exception ex)
    //            {
    //                Console.WriteLine(ex.Message);
    //                _Cmd.Connection.Close();
    //                _Cmd.Dispose();
    //                if (_Cnn.State == ConnectionState.Open)
    //                {
    //                    _Cnn.Close();
    //                }
    //                _Cnn.Dispose();
    //            }
    //        }
    //        catch { }

    //        return objDT;
    //    }

    //    public static XmlDocument GetDataXML(string QryStr)
    //    {
    //        XmlDocument objXML = new XmlDocument();
    //        try
    //        {
    //            string _ConnString = "";
    //            System.Data.SqlClient.SqlConnection _Cnn = new System.Data.SqlClient.SqlConnection();
    //            System.Data.SqlClient.SqlCommand _Cmd = new System.Data.SqlClient.SqlCommand();

    //            try
    //            {
    //                _ConnString = "Server=hig00svr91;Database=HITECH_PRODUCTION;User Id=sa;Password=!@#5k,mew,;";
    //                //_ConnString = "Server=hig00svr91;Database=HITECH_PRODUCTION;User Id=wsmhig;Password=I-xfn-l6M.;";
    //                XmlReader xmlreader;
    //                if (_Cnn.State == ConnectionState.Open) { _Cnn.Close(); };
    //                _Cnn.ConnectionString = _ConnString;
    //                _Cnn.Open();
    //                _Cmd = _Cnn.CreateCommand();
    //                _Cmd.CommandText = QryStr;
    //                xmlreader = _Cmd.ExecuteXmlReader();

    //                while (xmlreader.Read())
    //                {
    //                    objXML.Load(xmlreader);
    //                }
    //                xmlreader.Close();

    //                _Cmd.Connection.Close();
    //                _Cmd.Dispose();
    //                if (_Cnn.State == ConnectionState.Open)
    //                {
    //                    _Cnn.Close();
    //                }
    //                _Cnn.Dispose();

    //            }
    //            catch (Exception ex)
    //            {
    //                Console.WriteLine(ex.Message);
    //                _Cmd.Connection.Close();
    //                _Cmd.Dispose();
    //                if (_Cnn.State == ConnectionState.Open)
    //                {
    //                    _Cnn.Close();
    //                }
    //                _Cnn.Dispose();
    //            }
    //        }
    //        catch { }

    //        return objXML;
    //    }
    //}
}
