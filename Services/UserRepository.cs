using System.Data;
using System.Linq;
using Dapper;
using Moodify.Models;
using MySql.Data.MySqlClient;

namespace Moodify.Data
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserModel GetUserByUsernameAndPassword(string username, string password)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM users WHERE username = @Username AND password = @Password";
                return connection.QuerySingleOrDefault<UserModel>(sql, new { Username = username, Password = password });
            }
        }
    }
}
