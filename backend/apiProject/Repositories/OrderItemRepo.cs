using apiProject.DBContexts;
using apiProject.Interfaces;
using apiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Repositories
{
    public class OrderItemRepo : MSSQLRepo<OrderItem>, IOrderItemRepo
    {
        private readonly MSSQLDbContext _db;
        public OrderItemRepo(MSSQLDbContext db):base(db)
        {
            _db = db;
        }

        public async Task<IEnumerable<OrderItem>> GetItemsByOrderId(long orderId)
        {
            var task = Task.Factory.StartNew(() =>
            {
                return (IEnumerable<OrderItem>)_db.OrderItem.Where(o => o.OrderId == orderId).ToList();
            });
            return await task;
        }

        public async Task<IEnumerable<OrderItem>> GetOrdersByItem(long itemId)
        {
            var task = Task.Factory.StartNew(() =>
            {
                return (IEnumerable<OrderItem>)_db.OrderItem.Where(o => o.ItemId == itemId).ToList();
            });
            return await task;
        }

        public async Task UpdateItemQuantity(long orderId, long ItemId, int quantity)
        {
            var task = Task.Factory.StartNew(() =>
            {
                return _db.OrderItem.FirstOrDefault(o => o.OrderId == orderId && o.ItemId == ItemId);
            });
            var findObj = task.GetAwaiter().GetResult();
            findObj.Quantity = quantity;
            await _db.SaveChangesAsync();
        }
    }
}
