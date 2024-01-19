using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wisdomserverExchange
{
    class Order
    {
        [JsonProperty("Job")]
        public string Job { get; set; }

        [JsonProperty("CustomerName")]
        public string CustomerName { get; set; }

        [JsonProperty("CustomerPo")]
        public string CustomerPo { get; set; }

        [JsonProperty("Note")]
        public string Note { get; set; }

        [JsonProperty("OrderDetails")]
        public List<OrderDetails> OrderDetails { get; set; }

    }
}
