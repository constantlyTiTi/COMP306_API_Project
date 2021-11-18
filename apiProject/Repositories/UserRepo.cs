using apiProject.DBContexts;
using apiProject.Interfaces;
using apiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace apiProject.Repositories
{
    public class UserRepo: IUserRepo
    {
        private readonly MSSQLDbContext _db;
        public UserRepo(MSSQLDbContext db)
        {
            _db = db;
        }

        public async Task<User> AddUser(User user)
        {
            await _db.User.AddAsync(user);
            return user;
        }

/*        public async Task<User> DeleteUser(string userName)
        {
             
        }*/

        public async Task<User> GetUser(string userName, string password)
        {
            var findObj = await _db.FindAsync<User>(userName);
            if(findObj == null)
            {
                throw new Exception("User name cannot be found");
            }
            if(findObj.Password != password)
            {
                throw new Exception("Password does not match our record");
            }
            return findObj;
        }

        public async Task<User> UpdateUser(User user)
        {
            var findObj = await _db.FindAsync<User>(user.UserName);
            findObj.Password = user.Password;
            findObj.IpAddress = user.IpAddress;
            await _db.SaveChangesAsync();
            return findObj;
        }
    }
}
