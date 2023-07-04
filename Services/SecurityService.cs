using Moodify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moodify.Services
{
    public class SecurityService
    {
        List<UserModel> knownUsers = new List<UserModel>();

        public SecurityService()
        {
            knownUsers.Add(new UserModel { Id = 0, UserName = "test1", Password = "test1" });
            knownUsers.Add(new UserModel { Id = 1, UserName = "test12", Password = "test12" });
            knownUsers.Add(new UserModel { Id = 3, UserName = "test123", Password = "test123" });
            knownUsers.Add(new UserModel { Id = 4, UserName = "test1234", Password = "test1234" });
        }

        public bool IsValid(UserModel user)
        {
            //return true if found in the list
            return knownUsers.Any(x => x.UserName == user.UserName && x.Password == user.Password);
        }

    }
}
