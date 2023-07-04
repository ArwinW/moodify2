using MySql.Data.MySqlClient;
using Dapper;
using Moodify.Models;

namespace Moodify.db
{
    public class db
    {
        public MySqlConnection GetConnection()
        {
            string connectionString = "Server=localhost; User ID=root; Database=moodify";
            return new MySqlConnection(connectionString);
        }

        public bool FindUserByUsernameAndPassword(UserModel user)
        {
            using (MySqlConnection connection = GetConnection())
            {
                string sqlStatement = "SELECT * FROM users WHERE username = @UserName AND password = @Password";
                var result = connection.QueryFirstOrDefault<UserModel>(sqlStatement, user);
                return result != null;
            }
        }

    }
}