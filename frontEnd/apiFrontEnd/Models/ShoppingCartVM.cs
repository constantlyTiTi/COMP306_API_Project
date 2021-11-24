using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace apiFrontEnd.Models
{
    class ShoppingCartVM
    {
        [JsonPropertyName("shopping_cart_items")]
        public IEnumerable<Item> shopping_cart_items { get; set; }
        [JsonPropertyName("total_cost")]
        public double total_cost { get; set; }
        [JsonPropertyName("shipping_address")]
        public string shipping_address { get; set; }
        [JsonPropertyName("user_name")]
        public string user_name { get; set; }
    }
}
