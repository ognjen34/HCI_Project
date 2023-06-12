using HCI.Models.Users.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Users.DTO
{
    public class LoginSuccessArgs
    {
        public User User { get; }

        public LoginSuccessArgs(User user)
        {
            User = user;
        }
    }
}
