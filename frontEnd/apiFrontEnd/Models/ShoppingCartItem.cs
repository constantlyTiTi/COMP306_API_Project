﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace apiFrontEnd.Models
{
    class ShoppingCartItem
    {
/*        [JsonPropertyName("shopping_cart_item_id")]*/
        public long shopping_cart_item_id { get; set; }
/*        [JsonPropertyName("user_name")]*/
        public string user_name { get; set; }
/*        [JsonPropertyName("item_id")]*/
        public long item_id { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}
