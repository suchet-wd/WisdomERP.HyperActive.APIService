using Newtonsoft.Json;
using System.Collections.Generic;

namespace wisdomserverExchange
{
    public class BundleInfo
    {
        [JsonProperty("PooNo")]
        public string PooNo { get; set; }

        [JsonProperty("CustomerPo")]
        public string CustomerPo { get; set; }

        [JsonProperty("StyleNo")]
        public string StyleNo { get; set; }

        [JsonProperty("Job")]
        public string Job { get; set; }

        [JsonProperty("JobProductionNo")]
        public string JobProductionNo { get; set; }

        [JsonProperty("BundleInfoDetails")]
        public List<BundleInfoDetails> BundleInfoDetails { get; set; }

    }
}