using Moodify.Models;
using Moodify.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moodify.Services
{
    public class SecurityService
    {
        private readonly DataAccess _database;

        public SecurityService()
        {
            _database = new DataAccess();
        }

        public bool IsValid(UserModel userModel)
        {
            var user = _database.GetUserByUsernameAndPassword(userModel.UserName, userModel.Password);
            return user != null;
        }
    }
}


