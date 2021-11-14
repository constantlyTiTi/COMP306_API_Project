using apiProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Interfaces
{
    public interface IOrderRepo:IMSSQLRepo<Order>
    {
        public Task<Order> GetOrderByItemAndOrderedTime(long itemId, DateTime orderTime, string userName);
        public Task<Order> UpdateOrder(Order order);
        public Task<IEnumerable<Order>> GetAllOrdersByUserName(string userName);
        public Task<IEnumerable<Order>> GetAllOrdersByItemId(long itemId);
        public Task<IEnumerable<Order>> GetAllOrdersByDateTime(DateTime orderTime);
        public Task<IEnumerable<Order>> GetAllOrdersByDateTimeAdnItemId(DateTime orderTime, long itemId);
    }
}
