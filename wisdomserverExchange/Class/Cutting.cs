using Newtonsoft.Json;
using System.Collections.Generic;

namespace wisdomserverExchange
{
    public class Cutting
    {
        [JsonProperty("Job")]
        public string Job { get; set; }

        [JsonProperty("CustomerName")]
        public string CustomerName { get; set; }

        [JsonProperty("CustomerPo")]
        public string CustomerPo { get; set; }

        [JsonProperty("JobProductionNo")]
        public string JobProductionNo { get; set; }

        [JsonProperty("SubjobNo")]
        public string SubjobNo { get; set; }

        [JsonProperty("GacDate")]
        public string GacDate { get; set; }

        [JsonProperty("POLineItem")]
        public string POLineItem { get; set; }

        [JsonProperty("PooDetails")]
        public List<CuttingPooDetails> PooDetails { get; set; }

    }
}