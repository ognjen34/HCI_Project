using HCI.Models.Users.Model;
using HCI.Models.Users.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Users.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User GetUserById(int id)
        {
            return _userRepository.GetById(id);
        }

        public void AddUser(User user)
        {
            _userRepository.Add(user);
        }

        public void RemoveUser(User user)
        {
            _userRepository.Remove(user);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAll();
        }
        public User GetUserByEmailAndPassword(string email, string password)
        {
            return _userRepository.GetUserByEmailAndPassword(email, password);
        }
    }
}
