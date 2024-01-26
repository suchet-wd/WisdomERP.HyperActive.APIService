using Newtonsoft.Json;
using System.Xml.Serialization;

namespace wisdomserverExchange
{
    public class OrderDetailsRatio
    {
        [XmlElement, JsonProperty("Colorway")]
        public string Colorway { get; set; }

        [XmlElement, JsonProperty("Size")]
        public string Size { get; set; }

        [XmlElement, JsonProperty("Quantity")]
        public int Quantity { get; set; }

    }
}