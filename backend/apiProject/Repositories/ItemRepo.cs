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
                return (IEnumerable<Item>)_db.Item.Where(i => i.Category == category).ToList();
            });
            return await task;
        }

        public async Task<IEnumerable<Item>> GetItemByItemName(string itemName)
        {
            var task = Task.Factory.StartNew(() =>
            {
                return (IEnumerable<Item>)_db.Item.Where(i => i.Category.Contains(itemName)).ToList();
            });
            return await task;
        }

        public async Task<IEnumerable<Item>> GetItemByItemNamePostalCode(string itemName, string postCode)
        {
            var task = Task.Factory.StartNew(() =>
            {
                return (IEnumerable<Item>)_db.Item.Where(i => i.Category.Contains(itemName) && i.LocationPostalCode.Substring(0, 3) == postCode.Substring(0, 3)).ToList();
            });
            return await task;
        }

        public async Task<IEnumerable<Item>> GetItemByCategoryAndUserName(string category, string userName)
        {
            var task = Task.Factory.StartNew(() =>
            {
                return (IEnumerable<Item>)_db.Item.Where(i => i.Category == category && i.UserName == userName).ToList();
            });
            return await task;
        }

        public async Task<IEnumerable<Item>> GetItemByLocation(string PostCode)
        {
            var task = Task.Factory.StartNew(() =>
            {
                return (IEnumerable<Item>)_db.Item.Where(i => i.LocationPostalCode.Substring(0, 3) == PostCode.Substring(0, 3)).ToList();
            });
            return await task;
        }

        public async Task<IEnumerable<Item>> GetItemByPrice(double lowPrice, double highPrice)
        {
            var task = Task.Factory.StartNew(() =>
            {
                return (IEnumerable<Item>)_db.Item.Where(i => i.Price > lowPrice && i.Price < highPrice).ToList();
            });
            return await task;
        }

        public async Task<IEnumerable<Item>> GetItemByUploadedDateTime(DateTime startDate, DateTime endDate)
        {
            var task = Task.Factory.StartNew(() =>
            {
                return (IEnumerable<Item>)_db.Item.Where(i => i.UploadItemDateTime.Date >= startDate.Date && i.UploadItemDateTime.Date < endDate.Date).ToList();
            });
            return await task;
        }

        public async Task<IEnumerable<Item>> GetItemByUserName(string userName)
        {
            var task = Task.Factory.StartNew(() =>
            {
                return (IEnumerable<Item>)_db.Item.Where(i => i.UserName == userName).ToList();
            });
            return await task;
        }

        public async Task<IEnumerable<Item>> GetItemByUserNameAndLocation(string userName, string PostCode)
        {
            var task = Task.Factory.StartNew(() =>
            {
                return (IEnumerable<Item>)_db.Item.Where(i => i.UserName == userName && i.LocationPostalCode == PostCode).ToList();
            });
            return await task;
        }

        public async Task UpdateItem(Item item)
        {
            var findObj = await _db.Item.FindAsync(item.ItemId);
            findObj.ItemName = item.ItemName;
            findObj.Description = item.Description;
            findObj.Price = item.Price;
            findObj.LocationPostalCode = item.LocationPostalCode;
            findObj.Category = item.Category;
            _db.SaveChanges();
        }


        public async Task<IEnumerable<Item>> GetAllByIds(IEnumerable<long> itemIds)
        {
            var task = Task.Factory.StartNew(() =>
            {
                return (IEnumerable<Item>)_db.Item.Where(i => itemIds.Contains(i.ItemId)).ToList();
            });
            return await task;
        }

}
}
