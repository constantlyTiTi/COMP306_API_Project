using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace apiProject.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("ShoppingCart")]
    public class ShoppingCartItem
    {
        [PrimaryKey]
        [JsonPropertyName("shopping_cart_item_id")]
        public long ShoppingCartItemId { get; set; }
        [JsonPropertyName("user_name")]
        public string UserName { get; set; }
        [JsonPropertyName("item_id")]
        public long ItemId { get; set; }
        public double Price { get; set; }
        [System.ComponentModel.DataAnnotations.Range(1,100,ErrorMessage="Please enter a value between 1 and 100")]
        public int Quantity { get; set; }

    }
}
