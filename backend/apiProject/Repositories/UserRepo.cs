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

        public User AddUser(User user)
        {
            _db.User.Add(user);
            _db.SaveChanges();
            return user;
        }

/*        public async Task<User> DeleteUser(string userName)
        {
             
        }*/

        public async Task<User> GetUser(string userName)
        {
            var findObj = await _db.FindAsync<User>(userName);
            if(findObj == null)
            {
                throw new Exception("User name cannot be found");
            }
            return findObj;
        }

        public async Task<User> UpdateUser(User user)
        {
            var findObj = await _db.FindAsync<User>(user.UserName);
            findObj.Password = user.Password;
            findObj.IpAddress = user.IpAddress;
            _db.SaveChanges();
            return findObj;
        }
    }
}
