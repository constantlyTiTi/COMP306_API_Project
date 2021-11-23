using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace apiFrontEnd.Models
{
    class OrderDetails
    {
        [JsonPropertyName("order_id")]
        public long order_id { get; set; }
        public string Status { get; set; }

/*        [JsonPropertyName("total_cost")]*/
        public double total_cost { get; set; }

/*        [JsonPropertyName("shipping_address")]*/
        public string shipping_address { get; set; }

/*        [JsonPropertyName("order_time")]*/
        public DateTime order_time { get; set; }

/*        [JsonPropertyName("user_name")]*/
        public string user_name { get; set; }

/*        [JsonPropertyName("items")]*/
        public IEnumerable<Item> items { get; set; }
    }
}
