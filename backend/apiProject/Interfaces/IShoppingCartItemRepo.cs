using apiProject.Models;
using apiProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Interfaces
{
    public interface IShoppingCartItemRepo: IMSSQLRepo<ShoppingCartItem>
    {
        void UpdateItemQuantity(long itemId, int quantity, string userName);
        void DeleteItem(long itemId, string userName);
        IEnumerable<ShoppingCartItem> GetItems(string userName);
        Task RemoveAll(string userName);
    }
}
