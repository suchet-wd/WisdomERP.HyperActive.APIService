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

        private HttpClient httpClient;
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