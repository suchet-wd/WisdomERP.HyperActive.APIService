
using Newtonsoft.Json;
using System.Collections.Generic;

namespace wisdomserverExchange
{
    public class CuttingPartDetails
    {

        [JsonProperty("StyleNo")]
        public string StyleNo { get; set; }

        [JsonProperty("PartNo")]
        public string PartNo { get; set; }

        [JsonProperty("FabricNo")]
        public string FabricNo { get; set; }

        [JsonProperty("PartType")]
        public string PartType { get; set; }

        [JsonProperty("ColorWay")]
        public string ColorWay { get; set; }

        [JsonProperty("ColorShades")]
        public string ColorShades { get; set; }

        [JsonProperty("Size")]
        public string size { get; set; }

        [JsonProperty("FTMarkCode")]
        public string FTMarkCode { get; set; }

        [JsonProperty("FTMarkName")]
        public string FTMarkName { get; set; }

        [JsonProperty("PooNo")]
        public string PooNo { get; set; }

        [JsonProperty("TableNo")]
        public string TableNo { get; set; }


    }
}