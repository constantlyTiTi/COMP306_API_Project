using apiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Interfaces
{
    public interface IRateRepo : IMSSQLRepo<Rate>
    {
        Task<Rate> GetRateByOrderId(long orderId);
        Task<double> GetAvgRateByItemId(long itemId);
        Task DeleteRateByOrderId(long orderId);
        Task<Rate> UpdateRateByOrderId(Rate rate);
        Task<IEnumerable<Rate>> GetRatesByItemId(long itemId);
        Task<IEnumerable<Rate>> GetRatesByUserName(string userName);
        Task<Rate> GetRatesByUserNameAndOrderId(string userName, long orderId);
    }
}
