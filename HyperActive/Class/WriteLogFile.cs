using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperActive.Class
{
    class WriteLogFile
    {
        public WriteLogFile(string message)
        {
            StreamWriter sw = null;
            sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\WisdomLogFile.txt", true);
            sw.WriteLine("{DateTime.Now.ToString()} :" + message);
            sw.Flush();
            sw.Close();
        }
    }
}
