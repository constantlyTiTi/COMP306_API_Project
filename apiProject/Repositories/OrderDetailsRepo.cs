using apiProject.DBContexts;
using apiProject.Interfaces;
using apiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Repositories
{
    public class OrderDetailsRepo:MSSQLRepo<OrderDetails>, IOrderDetailsRepo
    {
        private readonly MSSQLDbContext _db;
        public OrderDetailsRepo(MSSQLDbContext db) : base(db)
        {
            _db = db;
        }

        public Task<IEnumerable<OrderDetails>> GetAllOrdersByDateTime(DateTime orderTime)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderDetails>> GetAllOrdersByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDetails> GetOrderByOrderedTime(DateTime orderTime, string userName)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDetails> UpdateOrder(OrderDetails order)
        {
            throw new NotImplementedException();
        }
    }
}
