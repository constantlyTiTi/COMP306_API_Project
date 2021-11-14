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

        public Task AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUser(string userName)
        {
            throw new NotImplementedException();
        }

        public Task GetUser(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
