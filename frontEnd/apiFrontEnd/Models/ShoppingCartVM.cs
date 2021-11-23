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
        public IEnumerable<Item> ShoppingCartItems { get; set; }
        [JsonPropertyName("total_cost")]
        public double TotalCost { get; set; }
        [JsonPropertyName("shipping_address")]
        public string ShippingAddress { get; set; }
        [JsonPropertyName("user_name")]
        public string UserName { get; set; }
    }
}
