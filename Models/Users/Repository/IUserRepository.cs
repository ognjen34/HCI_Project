using HCI.Models.Users.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Users.Repository
{
    public interface IUserRepository
    {
        User GetById(int id);
        void Add(User user);
        void Remove(User user);
        IEnumerable<User> GetAll();
        User GetUserByEmailAndPassword(string email, string password);
        User GetUserByEmail(string email);

    }

}
