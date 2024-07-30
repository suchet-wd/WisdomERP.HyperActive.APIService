using DevExpress.Utils.Menu;
using HyperActive;
using System;
using System.Data;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;

namespace HyperActiveWF
{

    public partial class HyperActiveForm : Form
    {

        private HttpClient httpClient;
        //private readonly string apiToken = "6HEZZHkAAiFuWb5h";
        private HyperActiveAPI hyper;
        private readonly string apiToken = HI.Conn.DB._apiToken;

        public HyperActiveForm()
        {
            InitializeComponent();
            HI.Conn.DB.GetXmlConnectionString(); // Load Link Address API HyperActtive
            getAddressList(); // Load Link Address API HyperActtive to DropdownList
        }

        private void btnTestConnect_Click(object sender, EventArgs e)
        {
            lbResult.Text = "";
            try
            {
                //string[] args = null;
                Ping pingSender = new Ping();
                PingOptions options = new PingOptions();

                // Use the default Ttl value which is 128,
                // but change the fragmentation behavior.
                options.DontFragment = true;

                // Create a buffer of 32 bytes of data to be transmitted.
                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 120;
                PingReply reply = pingSender.Send(lbcAPIlist.SelectedItem.ToString(), timeout, buffer, options);
                if (reply.Status == IPStatus.Success)
                {
                    lbResult.ForeColor = System.Drawing.Color.Green;
                    lbResult.Text = "Successful... [" + reply.Address.ToString() + "]";
                    Console.WriteLine("Address: {0}", reply.Address.ToString());
                    Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
                    Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
                    Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
                    Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);
                }
            }
            catch (Exception ex)
            {
                lbResult.ForeColor = System.Drawing.Color.Red;
                lbResult.Text = "Cannot connect to : " + lbcAPIlist.SelectedItem.ToString();
                Console.WriteLine("Cannot connect to : " + lbcAPIlist.SelectedItem.ToString());
            }

        }


        private void btnStart_Click(object sender, EventArgs e)
        {
            lbResult.Text = "";
            httpClient = new HttpClient();
            //hyper = new HyperActive.HyperActiveAPI();
            if (httpClient != null)
            {
                lbResult.ForeColor = System.Drawing.Color.Green;
                lbResult.Text = "Connection Successful ...";
                hyper = new HyperActive.HyperActiveAPI();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            lbResult.Text = "";
            if (httpClient == null)
            {
                lbResult.ForeColor = System.Drawing.Color.Red;
                lbResult.Text = "No Connection !!!";
            }
            else
            {
                httpClient.Dispose();
                httpClient = null;
                lbResult.Text = "Network is Disconnect !!!";

                //if (hyper != null)
                //{
                    
                //}
            }

        }

        private void getAddressList()
        {
            //DXPopupMenu menu = new DXPopupMenu();
            //ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            lbcAPIlist.Items.Add("127.0.0.1");
            lbcAPIlist.Items.Add("WSM-MER");
            lbcAPIlist.Items.Add("hig00svr91");
            lbcAPIlist.Items.Add("10.92.10.150");
            lbcAPIlist.Items.Add("203.150.53.240");
            //lbcAPIlist.Items.Add(HI.Conn.DB.GetHyperActiveAPIName(HI.Conn.DB.HyperActiveName.api1));
            //lbcAPIlist.Items.Add(HI.Conn.DB.GetHyperActiveAPIName(HI.Conn.DB.HyperActiveName.api2));
            //lbcAPIlist.Items.Add(HI.Conn.DB.GetHyperActiveAPIName(HI.Conn.DB.HyperActiveName.api5));
            //lbcAPIlist.Items.Add(HI.Conn.DB.GetHyperActiveAPIName(HI.Conn.DB.HyperActiveName.api6));
            //lbcAPIlist.Items.Add(HI.Conn.DB.GetHyperActiveAPIName(HI.Conn.DB.HyperActiveName.api7));
            //lbcAPIlist.Items.Add(HI.Conn.DB.GetHyperActiveAPIName(HI.Conn.DB.HyperActiveName.api9));
            //lbcAPIlist.Items.Add(HI.Conn.DB.GetHyperActiveAPIName(HI.Conn.DB.HyperActiveName.api10));
        }


    }
}