using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace wisdomserverExchange.Class
{
    [XmlRoot("ProductionPlan"), JsonObject]
    class ProductionPlan
    {
        [XmlElement, JsonProperty("Job")]
        public string Job { get; set; }

        [XmlElement, JsonProperty("CustomerName")]
        public string CustomerName { get; set; }

        [XmlElement, JsonProperty("CustomerPo")]
        public string CustomerPo { get; set; }

        [XmlElement, JsonProperty("Note")]
        public string Note { get; set; }

        [XmlElement, JsonProperty("OrderDetails")]
        public List<OrderDetails> OrderDetails { get; set; }
    }

    
}
