using Amazon.DynamoDBv2.DataModel;
using apiProject.DBContexts;
using apiProject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Repositories
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly MSSQLDbContext _db;
        private readonly DynamoDBContext _dynamoDb;

        public UnitOfWork(MSSQLDbContext db, DynamoDBContext dynamoDb)
        {
            _db = db;
            _dynamoDb = dynamoDb;
            Rate = new RateRepo(_db);
            SP_Call = new SP_Call(_db);
            User = new UserRepo(_db);
            OrderItem = new OrderItemRepo(_db);
            Item = new ItemFileRepo(_dynamoDb);
            OrderDetails = new OrderDetailsRepo(_db);
        }

        public IRateRepo Rate { get; set; }
        public ISP_Call SP_Call { get; set; }
        public IUserRepo User { get; set; }
        public IOrderItemRepo OrderItem { get; set; }
        public IOrderDetailsRepo OrderDetails { get; set; }
        public IItemFileRepo Item { get; set; }

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
