using apiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.DTO
{
    public class OrderRate
    {
        public OrderDetails OrderDetails { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
        public Rate Rate  { get; set; }
        public IEnumerable<Item> Items { get; set; }
        public IEnumerable<ItemFile> ItemCovers { get; set; } 
    }
}
