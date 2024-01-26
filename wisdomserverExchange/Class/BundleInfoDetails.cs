using Newtonsoft.Json;
using System.Xml.Serialization;

namespace wisdomserverExchange
{
    public class BundleInfoDetails
    {
        [XmlElement, JsonProperty("ParentBundleBarcode")]
        public string ParentBundleBarcode { get; set; }

        [XmlElement, JsonProperty("BundleBarcode")]
        public string BundleBarcode { get; set; }

        [XmlElement, JsonProperty("BundleNo")]
        public string BundleNo { get; set; }

        [XmlElement, JsonProperty("Colorway")]
        public string Colorway { get; set; }

        [XmlElement, JsonProperty("PoLineItem")]
        public string PoLineItem { get; set; }

        [XmlElement, JsonProperty("Size")]
        public string Size { get; set; }

        [XmlElement, JsonProperty("BundleQuantity")]
        public int BundleQuantity { get; set; }

        [XmlElement, JsonProperty("Marker")]
        public string Marker { get; set; }

        [XmlElement, JsonProperty("Route")]
        public string Route { get; set; }

        [XmlElement, JsonProperty("Supplier")]
        public string Supplier { get; set; }

    }
}