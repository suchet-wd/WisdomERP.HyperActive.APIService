using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace wisdomserverExchange
{
    public class Cutting
    {
        [XmlElement, JsonProperty("Job")]
        public string Job { get; set; }

        [XmlElement, JsonProperty("CustomerName")]
        public string CustomerName { get; set; }

        [XmlElement, JsonProperty("CustomerPo")]
        public string CustomerPo { get; set; }

        [XmlElement, JsonProperty("JobProductionNo")]
        public string JobProductionNo { get; set; }

        [XmlElement, JsonProperty("SubjobNo")]
        public string SubjobNo { get; set; }

        [XmlElement, JsonProperty("GacDate")]
        public string GacDate { get; set; }

        [XmlElement, JsonProperty("POLineItem")]
        public string POLineItem { get; set; }

        [XmlElement, JsonProperty("PooDetails")]
        public List<CuttingPooDetails> PooDetails { get; set; }

    }
}