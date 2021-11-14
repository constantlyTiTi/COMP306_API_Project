using apiProject.DBContexts;
using apiProject.Interfaces;
using apiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Repositories
{
    public class OrderRepo : MSSQLRepo<Order>, IOrderRepo
    {
        private readonly MSSQLDbContext _db;
        public OrderRepo(MSSQLDbContext db):base(db)
        {
            _db = db;
        }
        public Task<Order> AddOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteOrder(long orderId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> GetAllOrdersByDateTime(DateTime orderTime)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> GetAllOrdersByDateTimeAdnItemId(DateTime orderTime, long itemId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> GetAllOrdersByItemId(long itemId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> GetAllOrdersByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrderById(long orderId)
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrderByItemAndOrderedTime(long itemId, DateTime orderTime, string userName)
        {
            throw new NotImplementedException();
        }

        public Task<Order> UpdateOrder(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
