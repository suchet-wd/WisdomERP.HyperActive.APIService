using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperActive
{
    class ResponseAPI
    {
        private string code;
        private string msg;

        //public ResponseAPI()
        //{
        //    this.Code = code;
        //    this.msg = msg;
        //}

        public ResponseAPI(string code, string msg)
        {
            this.Code = code;
            this.msg = msg;
        }

        public string Msg { get => msg; set => msg = value; }
        public string Code { get => code; set => code = value; }
    }
}
