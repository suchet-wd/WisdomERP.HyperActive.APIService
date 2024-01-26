using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace wisdomserverExchange
{
    public class OrderDetails
    {
        [XmlElement, JsonProperty("StyleNo")]
        public string StyleNo { get; set; }

        [XmlElement, JsonProperty("JobProductionNo")]
        public string JobProductionNo { get; set; }

        [XmlElement, JsonProperty("SubjobNo")]
        public string SubjobNo { get; set; }

        [XmlElement, JsonProperty("GacDate")]
        public string GacDate { get; set; }

        [XmlElement, JsonProperty("PoLineItem")]
        public string PoLineItem { get; set; }

        [XmlElement, JsonProperty("Colorway")]
        public string Colorway { get; set; }

        [XmlElement, JsonProperty("Size")]
        public string Size { get; set; }

        [XmlElement, JsonProperty("SubJobQuantity")]
        public int SubJobQuantity { get; set; }

        [XmlElement, JsonProperty("AssortQuantity")]
        public List<OrderDetailsRatio> AssortQuantity { get; set; }

        [XmlElement, JsonProperty("BoxQuantity")]
        public int BoxQuantity { get; set; }

        [XmlElement, JsonProperty("PackType")]
        public string PackType { get; set; }

        [XmlElement, JsonProperty("PackRatio")]
        public List<OrderDetailsRatio> PackRatio { get; set; }

    }
}