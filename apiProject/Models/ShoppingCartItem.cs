using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Models
{
    public class ShoppingCartItem
    {
        public string UserName { get; set; }
        public long ItemId { get; set; }
        public Item Item { get; set; }
        [Range(1,100,ErrorMessage="Please enter a value between 1 and 100")]
        public int Quantity { get; set; }

    }
}
