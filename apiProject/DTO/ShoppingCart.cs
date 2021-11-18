using apiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.DTO
{
    public class ShoppingCart
    {
        public IEnumerable<ShoppingCartItem> ShoppingCartItems { get; set; }
        public double TotalCost { get; set; }
    }
}
