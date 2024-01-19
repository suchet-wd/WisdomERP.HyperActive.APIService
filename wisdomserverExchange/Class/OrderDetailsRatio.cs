using Newtonsoft.Json;

namespace wisdomserverExchange
{
    public class OrderDetailsRatio
    {
        [JsonProperty("Colorway")]
        public string Colorway { get; set; }

        [JsonProperty("Size")]
        public string Size { get; set; }

        [JsonProperty("Quantity")]
        public int Quantity { get; set; }

    }
}