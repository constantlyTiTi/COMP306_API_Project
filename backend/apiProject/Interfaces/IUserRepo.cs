using apiProject.Interfaces;
using apiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Repositories
{
    public interface IUserRepo
    {
/*        Task DeleteUser(string userName);*/
        Task<User> UpdateUser(User user);
        User AddUser(User user);
        Task<User> GetUser(string userName, string password);
    }
}
