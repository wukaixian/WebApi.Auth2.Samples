using System.Collections.Generic;
using System.Linq;
using WebApi.Sample.Models;

namespace WebApi.Sample.Service
{
    public class UserService
    {
        private readonly List<User> _users=new List<User>
        {
            new User{UserName = "wkx",Password = "123"}
        };
        public bool ValidateUser(string userName, string password)
        {
            return _users.Any(m => m.UserName == userName && m.Password == password);
        }
    }
}