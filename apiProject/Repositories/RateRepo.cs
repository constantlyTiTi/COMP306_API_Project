using apiProject.DBContexts;
using apiProject.Interfaces;
using apiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Repositories
{
    public class RateRepo:MSSQLRepo<Rate>,IRateRepo
    {
        private readonly MSSQLDbContext _db;
        public RateRepo(MSSQLDbContext db):base(db)
        {
            _db = db;
        }

        public async Task DeleteRateByOrderId(long orderId)
        {
            var findObj = _db.Rate.FirstOrDefault(r => r.OrderId == orderId);
            _db.Rate.Remove(findObj);
            await _db.SaveChangesAsync();
        }

        public async Task<double> GetAvgRateByItemId(long itemId)
        {
            Task<double> result = Task.Factory
                                       .StartNew(() => { 
                                           return(double) _db.Rate
                                           .Where(r=>r.ItemId==itemId)
                                           .Average(r=>r.RateScore); 
                                       });
            return await result;
        }

        public async Task<Rate> GetRateByOrderId(long orderId)
        {
            Task<Rate> findObj = Task.Factory.StartNew(() =>
            {
                return (Rate)_db.Rate.FirstOrDefault(r => r.OrderId == orderId);
            });

            return await findObj;
        }

        public async Task<IEnumerable<Rate>> GetRatesByItemId(long itemId)
        {
            Task<IEnumerable<Rate>> findObjs = Task.Factory.StartNew(() =>
            {
                return (IEnumerable<Rate>)_db.Rate.Where(r => r.ItemId == itemId).ToList();
            });
            return await findObjs;
        }

        public async Task<IEnumerable<Rate>> GetRatesByUserName(string userName)
        {
            Task<IEnumerable<Rate>> findObjs = Task.Factory.StartNew(() =>
            {
                return (IEnumerable<Rate>)_db.Rate.Where(r => r.UserName == userName).ToList();
            });
            return await findObjs;
        }

        public async Task<Rate> GetRatesByUserNameAndOrderId(string userName, long orderId)
        {
            Task<Rate> findObj = Task.Factory.StartNew(() =>
            {
                return (Rate)_db.Rate.FirstOrDefault(r => r.OrderId == orderId && r.UserName == userName);
            });

            return await findObj;
        }

        public async Task<Rate> UpdateRateByOrderId(Rate rate)
        {
            var updateObj = _db.Rate.FirstOrDefault(s => s.OrderId == rate.OrderId && s.UserName == rate.UserName);
            updateObj.RateScore = rate.RateScore;
            updateObj.Comment = rate.Comment;
            updateObj.RateTime = rate.RateTime;
            await _db.SaveChangesAsync();
            return updateObj;
        }
    }
}
