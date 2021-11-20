using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("ShoppingCart")]
    public class ShoppingCartItem
    {
        [PrimaryKey]
        public long ShoppingCartItemId { get; set; }
        public string UserName { get; set; }
        public long ItemId { get; set; }
        public double Price { get; set; }
        [System.ComponentModel.DataAnnotations.Range(1,100,ErrorMessage="Please enter a value between 1 and 100")]
        public int Quantity { get; set; }

    }
}
