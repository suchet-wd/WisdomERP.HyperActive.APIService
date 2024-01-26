using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace wisdomserverExchange
{
    public class BundleInfo
    {
        [XmlElement, JsonProperty("PooNo")]
        public string PooNo { get; set; }

        [XmlElement, JsonProperty("CustomerPo")]
        public string CustomerPo { get; set; }

        [XmlElement, JsonProperty("StyleNo")]
        public string StyleNo { get; set; }

        [XmlElement, JsonProperty("Job")]
        public string Job { get; set; }

        [XmlElement, JsonProperty("JobProductionNo")]
        public string JobProductionNo { get; set; }

        [XmlElement, JsonProperty("BundleInfoDetails")]
        public List<BundleInfoDetails> BundleInfoDetails { get; set; }

    }
}