using apiProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Interfaces
{
    public interface IOrderItemRepo:IMSSQLRepo<OrderItem>
    {
        public Task<IEnumerable<OrderItem>> GetOrdersByItem(long itemId);
        public Task<IEnumerable<OrderItem>> GetItemsByOrderId(long orderId);
    }
}
