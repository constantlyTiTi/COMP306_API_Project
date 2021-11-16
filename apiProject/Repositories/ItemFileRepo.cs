using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using apiProject.DBContexts;
using apiProject.Interfaces;
using apiProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Repositories
{
    public class ItemFileRepo: MSSQLRepo<ItemFile>,IItemFileRepo

    {
        private readonly MSSQLDbContext _db;
        public ItemFileRepo(MSSQLDbContext db):base(db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ItemFile>> GetItemByItemId(long itemId)
        {

            var task = Task.Factory.StartNew(() =>
            {
                return (IEnumerable<ItemFile>)_db.ItemFile.Where(i => i.ItemId == itemId).ToList();
 
            });
            return await task;
        }

        public async Task UpdateItem(ItemFile item)
        {
            var findObj = await _db.ItemFile.FirstOrDefaultAsync(i => i.ItemFileId == item.ItemFileId);
            findObj.ImgFileKey = item.ImgFileKey;
            findObj.ImgDescription = item.ImgDescription;
            await _db.SaveChangesAsync();

        }

        async Task IItemFileRepo.RemoveByItemId(long itemId)
        {
            var entities = GetItemByItemId(itemId);
            _db.RemoveRange(entities);
            await _db.SaveChangesAsync();
        }
    }
}
