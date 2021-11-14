using apiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Interfaces
{
    public interface IItemFileRepo
    {
        Task UpdateItem(ItemFile item);
        Task AddItem(ItemFile item);
        Task Remove(long itemId);
        Task<ItemFile> GetItemByItemId(long itemId);

    }
}
