using apiProject.DBContexts;
using apiProject.Interfaces;
using apiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Repositories
{
    public class ItemRepo : MSSQLRepo<Item>, IItemRepo
    {
        private readonly MSSQLDbContext _db;
        public ItemRepo(MSSQLDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Item>> GetItemByCategory(string category)
        {
            var task = Task.Factory.StartNew(() =>
            {
                return (IEnumerable<Item>)_db.Item.Where(i => i.Category == category);
            });
            return await task;
        }

        public async Task<IEnumerable<Item>> GetItemByCategoryAndUserName(string category, string userName)
        {
            var task = Task.Factory.StartNew(() =>
            {
                return (IEnumerable<Item>)_db.Item.Where(i => i.Category == category && i.UserName == userName);
            });
            return await task;
        }

        public async Task<IEnumerable<Item>> GetItemByLocation(string PostCode)
        {
            var task = Task.Factory.StartNew(() =>
            {
                return (IEnumerable<Item>)_db.Item.Where(i => i.LocationPostalCode.Substring(0,3) == PostCode.Substring(0,3));
            });
            return await task;
        }

        public async Task<IEnumerable<Item>> GetItemByPrice(double lowPrice, double highPrice)
        {
            var task = Task.Factory.StartNew(() =>
            {
                return (IEnumerable<Item>)_db.Item.Where(i => i.Price > lowPrice && i.Price < highPrice);
            });
            return await task;
        }

        public async Task<IEnumerable<Item>> GetItemByUploadedDateTime(DateTime startDate, DateTime endDate)
        {
            var task = Task.Factory.StartNew(() =>
            {
                return (IEnumerable<Item>)_db.Item.Where(i => i.UploadItemDateTime > startDate && i.UploadItemDateTime < endDate);
            });
            return await task;
        }

        public async Task<IEnumerable<Item>> GetItemByUserName(string userName)
        {
            var task = Task.Factory.StartNew(() =>
            {
                return (IEnumerable<Item>)_db.Item.Where(i => i.UserName == userName);
            });
            return await task;
        }

        public async Task<IEnumerable<Item>> GetItemByUserNameAndLocation(string userName, string PostCode)
        {
            var task = Task.Factory.StartNew(() =>
            {
                return (IEnumerable<Item>)_db.Item.Where(i => i.UserName == userName && i.LocationPostalCode == PostCode);
            });
            return await task;
        }
    }
}
