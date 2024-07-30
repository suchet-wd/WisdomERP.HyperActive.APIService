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
        private string boxRfid;
        private string boxBarcode;

        private List<BundleRfidBarcodeList> bundleList;

        public ResponseAPI()
        {
        }

        public ResponseAPI(string code, string msg)
        {
            this.Code = code;
            this.msg = msg;
        }

        public ResponseAPI(string boxRfid, string boxBarcode, string code, string msg, List<BundleRfidBarcodeList> bundleRfidBarcodeList)
        {
            this.boxBarcode = boxBarcode;
            this.boxRfid = boxRfid;
            this.bundleList = bundleRfidBarcodeList;
            this.Code = code;
            this.msg = msg;
        }

        public string BoxBarcode { get => boxBarcode; set => boxBarcode = value; }
        public string BoxRfid { get => boxRfid; set => boxRfid = value; }
        public string Msg { get => msg; set => msg = value; }
        public string Code { get => code; set => code = value; }
        public List<BundleRfidBarcodeList> BundleList { get => bundleList; set => bundleList = value; }

    }

    class BundleRfidBarcodeList
    {
        private string rfid;
        private string parentBundleBarcode;
        private string bundleBarcode;

        public string Rfid { get => rfid; set => rfid = value; }
        public string ParentBundleBarcode { get => parentBundleBarcode; set => parentBundleBarcode = value; }
        public string BundleBarcode { get => bundleBarcode; set => bundleBarcode = value; }
    }
}
