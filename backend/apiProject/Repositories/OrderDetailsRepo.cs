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

        public async Task<IEnumerable<OrderDetails>> GetAllOrdersByDateTime(DateTime? startTime, DateTime? endTime, string userName = "")
        {
            Task<IEnumerable<OrderDetails>> task = Task.Factory.StartNew(()=> { return new List<OrderDetails>().AsEnumerable(); });

            if(startTime == null && endTime == null && !string.IsNullOrWhiteSpace(userName))
            {
                task = Task.Factory.StartNew(() =>
                {
                    return (IEnumerable<OrderDetails>)_db.OrderDetail.Where(o => o.UserName == userName).ToList();
                });
            }

            if (startTime != null && endTime != null && string.IsNullOrWhiteSpace(userName))
            {
                task = Task.Factory.StartNew(() =>
                {
                    return (IEnumerable<OrderDetails>)_db.OrderDetail.Where(o => o.OrderTime >= startTime.Value && o.OrderTime < endTime.Value).ToList();
                });
            }
            if (startTime != null && endTime != null && !string.IsNullOrWhiteSpace(userName))
            {
                task = Task.Factory.StartNew(() =>
                {
                    return (IEnumerable<OrderDetails>)_db.OrderDetail.Where(o => o.OrderTime >= startTime.Value && o.OrderTime < endTime.Value && o.UserName == userName).ToList();
                });
            }

            if (startTime != null && string.IsNullOrWhiteSpace(userName))
            {
                task = Task.Factory.StartNew(() =>
                {
                    return (IEnumerable<OrderDetails>)_db.OrderDetail.Where(o => o.OrderTime >= startTime.Value).ToList();
                });
            }
            if (startTime != null && !string.IsNullOrWhiteSpace(userName))
            {
                task = Task.Factory.StartNew(() =>
                {
                    return (IEnumerable<OrderDetails>)_db.OrderDetail.Where(o => o.OrderTime >= startTime.Value && o.UserName == userName).ToList();
                });
            }
            if (endTime != null && string.IsNullOrWhiteSpace(userName))
            {
                task = Task.Factory.StartNew(() =>
                {
                    return (IEnumerable<OrderDetails>)_db.OrderDetail.Where(o => o.OrderTime < endTime.Value).ToList();
                });
            }

            if (endTime != null && !string.IsNullOrWhiteSpace(userName))
            {
                task = Task.Factory.StartNew(() =>
                {
                    return (IEnumerable<OrderDetails>)_db.OrderDetail.Where(o => o.OrderTime < endTime.Value && o.UserName == userName).ToList();
                });
            }

            return await task;
              
        }

        public async Task<IEnumerable<OrderDetails>> GetAllOrdersByUserName(string userName)
        {
            var task = Task.Factory.StartNew(() =>
            {
                return (IEnumerable<OrderDetails>)_db.OrderDetail.Where(o => o.UserName == userName).ToList();
            });
            return await task;
        }

        public async Task<IEnumerable<OrderDetails>> GetOrderByOrderedTime(DateTime startTime, DateTime endTime, string userName)
        {
            var task = Task.Factory.StartNew(() =>
            {
                return (IEnumerable<OrderDetails>)_db.OrderDetail
                .Where(o => o.UserName == userName && o.OrderTime >= startTime && o.OrderTime < endTime).ToList();
            });
            return await task;
        }

        public async Task UpdateOrder(OrderDetails order)
        {
            var task = Task.Factory.StartNew(() =>
            {
                return _db.OrderDetail.FirstOrDefault(o => o.OrderId == order.OrderId);
            });

            var findobj = task.GetAwaiter().GetResult();
            findobj.OrderTime = order.OrderTime;
            findobj.UserName = order.UserName;
            findobj.Status = order.Status;
            findobj.ShippingAddress = order.Status;
            findobj.TotalCost = order.TotalCost;

            await _db.SaveChangesAsync();
        }
    }
}
