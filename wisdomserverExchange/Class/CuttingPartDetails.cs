
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace wisdomserverExchange
{
    public class CuttingPartDetails
    {

        [XmlElement, JsonProperty("StyleNo")]
        public string StyleNo { get; set; }

        [XmlElement, JsonProperty("PartNo")]
        public string PartNo { get; set; }

        [XmlElement, JsonProperty("FabricNo")]
        public string FabricNo { get; set; }

        [XmlElement, JsonProperty("PartType")]
        public string PartType { get; set; }

        [XmlElement, JsonProperty("ColorWay")]
        public string ColorWay { get; set; }

        [XmlElement, JsonProperty("ColorShades")]
        public string ColorShades { get; set; }

        [XmlElement, JsonProperty("Size")]
        public string size { get; set; }

        [XmlElement, JsonProperty("FTMarkCode")]
        public string FTMarkCode { get; set; }

        [XmlElement, JsonProperty("FTMarkName")]
        public string FTMarkName { get; set; }

        [XmlElement, JsonProperty("PooNo")]
        public string PooNo { get; set; }

        [XmlElement, JsonProperty("TableNo")]
        public string TableNo { get; set; }


    }
}