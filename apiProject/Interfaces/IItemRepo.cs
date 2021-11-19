using apiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Interfaces
{
    public interface IItemRepo:IMSSQLRepo<Item>
    {
        Task<IEnumerable<Item>> GetItemByUploadedDateTime(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Item>> GetItemByUserName(string userName);
        Task<IEnumerable<Item>> GetItemByLocation(string PostCode);
        Task<IEnumerable<Item>> GetItemByUserNameAndLocation(string userName,string PostCode);
        Task<IEnumerable<Item>> GetItemByPrice(double lowPrice, double highPrice);
        Task<IEnumerable<Item>> GetItemByCategory(string category);
        Task<IEnumerable<Item>> GetItemByCategoryAndUserName(string category, string userName);
        Task<IEnumerable<Item>> GetItemByItemName(string itemName);
        Task<IEnumerable<Item>> GetItemByItemNamePostalCode(string itemName, string postCode);
        Task UpdateItem(Item item);
    }
}
