using Amazon.DynamoDBv2.DataModel;
using apiProject.DBContexts;
using apiProject.Interfaces;
using apiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Repositories
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly MSSQLDbContext _db;

        public UnitOfWork(MSSQLDbContext db)
        {
            _db = db;
            Rate = new RateRepo(_db);
            SP_Call = new SP_Call(_db);
            User = new UserRepo(_db);
            OrderItem = new OrderItemRepo(_db);
            OrderDetails = new OrderDetailsRepo(_db);
            ItemFile = new ItemFileRepo(_db);
            Item = new ItemRepo(_db);
        }

        public IRateRepo Rate { get; set; }
        public ISP_Call SP_Call { get; set; }
        public IUserRepo User { get; set; }
        public IOrderItemRepo OrderItem { get; set; }
        public IOrderDetailsRepo OrderDetails { get; set; }
        public IItemFileRepo ItemFile { get; set; }
        public IItemRepo Item { get; set; }

        public void Dispose()
        {
            var result = DisposeAsync();
        }

        public async Task DisposeAsync()
        {
            await _db.DisposeAsync();
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }

    }
}
