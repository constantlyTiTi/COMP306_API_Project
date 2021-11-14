using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using apiProject.DBContexts;
using apiProject.Interfaces;
using apiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Repositories
{
    public class ItemFileRepo: IItemFileRepo

    {
        private readonly DynamoDBContext _db;
        public ItemFileRepo(DynamoDBContext db)
        {
            _db = db;
        }

        public async Task AddItem(ItemFile item)
        {
           await _db.SaveAsync<ItemFile>(item);
        }

        public async Task<ItemFile> GetItemByItemId(long itemId)
        {
            return await _db.LoadAsync<ItemFile>(itemId);
        }

        public async Task Remove(long itemId)
        {
            await _db.DeleteAsync(itemId);
        }

        public async Task UpdateItem(ItemFile item)
        {
            var findObj = await _db.LoadAsync<ItemFile>(item.ItemId);
            findObj.ImgFileKeys = item.ImgFileKeys;
            findObj.ImgDescriptions = item.ImgDescriptions;
            await _db.SaveAsync<ItemFile>(findObj);
        }

    }
}
