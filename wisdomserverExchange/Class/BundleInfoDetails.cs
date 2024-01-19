using Newtonsoft.Json;

namespace wisdomserverExchange
{
    public class BundleInfoDetails
    {
        [JsonProperty("ParentBundleBarcode")]
        public string ParentBundleBarcode { get; set; }

        [JsonProperty("BundleBarcode")]
        public string BundleBarcode { get; set; }

        [JsonProperty("BundleNo")]
        public string BundleNo { get; set; }

        [JsonProperty("Colorway")]
        public string Colorway { get; set; }

        [JsonProperty("PoLineItem")]
        public string PoLineItem { get; set; }

        [JsonProperty("Size")]
        public string Size { get; set; }

        [JsonProperty("BundleQuantity")]
        public int BundleQuantity { get; set; }

        [JsonProperty("Marker")]
        public string Marker { get; set; }

        [JsonProperty("Route")]
        public string Route { get; set; }

        [JsonProperty("Supplier")]
        public string Supplier { get; set; }

    }
}