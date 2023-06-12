using HCI.Models.Users.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Users.Service
{
    public interface IUserService
    {
        User GetUserById(int id);
        void AddUser(User user);
        void RemoveUser(User user);
        IEnumerable<User> GetAllUsers();
        User GetUserByEmailAndPassword(string email, string password);
        User GetUserByEmail(string email);

    }
}
