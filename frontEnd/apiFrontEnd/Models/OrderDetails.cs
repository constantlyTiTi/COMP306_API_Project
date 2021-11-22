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
        public long OrderId { get; set; }
        public string Status { get; set; }

        [JsonPropertyName("total_cost")]
        public double TotalCost { get; set; }

        [JsonPropertyName("shipping_address")]
        public string ShippingAddress { get; set; }

        [JsonPropertyName("order_time")]
        public DateTime OrderTime { get; set; }

        [JsonPropertyName("user_name")]
        public string UserName { get; set; }

        [JsonPropertyName("items")]
        public IEnumerable<Item> items { get; set; }
    }
}
